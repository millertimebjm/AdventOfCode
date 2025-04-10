use std::fs;

fn main() {
    let file_path = "input_test.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }
    let mut length: usize = 0;
    for line in contents.lines() {
        length = line.len();
        break;
    }
    println!("length: {length}");

    for n in 0..length {
        let mut sum: usize = 0;
        for text in contents.lines() {
            sum += text[n].parse();
        }
        println!("sum: {sum}");
    }
}
