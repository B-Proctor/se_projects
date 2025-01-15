#ifndef PCB_H
#define PCB_H

#include <string>
#include <vector>
using namespace std;

enum ProcessState {
    READY,
    RUNNING,
    BLOCKED,
    TERMINATED
};

struct PCB {
    string name;
    ProcessState state;
    int arrivalTime;
    int CPUTime;
    int CPUTimeUsed;
    int priority;
    vector<string> resourcesNeeded;       // Resources required
    vector<int> resourceAcquisitionTimes; // Times when resources are needed

    PCB(string name, int arrival, int totalTime, int priorityLevel,
        const vector<string>& resources, const vector<int>& acquisitionTimes)
        : name(name),
        arrivalTime(arrival),
        CPUTime(totalTime),
        CPUTimeUsed(0),
        priority(priorityLevel),
        state(READY),
        resourcesNeeded(resources),
        resourceAcquisitionTimes(acquisitionTimes) {}
};

#endif
