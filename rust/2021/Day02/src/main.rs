use std::fs;

fn main() {
    println!("Hello, world!");

    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    println!("With text: {contents}");
    let mut vec_direction: Vec<String> = Vec::new();
    let mut vec_magnitude: Vec<i64> = Vec::new();

    for text in contents.lines() {
        let mut line_parts = text.split_whitespace();
        vec_direction.push(line_parts.next().unwrap().to_string());
        vec_magnitude.push(line_parts.next().unwrap().parse().expect("Line magnitude couldn't be parsed"));
    }
    for index in 0..vec_direction.len() {
        println!("row {index} has {} and magnitude {}", vec_direction[index],vec_magnitude[index]);
    }

    let mut horizontal: i64 = 0;
    let mut vertical: i64 = 0;
    let mut aim: i64 = 0;

    for index in 0..vec_direction.len() {
        if vec_direction[index] == "forward" {
            horizontal += vec_magnitude[index];
            vertical += vec_magnitude[index] * aim;
        }
        if vec_direction[index] == "up" {
            aim -= vec_magnitude[index];
        }
        if vec_direction[index] == "down" {
            aim += vec_magnitude[index];
        }
        println!("horizontal {horizontal} vertical {vertical} aim {aim}");
    }

    println!("answer is {}", horizontal * vertical);
}
