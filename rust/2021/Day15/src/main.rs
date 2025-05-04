use std::fs;
use std::io;
use std::collections::HashMap;

struct Point {
    x: usize,
    y: usize,
    cost: i32,
    lowest_cost: i32,
    checked_neighbors: bool
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut points: Vec<Vec<Point>> = vec![];
    let mut line_y: usize = 0;
    for text in contents.lines() {
        let mut row: Vec<Point> = vec![];
        for x in 0..text.len() {
            let x_usize = x;
            row.push(Point {
                y: line_y,
                x: x_usize,
                cost: text[x_usize..x_usize+1].parse().expect(""),
                lowest_cost: 0,
                checked_neighbors: false,
            });
        }
        points.push(row);
        line_y += 1;
    }

    println!();
    for y in 0..points.len() {
        if y % 10 == 0 {
            println!();
        }
        for x in 0..points[y].len() {
            if x % 10 == 0 {
                print!(" ");
            }
            print!("{}", points[y][x].y);
        }
        println!();
    }
    println!();

    increase_size_of_grid(&mut points);

    println!();
    for y in 0..points.len() {
        if y % 10 == 0 {
            println!();
        }
        for x in 0..points[y].len() {
            if x % 10 == 0 {
                print!(" ");
            }
            print!("{}", points[y][x].y);
        }
        println!();
    }
    println!();


    review_lowest_cost(&mut points, 0, 0);

    for index in 0..100000 {

        if index % 100 == 0 {

            println!("cost:");
            for y in 0..points.len() {
                if y % 10 == 0 {
                    println!();
                }
                for x in 0..points[y].len() {
                    if x % 10 == 0 {
                        print!(" ");
                    }
                    print!("{}", points[y][x].cost);
                }
                println!();
            }
            println!();
            println!("neighbors:");
            for y in 0..points.len() {
                if y % 10 == 0 {
                    println!();
                }
                for x in 0..points[y].len() {
                    if x % 10 == 0 {
                        print!(" ");
                    }
                    print!("{}", if points[y][x].checked_neighbors { "Y" } else {"N"});
                }
                println!();
            }
            println!();
            println!("press any key...");
            let mut buffer = String::new();
            let stdin = io::stdin(); // We get `Stdin` here.
            stdin.read_line(&mut buffer);
        }

        let mut point = find_next_point_to_review(&points).unwrap();
        let (x, y) = (point.x, point.y);
        drop(point);
        review_lowest_cost(&mut points, x, y);
        println!("reviewing {x}, {y}; vec lengths are {}", points.len());

        if y == points.len()-1 && x == points[0].len() -1 {
            break;
        }
    }
    println!("point at {}, {}: lowest_cost: {}", points[0].len() -1, points.len() -1, points[points.len()-1][points[0].len()-1].lowest_cost);
}

fn increase_size_of_grid(points: &mut Vec<Vec<Point>>) {
    let original_line_count = points.len();
    let original_column_count = points[0].len();
    for set_index in 0..4 {
        for line_index in 0..original_line_count {
            let mut new_line: Vec<Point> = vec![];
            for x in 0..original_column_count {
                let mut new_value = points[line_index][x].cost + 1 + set_index;
                if new_value >= 10 { new_value = new_value % 10 + 1; }
                let new_point = Point {
                    x: x,
                    y: line_index + ((set_index + 1) * 10) as usize,
                    cost: new_value,
                    lowest_cost: 0,
                    checked_neighbors: false,
                };
                new_line.push(new_point);
            }
            points.push(new_line);
        }
    }
    for set_index in 0..4 {
        for row_index in 0..points.len() {
            for column_index in 0..original_column_count {
                let mut new_value = points[row_index][column_index].cost + 1 + set_index;
                if new_value >= 10 { new_value = new_value % 10 + 1; }
                let new_point = Point {
                    x: points[row_index].len(),
                    y: row_index,
                    cost: new_value,
                    lowest_cost: 0,
                    checked_neighbors: false,
                };
                points[row_index].push(new_point);
            }
        }
    }
}

fn find_next_point_to_review(points: &Vec<Vec<Point>>) -> Option<&Point> {
    let mut lowest_point_not_reviewed: Option<&Point> = None;
    for y in 0..points.len() {
        for x in 0..points[y as usize].len() {
            if (lowest_point_not_reviewed.is_none() && !points[y as usize][x as usize].checked_neighbors) ||
                (!points[y as usize][x as usize].checked_neighbors 
                    && points[y as usize][x as usize].lowest_cost != 0
                    && points[y as usize][x as usize].lowest_cost < lowest_point_not_reviewed.unwrap().lowest_cost) {
                lowest_point_not_reviewed = Some(&points[y as usize][x as usize]);
                // println!("found a new lowewst_point: {}, {}", lowest_point_not_reviewed.unwrap().x, lowest_point_not_reviewed.unwrap().y);
            }
        }
    }
    lowest_point_not_reviewed
}

fn review_lowest_cost(points: &mut Vec<Vec<Point>>, x: usize, y: usize) {
    let current_cost = points[y][x].lowest_cost;
     println!("current_cost is: {current_cost}; {x},{y} with cost: {}", current_cost);
    if x > 0 && !points[y][x-1].checked_neighbors && points[y][x-1].lowest_cost == 0 {
        points[y][x-1].lowest_cost = current_cost + points[y][x-1].cost;
    }
    if x < points[y].len()-1 && !points[y][x+1].checked_neighbors && points[y][x+1].lowest_cost == 0 {
        points[y][x+1].lowest_cost = current_cost + points[y][x+1].cost;
    }
    if y > 0 && !points[y-1][x].checked_neighbors && points[y-1][x].lowest_cost == 0 {
        points[y-1][x].lowest_cost = current_cost + points[y-1][x].cost;
    }
    if y < points.len()-1 && !points[y+1][x].checked_neighbors && points[y+1][x].lowest_cost == 0 {
        points[y+1][x].lowest_cost = current_cost + points[y+1][x].cost;
    }
    points[y][x].checked_neighbors = true;
    // println!("reviewing lowewst_point: {}, {}", x, y);
}