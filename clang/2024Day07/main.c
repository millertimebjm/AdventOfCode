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

struct calibration {
    ulong total;
    ulong *values;
    int num_values;
};

void parse_calibrations(
    struct calibration **calibrations, 
    char **strings,
    int num_strings) {
        printf("checking num_strings %d\n", num_strings);
        for (int i = 0; i < num_strings; i++) {
            calibrations[i] = malloc(sizeof(struct calibration));
            fflush(stdout);
            char *string_dup = strdup(strings[i]);
            char *token = strtok(string_dup, ":");
            calibrations[i]->total = atoi(token);
            char *values_string = strtok(NULL, ":");
            printf("values string: %s\n", values_string);
            int value_total_count = 0;
            for (int j = 0; j < strlen(values_string); j++) {
                if (values_string[j] == ' ') value_total_count++;
            }
            calibrations[i]->values = malloc(value_total_count * sizeof(ulong));
            int value_current_count = 0;
            token = strtok(values_string, " ");
            while (token != NULL) {
                calibrations[i]->values[value_current_count] = atol(token);
                value_current_count++;
                token = strtok(NULL, " ");
            }
            calibrations[i]->num_values = value_total_count;
            printf("values count: %d\n", calibrations[i]->num_values);
        }
    }

bool try_calculation(struct calibration *calibration, int index, ulong current_value) {
    bool is_equal = false;

    if (index == calibration->num_values) {
        if (current_value == calibration->total) return true;
        return false;
    }

    is_equal |= try_calculation(calibration, index+1, current_value + (ulong)calibration->values[index]);
    is_equal |= try_calculation(calibration, index+1, current_value * (ulong)calibration->values[index]);

    return is_equal;
}

ulong try_calculations(struct calibration **calibrations, int num_calibrations) {
    ulong valid_count = 0;
    for (int i = 0; i < num_calibrations; i++) {
        printf("starting calculation %d\n", i);
        bool is_valid = try_calculation(calibrations[i], 1, calibrations[i]->values[0]);
        if (is_valid) {valid_count += calibrations[i]->total;
            printf("found valid: %ld\n", calibrations[i]->total);
            printf("current total: %ld\n", valid_count);};
    }
    return valid_count;
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

    struct calibration **calibrations = malloc(num_strings * sizeof(struct calibration*));

    parse_calibrations(calibrations, strings, num_strings);

    for (int i = 0; i < num_strings; i++) {
        printf("%lu : ", calibrations[i]->total);
        for (int j = 0; j < calibrations[i]->num_values; j++) {
            printf(" %lu ", calibrations[i]->values[j]);
        }
        printf("\n");
    }

    ulong valid_count = try_calculations(calibrations, num_strings);
    printf("valid count: %lu\n", valid_count);

    for (int i = 0; i < num_strings; i++) {
        free(calibrations[i]->values);
        free(calibrations[i]);
    }
    free(calibrations);

    release_all_strings(strings, num_strings);
    return 0;
}