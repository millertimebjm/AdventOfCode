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

enum direction {
    direction_none = 0,
    direction_up_left = 1,
    direction_up = 2,
    direction_up_right = 3,
    direction_left = 4,
    direction_right = 5,
    direction_down_left = 6,
    direction_down = 7,
    direction_down_right = 8
};

int find_next_char_recursive(int current_x, int current_y, char current_char, enum direction direction, char** strings, int num_lengths) {
    printf("first check: current_char: %c, strings: %c\n", current_char, strings[current_y][current_x]);
    if (strings[current_y][current_x] != current_char) {
        return 0;
    }
    
    char next_char = 'X';
    switch (current_char)
    {
        case 'X':
            next_char = 'M';
            break;
        case 'M':
            next_char = 'A';
            break;
        case 'A':
            next_char = 'S';
            break;
        case 'S':
            printf("found xmas %c\n", current_char);
            return 1;
        default:
            return 0;
    }

    int count = 0;
    printf("testing %d, %d, %c\n", current_x-1, current_y-1, next_char);
    if ((direction == direction_none || direction == direction_up_left) && current_x > 0 && current_y > 0) {
        count += find_next_char_recursive(current_x -1, current_y -1, next_char, direction_up_left, strings, num_lengths);
    }
    printf("testing %d, %d, %c\n", current_x, current_y-1, next_char);
    if ((direction == direction_none || direction == direction_up) && current_y > 0) {
        count += find_next_char_recursive(current_x, current_y -1, next_char, direction_up, strings, num_lengths);
    }
    printf("testing %d, %d, %c\n", current_x+1, current_y-1, next_char);
    if ((direction == direction_none || direction == direction_up_right) && current_x < strlen(strings[0])-1 && current_y > 0) {
        count += find_next_char_recursive(current_x+1, current_y -1, next_char, direction_up_right, strings, num_lengths);
    }
    
    printf("testing %d, %d, %c\n", current_x-1, current_y, next_char);
    if ((direction == direction_none || direction == direction_left) && current_x > 0) {
        count += find_next_char_recursive(current_x - 1, current_y, next_char, direction_left, strings, num_lengths);
    }
    printf("testing %d, %d, %c\n", current_x+1, current_y, next_char);
    if ((direction == direction_none || direction == direction_right) && current_x < strlen(strings[0])-1) {
        count += find_next_char_recursive(current_x +1, current_y, next_char, direction_right, strings, num_lengths);
    }
    
    printf("testing %d, %d, %c\n", current_x-1, current_y+1, next_char);
    if ((direction == direction_none || direction == direction_down_left) && current_x > 0 && current_y < num_lengths-1) {
        count += find_next_char_recursive(current_x -1, current_y +1, next_char, direction_down_left, strings, num_lengths);
    }
    printf("testing %d, %d, %c\n", current_x, current_y+1, next_char);
    if ((direction == direction_none || direction == direction_down) && current_y < num_lengths-1) {
        count += find_next_char_recursive(current_x, current_y +1, next_char, direction_down, strings, num_lengths);
    }
    printf("testing %d, %d, %c\n", current_x+1, current_y+1, next_char);
    if ((direction == direction_none || direction == direction_down_right) && current_x < strlen(strings[0])-1 && current_y < num_lengths-1) {
        count += find_next_char_recursive(current_x +1, current_y +1, next_char, direction_down_right, strings, num_lengths);
    }
    return count;
}

int find_xmas(int x, int y, char **strings, int num_lengths) {
    if ((strings[y-1][x-1] == 'M' && strings[y+1][x+1] == 'S'
            || strings[y-1][x-1] == 'S' && strings[y+1][x+1] == 'M')
        && (strings[y-1][x+1] == 'M' && strings[y+1][x-1] == 'S'
            || strings[y-1][x+1] == 'S' && strings[y+1][x-1] == 'M')) {
        return 1;
    }
    return 0;
}

int main() {
    int num_strings = 0;
    char **strings = read_strings_from_filename("input.txt", &num_strings);

    if (num_strings == 0) {
        printf("strings is null or num_strings is 0");
        return 1;
    }

    printf("number of strings: %d\n", num_strings);
    print_all_strings(strings, num_strings);

    enum direction direction = direction_none;
    int count = 0;
    // for (int i = 0; i < num_strings; i++) {
    //     for (int j = 0; j < strlen(strings[i]); j++) {
    //         printf("for loop: %d, %d, %c\n", j, i, strings[i][j]);
    //         fflush(stdout);
    //         count += find_next_char_recursive(j, i, 'X', direction, strings, num_strings);
    //         // char charVariable;
    //         // scanf("%c", &charVariable);
    //         printf("total: %d\n\n", count);
    //     }
    // }
    // printf("count: %d\n", count);

    count = 0;
    for (int i = 1; i < num_strings-1; i++) {
        for (int j = 1; j < strlen(strings[i])-1; j++) {
            // printf("for loop: %d, %d, %c\n", j, i, strings[i][j]);
            // fflush(stdout);
            if (strings[i][j] == 'A') {
                count += find_xmas(j, i, strings, num_strings);
            }
            // char charVariable;
            // scanf("%c", &charVariable);
        }
    }
    printf("count: %d\n", count);


    release_all_strings(strings, num_strings);
    printf("number of lines: %d\n", num_strings);
}