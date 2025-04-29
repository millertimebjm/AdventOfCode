use std::fs;

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut completed_results: Vec<u64> = vec![];
    let mut corrupted_result: i32 = 0;
    for text in contents.lines() {
        let mut is_broke = false;
        let mut code: Vec<char> = vec![];
        let mut broken_character = ' ';
        for character in text.chars() {
            if character == '('
                || character == '['
                || character == '{'
                || character == '<' { 
                    code.push(character);
                    continue;
                }
            if character == ')'
                || character == ']'
                || character == '}'
                || character == '>' {
                if code.len() == 0 {
                    is_broke = true;
                    broken_character = character;
                    break;
                }
                let code_check = code.pop().unwrap();
                if (character == ')' && code_check == '(')
                    || (character == '}' && code_check == '{')
                    || (character == ']' && code_check == '[')
                    || (character == '>' && code_check == '<') {

                } else {
                    is_broke = true;
                    broken_character = character;
                    break;
                }
            }
        }
        if code.len() > 0 && is_broke {
            if broken_character == ')' {
                corrupted_result = corrupted_result + 3;
            }
            if broken_character == ']' {
                corrupted_result = corrupted_result + 57;
            }
            if broken_character == '}' {
                corrupted_result = corrupted_result + 1197
            }
            if broken_character == '>' { 
                corrupted_result = corrupted_result + 25137
            }
        } else if code.len() > 0 && !is_broke {
            let mut completed_result_temp: u64 = 0;
            for index in 0..code.len() {
                let incomplete_character = code.pop().unwrap();
                completed_result_temp = completed_result_temp * 5;
                if incomplete_character == '(' {
                    completed_result_temp = completed_result_temp + 1;
                } else if incomplete_character == '[' {
                    completed_result_temp = completed_result_temp + 2;
                } else if incomplete_character == '{' {
                    completed_result_temp = completed_result_temp + 3;
                } else if incomplete_character == '<' {
                    completed_result_temp = completed_result_temp + 4;
                }
            }
            completed_results.push(completed_result_temp);
        }
    }

    println!("corrupted_result is {}", corrupted_result);
    completed_results.sort();
    let half: usize = completed_results.len()/ 2;
    println!("half of completed result is {}", completed_results[half]);
}