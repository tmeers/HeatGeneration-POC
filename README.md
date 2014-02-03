Heat Generation - POC
==================

Proof of Concept code for Heat Generation

This ties in with [my thoughts on a gist](https://gist.github.com/tmeers/8701826 ). Hoping to make this more clear to figure out. 

Another train of thought would be to think of this as a schuduler for staff like so: 
Goal: Create a random schedule of Staff
====
Data given:  
  - List Of staff: staffList
  - Total number of days in a work week: totalDays
  - Total number of days allowed to work, per week, per staff member: allowedDays
  - Total number of Staff per day: staffPerDay

Item counts: 
  - staffList: 5
  - totalDays: 5
  - allowedDays: 4
  - staffPerDay: 4

Idea: 
Loop Days filling in each position in staffPerDay. 
Each staff member must work the maximum number of days.
Each day should have as many positions filled as possible.

