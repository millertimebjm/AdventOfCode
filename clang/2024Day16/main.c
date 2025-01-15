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
    int minimumCost;
};

struct CellVector {
    int max_size;
    int size;
    struct Cell **cells;
};

struct Cell* FindStartingCell(char **strings, int num_strings) {
    for (int i = 0; i < num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++) {
            if (strings[i][j] == 'S') {
                struct Cell *currentCell = malloc(sizeof(struct Cell));
                currentCell->x = j;
                currentCell->y = i;
                currentCell->minimumCost = 0;
                return currentCell;
            }
        }
    }
}

struct Cell* FindEndingCell(char **strings, int num_strings) {
    for (int i = 0; i < num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++) {
            if (strings[i][j] == 'E') {
                struct Cell *currentCell = malloc(sizeof(struct Cell));
                currentCell->x = j;
                currentCell->y = i;
                currentCell->minimumCost = 0;
                return currentCell;
            }
        }
    }
}

struct Cell* Exists(struct CellVector *cellVector, struct Cell *cell) {
    printf("exists\n");
    printf("getting cell info in exists: %d, %d, %d\n", cell->x, cell->y, cell->minimumCost);
    for (int i = 0; i < cellVector->size; i++) {
        printf("internal cell info %d, %d\n", cellVector->cells[i]->x, cellVector->cells[i]->y);
        if (cellVector->cells[i]->x == cell->x && cellVector->cells[i]->y == cell->y)
            return cellVector->cells[i];
    }
    return NULL;
}

struct Cell* Remove(struct CellVector *cellVector, struct Cell *cell) {
    return NULL;   
}

struct Cell* GetMinimumCostCell(struct CellVector *cellVector) {
    struct Cell *cell = cellVector->cells[0];
    int minimumCost = cell->minimumCost;
    for (int i = 1; i < cellVector->size; i++) {
        if (cellVector->cells[i]->minimumCost < minimumCost)
            cell = cellVector->cells[i];
    }
    return cell;
}

void release_all_cell_vector(struct CellVector *cellVector) {
    for (int i = 0; i < cellVector->size; i++) {
        free(cellVector->cells[i]);
    }
    free(cellVector->cells);
    free(cellVector);
}

int ConvertDirectionToInt(char direction) {
    if (direction == '>') return 0;
    if (direction == 'v') return 1;
    if (direction == '<') return 2;
    if (direction == '^') return 3;
}

int GetTurnCost(char oldDirection, char newDirection) {
    int result = ConvertDirectionToInt(oldDirection) - ConvertDirectionToInt(newDirection);
    result = abs(result);
    if (result == 0) return 0;
    if (result == 1 || result == 3) return 1000;
    if (result == 2) return 2000; 
}

void AddToCellVector(struct CellVector *cellVector, int x, int y, int minimumCost) {
    struct Cell *newCell = malloc(sizeof(struct Cell));
    newCell->x = x;
    newCell->y = y;
    newCell->minimumCost = minimumCost;
    if (Exists(cellVector, newCell) == NULL) {
        cellVector->cells[cellVector->size] = newCell;
        cellVector->size++;
    }
}

void AddCellsToReview(struct CellVector *cellsToReview, struct Cell *minimumCostCell, char **strings, int num_strings, char direction) {
    int turnCost = 0;
    if (strings[minimumCostCell->y][minimumCostCell->x+1] == '.' || strings[minimumCostCell->y][minimumCostCell->x+1] == 'E') {
        turnCost = GetTurnCost(direction, '>');
        AddToCellVector(cellsToReview, minimumCostCell->x+1, minimumCostCell->y, minimumCostCell->minimumCost + turnCost + 1);
    }
    else if (strings[minimumCostCell->y][minimumCostCell->x-1] == '.' || strings[minimumCostCell->y][minimumCostCell->x-1] == 'E') {
        turnCost = GetTurnCost(direction, '<');
        AddToCellVector(cellsToReview, minimumCostCell->x-1, minimumCostCell->y, minimumCostCell->minimumCost + turnCost + 1);
    }
    else if (strings[minimumCostCell->y+1][minimumCostCell->x] == '.' || strings[minimumCostCell->y+1][minimumCostCell->x] == 'E') {
        turnCost = GetTurnCost(direction, 'v');
        AddToCellVector(cellsToReview, minimumCostCell->x, minimumCostCell->y+1, minimumCostCell->minimumCost + turnCost + 1);
    }
    else if (strings[minimumCostCell->y-1][minimumCostCell->x] == '.' || strings[minimumCostCell->y-1][minimumCostCell->x] == 'E') {
        turnCost = GetTurnCost(direction, '^');
        AddToCellVector(cellsToReview, minimumCostCell->x, minimumCostCell->y-1, minimumCostCell->minimumCost + turnCost + 1);
    }
}

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input_test.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0\n");
        return 1;
    }

    printf("number of strings: %d\n", num_strings);
    print_all_strings(strings, num_strings);
    
    int max_size = num_strings * strlen(strings[0]); 

    struct CellVector *cellsToReview = malloc(sizeof(struct CellVector));
    cellsToReview->max_size = max_size;
    cellsToReview->size = 0;
    cellsToReview->cells = malloc(cellsToReview->max_size * sizeof(struct Cell*));

    struct CellVector *cellsMinimumCost = malloc(sizeof(struct CellVector));
    cellsToReview->max_size = max_size;
    cellsToReview->size = 0;
    cellsToReview->cells = malloc(cellsToReview->max_size * sizeof(struct Cell*));

    struct Cell *startingCell = FindStartingCell(strings, num_strings);
    struct Cell *endingCell = FindEndingCell(strings, num_strings);
    char facingDirection = '>';

    printf("starting cell: %d, %d\n", startingCell->x, startingCell->y);
    printf("ending cell: %d, %d\n", endingCell->x, endingCell->y);


    AddCellsToReview(cellsToReview, startingCell, strings, num_strings, '>');
    printf("after first cells to review\n");
    while (cellsToReview->size > 0
        && Exists(cellsMinimumCost, endingCell) == NULL) {
        printf("get minimum cost cell\n");
        struct Cell *minimumCostCell = GetMinimumCostCell(cellsMinimumCost);
        Remove(cellsMinimumCost, minimumCostCell);
        AddCellsToReview(cellsToReview, minimumCostCell, strings, num_strings, facingDirection);
    }

    struct Cell *minimumCostEndingCell = Exists(cellsMinimumCost, endingCell);
    printf("minimum cost ending cell: %d\n", minimumCostEndingCell->minimumCost);

    release_all_cell_vector(cellsToReview);
    release_all_cell_vector(cellsMinimumCost);
    free(startingCell);

    release_all_strings(strings, num_strings);
    return 0;
}