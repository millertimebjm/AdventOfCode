use std::fs;

fn main() {
    let file_path = "input_test.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut caves: Vec<Vec<i32>> = vec![];
    for text in contents.lines() {
        let mut cave: Vec<i32> = vec![];
        for char in text.chars() {
            cave.push(char as i32 - 48);
        }
        caves.push(cave);
    }

    println!();
    let mut total = 0;
    for y in 0..caves.len() {
        for x in 0..caves[y].len() {
            let is_lowest_height = check_cave_height(&caves, x.try_into().unwrap(), y.try_into().unwrap());
            if (is_lowest_height) {
                println!("found lowest point: {}", caves[y][x]);
                total = total + caves[y][x] + 1;
            }
        }
    }

    println!("final value is: {total}");

}

fn check_cave_height(caves: &Vec<Vec<i32>>, x: usize, y: usize) -> bool {
    let current_height = caves[y][x];
    if x > 0 && caves[y][x-1] < current_height {
        return false;
    }
    if x < caves[y].len()-1 && caves[y][x+1] < current_height {
        return false;
    }
    if y > 0 && caves[y-1][x] < current_height {
        return false;
    }
    if y < caves.len()-1 && caves[y+1][x] < current_height {
        return false;
    }
    true
}