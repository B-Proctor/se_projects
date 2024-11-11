/* Braydyn Proctor
   CS240
   Simulated OS using a MLFQ scheduler algorithm
*/

#include "PCB.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <deque>
#include <string>
#include <vector>
using namespace std; // here to make my life everso slightly easier not having to put std:: 

// Constants and counters
const int maxTicks = 1000000000; 
const int priorityLevels = 3;   // Number of priority levels for the MLFQ  
const int boostInterval = 1000; // priority boost interval
int tCounter = 0;                // Current tick count

// Ready queues for MLFQ scheduler
vector<deque<PCB*>> readyQueues(priorityLevels); // Multi-level feedback queues by priority level
PCB* runningProcess = nullptr;                    // Currently running process
vector<PCB*> terminatedProcesses;                 // Stores terminated processes

// Opens the provided processes.csv with error handling, which is neat for larger scale things
// If it breaks, I will cry. (I spent forever on this part)
void readProcesses(const string& filename) {
    ifstream file(filename);
    if (!file.is_open()) { // error handling
        cout << "Error: Could not open file " << filename << endl;
        exit(1); // Rest in peace you have no files 
    }
    string line;
    while (getline(file, line)) {
        stringstream ss(line);
        string name;
        int arrival, totalCPU, priority;

        // Error handling for incorrectly formatted lines
        if (!(getline(ss, name, ',') && ss >> arrival && ss.ignore() && ss >> totalCPU && ss.ignore() && ss >> priority)) {
            cerr << "Highly unfortunate error at: (unexpected comma) " << line << endl;
            continue;
        }

        PCB* process = new PCB(name, arrival, totalCPU, priority); // make new PCB with data
        readyQueues[priority].push_back(process); // push into the appropriate priority queue
    }
    file.close();
}

void scheduleProcess() {
    // This function runs a process by selecting it from the highest-priority non-empty queue
    for (int i = 0; i < priorityLevels; ++i) {
        if (!readyQueues[i].empty()) {
            runningProcess = readyQueues[i].front();
            readyQueues[i].pop_front();
            runningProcess->state = RUNNING;
            cout << "Process " << runningProcess->name << " from priority " << i << " is now running.\n";
            break;
        }
    }
}

void runProcess() {
    // Function checks if it's a valid process, increments counters, and checks for termination
    if (runningProcess == nullptr) {
        return; // just in case there's no running process
    }
    runningProcess->CPUTimeUsed++;
    tCounter++;

    // Terminate if the process is finished with its CPU time.
    if (runningProcess->CPUTimeUsed >= runningProcess->CPUTime) {
        runningProcess->state = TERMINATED;
        terminatedProcesses.push_back(runningProcess);
        cout << "Process " << runningProcess->name << " has terminated.\n";
        runningProcess = nullptr;
    }
    // Check if time slice is up if so banish to a lower level
    else if (runningProcess->CPUTimeUsed % 10 == 0) {
        int priority = runningProcess->priority;
        priority = min(priority + 1, priorityLevels - 1); // banish to the next lower priority if possible
        readyQueues[priority].push_back(runningProcess);
        runningProcess->state = READY;
        runningProcess = nullptr;
    }
}

// Priority boost function that prevents starvation of the processes in lower queues
void priorityBoost() {
    if (tCounter % boostInterval == 0) { // Boost interval reached
        for (int i = 1; i < priorityLevels; ++i) { // Start at the second highest priority
            while (!readyQueues[i].empty()) {
                PCB* process = readyQueues[i].front();
                readyQueues[i].pop_front();
                readyQueues[0].push_back(process); // Boost to higherest priority
            }
        }
    }
}

// Final printer runs through the processes and prints all their beautiful states. If they are finished or not, etc.
void finalPrinter() {
    if (runningProcess != nullptr) {
        cout << "Process " << runningProcess->name << " is still running.\n";
    }

    for (int i = 0; i < priorityLevels; ++i) {
        while (!readyQueues[i].empty()) {
            PCB* process = readyQueues[i].front();
            readyQueues[i].pop_front();
            cout << "Process " << process->name << " is still in the queue at priority " << i << ".\n";
        }
    }

    cout << "Terminated processes:\n";
    for (PCB* process : terminatedProcesses) {
        cout << "Process " << process->name << " terminated after " << process->CPUTimeUsed << " CPU ticks.\n";
    }
}


bool allQueuesEmpty() {
    for (const auto& queue : readyQueues) {
        if (!queue.empty()) {
            return false;
        }
    }
    return true;
}


int main() { // The actual main loop
    string input;
    readProcesses("processes.csv"); 
    while (tCounter < maxTicks && (!allQueuesEmpty() || runningProcess != nullptr)) { // Go till timeout or no process left :)
        scheduleProcess();
        runProcess();
        priorityBoost();
    }
    finalPrinter();
}
