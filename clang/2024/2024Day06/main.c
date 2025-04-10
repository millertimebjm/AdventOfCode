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

struct cell {
    int x;
    int y;
    char direction;
};

void find_guard_cell(struct cell *guard_cell, char **strings, int num_strings) {
    for (int y = 0; y < num_strings; y++) {
        for (int x = 0; x < strlen(strings[0]); x++) {
            if (strings[y][x] != '.' && strings[y][x] != '#') {
                guard_cell->x = x;
                guard_cell->y = y;
                guard_cell->direction = strings[y][x];
            }
        }
    }
}

void add_travelled_cell(struct cell *guard_cell, struct cell **travelled_cells, int *travelled_cells_count) {
    bool found = false;
    printf("cells travelled: %d\n", (*travelled_cells_count));
    for (int i = 0; i < (*travelled_cells_count); i++) {
        if (travelled_cells[i]->x == guard_cell->x 
            && travelled_cells[i]->y == guard_cell->y) found = true;
    }
    if (found) {
        printf("already found.\n");
    }
    if (!found) {
        travelled_cells[*travelled_cells_count] = (struct cell*)malloc(sizeof(struct cell));
        travelled_cells[*travelled_cells_count]->x = guard_cell->x;
        travelled_cells[*travelled_cells_count]->y = guard_cell->y;
        (*travelled_cells_count)++;
    }
}

bool add_travelled_cell_with_direction(struct cell *guard_cell, struct cell **travelled_cells, int *travelled_cells_count) {
    printf("cells travelled: %d\n", (*travelled_cells_count));
    for (int i = 0; i < (*travelled_cells_count); i++) {
        if (travelled_cells[i]->x == guard_cell->x 
            && travelled_cells[i]->y == guard_cell->y
            && travelled_cells[i]->direction == guard_cell->direction) return true;
    }

    travelled_cells[*travelled_cells_count] = (struct cell*)malloc(sizeof(struct cell));
    travelled_cells[*travelled_cells_count]->x = guard_cell->x;
    travelled_cells[*travelled_cells_count]->y = guard_cell->y;
    travelled_cells[*travelled_cells_count]->direction = guard_cell->direction;
    (*travelled_cells_count)++;
    return false;
}

bool get_next_guard_cell(struct cell *guard_cell, char **strings, int num_strings) {
    int x = guard_cell->x;
    int y = guard_cell->y;
    char direction = guard_cell->direction;
    if (direction == '^') y--;
    if (direction == '>') x++;
    if (direction == 'v') y++;
    if (direction == '<') x--;

    if (x < 0 
        || y < 0
        || x >= strlen(strings[0])
        || y >= num_strings) {
            return false;
        }

    if (strings[y][x] == '#') {
        if (direction == '^') direction = '>';
        else if (direction == '>') direction = 'v';
        else if (direction == 'v') direction = '<';
        else if (direction == '<') direction = '^';
        printf("new guard direction %c\n", direction);
        guard_cell->direction = direction;
    } else {
        guard_cell->x = x;
        guard_cell->y = y;
        guard_cell->direction = direction;
    }

    return true;
}

bool check_for_loop(struct cell *guard_cell, char **strings, int num_strings) {
    struct cell **travelled_cells = malloc(num_strings * strlen(strings[0]) * sizeof(struct cell*));
    int found_next_cell = true;
    int travelled_cells_count = 0;
    while (found_next_cell) {
        printf("current guard cell: (%d, %d) direction: %c\n", guard_cell->x, guard_cell->y, guard_cell->direction);
        bool loop_found = add_travelled_cell_with_direction(guard_cell, travelled_cells, &travelled_cells_count);
        if (loop_found) {
            free(travelled_cells);
            return true;
        }
        found_next_cell = get_next_guard_cell(guard_cell, strings, num_strings);
    }
    free(travelled_cells);
    return false;
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

    struct cell **travelled_cells = malloc(num_strings * strlen(strings[0]) * sizeof(struct cell*));
    int travelled_cells_count = 0;
    struct cell *guard_cell = malloc(sizeof(struct cell));

    find_guard_cell(guard_cell, strings, num_strings);
    int found_next_cell = true;
    while (found_next_cell) {
        printf("current guard cell: (%d, %d) direction: %c\n", guard_cell->x, guard_cell->y, guard_cell->direction);
        add_travelled_cell(guard_cell, travelled_cells, &travelled_cells_count);
        found_next_cell = get_next_guard_cell(guard_cell, strings, num_strings);
    }
    printf("total cells travelled: %d\n\n", travelled_cells_count);

    int count_loops = 0;
    find_guard_cell(guard_cell, strings, num_strings);
    for (int i = 1; i < travelled_cells_count; i++) {
        strings[travelled_cells[i]->y][travelled_cells[i]->x] = '#';
        bool is_loop = check_for_loop(guard_cell, strings, num_strings);
        strings[travelled_cells[i]->y][travelled_cells[i]->x] = '.';
        if (is_loop) {
            count_loops++;
            printf("loop found\n");
        } else {
            printf("loop not found\n");
        }
        find_guard_cell(guard_cell, strings, num_strings);
    }
    printf("number of loops: %d\n", count_loops);

    release_all_strings(strings, num_strings);
    return 0;
}