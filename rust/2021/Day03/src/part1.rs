use std::fs;

fn main() {
    println!("Hello, world!");

    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    let mut total = 0;
    let mut last_line: u64 = contents.lines().next().expect("Issue getting next item in list").parse()
        .expect("There was an error parsing a line");

    println!("With text: {contents}");

    for line in contents.lines() {
        let line_int: u64 = line.parse()
            .expect("There was an error parsing a line");
        if line_int > last_line {
            total += 1;
        }
        last_line = line_int;
    }
    println!("total: {total}");
}
