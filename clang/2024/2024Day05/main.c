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
            printf("%s\n", strings[i]);
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

struct ordered_pair {
    int key;
    int *values;
    int values_count;
};

struct manual {
    int *keys;
    int key_count;
};

void split_pairs(char* key, char* value, char *line) {
    key[0] = line[0];
    key[1] = line[1];
    value[0] = line[3];
    value[1] = line[4];
}

void get_manual(char* line, struct manual *manual) {
    int delim_count = 0;
    char delim = ',';
    for (int i = 0; i < strlen(line); i++) {
        if (line[i] == delim) delim_count++;
    }
    delim_count++;

    manual->keys = (int*)malloc(delim_count * sizeof(int));
    manual->key_count = delim_count;
    int int_array_length = 0;
    char *line_copy = strdup(line);
    char *token = strtok(line_copy, &delim);
    while (token != NULL) {
        manual->keys[int_array_length] = atoi(token);
        int_array_length++;
        token = strtok(NULL, &delim);
    }
}

bool check_manual(struct manual manual, struct ordered_pair *ordered_pairs){
    for (int i = 1; i < manual.key_count; i++) {
        for (int j = 0; j < 100; j++) {
            if (ordered_pairs[j].values != NULL && manual.keys[i] == j) {
                for (int k = 0; k < ordered_pairs[j].values_count; k++){
                    for (int l = 0; l < i; l++) {
                        if (manual.keys[l] == ordered_pairs[j].values[k]) {
                            printf("found false case: manual entry: %d\n", manual.keys[l]);
                            return false;
                        }
                    }
                }
            }
        }
    }
    return true;
}

bool check_manual_with_swap(struct manual manual, struct ordered_pair *ordered_pairs){
    for (int i = 1; i < manual.key_count; i++) {
        for (int j = 0; j < 100; j++) {
            if (ordered_pairs[j].values != NULL && manual.keys[i] == j) {
                for (int k = 0; k < ordered_pairs[j].values_count; k++){
                    for (int l = 0; l < i; l++) {
                        if (manual.keys[l] == ordered_pairs[j].values[k]) {
                            printf("found false case: manual entry: %d\n", manual.keys[l]);
                            int temp = manual.keys[i];
                            manual.keys[i] = manual.keys[l];
                            manual.keys[l] = temp;
                            return false;
                        }
                    }
                }
            }
        }
    }
    return true;
}

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0\n");
        return 1;
    }

    printf("number of strings: %d\n", num_strings);
    print_all_strings(strings, num_strings);

    struct ordered_pair *ordered_pairs = malloc(100 * sizeof(struct ordered_pair));
    for (int i = 0; i < 100; i++) {
        ordered_pairs[i].values = NULL;  // Initialize values to NULL
        ordered_pairs[i].values_count = 0; // Initialize values_count to 0
    }

    int string_line = 0; 
    while (strcmp(strings[string_line], "") != 0) {
        char *key = malloc(2 * sizeof(char));
        char *value = malloc(2 * sizeof(char));
        split_pairs(key, value, strings[string_line]);

        int key_int = atoi(key);

        if (ordered_pairs[key_int].values == NULL) {
            ordered_pairs[key_int].values = (int*)malloc(100 * sizeof(int));
            ordered_pairs[key_int].values_count = 0;
        }
        ordered_pairs[key_int].values[ordered_pairs[key_int].values_count] = atoi(value);
        ordered_pairs[key_int].values_count++;

        free(key);
        free(value);
        string_line++;
    }

    for (int i = 0; i < 100; i++) {
        if (ordered_pairs[i].values_count > 0) {
            printf("%d : %d ", i, ordered_pairs[i].values_count);
            for (int j = 0; j < ordered_pairs[i].values_count; j++){
                printf(" %d ", ordered_pairs[i].values[j]);
            }
            printf("\n");
        }
    }

    string_line++;
    int manuals_set_count = num_strings - string_line;
    printf("manuals count: %d\n", manuals_set_count);
    fflush(stdout);
    //struct manual **manuals = (struct manual*)malloc((manuals_count) * sizeof(struct manual));
    struct manual manuals[manuals_set_count];

    for (int i = 0; i < manuals_set_count; i++) {
        printf("getting manual for %s\n", strings[string_line + i]);
        get_manual(strings[string_line + i], &manuals[i]);
        if (manuals[i].keys == NULL) {
            printf("There was an issue parsing the manual line.");
            return 1;
        }
    }

    int valid_manual_middle_sum = 0;
    for(int i = 0; i < manuals_set_count; i++) {
        for(int j = 0; j < manuals[i].key_count; j++) {
            printf(" %d ", manuals[i].keys[j]);
        }
        printf("\n");
        bool is_correct = check_manual(manuals[i], ordered_pairs);
        if (is_correct) printf("%d is correct\n", i);
        if (is_correct) valid_manual_middle_sum += manuals[i].keys[manuals[i].key_count / 2];
    }
    printf("total valid: %d\n", valid_manual_middle_sum);

    valid_manual_middle_sum = 0;
    for(int i = 0; i < manuals_set_count; i++) {
        for(int j = 0; j < manuals[i].key_count; j++) {
            printf(" %d ", manuals[i].keys[j]);
        }
        printf("\n");
        bool found_invalid = false;
        bool is_correct = check_manual_with_swap(manuals[i], ordered_pairs);
        if (!is_correct) found_invalid = true;
        while (!is_correct) is_correct = check_manual_with_swap(manuals[i], ordered_pairs);
        if (is_correct) printf("%d is correct\n", i);
        if (found_invalid) valid_manual_middle_sum += manuals[i].keys[manuals[i].key_count / 2];
        printf("corrected incorrect manual, total: %d\n", valid_manual_middle_sum);
    }
    printf("total valid: %d\n", valid_manual_middle_sum);

    release_all_strings(strings, num_strings);
    return 0;
}