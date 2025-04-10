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

struct ConnectionsVector {
    int max_size;
    int size;
    struct ComputersVector **computers;
};

struct ComputersVector {
    int max_size;
    int size;
    int connections_count;
    char ***computers;
};

struct ConnectionsVector* AllocateConnections(char **strings, int num_strings) {
    struct ConnectionsVector *connections = malloc(sizeof(struct ConnectionsVector));
    connections->max_size = num_strings;
    connections->size = num_strings;
    connections->computers = malloc(num_strings * sizeof(struct ComputersVector*));
    for (int i = 0; i < num_strings; i++) {
        struct ComputersVector *computer = malloc(sizeof(struct ComputersVector));
        computer->max_size = 2;
        computer->size = 2;
        computer->connections_count = 2;
        computer->computers = malloc(computer->max_size * sizeof(char**));

        char *token1 = strtok(strings[i], "-");
        computer->computers[0] = malloc(2 * sizeof(char*));
        computer->computers[0][0] = malloc(2 * sizeof(char));
        printf("found this: %s", computer->computers[0][0]);

        strcpy(token1, computer->computers[0][0]);
        

        char *token2 = strtok(NULL, "-");
        computer->computers[0][1] = malloc(2 * sizeof(char));
        strcpy(token2, computer->computers[0][1]);;
        

        connections->computers[i] = computer;
    }
    
    return connections;
}

void FreeConnectionsVector(struct ConnectionsVector *connections) {
    for (int i = 0; i < connections->size; i++) {
        for(int j = 0; j < connections->computers[i]->size; j++) {
            free(connections->computers[i]->computers[j]);
        }
        free(connections->computers[i]->computers);
        free(connections->computers[i]);
    }
    free(connections->computers);
    free(connections);
}

void PrintConnections(struct ConnectionsVector *connections) {
    printf("connections size: %d\n", connections->size);
    printf("first entry: %s", connections->computers[0][0]);
    for (int i = 0; i < connections->size; i++) {
        for (int j = 0; j < connections->computers[i]->size; j++) {
            printf("%s %s", connections->computers[i][0], connections->computers[i][1]);
            printf("\n");
        }
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
    
    struct ConnectionsVector* connections = AllocateConnections(strings, num_strings);
    PrintConnections(connections);

    release_all_strings(strings, num_strings);
    return 0;
}
