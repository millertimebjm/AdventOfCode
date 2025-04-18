use std::fs;

struct Fish {
    countdown: i32
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut fishes: Vec<Fish> = vec![];

    for text in contents.lines() {
        let fish_spawning_time_array = text.split(",");
        for fish_spawning_time_string in fish_spawning_time_array {
            println!("adding fish with countdown of {}", fish_spawning_time_string);

            fishes.push(Fish {countdown: fish_spawning_time_string.parse().expect("")});
        }
    }

    for _ in 0..80 {
        perform_day(&mut fishes);
    }

    println!("after 80 days, there are {} fishes", fishes.len());
}

fn perform_day(fishes: &mut Vec<Fish>) {
    for index in 0..fishes.len() {
        if fishes[index].countdown == 0 {
            fishes.push(Fish { countdown: 8 });
            fishes[index].countdown = 6;
        }
        else {
            fishes[index].countdown = fishes[index].countdown - 1;
        }
    }
}