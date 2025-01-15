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

struct Cell {
    int x;
    int y;
};

int FindMapEnd(char **strings) {
    int index = 0;
    while (strings[index][0] == '#') {
        index++;
    }
    return index;
}

int FindDirectionsIndexStart(char **strings) {
    int index = 0;
    while (strings[index][0] != '>' 
        && strings[index][0] != '^'
        && strings[index][0] != '<'
        && strings[index][0] != 'v') {
        index++;
    }
    return index;
}

struct Cell* GetStartingCell(char **strings, int map_num_strings) {
    for (int i = 0; i < map_num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++){
            if (strings[j][i] == '@') {
                struct Cell *cell = malloc(sizeof(struct Cell));
                cell->x = j;
                cell->y = i;
                return cell;
            }
        }
    }
}

bool GetNextCellCharacter(char **strings, int x, int y, char lastCharacter, char direction) {
    printf("strings[y][x]: %c\n", strings[y][x]);
    int currentX = x;
    int currentY = y;

    if (strings[y][x] == '.') {
        strings[y][x] = lastCharacter;
        return true;
    }

    if (direction == '<') {
        currentX--;
    } else if (direction == '>') {
        currentX++;
    } else if (direction == '^') {
        currentY--;
    } else if (direction == 'v') {
        currentY++;
    }

    if (strings[y][x] == '#') {
        return false;
    }



    bool result = GetNextCellCharacter(strings, currentX, currentY, strings[y][x], direction);
    if (result) {
        strings[y][x] = lastCharacter;
        return true;
    }
}

void MoveCell(struct Cell *cell, char **strings, int num_strings, char direction) {
    printf("starting move cell: %c\n", direction);
    int result = GetNextCellCharacter(strings, cell->x, cell->y, '.', direction);
    printf("after get next cell character: %c\n", direction);

    if (!result) {
        return;
    }
    if (direction == '<') {
        cell->x--;
    } else if (direction == '>') {
        cell->x++;
    } else if (direction == '^') {
        cell->y--;
    } else if (direction == 'v') {
        cell->y++;
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
    
    int map_num_strings = FindMapEnd(strings);
    printf("map_num_strings: %d\n", map_num_strings);
    int directions_index_start = FindDirectionsIndexStart(strings);
    printf("directions_index_start: %d\n", directions_index_start);

    struct Cell *cell = GetStartingCell(strings, map_num_strings);

    for (int i = directions_index_start; i < num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++) {
            MoveCell(cell, strings, num_strings, strings[i][j]);
            printf("after move cell\n");
            //print_all_strings(strings, num_strings);
        }
    }

    int result = 0;
    for (int i = 0; i < num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++) {
            if (strings[i][j] == 'O') {
                result += 100 * i + j;
            }
        }
    }
    printf("result: %d\n", result);

    release_all_strings(strings, num_strings);
    return 0;
}