# Lab Management App

The app should be able to handle the following requirements:
- It should be able to connect to Trello's API when the connection becomes available.
  - The connection becomes available once per day for a limited amount of time
  - The timing at which the connection becomes available can be predicted and should be set just once when launching the application 
- The app receives Data from the Trello's API but it uses just a limited set of that Object. The information to store and use is the following
  - **List**
    - (string) Title
    - List<Experiment> Experiments
  -  **Experiment**
     -  (string) Title
     -  (string) Description
     -  (DateTime) Deadline (Not required)
     -  (Enum or string) Priority
     -  (string) Last comment
     -  List<Scientist>? Assignee (Not Required)

- On these data collections, the scientists should be able to do the following things:
  - Add a Comment to an experiment
  - Move an experiment from a List to another one (i.e. from uncompleted to completed).
  - Scientists can do everything to not assigned experiments.

- Once the connection is established, the Application should:
  - Retrieve the possible new experiments from all around the globe and add them to the "Undone experiments"
  - Download the comments for each experiment and update the comments' list
  - Upload all the changes done by the scientists, updating all the experiments' statuses
  
- Each scientists, apart from being able to see all the informations regarding the experiments, should be able to have informations regarding experiments assigned to them in the following way:
  - Tasks due to expire (less than 5 days from the deadline) with **High Priority**
  - High priority tasks
  - Other tasks due to expire
  - Medium priority tasks
  - Low Priority tasks
  - ( Avoid duplicates, inside the same category they should be ordered by expiration date )
  - ( Filter Option, a button or a filter that allows to show only the specific parts of that list )
  - ( Not assigned projects ?? ) A scientist can decide to pick an unassigned experiment [Functionality about unassigned experiments is to be determined at a later time]

- Each scientist should be able to login using a simple login system.
  -  Once logged in they should be able to see the assignments to which they are assigned, they should be able to edit them as shown above. 
  -  On the other assignments they should only be able to add comments.
  

