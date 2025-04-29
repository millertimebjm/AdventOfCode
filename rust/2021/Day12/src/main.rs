use std::fs;
use std::collections::HashMap;

#[derive(Clone)]
struct Segment {
    point1: String,
    point2: String
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");


    for text in contents.lines() {
        println!("{text}");
    }

    let mut segments: Vec<Segment> = vec![];
    
    for text in contents.lines() {
        let text_array = text.split("-");
        let collection = text_array.collect::<Vec<&str>>();
        segments.push(Segment { point1: collection[0].to_string(), point2: collection[1].to_string() });
    }

    for index in 0..segments.len() {
        println!("point1: {}, point2: {}", segments[index].point1, segments[index].point2);
    }

    let mut path: Vec<String> = vec![];
    path.push("start".to_string());
    let paths = spelunking_recursive(&segments, &mut path);


    for index in 0..paths.len() {
        for index2 in 0..paths[index].len() {
            //println!("path {index} and segment {index2} with string {}", paths[index][index2]);
            print!("{},", paths[index][index2]);
        }
        println!();
    }

    println!("paths count: {}", paths.len());

}

fn is_big_cave(input: &String) -> bool {
    for character in input.chars() {
        if character as i32 > 91 {
            return false;
        }
    }
    true
}

fn spelunking_recursive<'a>(segments: &Vec<Segment>, path: &mut Vec<String>) -> Vec<Vec<String>> {
    let mut paths: Vec<Vec<String>> = vec![];
    let last = path.last().expect("");
    if last == "end" {
        paths.push(path.clone());
        return paths;
    }
    let possible_segments = get_possible_paths(segments, last);
    for index in 0..possible_segments.len() {
        print!("possible_segment: {} ", possible_segments[index]);
        if is_big_cave(&possible_segments[index]) || exists_in_path_twice(path, possible_segments[index].clone()) {
            let mut new_path = path.clone(); // Create fresh copy
            new_path.push(possible_segments[index].clone());
            
            let mut new_paths = spelunking_recursive(segments, &mut new_path);
            paths.extend(new_paths.drain(..));
        }
    }
    println!();
    paths
}

fn get_possible_paths(segments: &Vec<Segment>, point: &String) -> Vec<String> {
    let mut possible_paths: Vec<String> = vec![];
    for index in 0..segments.len() {
        if segments[index].point1 == *point {
            possible_paths.push(segments[index].point2.clone());
        } else if segments[index].point2 == *point {
            possible_paths.push(segments[index].point1.clone());
        }
    }
    possible_paths
}

fn exists_in_path_twice(path: &Vec<String>, point: String) -> bool {
    if point == "start" {
        return false;
    }
    
    let mut path_count = HashMap::new();
    let mut any_two = false;
    
    // First pass: count occurrences
    for index in 0..path.len() {
        if path[index] == "end" {
            continue;
        }
        path_count.entry(path[index].clone())
                  .and_modify(|count| *count += 1)
                  .or_insert(1);
        
        if is_big_cave(&path[index].clone()) {
            continue;
        }

        if *path_count.get(&path[index].clone()).unwrap() == 2 {
            any_two = true;
        }
    }
    
    // Second pass: check conditions
    if any_two {
        let current_count = path_count.get(&point).unwrap_or(&0);
        return *current_count == 0;
    }
    
    true
}