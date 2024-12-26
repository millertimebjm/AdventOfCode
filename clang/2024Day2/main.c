#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

#define INITIAL_SIZE 10  // Initial number of strings that can be stored

char **read_strings_from_file(FILE *file, int *num_strings) {
    char **strings = malloc(INITIAL_SIZE * sizeof(char *));  // Allocate space for an array of string pointers
    if (!strings) {
        perror("Failed to allocate memory");
        return NULL;
    }
    
    int capacity = INITIAL_SIZE;  // Initial capacity for the array
    *num_strings = 0;  // Initialize the count of strings

    char buffer[256];  // Buffer to hold each line read from the file
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

struct report {
    int *levels;
    int level_length;
};

void release_all_structs(struct report **reports, int num_reports) {
    if (reports) {
        for (int i = 0; i < num_reports; i++) {
            free(reports[i]);  // Free each string
        }
        free(reports);  // Free the array of pointers
    }
}

void split_string_into_int_array(char *string, struct report *array) {
    char *string_copy = strdup(string);
    int size = 0;
    char* token = strtok(string_copy, " ");
    while (token != NULL) {
        token = strtok(NULL, " ");
        size++;
    }
    free(string_copy);

    array->levels = malloc(size * sizeof(int));
    array->level_length = size;

    string_copy = strdup(string);
    int count = 0;
    token = strtok(string_copy, " ");
    while (token != NULL) {
        array->levels[count] = atoi(token);
        token = strtok(NULL, " ");
        count++;
    }
    free(string_copy);
}

bool determine_is_safe(struct report *array) {
    bool is_increasing = false;
    if (array->level_length < 2) {
        return false;
    }
    if (array->levels[0] - array->levels[1] > 0) {
        is_increasing = true;
    }

    for (int i = 0; i < array->level_length - 1; i++) {
        if (is_increasing && array->levels[i] - array->levels[i+1] <= 0) {
            return false;
        }
        if (!is_increasing && array->levels[i] - array->levels[i+1] >= 0) {
            return false;
        }
        if (is_increasing && array->levels[i] - array->levels[i+1] >= 4) {
            return false;
        }
        if (!is_increasing && array->levels[i] - array->levels[i+1] <= -4) {
            return false;
        }
    }
    return true;
}

bool determine_is_safe_with_skip(struct report *array, int index_to_skip, bool is_increasing) {
    if (array->level_length < 2) {
        return false;
    }

    int x = 0;
    int y = 1;
    if (index_to_skip == 0) {
        x++;
        y++;
    }
    if (index_to_skip == 1) y++;
    while (y < array->level_length) {

        if (is_increasing && array->levels[x] - array->levels[y] <= 0) {
            return false;
        }
        if (!is_increasing && array->levels[x] - array->levels[y] >= 0) {
            return false;
        }
        if (is_increasing && array->levels[x] - array->levels[y] >= 4) {
            return false;
        }
        if (!is_increasing && array->levels[x] - array->levels[y] <= -4) {
            return false;
        }
        x++;
        y++;
        if (x == index_to_skip) x++;
        if (y == index_to_skip) y++;
    }
    
    return true;
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

    printf("splitting into int array\n");

    struct report **reports = malloc(num_strings * sizeof(struct report));

    for (int i = 0; i < num_strings; i++) {
        reports[i] = malloc(sizeof(struct report));
        split_string_into_int_array(strings[i], reports[i]);
    }

    int is_safe_count = 0;
    for (int i = 0; i < num_strings; i++) {
        bool is_safe = determine_is_safe(reports[i]);
        // printf("%d ", is_safe);
        // for (int j = 0; j < reports[i]->level_length; j++) {
        //     printf("%d ", reports[i]->levels[j]);
        // }
        // printf("\n");
        if (is_safe) {
            is_safe_count++;
        }
    }
    printf("safe count: %d\n", is_safe_count);

    int is_safe_with_dampener_count = 0;
    for (int i = 0; i < num_strings; i++) {
        bool is_safe = false;
        for (int j = 0; j < num_strings; j++) {
            is_safe |= determine_is_safe_with_skip(reports[i], j, true);
            is_safe |= determine_is_safe_with_skip(reports[i], j, false);
        }
        
        printf("%d ", is_safe);
        for (int j = 0; j < reports[i]->level_length; j++) {
            printf("%d ", reports[i]->levels[j]);
        }
        printf("\n");
        if (is_safe) {
            is_safe_with_dampener_count++;
        }
    }
    printf("safe with dampener count: %d\n", is_safe_with_dampener_count);

    release_all_strings(strings, num_strings);
    release_all_structs(reports, num_strings);
}