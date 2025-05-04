use std::fs;
use std::collections::HashMap;

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let starter: String = contents.lines().nth(0).unwrap().to_string();
    let mut pairs = HashMap::new();
    let pairs_array: Vec<&str> = contents.lines().collect();
    for line in pairs_array.iter().skip(2) {
        let mut parts = line.split(" -> ");
        if let (Some(key), Some(value)) = (parts.next(), parts.next()) {
            pairs.insert(key, value);
        }
    }

    println!("starter: {starter}");
    for (key, val) in pairs.iter() {
        println!("key: {key} val: {val}");
    }

    // part 1
    // let mut iteration = starter;
    // for index in 0..10 {
    //     iteration = grow_polymer(&iteration[..], &pairs);
    //     //println!("new iteration ({index}): {iteration}");
    // }

    // let mut letter_counts: HashMap<&str, i64> = HashMap::new();
    // for index in 0..iteration.len() {
    //     letter_counts.entry(&iteration[index..index+1]).and_modify(|counter| *counter += 1).or_insert(1);
    // }
    // let mut max: i64 = 0;
    // let mut min: i64 = i64::MAX;
    // for (key, value) in letter_counts.iter() {
    //     if value < &min {
    //         min = *value;
    //     }
    //     if value > &max {
    //         max = *value;
    //     }
    // }
    // println!("max is {max}, min is {min}, result is {}", max-min);
    // part 1

    // part 2
    let mut initial_hash_map: HashMap<String, i64> = HashMap::new();
    for index in 0..starter.len()-1 {
        initial_hash_map.entry((&starter[index..index+2]).to_string()).and_modify(|counter| *counter += 1).or_insert(1);
    }
    println!("initial:");
    for (key, value) in initial_hash_map.iter() {
        print!("{key}, {value}, ");
    }
    println!();

    let mut current_hash_map = initial_hash_map;

    for index in 0..40 {
        let mut new_hash_map: HashMap<String, i64> = HashMap::new();
        for (key, value) in current_hash_map.iter() {
            let pair_new_value = pairs.get(&key[..]).unwrap();
            let mut new_pair_1 = String::new();
            new_pair_1.push_str(&key[0..1]);
            new_pair_1.push_str(&pair_new_value);
            new_hash_map.entry(new_pair_1).and_modify(|counter| *counter += value).or_insert(*value);
            
            let mut new_pair_2 = String::new();
            new_pair_2.push_str(&pair_new_value);
            new_pair_2.push_str(&key[1..2]);
            new_hash_map.entry(new_pair_2).and_modify(|counter| *counter += value).or_insert(*value);
        }
        println!("new map:");
        for (key, value) in new_hash_map.iter() {
            print!("{key}, {value}, ");
        }
        println!();
        current_hash_map = new_hash_map;
    }
    
    let mut character_count_hash: HashMap<String, i64> = HashMap::new();
    for (key, value) in current_hash_map.iter() {
        character_count_hash.entry(key[0..1].to_string()).and_modify(|counter| *counter += value).or_insert(*value);
    }

    let mut max: i64 = 0;
    let mut max_letter: String = "".to_string();
    let mut min: i64 = i64::MAX;
    let mut min_letter: String = "".to_string();
    for (key, value) in character_count_hash.iter() {
        if value < &min {
            min = *value;
            min_letter = key.to_string();
        }
        if value > &max {
            max = *value;
            max_letter = key.to_string();
        }
    }
    println!("max is {max} ({max_letter}), min is {min} ({min_letter}), result is {}", max-min);
    // part 2
}

fn grow_polymer(original: &str, pairs: &HashMap<&str, &str>) -> String {
    let mut iteration: String = String::new();
    for index in 0..original.len()-1 {
        let slice = &original[index..index+2];
        match pairs.get(slice) {
            Some(found) => {
                iteration.push_str(&original[index..index+1]);
                iteration.push_str(found);
            },
            None => iteration.push_str(&original[index..index+1])
        }
    }
    iteration.push_str(&original[&original.len()-1..original.len()]);
    iteration
}