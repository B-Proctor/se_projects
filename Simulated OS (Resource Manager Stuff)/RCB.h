#ifndef RCB_H
#define RCB_H

#include <string>
#include <deque>
#include "PCB.h"
using namespace std;

struct ResourceControlBlock {
    string resourceName;
    bool allocated;                // True if resource is allocated
    deque<PCB*> waitingList;       // Processes waiting for this resource
    int resourceTimer;             // Countdown timer for resource usage

    ResourceControlBlock(string name)
        : resourceName(name), allocated(false), resourceTimer(0) {}
};

#endif
