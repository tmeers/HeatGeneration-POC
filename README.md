Heat Generation - POC
==================

Proof of Concept code for generating heats for a Pinewood Derby

This ties in with [my thoughts on a gist](https://gist.github.com/tmeers/8701826). Hoping to make this more clear to figure out. 

Another train of thought would be to think of this as a schuduler for staff like so: 
>####Goal: Create a random schedule of Staff####
>Data given:  
>  - List Of staff: staffList
>  - Total number of days in a work week: totalDays
>  - Total number of days allowed to work, per week, per staff member: allowedDays
>  - Total number of Staff per day: staffPerDay
>
>Item counts: 
>  - staffList: 5
>  - totalDays: 5
>  - allowedDays: 4
>  - staffPerDay: 4
>
>Idea: 
>Loop Days filling in each position in staffPerDay. 
>Each staff member must work the maximum number of days.
>Each day should have as many positions filled as possible.

So what does this mean for racing
----
Following the rules for a ["Chaotic-Rotation Method"](http://www.rahul.net/mcgrew/derby/methods.html#chaotic)
  1. Add a Race per Den (POC will deal with generating only 1 race to build the algorithm)
  2. Get number of heats based on LaneCount and RacerCount: totalHeats  
  3. Select random racers for the lineup   
  4. Each Racer must race at least 1 time in each lane 
    - A 4 lane track each racer would compete in 4 heats   
    - A 6 lane track each racer would compete in 6 heats   
  5. Each racer must race the same number of times  
  6. Racers should be held in a race through as many heats as possible  
  7. Racers cannot compete against themselves  
  8. Racers should compete against as many different opponents as possible  
    
###Given data:####
  - Den (Id: 1, Name: Tigers)
  - List of Racers (Id, Name) 
    - Id = 1, Name = "aaaa" 
    - Id = 2, Name = "bbbb" 
    - Id = 3, Name = "cccc" 
    - Id = 4, Name = "dddd" 
    - Id = 5, Name = "eeee" 
  - Number of Lanes (4 lanes is the easiest)
    - This is a static numbe rand will not change for the lifetime of the heat generation
 
###Conceptual Workflow###
  - Build the list of Racers
  - Calculate the Number of heats bassed on this formula:  
    __(racers * lanes) / desired number of races = heat count__
  - Loop each Heat to build list on Contestants
    - First heat should be the first N racers to fill the track
      - If there are not enough racers to fill the track, fill with an empty racer
    - Additional heats should start filling with remaining racers
      - If racer A, B, C, D filled the previous heat, racer E should start in lane 1, filling in lanes 2-4 with racers A-C
      - If the previous heat did not have enough racers to fill the track, fill lane 1 with an empty racer
        - Each racer must race in each lane per rule 4
    - Heats should build following above rules until all racers have competed the required number of times

###Example Data###
####4 Lanes, 5 Racers####
|           | Heat 1    | Heat 2    | Heat 3    | Heat 4    | Heat 5    
| --------- | --------- | --------- | --------- | --------- | --------- 
| Lane 1    | aaaa      | eeee      | dddd      | cccc      | bbbb
| Lane 2    | bbbb      | aaaa      | eeee      | dddd      | cccc
| Lane 3    | cccc      | bbbb      | aaaa      | eeee      | dddd
| Lane 4    | dddd      | cccc      | bbbb      | aaaa      | eeee
| 
| BYE       | eeee      | dddd      | cccc      | bbbb      | aaaa
####4 Lanes, 3 Racers####
|           | Heat 1    | Heat 2    | Heat 3    | Heat 4    
| --------- | --------- | --------- | --------- | --------- 
| Lane 1    | aaaa      |           | cccc      | bbbb      
| Lane 2    | bbbb      | aaaa      |           | cccc      
| Lane 3    | cccc      | bbbb      | aaaa      |           
| Lane 4    |           | cccc      | bbbb      | aaaa      
| 
| BYE       |           |           |           |       
####Additional assumptions for 4 lanes####
With 4 lanes and 5 racers there should be 5 heats and each should have all 4 lanes filled, and **one** racer with a BYE for each heat. That racer should be in the next heat. 

With 4 lanes and 6 racers there should be 6 heats and each should have all 4 lanes filled, and **two** racers with a BYE for each heat. 

With 4 lanes and 7 racers there should be 8 heats and each should have all 4 lanes filled for 4 heats, and 3 lanes filled for the other 4, and **no** racers with a BYE for any heat. 

With 4 lanes and 8 racers there should be 8 heats and each should have all 4 lanes filled, and **no** racer with a BYE for each heat. 

With 4 lanes and 9 racers there should be 9 heats and each should have all 4 lanes filled, and **one** racer with a BYE for each heat. That racer should be in the next heat. 
