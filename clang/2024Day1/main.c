#include <stdio.h>
#include <stdlib.h>
#include <string.h>

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

int convert_strings_to_int_arrays(char **strings, int *array1, int *array2, int num_strings) {
    int number_length = ((strlen(strings[0]) - 3) / 2);
    for (int i = 0; i < num_strings; i++) {
        char *first_num = malloc(number_length * sizeof(char*));
        for (int j = 0; j < number_length; j++) {
            first_num[j] = strings[i][j];
        }
        array1[i] = atoi(first_num);

        char *second_num = malloc(number_length * sizeof(char*));
        for (int j = 0; j < number_length; j++) {
            second_num[j] = strings[i][(number_length + 3) + j];
        }
        array2[i] = atoi(second_num);
    }
    return 0;
}

void print_all_strings(char **strings, int num_strings) {
    if (strings) {
        // Print all strings
        for (int i = 0; i < num_strings; i++) {
            printf("%s\n", strings[i]);
        }
    }
}

void print_all_ints(int *array, int num_ints) {
    for (int i = 0; i < num_ints; i++) {
        printf("%d\n", array[i]);
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

void release_all_ints(int *array, int num_strings) {
    free(array);
}

int compare( const void* a, const void* b) {
     int int_a = * ( (int*) a );
     int int_b = * ( (int*) b );
     
     if ( int_a == int_b ) return 0;
     else if ( int_a < int_b ) return -1;
     else return 1;
}

int get_total_distance(int *array1, int *array2, int num_arrays) {
    int distance = 0;
    for (int i = 0; i < num_arrays; i++) {
        distance += abs(array1[i] - array2[i]);
    }
    return distance;
}

int calculate_similarity_score(int *array1, int *array2, int num_arrays) {
    int score = 0;
    for (int i = 0; i < num_arrays; i++) {
        int count = 0;
        for (int j = 0; j < num_arrays && array1[i] >= array2[j]; j++) {
            if (array1[i] == array2[j]) {
                count++;
            }
        }
        score += array1[i] * count;
    }
    return score;
}

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0");
        return 1;
    }

    // print_all_strings(strings, num_strings);

    int *array1 = malloc(num_strings * sizeof(int *));
    int *array2 = malloc(num_strings * sizeof(int *));

    int convert_result = convert_strings_to_int_arrays(
        strings,
        array1,
        array2,
        num_strings);

    if (convert_result > 0) {
        printf("Error convert_strings_to_int_arrays");
        return convert_result;
    }

    // print_all_ints(array1, num_strings);
    // print_all_ints(array2, num_strings);

    qsort(array1, num_strings, sizeof(int), compare);
    qsort(array2, num_strings, sizeof(int), compare);

    // print_all_ints(array1, num_strings);
    // print_all_ints(array2, num_strings);

    int distance = get_total_distance(array1, array2, num_strings);
    printf("total distance: %d\n", distance);
    int score = calculate_similarity_score(array1, array2, num_strings);
    printf("total score: %d\n", score);

    release_all_strings(strings, num_strings);
    release_all_ints(array1, num_strings);
    release_all_ints(array2, num_strings);

    return 0;
}

