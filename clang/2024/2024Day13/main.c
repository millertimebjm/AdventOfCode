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

struct ClawMachine {
    long xPrize;
    long yPrize;
    long aButtonX;
    long aButtonY;
    long bButtonX;
    long bButtonY;
};

struct ClawMachineSolution {
    long aPresses;
    long bPresses;
};

struct ClawMachineSolutionsVector {
    int max_size;
    int size;
    struct ClawMachineSolution **clawSolutions;
};

char* ConvertLeadingCharactersToZero(char *input) {
    for (int i = 0; i < strlen(input); i++) {
        if (input[i] != 48 && atoi(input) == 0) {
            input[i] = 48;
        } else {
            break;
        }
    }
    return input;
}

struct ClawMachine* ParseClawMachine(char *buttonA, char *buttonB, char *prize) {
    struct ClawMachine *clawMachine = malloc(sizeof(struct ClawMachine));
    char *token = strtok(buttonA, " "); // button text
    token = strtok(NULL, " "); // button type text

    clawMachine->aButtonX = atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));
    clawMachine->aButtonY = atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));

    token = strtok(buttonB, " ");
    token = strtok(NULL, " ");
    clawMachine->bButtonX = atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));
    clawMachine->bButtonY = atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));

    token = strtok(prize, " ");
    clawMachine->xPrize = 10000000000000 + (long)atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));
    clawMachine->yPrize = 10000000000000 + (long)atoi(ConvertLeadingCharactersToZero(strtok(NULL, " ")));
    
    return clawMachine;
}

struct ClawMachineSolutionsVector* FindClawMachineSolutions(struct ClawMachine *clawMachine) {
    long buttonAPresses = 0;
    long buttonBPresses = 0;
    long currentX = 0;
    long currentY = 0;
    struct ClawMachineSolutionsVector *clawMachineSolutionsVector = malloc(sizeof(struct ClawMachineSolutionsVector*));
    clawMachineSolutionsVector->max_size = 100;
    clawMachineSolutionsVector->size = 0;
    clawMachineSolutionsVector->clawSolutions = malloc(clawMachineSolutionsVector->max_size * sizeof(struct ClawMachineSolution*));

    while (currentX < clawMachine->xPrize && currentY < clawMachine->yPrize) {
        if (currentX == clawMachine->xPrize && currentY == clawMachine->yPrize) {
            struct ClawMachineSolution *solution = malloc(sizeof(struct ClawMachineSolution));
            clawMachineSolutionsVector->clawSolutions[clawMachineSolutionsVector->size] = solution;
            clawMachineSolutionsVector->size++;

            solution->aPresses = buttonAPresses;
            solution->bPresses = buttonBPresses;
        }

        buttonAPresses++;
        currentX += clawMachine->aButtonX;
        currentY += clawMachine->aButtonY;
    }

    while (buttonAPresses > 0) {
        if (currentX == clawMachine->xPrize && currentY == clawMachine->yPrize) {
            struct ClawMachineSolution *solution = malloc(sizeof(struct ClawMachineSolution));
            clawMachineSolutionsVector->clawSolutions[clawMachineSolutionsVector->size] = solution;
            clawMachineSolutionsVector->size++;

            solution->aPresses = buttonAPresses;
            solution->bPresses = buttonBPresses;
        }

        if (currentX >= clawMachine->xPrize || currentY >= clawMachine->yPrize) {
            buttonAPresses--;
            currentX -= clawMachine->aButtonX;
            currentY -= clawMachine->aButtonY;
        } else {
            buttonBPresses++;
            currentX += clawMachine->bButtonX;
            currentY += clawMachine->bButtonY;
        }
    }
    return clawMachineSolutionsVector;
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
    
    
    int total = 0;
    for (int i = 0; i < num_strings; i += 4) {
        struct ClawMachine *clawMachine = ParseClawMachine(strings[i], strings[i+1], strings[i+2]);
        printf("ClawMachine %d, %d; %d, %d; %d, %d\n", clawMachine->aButtonX, clawMachine->aButtonY, clawMachine->bButtonX, clawMachine->bButtonY, clawMachine->xPrize, clawMachine->yPrize);
        struct ClawMachineSolutionsVector *newClawMachineSolutionsVector = FindClawMachineSolutions(clawMachine);

        int minimumTokens = 10000000;
        for (int j = 0; j < newClawMachineSolutionsVector->size; j++) {
            printf("Solutions Found: A-Press: %d | B-Press: %d | total tokens: %d\n", newClawMachineSolutionsVector->clawSolutions[j]->aPresses, newClawMachineSolutionsVector->clawSolutions[j]->bPresses, newClawMachineSolutionsVector->clawSolutions[j]->aPresses * 3 + newClawMachineSolutionsVector->clawSolutions[j]->bPresses);
            int tokens = newClawMachineSolutionsVector->clawSolutions[j]->aPresses * 3 + newClawMachineSolutionsVector->clawSolutions[j]->bPresses;
            if (tokens < minimumTokens) minimumTokens = tokens;
        }
        if (minimumTokens != 10000000) total += minimumTokens;
        printf("current total: %d\n", total);
    }

    printf("total result: %d\n", total);

    release_all_strings(strings, num_strings);
    return 0;
}