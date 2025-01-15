/*Braydyn Proctor
this is a simulated operating system with processes using recources.
This was weird to code :(
*/
#include "PCB.h"
#include "RCB.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include <deque>
#include <vector>
#include <algorithm>
using namespace std;
// sneaky lil counters n constant numbers 
const int maxTicks = 1000000000;
const int priorityLevels = 3;
const int boostInterval = 1000;
int tCounter = 0;
// Ready queues and resource management stuffs
vector<deque<PCB*>> readyQueues(priorityLevels);
PCB* runningProcess = nullptr;
vector<PCB*> terminatedProcesses;
vector<ResourceControlBlock> resources = { {"r0"}, {"r1"}, {"r2"} };


void readProcesses(const string& filename) { //reads processes at priority levels
    ifstream file(filename);
    if (!file.is_open()) {
        cout << "Error: Could not open file " << filename << endl;
        exit(1);
    }

    string line;
    while (getline(file, line)) {
        stringstream ss(line);
        string name, resourcesField, timesField;
        int arrival, totalCPU, priority;
        if (!(getline(ss, name, ',') && ss >> arrival && ss.ignore() && ss >> totalCPU && ss.ignore() &&
            ss >> priority && ss.ignore() && getline(ss, resourcesField, ',') && getline(ss, timesField))) {
            cerr << "Highly unfortunate error at: " << line << endl;
            continue;
        }
        //extraction time bois
        vector<string> resourcesNeeded;
        stringstream resourceStream(resourcesField);
        string resource;
        while (getline(resourceStream, resource, ';')) {
            resourcesNeeded.push_back(resource);
        }
        //more extraction stuff
        vector<int> acquisitionTimes;
        stringstream timeStream(timesField);
        int time;
        while (timeStream >> time) {
            acquisitionTimes.push_back(time);
            if (timeStream.peek() == ';') timeStream.ignore();
        }
        PCB* process = new PCB(name, arrival, totalCPU, priority, resourcesNeeded, acquisitionTimes);
        readyQueues[priority].push_back(process);
    }
    file.close();
}
//resource aqcuisition for da procesesses
void acquireResource(PCB* process, ResourceControlBlock& resource) {
    if (!resource.allocated) { 
        resource.allocated = true;
        resource.resourceTimer = 10; //ex for usage
        process->state = BLOCKED;
        cout << "Process " << process->name << " acquired da resource " << resource.resourceName << ".\n";
    } 
    else {// if its already in use put it in waiting
        resource.waitingList.push_back(process);
        process->state = BLOCKED;
        cout << "Process " << process->name << " is waiting for resource " << resource.resourceName << ".\n";
    }
}
//realeaess the resourceses + checks if others are waiting
void releaseResource(ResourceControlBlock& resource) {
    if (resource.allocated) {
        resource.resourceTimer--;
        if (resource.resourceTimer <= 0) {
            resource.allocated = false;
            cout << "Resource " << resource.resourceName << " has been presented with a sock, it is now a free resource.\n";
            if (!resource.waitingList.empty()) { //put resource to the next processes in w_list
                PCB* nextProcess = resource.waitingList.front();
                resource.waitingList.pop_front();
                acquireResource(nextProcess, resource);
            }
        }
    }
}
//selects the next process to run from highest priority
void scheduleProcess() {
    for (int i = 0; i < priorityLevels; ++i) {
        if (!readyQueues[i].empty()) { // only do if theres more to do.
            runningProcess = readyQueues[i].front();
            readyQueues[i].pop_front();
            runningProcess->state = RUNNING;
            cout << "Process " << runningProcess->name << " from priority " << i << " is now running.\n";
            break;
        }
    }
}
//simulate process running.
void runProcess() {
    if (runningProcess == nullptr) return;
    runningProcess->CPUTimeUsed++;
    tCounter++;
    // Check if the process needs to acquire resources :)
    for (size_t i = 0; i < runningProcess->resourceAcquisitionTimes.size(); ++i) {
        if (runningProcess->CPUTimeUsed == runningProcess->resourceAcquisitionTimes[i]) {
            for (const string& resourceName : runningProcess->resourcesNeeded) {
                auto it = find_if(resources.begin(), resources.end(),
                    [&resourceName](const ResourceControlBlock& r) { return r.resourceName == resourceName; });
                if (it != resources.end()) {
                    acquireResource(runningProcess, *it);
                }
            }
        }
    }
    // Check for termination (ill be back - arnold) or downgrade :(
    if (runningProcess->CPUTimeUsed >= runningProcess->CPUTime) {
        runningProcess->state = TERMINATED;
        terminatedProcesses.push_back(runningProcess);
        cout << "Process " << runningProcess->name << " has terminated.\n";
        runningProcess = nullptr;
    }
    else if (runningProcess->CPUTimeUsed % 10 == 0) {
        int priority = runningProcess->priority;
        priority = min(priority + 1, priorityLevels - 1);
        readyQueues[priority].push_back(runningProcess);
        runningProcess->state = READY;
        runningProcess = nullptr;
    }
}
void priorityBoost() {//priorities *hair flip*
    if (tCounter % boostInterval == 0) {
        for (int i = 1; i < priorityLevels; ++i) {
            while (!readyQueues[i].empty()) {
                PCB* process = readyQueues[i].front();
                readyQueues[i].pop_front();
                readyQueues[0].push_back(process);
            }
        }
    }
}
bool allQueuesEmpty() {//are the queues reallllly empty?
    for (const auto& queue : readyQueues) {
        if (!queue.empty()) {
            return false;
        }
    }
    return true;
}
//Print final states muahahaha (lots of this code is recycled from my last assignment)
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
int main() { // the part that actually does something to make it work lmao
    readProcesses("processes.csv");
    while (tCounter < maxTicks && (!allQueuesEmpty() || runningProcess != nullptr)) {
        scheduleProcess();
        runProcess();
        for (auto& resource : resources) {
            releaseResource(resource);
        }
        priorityBoost();
    }
    finalPrinter();
}
