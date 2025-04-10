use std::fs;

struct Board {
    cells: Vec<(u32, usize, usize)>
}

fn main() {
    let file_path = "input_test.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let bingo_numbers_string = contents.lines().into_iter().next().expect("no lines worked");
    let bingo_numbers = bingo_numbers_string.split(',');

    let mut boards: Vec<Board> = vec![];

    let mut first: bool = true;
    let mut row_number = 0;
    let mut current_board: Board = Board {
        cells: vec![]
    };

    for text in contents.lines().skip(2) {
        if first {
            first = !first;
            continue;
        }
        if text.is_empty() {
            row_number = 0;
            boards.push(current_board);
            current_board = Board {
                cells: vec![]
            };
        }
        let row: Vec<&str> = text.split_whitespace().filter(|s| !s.is_empty()).collect();
        for row_n in 0..row.len() {
            current_board.cells.push((row_number, row_n, row[row_n].parse().expect("unable to parse")));
        }
    }
    boards.push(current_board);

    println!("{}", boards.len());
}