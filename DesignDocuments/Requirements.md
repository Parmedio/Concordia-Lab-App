# Lab Management App

The app should be able to handle the following requirements:
- It should be able to connect to the API when the connection becomes available.
  - The connection becomes available once per day for a limited amount of time
  - The timing can be predicted and should be set just once when launching the application 
- The app receives Data from the Trello API but it uses just a limited set of that Object. The information to store and use is the following


## **List**

(string) Title

List<Experiment> Experiments

## **Experiment**

(string) Title
(string) Description
(DateTime) Deadline (Not required)
(Enum or string) Priority
(string) Last comment
List<Scientist>? Assignee (Not Required)

- On these data collection, the scientists should be able to do the following things:
  - Add A Comment to the experiment
  - Move an experiment from a List to Another one

- Once the connection is established, the Application should:
  - retrieve the possible new experiments from all around the globe and add them to the "Undone experiments"
  - Download the comments for each experiment and update the comments' list
  - Upload all the changes done by the scientists, updating all the experiments' statuses
  
- Each scientists, apart from being able to see all the informations regarding the experiments, should be able to have informations regarding personal or unassigned experiments (those uncocluded) in the following way:
  - tasks due to expire (less than 5 days from the deadline) with **High Priority**
  - High priority tasks
  - other tasks due to expire
  - Medium priority tasks
  - Low Priority tasks
  - ( Avoid duplicates, inside the same category they should be ordered by expiration date )
  - ( Filter Option, a button or a filter that allows to show only the specific parts of that list )
  

