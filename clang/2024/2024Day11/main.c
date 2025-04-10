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

struct Stones {
    int max_size;
    int size;
    long *numbers;
};

struct StonesMemoized {
    long initialNumber;
    int levelsCount;
    long *numberPerLevel;
};

struct Stones* ConvertStringsToNumbers(char *string) {
    int countSpaces = 1;
    for (int i = 0; i < strlen(string); i++) {
        if (string[i] == ' ') countSpaces++;
    }

    printf("countSpaces: %d\n", countSpaces);

    struct Stones *stones = malloc(sizeof(struct Stones));

    stones->numbers = malloc(countSpaces * sizeof(long));
    stones->size = countSpaces;
    stones->max_size = countSpaces;

    long total = 0;
    int numberIndex = 0;
    for (int i = 0; i < strlen(string); i++) {
        if (string[i] == ' ') { 
            printf("setting numbers at %d with value %ld\n", numberIndex, total);
            stones->numbers[numberIndex] = total;
            total = 0;
            numberIndex++;
            continue;
        }
        total = (total * 10) + (string[i] - '0');
    }
    printf("setting numbers at %d with value %ld\n", numberIndex, total);
    stones->numbers[numberIndex] = total;
    return stones;
}


struct Stones* AllocateNewStones(struct Stones *stones) {
    struct Stones *stonesNew = malloc(sizeof(struct Stones));
    stonesNew->max_size = stones->size * 2;
    stonesNew->size = 0;
    stonesNew->numbers = malloc(stonesNew->max_size * sizeof(long));
    return stonesNew;
}

char* ltostr(long value) {
    char buffer[ 64 ] = { 0 };
    snprintf(buffer, sizeof(buffer), "%li", value);
    return strdup(buffer);
}

void PerformBlink(struct Stones *stones, long currentNumber) {
    char* numbersChar = ltostr(currentNumber);
    if (currentNumber == 0) {
        stones->numbers[stones->size] = 1;
        stones->size++;
    } 
    else if (strlen(numbersChar) % 2 == 0) {
        long newNumber1 = 0;
        for (int i = 0; i < strlen(numbersChar) / 2; i++) {
            newNumber1 = newNumber1 * 10 + (numbersChar[i] - 48);
        }
        stones->numbers[stones->size] = newNumber1;
        stones->size++;

        long newNumber2 = 0;
        for (int i = strlen(numbersChar) / 2; i < strlen(numbersChar); i++) {
            newNumber2 = newNumber2 * 10 + (numbersChar[i] - 48);
        }
        stones->numbers[stones->size] = newNumber2;
        stones->size++;
    }
    else {
        stones->numbers[stones->size] = currentNumber * 2024;
        stones->size++;
    }
}

struct Stones* PerformIteration(struct Stones *stones) {
    struct Stones *stonesNew = AllocateNewStones(stones);
    
    for (int i = 0; i < stones->size; i++) {
        PerformBlink(stonesNew, stones->numbers[i]);
    }
    
    free(stones->numbers);
    free(stones);

    return stonesNew;
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
    
    struct Stones *stones = ConvertStringsToNumbers(strings[0]);
    printf("print numbers, num_length: %d\n", stones->size);
    for (int i = 0; i < stones->size; i++) {
        printf("%d ", stones->numbers[i]);
    }
    printf("\n");

    struct Stones *stones2 = malloc(sizeof(struct Stones));
    stones2->numbers = malloc(10 * sizeof(long));
    stones2->max_size = 10;
    stones2->size = 1;
    stones2->numbers[0] = 0;
    for (int i = 0; i < 25; i++) {
        stones2 = PerformIteration(stones2);
        printf("number of stones at iteration %d out of %d: %d\n", i, stones2->size, stones2->max_size);
    }

    // for (int i = 0; i < 75; i++) {
    //     stones = PerformIteration(stones);
    //     printf("number of stones at iteration %d: %d\n", i, stones->size);
    // }

    release_all_strings(strings, num_strings);
    return 0;
}