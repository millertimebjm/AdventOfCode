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

struct PathWalker {
    int max_size;
    int size;
    int elevation;
    struct Cell **cells;
};

void ReleasePathWalker(struct PathWalker *pathWalker) {
    for (int i = 0; i < pathWalker->size; i++) {
        free(pathWalker->cells[i]);
    }
    free(pathWalker->cells);
    free(pathWalker);
}

struct PathWalker* AllocatePathWalker(int elevation) {
    struct PathWalker *pathWalker = malloc(sizeof(struct PathWalker));
    if (!pathWalker) {
        perror("failed to allocate pathwalker");
        return NULL;
    }

    pathWalker->max_size = 100;
    pathWalker->cells = malloc(pathWalker->max_size * sizeof(struct Cell*));
    pathWalker->size = 0;
    pathWalker->elevation = elevation;
    return pathWalker;
}

void AddUniquePathWalkerCell(struct PathWalker *pathWalker, int y, int x) {
    printf("adding new cell %d, %d\n", x, y);
    if (pathWalker->size == pathWalker->max_size) {
        pathWalker->max_size = pathWalker->max_size * 2;
        pathWalker->cells = realloc(pathWalker->cells, pathWalker->max_size * sizeof(struct Cell*));
    }
    for (int i = 0; i < pathWalker->size; i++) {
        if (pathWalker->cells[i]->x == x && pathWalker->cells[i]->y == y) return;
    }
    pathWalker->cells[pathWalker->size] = malloc(sizeof(struct Cell));
    pathWalker->cells[pathWalker->size]->x = x;
    pathWalker->cells[pathWalker->size]->y = y;
    pathWalker->size++;
}

void AddPathWalkerCell(struct PathWalker *pathWalker, int y, int x) {
    printf("adding new cell %d, %d\n", x, y);
    if (pathWalker->size == pathWalker->max_size) {
        pathWalker->max_size = pathWalker->max_size * 2;
        pathWalker->cells = realloc(pathWalker->cells, pathWalker->max_size * sizeof(struct Cell*));
    }
    // for (int i = 0; i < pathWalker->size; i++) {
    //     if (pathWalker->cells[i]->x == x && pathWalker->cells[i]->y == y) return;
    // }
    pathWalker->cells[pathWalker->size] = malloc(sizeof(struct Cell));
    pathWalker->cells[pathWalker->size]->x = x;
    pathWalker->cells[pathWalker->size]->y = y;
    pathWalker->size++;
}

struct PathWalker* FindNextElevation(char **strings, int num_strings, struct PathWalker* pathWalker) {
    int elevationToLookFor = pathWalker->elevation + 1;
    printf("looking for elevation %d, with the number of cells: %d\n", elevationToLookFor, pathWalker->size, num_strings);
    struct PathWalker *newPathWalker = AllocatePathWalker(elevationToLookFor);
    for (int i = 0; i < pathWalker->size; i++) {
        if (pathWalker->cells[i]->y < num_strings - 1
            && strings[pathWalker->cells[i]->y+1][pathWalker->cells[i]->x] -48 == elevationToLookFor) {
            AddPathWalkerCell(newPathWalker, pathWalker->cells[i]->y+1, pathWalker->cells[i]->x);
        }
        if (pathWalker->cells[i]->x < strlen(strings[0]) - 1
            && strings[pathWalker->cells[i]->y][pathWalker->cells[i]->x+1] -48 == elevationToLookFor) {
            AddPathWalkerCell(newPathWalker, pathWalker->cells[i]->y, pathWalker->cells[i]->x+1);
        }
        if (pathWalker->cells[i]->x > 0
            && strings[pathWalker->cells[i]->y][pathWalker->cells[i]->x-1] -48 == elevationToLookFor) {
            AddPathWalkerCell(newPathWalker, pathWalker->cells[i]->y, pathWalker->cells[i]->x-1);
        }
        if (pathWalker->cells[i]->y > 0
            && strings[pathWalker->cells[i]->y-1][pathWalker->cells[i]->x] -48 == elevationToLookFor) {
            AddPathWalkerCell(newPathWalker, pathWalker->cells[i]->y-1, pathWalker->cells[i]->x);
        }
    }
    return newPathWalker;
}

int FindAllSummits(char **strings, int num_strings, int i, int j)
{
    printf("Find all summits (%d, %d)\n", j, i);
    struct PathWalker *pathWalker = AllocatePathWalker(0);
    AddUniquePathWalkerCell(pathWalker, i, j);

    while (pathWalker->elevation < 9) {
        struct PathWalker* oldPathWalker = pathWalker;
        pathWalker = FindNextElevation(strings, num_strings, pathWalker);
        ReleasePathWalker(oldPathWalker);
    }
    int size = pathWalker->size;
    ReleasePathWalker(pathWalker);
    return size;
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
    
    
    int trailheadSummits = 0;

    for (int i = 0; i < num_strings; i++)
    {
        for (int j = 0; j < strlen(strings[i]); j++)
        {
            if (strings[i][j] == '0')
            {
                trailheadSummits += FindAllSummits(strings, num_strings, i, j);
                printf("next trailhead summits count: %d\n", trailheadSummits);
            }
        }
    }

    printf("total: %d\n", trailheadSummits);

    release_all_strings(strings, num_strings);
    return 0;
}