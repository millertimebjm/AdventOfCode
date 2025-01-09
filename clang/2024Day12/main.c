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

struct Plant {
    char character;
    int area;
    int perimeter;
    struct Cell **cells;
    int cells_size;
    int cells_max_size;
};

struct PlantVector {
    struct Plant **plants;
    int max_size;
    int size;
};

void RecursivePlantSearch(struct Plant* plant, int x, int y, char **strings, int num_strings);

void AddToArea(char character, struct Plant *plant) {
    plant->area++;
}

void AddToPerimeterAndRecurse(char character, int x, int y, struct Plant *plant, char **strings, int num_strings) {
    if (y == 0 
        || strings[y-1][x] != character) {
            plant->perimeter++;
        }
    if (y > 0 && strings[y-1][x] == character) {
        RecursivePlantSearch(plant, x, y-1, strings, num_strings);
    }

    if (y == num_strings -1
        || strings[y+1][x] != character) {
            plant->perimeter++;
        }
    if (y < num_strings -1 && strings[y+1][x] == character) {
        RecursivePlantSearch(plant, x, y+1, strings, num_strings);
    }

    if (x == 0
        || strings[y][x-1] != character) {
            plant->perimeter++;
        }
    if (x > 0 && strings[y][x-1] == character) {
        RecursivePlantSearch(plant, x-1, y, strings, num_strings);
    }

    if (x == strlen(strings[0]) -1
        || strings[y][x+1] != character) {
            plant->perimeter++;
        }
    if (x < strlen(strings[0]) -1 && strings[y][x+1] == character) {
        RecursivePlantSearch(plant, x+1, y, strings, num_strings);
    }
}

bool GetIsExistingPlant(struct PlantVector *plantVector, char character, int x, int y) {
    for (int i = 0; i < plantVector->size; i++) {
        if (plantVector->plants[i]->character == character) {
            struct Plant *plant = plantVector->plants[i];
            for (int j = 0; j < plant->cells_size; j++) {
                if (plant->cells[j]->x == x && plant->cells[j]->y == y) {
                    return true;
                }
            }
        }
    }
    return false;
}

void RecursivePlantSearch(struct Plant* plant, int x, int y, char **strings, int num_strings) {
    printf("checking plant %c %d,%d with area %d\n", strings[y][x], x, y, plant->area);
    if (plant->character == strings[y][x]) {
        for (int i = 0; i < plant->cells_size; i++) {
            if (plant->cells[i]->x == x && plant->cells[i]->y == y) {
                return;
            }
        }
        struct Cell *cell = malloc(sizeof(struct Cell));
        cell->x = x;
        cell->y = y;

        plant->cells[plant->cells_size] = cell; 
        plant->cells_size++;

        AddToArea(plant->character, plant);
        AddToPerimeterAndRecurse(plant->character, x, y, plant, strings, num_strings);
    }
}

void release_all_plants(struct PlantVector *plantVector) {
    for (int i = 0; i < plantVector->size; i++) {
        for (int j = 0; j < plantVector->plants[i]->cells_size; j++) {
            free(plantVector->plants[i]->cells[j]);
        }
        free(plantVector->plants[i]->cells);
        free(plantVector->plants[i]);
    }
    free(plantVector->plants);
    free(plantVector);
}

// struct Plant* GetPlant(char character, struct PlantVector *plantVector) {
//     for (int i = 0; i < plantVector->size; i++) {
//         if (plantVector->plants[i]->character == character) {
//             return plantVector->plants[i];
//         }
//     }

//     struct Plant *plant = malloc(sizeof(struct Plant*));
//     plantVector->plants[plantVector->size] = plant;
//     plantVector->size++;
    
//     plant->character = character;
//     plant->area = 1;
//     plant->perimeter = 0;
    
//     return plant;
// }

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0\n");
        return 1;
    }

    printf("number of strings: %d\n", num_strings);
    print_all_strings(strings, num_strings);
    
    struct PlantVector *plantVector = malloc(sizeof(struct PlantVector));
    plantVector->max_size = 1000;
    plantVector->size = 0;
    plantVector->plants = malloc(plantVector->max_size * sizeof(struct Plant*));

    for (int i = 0; i < num_strings; i++) {
        for (int j = 0; j < strlen(strings[i]); j++) {
            //struct Plant *plant = GetPlant(strings[j][i], j, i, plantVector);
            bool isExistingPlant = GetIsExistingPlant(plantVector, strings[i][j], j, i);
            if (!isExistingPlant) {
                struct Plant *plant = malloc(sizeof(struct Plant));
                plantVector->plants[plantVector->size] = plant;
                plantVector->size++;

                plant->character = strings[i][j];
                plant->cells_max_size = 1000;
                plant->cells_size = 0;
                plant->area = 0;
                plant->perimeter = 0;
                plant->cells = malloc(plant->cells_max_size * sizeof(struct Cell));

                RecursivePlantSearch(plant, j, i, strings, num_strings);
            }
        }
    }

    long total = 0;
    for (int i = 0; i < plantVector->size; i++) {
        printf("Plant %c has area %d and perimeter %d\n", plantVector->plants[i]->character, plantVector->plants[i]->area, plantVector->plants[i]->perimeter);
        total += plantVector->plants[i]->area * plantVector->plants[i]->perimeter;
    }

    printf("total cost: %ld\n", total);

    release_all_plants(plantVector);

    release_all_strings(strings, num_strings);
    return 0;
}