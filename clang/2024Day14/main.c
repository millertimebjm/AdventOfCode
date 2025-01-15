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

struct Robot {
    int velocityX;
    int velocityY;
    int cellX;
    int cellY;
};

struct RobotsVector {
    struct Robot **robots;
    int size;
    int max_size;
    int maxX;
    int maxY;
};

struct RobotsVector* AllocateRobotsVector(int maxX, int maxY) {
    struct RobotsVector *robotsVector = malloc(sizeof(struct RobotsVector));
    robotsVector->max_size = 500;
    robotsVector->size = 0;
    robotsVector->robots = malloc(500 * sizeof(struct Robot*));
    robotsVector->maxX = 11;
    robotsVector->maxY = 7;
    return robotsVector;
}

void AddRobotToVector(struct RobotsVector *robotsVector, struct Robot *robot) {
    robotsVector->robots[robotsVector->size] = robot;
    robotsVector->size++;
}

int ConvertNumericsToInt(char *string) {
    int result = 0;
    bool isNegative = false;
    for (int i = 0; i < strlen(string); i++) {
        if (string[i] == '-') {
            isNegative = true;
        }
        if (string[i] >= 48 && string[i] < 58) result = 10 * result + (string[i] - 48);
    }
    if (isNegative) result *= -1;
    return result;
}

void ImportRobotsVector(struct RobotsVector *robotsVector, char **strings, int num_strings) {
    for (int i = 0; i < num_strings; i++) {
        struct Robot *robot = malloc(sizeof(struct Robot));
        
        char *positionString = strtok(strings[i], " ");
        char *velocityString = strtok(NULL, " ");

        char *xPositionString = strtok(positionString, ",");
        char *yPositionString = strtok(NULL, ",");
        robot->cellX = ConvertNumericsToInt(xPositionString);
        robot->cellY = ConvertNumericsToInt(yPositionString);

        char *xVelocityString = strtok(velocityString, ",");
        char *yVelocityString = strtok(NULL, ",");
        robot->velocityX = ConvertNumericsToInt(xVelocityString);
        robot->velocityY = ConvertNumericsToInt(yVelocityString);

        AddRobotToVector(robotsVector, robot);
    }
}

void ReleaseRobotsVector(struct RobotsVector *robotsVector) {
    for (int i = 0; i < robotsVector->size; i++) {
        free(robotsVector->robots[i]);
    }
    free(robotsVector);
}

void MoveRobot(struct Robot *robot, int maxX, int maxY) {
    robot->cellX += robot->velocityX;
    if (robot->cellX > maxX) robot->cellX = robot->cellX % maxX;
    if (robot->cellX < 0) while (robot->cellX < 0) robot->cellX += maxX;

    robot->cellY += robot->velocityY;
    if (robot->cellY > maxY) robot->cellY = robot->cellY % maxY;
    if (robot->cellY < 0) while (robot->cellY < 0) robot->cellY += maxY;
}
 
void MoveRobots(struct RobotsVector *robotsVector) {
    for (int i = 0; i < robotsVector->size; i++) {
        MoveRobot(robotsVector->robots[i], robotsVector->maxX, robotsVector->maxY);
    }
}

int GetQuadrantCount(struct RobotsVector *robotsVector) {
    long quadrant1 = 0;
    long quadrant2 = 0;
    long quadrant3 = 0;
    long quadrant4 = 0;
    for (int i = 0; i < robotsVector->size; i++) {
        struct Robot *robot = robotsVector->robots[i];
        if (robot->cellX >= 0 && robot->cellX <= robotsVector->maxX / 2 - 1
            && robot->cellY >= 0 && robot->cellY <= robotsVector->maxY / 2 - 1) quadrant1++;
        if (robot->cellX >= robotsVector->maxX / 2 + 1 && robot->cellX < robotsVector->maxX
            && robot->cellY >= 0 && robot->cellY <= robotsVector->maxY / 2 - 1) quadrant2++;

        if (robot->cellX >= 0 && robot->cellX <= robotsVector->maxX / 2 - 1
            && robot->cellY >= robotsVector->maxY / 2 + 1 && robot->cellY < robotsVector->maxY) quadrant3++;
        if (robot->cellX >= robotsVector->maxX / 2 + 1 && robot->cellX < robotsVector->maxX
            && robot->cellY >= robotsVector->maxY / 2 + 1 && robot->cellY < robotsVector->maxY) quadrant4++;
    }
    printf("%ld, %ld, %ld, %ld\n", quadrant1, quadrant2, quadrant3, quadrant4);
    return quadrant1 * quadrant2 * quadrant3 * quadrant4;
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
    
    struct RobotsVector *robotsVector = AllocateRobotsVector(11, 7);
    ImportRobotsVector(robotsVector, strings, num_strings);

    for (int i = 0; i < robotsVector->size; i++) {
        printf(" (%d,%d|%d,%d) ", robotsVector->robots[i]->cellX,robotsVector->robots[i]->cellY,robotsVector->robots[i]->velocityX,robotsVector->robots[i]->velocityY);
    }
    printf("\n");

    for (int i = 0; i < 100; i++) {
        MoveRobots(robotsVector);
        for (int i = 0; i < robotsVector->size; i++) {
        printf(" (%d,%d|%d,%d) ", robotsVector->robots[i]->cellX,robotsVector->robots[i]->cellY,robotsVector->robots[i]->velocityX,robotsVector->robots[i]->velocityY);
    }
    printf("\n");
    }

    for (int i = 0; i < robotsVector->size; i++) {
        printf(" (%d,%d|%d,%d) ", robotsVector->robots[i]->cellX,robotsVector->robots[i]->cellY,robotsVector->robots[i]->velocityX,robotsVector->robots[i]->velocityY);
    }
    printf("\n");

    long result = GetQuadrantCount(robotsVector);
    printf("result: %d\n", result);

    ReleaseRobotsVector(robotsVector);

    release_all_strings(strings, num_strings);
    return 0;
}