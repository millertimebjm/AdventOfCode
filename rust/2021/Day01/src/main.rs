use std::fs;

fn main() {
    println!("Hello, world!");

    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    let mut total = 0;

    println!("With text: {contents}");
let number_vec: Vec<i64> = contents
        .lines()
        .map(|line| line.trim().parse::<i64>().unwrap())
        .collect();
    let mut index = 0;
    for number in 3..number_vec.len() {
        println!("number is: {number}, index is: {index}");
        if number_vec[number-3] + number_vec[number-2] + number_vec[number-1]
            < number_vec[number-2] + number_vec[number-1] + number_vec[number] {
            total += 1;
        }
        index += 1;
    }
    println!("total: {total}");
}
