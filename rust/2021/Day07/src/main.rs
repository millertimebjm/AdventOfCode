use std::fs;

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut crabs: Vec<i32> = vec![];

    for text in contents.lines() {
        for number_string in text.split(",") {
            crabs.push(number_string.parse().expect(""));
        }
    }

    let min = get_min(&crabs);
    let max = get_max(&crabs);

    let mut minimum_position = min-1;
    let mut minimum_position_count = 2000000000;
    for index in 0..(max - min) {
        let temp_position_count = get_position_count(&crabs, min + index);
        if temp_position_count < minimum_position_count {
            minimum_position = min + index;
            minimum_position_count = temp_position_count;
        }
        println!();
    }
    println!("position is {minimum_position} for a count of {minimum_position_count}");
}

fn get_position_count(crabs: &Vec<i32>, position: i32) -> i32 {
    let mut total: i32 = 0;
    for index in 0..crabs.len() {
        let mut difference = crabs[index].abs_diff(position).try_into().unwrap_or(0);
        for index in 0..(difference) {
            difference = difference + index;
        }
        total = total + difference;
        println!("position: {position}, current number {}, difference: {}, current total: {}", crabs[index], crabs[index].abs_diff(position).try_into().unwrap_or(0), total);
    }
    total
}

fn get_min(crabs: &Vec<i32>) -> i32 {
    let mut min = 100000;
    for index in 0..crabs.len() {
        if crabs[index] < min {
            min = crabs[index];
        }
    }
    min
}

fn get_max(crabs: &Vec<i32>) -> i32 {
    let mut max = -100000;
    for index in 0..crabs.len() {
        if crabs[index] > max {
            max = crabs[index];
        }
    }
    max
}