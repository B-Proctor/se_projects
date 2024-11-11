#ifndef PCB_H
#define PCB_H

#include <string>
using namespace std;

enum ProcessState { // required process states as dictated by assignment gods.
    READY,
    RUNNING,
    TERMINATED
};

struct PCB { // Made into a struct for easy access and better management
    string name;
    ProcessState state;
    int arrivalTime;
    int CPUTime;
    int CPUTimeUsed;
    int priority;

    PCB(string name, int arrival, int totalTime, int priorityLevel) // cool guy type constructor
        : name(name),
          arrivalTime(arrival),
          CPUTime(totalTime),
          CPUTimeUsed(0),
          priority(priorityLevel),
          state(READY)
    {}
};

#endif
