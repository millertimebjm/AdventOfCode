use std::fs;
use std::collections::HashMap;

#[derive(Clone)]
struct Point {
    x: i32,
    y: i32
}

fn main() {
    let file_path = "input.txt";

    let contents = fs::read_to_string(file_path)
        .expect("There was an error reading the file.");

    for text in contents.lines() {
        println!("{text}");
    }

    let mut folds: Vec<Point> = vec![];
    let mut grid: Vec<Point> = vec![];
    let mut before_folds = true;
    for text in contents.lines() {
        if text == "" {
            before_folds = false;
            continue;
        }
        if before_folds {
            let text_array: Vec<&str> = text.split(",").collect();
            grid.push(Point { x: text_array[0].parse().expect(""), y: text_array[1].parse().expect("")});
        } else {
            let new_text = text.replace("fold along ", "");
            let text_array: Vec<&str> = new_text.split("=").collect();
            if text_array[0] == "x" {
                folds.push(Point { x: text_array[1].parse().expect(""), y: 0});
            } else {
                folds.push(Point { y: text_array[1].parse().expect(""), x: 0});
            }
        }
    }

    println!("");
    println!("grid length: {}", grid.len());

    // part1
    // fold(&folds[0], &mut grid);

    // println!("new grid length: {}", grid.len());
    // for index in 0..grid.len() {
    //     println!("{}, {}", grid[index].x, grid[index].y);
    // }

    // part2
    for index in 0..folds.len() {
        fold(&folds[index], &mut grid);
    }

    for y in 0..200 {
        for x in 0..200 {
            if point_exists(&grid, x, y) {
                print!("#");
            }
            else {
                print!(".");
            }
        }
        println!();
    }
}

fn fold(fold: &Point, grid: &mut Vec<Point>) {
    let mut index_to_remove: Vec<i32> = vec![];
    if fold.y > 0 {
        for index in 0..grid.len() {
            if grid[index].y < fold.y {
                continue;
            }
            let mut cloned_grid = grid.clone();
            let point: &mut Point = &mut grid[index];
            point.y = fold.y * 2 - point.y;
            if point_exists(&cloned_grid, point.x, point.y) {
               index_to_remove.push(index.try_into().unwrap());
            }
        }
    } else {
        for index in 0..grid.len() {
            if grid[index].x < fold.x {
                continue;
            }
            let mut cloned_grid = grid.clone();
            let point: &mut Point = &mut grid[index];
            point.x = fold.x * 2 - point.x;
            if point_exists(&cloned_grid, point.x, point.y) {
                index_to_remove.push(index.try_into().unwrap());
            }
        }
    }
    index_to_remove.sort_by(|a, b| b.cmp(a));
    for index in 0..index_to_remove.len() {
        grid.remove(index_to_remove[index].try_into().unwrap());
    }
}

fn point_exists(grid: &Vec<Point>, x: i32, y: i32) -> bool {
    for index in 0..grid.len() {
        if grid[index].x == x && grid[index].y == y {
            return true;
        }
    }
    false
}