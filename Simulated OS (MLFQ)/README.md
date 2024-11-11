# Simulated Operating System with MLFQ Scheduler

This project is a simulated operating system written in C++ that utilizes a Multilevel Feedback Queue (MLFQ) scheduling algorithm. It demonstrates basic OS scheduling concepts by managing processes across multiple priority levels, with an implemented priority boost mechanism to prevent starvation. Processes load from a CSV file, enabling customizable testing.

## Features

- **Multilevel Feedback Queue (MLFQ) Scheduling**: 
  - Processes are divided into multiple priority queues, with higher-priority processes scheduled before lower-priority ones.
  - Processes are demoted if they exceed their time slice, simulating feedback behavior in CPU scheduling.
- **Priority Boost Mechanism**: 
  - Processes in lower-priority queues receive priority boosts at defined intervals to avoid starvation.
- **CSV Process Input**: 
  - Process data is read from a `processes.csv` file, allowing easy configuration and testing with different workloads.

## Project Structure

- `main.cpp`: Contains the main simulation loop, scheduler, and functions for handling processes.
- `PCB.h`: Defines the Process Control Block (PCB) structure, process states, and attributes, including priority.
- `processes.csv`: Sample CSV file listing processes with arrival times, CPU times, and priority levels.

## Usage

1. Clone this repository:
   ```bash
   git clone https://github.com/yourusername/se_projects.git
   cd se_projects/Simulated_OS_with_MLFQ
 2. Compile and Run  
```bash
   g++ main.cpp -o SimulatedOS
   ./SimulatedOS

