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

struct TowelVector {
    int size;
    int max_size;
    char **colors;
};

struct PatternVector {
    int size;
    int max_size;
    char **patterns;
};

struct TowelVector* GetTowels(char *string) {
    int spaceCount = 0;
    for (int i = 0; i < strlen(string); i++) {
        if (string[i] == ' ') spaceCount++;
    }
    spaceCount++;
    printf("spaceCount: %d\n", spaceCount);
    
    struct TowelVector *towels = malloc(sizeof(struct TowelVector*));
    towels->max_size = spaceCount;
    towels->size = 0;
    towels->colors = malloc(towels->max_size * sizeof(char*));

    char *token = strtok(string, " ");
    while (token != NULL) {
        towels->colors[towels->size] = token;
        if (towels->colors[towels->size][strlen(towels->colors[towels->size]) - 1] == ',') {
            towels->colors[towels->size][strlen(towels->colors[towels->size]) - 1] = '\0';
        }
        towels->size++;
        token = strtok(NULL, " ");
    }
    return towels;
}

struct PatternVector* GetPatterns(char **strings, int num_strings, int startingIndex) {
    struct PatternVector *patterns = malloc(sizeof(struct PatternVector*));
    patterns->max_size = num_strings - startingIndex;
    patterns->size = 0;
    patterns->patterns = malloc(patterns->max_size * sizeof(char*));

    for (int i = startingIndex; i < num_strings; i++) {
        patterns->patterns[patterns->size] = strings[i];
        patterns->size++;
    }

    return patterns;
}

bool ProcessPattern(char *pattern, struct TowelVector *towels, int currentIndex) {
    bool result = false;
    for (int i = 0; i < towels->size; i++) {
        int tempCurrentIndex = currentIndex;
        for (int j = 0; j < strlen(towels->colors[i]); j++) {
            if (towels->colors[i][j] == pattern[tempCurrentIndex]) {
                tempCurrentIndex++;
            }
            else {
                break;
            }
        }
        if (strlen(towels->colors[i]) == tempCurrentIndex - currentIndex && tempCurrentIndex < strlen(pattern)) {
            result = ProcessPattern(pattern, towels, tempCurrentIndex);
            if (result) return result;
        }
        if (tempCurrentIndex == strlen(pattern)) return true;
    }
    return result;
}

int ProcessPatternPart2(char *pattern, struct TowelVector *towels, int currentIndex) {
    int result = 0;
    for (int i = 0; i < towels->size; i++) {
        int tempCurrentIndex = currentIndex;
        int incorrect = false;
        for (int j = 0; j < strlen(towels->colors[i]); j++) {
            if (towels->colors[i][j] == pattern[tempCurrentIndex]) {
                tempCurrentIndex++;
            }
            else {
                incorrect = true;
                break;
            }
        }
        if (incorrect) continue;
        if (strlen(towels->colors[i]) == tempCurrentIndex - currentIndex && tempCurrentIndex < strlen(pattern)) {
            result += ProcessPatternPart2(pattern, towels, tempCurrentIndex);
        }
        if (tempCurrentIndex == strlen(pattern) && strlen(towels->colors[i]) == tempCurrentIndex - currentIndex) 
        {result++;
        }
    }
    return result;
}

bool Contains(char *string, char *shorterString) {
    for (int i = 0; i < strlen(string); i++) {
        int j = 0;
        for (j = 0; j < strlen(shorterString); j++) {
            if (string[i+j] != shorterString[j]) {
                break;
            }
        }
        if (j == strlen(shorterString)) return true;
    }
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
    
    printf("\nTowels:\n");
    struct TowelVector *towels = GetTowels(strings[0]);
    for (int i = 0; i < towels->size; i++) {
        printf("%s\n", towels->colors[i]);
    }

    int startingIndex = 2;
    printf("\nPatterns:\n");
    struct PatternVector *patterns = GetPatterns(strings, num_strings, startingIndex);
    for (int i = 0; i < patterns->size; i++) {
        printf("%s\n", patterns->patterns[i]);
    }
    printf("\n");

    int count = 0;
    for (int i = 0; i < patterns->size; i++) {
        bool result = ProcessPattern(patterns->patterns[i], towels, 0);
        if (result) count++;
    }
    printf("count: %d\n", count);

    count = 0;
    for (int i = 0; i < patterns->size; i++) {
        int tempCount = ProcessPatternPart2(patterns->patterns[i], towels, 0);
        count += tempCount;
        printf("tempcount: %d\n", tempCount);
    }
    printf("count_part2: %d\n", count);

    free(towels->colors);
    free(towels);

    for (int i = 0; i < patterns->size; i++) {
        free (patterns->patterns[i]);
    }
    free(patterns->patterns);
    free(patterns);

    release_all_strings(strings, num_strings);
    return 0;
}
