use std::fs;

struct octopus {
    power: i32,
    has_flashed: bool
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut octopii: Vec<Vec<octopus>> = vec![];
    let mut total_flashes: i32 = 0;
    for text in contents.lines() {
        let mut temp_oct = vec![];
        for character in text.chars() {
            temp_oct.push(octopus {power: character as i32 - 48, has_flashed: false});
        }
        octopii.push(temp_oct);
    }

    let mut total_flashes = 0;
    let mut all_flash: bool = false;
    let mut first_all_flash: i32 = 0;
    for index in 0..1000 {
        let mut temp_flashes = 0;
        (temp_flashes, all_flash) = pass_day(&mut octopii);
        total_flashes = total_flashes + temp_flashes;
        if all_flash && first_all_flash == 0 {
            first_all_flash = index;
            break;
        }
    }
    println!();
    for y in 0..octopii.len() {
        for x in 0..octopii.len() {
            print!("{}", octopii[y][x].power);
        }
        println!();
    }

    println!("total flashes ended with: {total_flashes}");
    println!("first flash: {first_all_flash}");

}

fn pass_day(octopii: &mut Vec<Vec<octopus>>) -> (i32, bool) {
    for y in 0..octopii.len() {
        for x in 0..octopii.len() {
            octopii[y][x].power = octopii[y][x].power + 1;
        }
    }

    let mut any_found = true;
    while any_found {
        any_found = false;
        for y in 0..octopii.len() {
            for x in 0..octopii.len() {
                if octopii[y][x].power > 9 && !octopii[y][x].has_flashed {
                    octopii[y][x].has_flashed = true;
                    any_found = true;
                    if x > 0 {
                        octopii[y][x-1].power = octopii[y][x-1].power + 1;
                    }
                    if x < octopii[y].len()-1 {
                        octopii[y][x+1].power = octopii[y][x+1].power + 1;
                    }
                    if y > 0 {
                        octopii[y-1][x].power = octopii[y-1][x].power + 1;
                    }
                    if y < octopii.len()-1 {
                        octopii[y+1][x].power = octopii[y+1][x].power + 1;
                    }
                    if x > 0 && y > 0 {
                        octopii[y-1][x-1].power = octopii[y-1][x-1].power + 1;
                    }
                    if x > 0 && y < octopii.len()-1 {
                        octopii[y+1][x-1].power = octopii[y+1][x-1].power + 1;
                    }
                    if x < octopii[y].len()-1 && y > 0 {
                        octopii[y-1][x+1].power = octopii[y-1][x+1].power + 1;
                    }
                    if x< octopii[y].len()-1 && y < octopii.len()-1 {
                        octopii[y+1][x+1].power = octopii[y+1][x+1].power + 1;
                    }
                }
            }
        }
    }
    
    let mut all_flash: bool = true;
    for y in 0..octopii.len() {
        for x in 0..octopii[y].len() {
            if !octopii[y][x].has_flashed {
                all_flash = false;
            }
        }
    }

    let mut total_flashes: i32 = 0;
    for y in 0..octopii.len() {
        for x in 0..octopii[y].len() {
            if octopii[y][x].has_flashed {
                octopii[y][x].power = 0;
                octopii[y][x].has_flashed = false;
                total_flashes = total_flashes + 1;
            }
        }
    }
    (total_flashes, all_flash)
}
