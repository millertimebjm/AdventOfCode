use std::fs;

struct Fish {
    countdown: i32
}

fn main() {
    let file_path = "input_test.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut memo: [i64; 10] = [0i64; 10];
    let mut fish_spawning_countdowns: Vec<i32> = vec![];
    let mut total: i64 = 0;

    for text in contents.lines() {
        let fish_spawning_time_array = text.split(",");
        for fish_spawning_time_string in fish_spawning_time_array {
            fish_spawning_countdowns.push(fish_spawning_time_string.parse().expect(""));
        }
    }

    for fish in fish_spawning_countdowns {
        if memo[fish.try_into().unwrap_or(0)] > 0 {
            total = total + memo[fish.try_into().unwrap_or(0)];
            println!("found fishes in memo {fish}");
            continue;
        } else {
            let mut fishes: Vec<Fish> = vec![];
            fishes.push(Fish {countdown: fish});
            for day in 0..256 {
                perform_day(&mut fishes);
                println!("on day {day}");
            }
            total = total + fishes.len().try_into().unwrap_or(0);
            memo[fish.try_into().unwrap_or(0)] = total;
            println!("added memo of {fish} with total {}", fishes.len());
        }
    }
    
    println!("total is {total}");

    // for fish_spawning_time_string in fish_spawning_time_array {
    //     println!("adding fish with countdown of {}", fish_spawning_time_string);

    //     fishes.push(Fish {countdown: fish_spawning_time_string.parse().expect("")});
    // }

    // for _ in 0..256 {
    //     perform_day(&mut fishes);
    // }
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