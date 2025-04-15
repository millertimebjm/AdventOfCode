use std::fs;

struct Board {
    cells: Vec<(u32, u32, u32, bool)>
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let bingo_numbers_string = contents.lines().into_iter().next().expect("no lines worked");
    let bingo_numbers = bingo_numbers_string.split(',');

    let mut boards: Vec<Board> = vec![];

    let mut row_number = 0;
    let mut current_board: Board = Board {
        cells: vec![]
    };

    for text in contents.lines().skip(2) {
        if text.is_empty() {
            row_number = 0;
            boards.push(current_board);
            current_board = Board {
                cells: vec![]
            };
        }
        let row: Vec<&str> = text.split_whitespace().filter(|s| !s.is_empty()).collect();
        for row_n in 0..row.len() {
            current_board.cells.push((row_number, row_n as u32, row[row_n].parse().expect("unable to parse"), false));
        }
        if !text.is_empty() {
            row_number += 1;
        }
    }
    boards.push(current_board);

    println!("{}", boards.len());

    println!("");
    for board in &boards {
        for cell in &board.cells {
            println!("({}, {}, {}, {})", cell.0, cell.1, cell.2, cell.3);
        }
    }

    let mut is_bingo: bool = false;
    for called_number_string in bingo_numbers {
        println!("checking called number: {}", called_number_string);
        let called_number_int:u32 = called_number_string.parse().expect("");
        //let mut called_number: u32 = called_number_string.parse().expect("");
        for board in &mut boards {
            mark_bingo_card(board, called_number_int);
            is_bingo = check_for_bingo(board);
            if is_bingo {
                //first_board_bingo = &board;

                is_bingo_fn(board);
                let unmarked_sum: u32 = get_unmarked_sum(&board);
                println!("unmarked_sum total is: {unmarked_sum}, last number is {called_number_int}, total is: {}", unmarked_sum * called_number_int);
                break;
            }
        }
        if is_bingo {
            break;
        }
    }
    
}

fn is_bingo_fn(board: &Board) {
    println!("Found Bingo!");

    for cell in &board.cells {
        println!("({}, {}, {}, {})", cell.0, cell.1, cell.2, cell.3);
    }
}

fn get_unmarked_sum(board: &Board) -> u32 {
    let mut unmarked_sum: u32 = 0;
    for cell in board.cells.iter() {
        if !cell.3 {
            println!("Unmarked Sum is now: {unmarked_sum}");
            unmarked_sum += cell.2;
        }
    }
    unmarked_sum
}

fn check_for_bingo(board: &Board) -> bool {
    for n in 0..5 {
        let mut found_bingo_row = true;
        let mut found_bingo_column = true;
        for cell in board.cells.iter() {
            println!("checking {n} and found {}, found_row {}, found_column {}", cell.3, found_bingo_row, found_bingo_column);
            if cell.0 == n {
                found_bingo_row = found_bingo_row && cell.3;
            }
            if cell.1 == n {
                found_bingo_column = found_bingo_column && cell.3;
            }
        }
        if found_bingo_row || found_bingo_column {
            return true;
        }
    }
    
    return false;
}

fn mark_bingo_card(board: &mut Board, called_number: u32) {
    for cell in board.cells.iter_mut() {
        if cell.2 == called_number {
            cell.3 = true;
        }
    }
}