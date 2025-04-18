use std::fs;
use std::cmp;

struct Vent {
    FromX: usize,
    FromY: usize,
    ToX: usize,
    ToY: usize
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut vents: Vec<Vent> = vec![];
    for text in contents.lines() {
        let vent: Vent = build_vent(&text);
        if vent.FromX == vent.ToX || vent.FromY == vent.ToY {
            vents.push(vent);
        }
    }
    println!("vents length: {}", vents.len());

    let mut points: [[i32; 1000]; 1000] = [[0i32; 1000]; 1000];
    for vent in &vents {
        // println!("add points {}, {} -> {}, {}", vent.FromX, vent.FromY, vent.ToX, vent.ToY);
        add_points(vent, &mut points);
    }

    let mut count: i32 = 0;
    for y in 0..points.len() {
        // println!("point found {}, {}, {}", points[index].x, points[index].y, points[index].count);
        for x in 0..points[y].len(){
            if points[y][x] > 1 {
                count += 1;
                // println!("point with more than one found {}, {}, {}", points[index].x, points[index].y, points[index].count)
            }
        }
    }

    println!("found {count} points that have more than 1");
}

fn add_points(vent: &Vent, points: &mut [[i32; 1000]; 1000]) {
    if vent.FromX == vent.ToX {
        for index in cmp::min(vent.FromY, vent.ToY)..(cmp::max(vent.FromY, vent.ToY)+1) {
            // println!("add point {}, {}", vent.FromX, index);
            add_get_point(points, vent.FromX, index);
        }
    }
    else if vent.FromY == vent.ToY {
        for index in cmp::min(vent.FromX, vent.ToX)..(cmp::max(vent.FromX, vent.ToX)+1) {
            // println!("add point {}, {}", index, vent.FromY);
            add_get_point(points, index, vent.FromY);
        }
    }
}

fn add_get_point(points: &mut [[i32; 1000]; 1000], x: usize, y: usize) {
    points[y][x] += 1;
}

fn build_vent(text: &str) -> Vent {
    let mut vents_array: Vec<usize> = vec![];
    let vents_text_array = text.split(" -> ");
    for vents_text in vents_text_array {
        let vent_text_array = vents_text.split(",");
        for vent_text in vent_text_array {
            vents_array.push(vent_text.parse().expect(""));
        }
    }
    let vent: Vent = Vent {
        FromX: vents_array[0],
        FromY: vents_array[1],
        ToX: vents_array[2],
        ToY: vents_array[3]
    };
    vent
}