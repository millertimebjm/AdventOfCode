#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <ctype.h>

#define INITIAL_SIZE 100 // Initial number of strings that can be stored

char **read_strings_from_file(FILE *file, int *num_strings) {
    char **strings = malloc(INITIAL_SIZE * sizeof(char *));  // Allocate space for an array of string pointers
    if (!strings) {
        perror("Failed to allocate memory");
        return NULL;
    }
    
    int capacity = INITIAL_SIZE;  // Initial capacity for the array
    *num_strings = 0;  // Initialize the count of strings

    char buffer[256000];  // Buffer to hold each line read from the file
    while (fgets(buffer, sizeof(buffer), file)) {
        // Remove newline character if present
        buffer[strcspn(buffer, "\n")] = '\0';

        // Check if we need to resize the array of pointers
        if (*num_strings >= capacity) {
            capacity *= 2;  // Double the capacity
            strings = realloc(strings, capacity * sizeof(char *));
            if (!strings) {
                perror("Failed to reallocate memory");
                return NULL;
            }
        }

        // Allocate memory for the new string and copy the content
        strings[*num_strings] = malloc(strlen(buffer) + 1);
        if (!strings[*num_strings]) {
            perror("Failed to allocate memory for string");
            return NULL;
        }
        strcpy(strings[*num_strings], buffer);
        (*num_strings)++;
    }

    return strings;
}

char **read_strings_from_filename(char *filename, int *num_strings) {
    FILE *file = fopen(filename, "r");
    if (!file) {
        perror("Failed to open file");
        return NULL;
    }
    
    char **strings = read_strings_from_file(file, num_strings);
    fclose(file);

    return strings;
}

void print_all_strings(char **strings, int num_strings) {
    if (strings) {
        // Print all strings
        for (int i = 0; i < num_strings; i++) {
            printf("%s\n\n", strings[i]);
        }
    }
}

void release_all_strings(char **strings, int num_strings) {
    if (strings) {
        for (int i = 0; i < num_strings; i++) {
            free(strings[i]);  // Free each string
        }
        free(strings);  // Free the array of pointers
    }
}

enum State {
    operator_none = 0,
    operator_m = 1, 
    operator_u = 2, 
    operator_l = 3, 
    operator_left_parens = 4,
    operator_first_integer = 5,
    operator_comma = 6,
    operator_second_integer = 7,
    operator_right_parens = 8,
    operator_d = 10,
    operator_o = 11,
    operator_n = 20,
    operator_apostrophe = 21,
    operator_t = 22
};

enum Type {
    type_none = 0,
    type_mul = 1,
    type_do = 2,
    type_dont = 3
};

long calculate_multiplies_with_toggle(char *string, long total) {
    enum State state = operator_none;
    enum Type type = type_none;
    int first_index = 0;
    int first_integer = 0;
    int second_integer = 0;
    bool enabled = true;
    for (int i = 0; i < strlen(string); i++) {
        if (type == type_none && state == operator_none && string[i] == 'm') {
            state = operator_m;
            type = type_mul;
        }
        else if (type == type_mul && state == operator_m && string[i] == 'u') state = operator_u;
        else if (type == type_mul && state == operator_u && string[i] == 'l') state = operator_l;
        else if (type == type_mul && state == operator_l && string[i] == '(') state = operator_left_parens;
        else if (type == type_mul && (state == operator_left_parens || state == operator_first_integer) && isdigit(string[i])) {
            state = operator_first_integer;
            first_integer = (first_integer * 10) + ((int)string[i] - 48);
        }
        else if (type == type_mul && state == operator_first_integer && string[i] == ',') state = operator_comma;
        else if (type == type_mul && (state == operator_comma || state == operator_second_integer) && isdigit(string[i])) {
            state = operator_second_integer;
            second_integer = (second_integer * 10) + ((int)string[i] - 48);
        }
        else if (type == type_mul && state == operator_second_integer && string[i] == ')') {
            if (enabled){
                total += first_integer * second_integer;
                printf("enabled: valid mul found, mul(%d,%d) %d => %ld\n", first_integer, second_integer, first_integer*second_integer, total);
            } else {
                printf("disabled: valid mul found, mul(%d,%d) %d => %ld\n", first_integer, second_integer, first_integer*second_integer, total);
            }
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
            type = type_none;
        }
        else if (type == type_none && state == operator_none && string[i] == 'd') {
            state = operator_d;
            type = type_do;
        }
        else if (type == type_do && state == operator_d && string[i] == 'o') state = operator_o;
        else if (type == type_do && state == operator_o && string[i] == '(') state = operator_left_parens;
        else if (type == type_do && state == operator_left_parens && string[i] == ')') {
            enabled = true;
            printf("enabled\n");
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
            type = type_none;
        }
        else if (type == type_do && state == operator_o && string[i] == 'n') {
            state = operator_n;
            type = type_dont;
        }
        else if (type == type_dont && state == operator_n && string[i] == '\'') state = operator_apostrophe;
        else if (type == type_dont && state == operator_apostrophe && string[i] == 't') state = operator_t;
        else if (type == type_dont && state == operator_t && string[i] == '(') state = operator_left_parens;
        else if (type == type_dont && state == operator_left_parens && string[i] == ')') {
            enabled = false;
            printf("disabled\n");
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
            type = type_none;
        }
        else if (i - first_index > 1) {
            i--;
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
            type = type_none;
        }
    }
    return total;
}

long calculate_multiplies(char *string, long total) {
    enum State state = operator_none;
    int first_index = 0;
    int first_integer = 0;
    int second_integer = 0;
    for (int i = 0; i < strlen(string); i++) {
        if (state == operator_none && string[i] == 'm') state = operator_m;
        else if (state == operator_m && string[i] == 'u') state = operator_u;
        else if (state == operator_u && string[i] == 'l') state = operator_l;
        else if (state == operator_l && string[i] == '(') state = operator_left_parens;
        else if ((state == operator_left_parens || state == operator_first_integer) && isdigit(string[i])) {
            state = operator_first_integer;
            first_integer = (first_integer * 10) + ((int)string[i] - 48);
        }
        else if (state == operator_first_integer && string[i] == ',') state = operator_comma;
        else if ((state == operator_comma || state == operator_second_integer) && isdigit(string[i])) {
            state = operator_second_integer;
            second_integer = (second_integer * 10) + ((int)string[i] - 48);
        }
        else if (state == operator_second_integer && string[i] == ')') {
            total += first_integer * second_integer;
            printf("valid mul found, mul(%d,%d) %d => %ld\n", first_integer, second_integer, first_integer*second_integer, total);
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
        }
        else if (i - first_index > 1) {
            i--;
            state = operator_none;
            first_index = i;
            first_integer = 0;
            second_integer = 0;
        }
    }
    return total;
}

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0");
        return 1;
    }

    printf("number of strings: %d\n", num_strings);
    print_all_strings(strings, num_strings);

    long total = 0;
    // for (int i = 0; i < num_strings; i++) {
    //     total = calculate_multiplies(strings[i], total);
    // }
    // printf("total total: %ld\n", total);

    total = 0;
    for (int i = 0; i < num_strings; i++) {
        total = calculate_multiplies_with_toggle(strings[i], total);
    }
    printf("total total with toggle: %ld\n", total);

    release_all_strings(strings, num_strings);

    printf("number of lines: %d\n", num_strings);
}