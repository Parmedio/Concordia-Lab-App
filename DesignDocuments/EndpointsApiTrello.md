# Endpoints
> Consuming the Trello Api we will access to two main endpoints only:
> (Note that the token must be obtained for each individual scientist so that their actions can be identified.)

## Get Cards On A List
Endpoint  
`GET /1/lists/{id}/cards`  
Url Example  
`https://api.trello.com/1/lists/64760975fbea80d6ef329080/cards?key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD`  

id is the list id, stored in appsettings  

This retrieves all cards on a list with a specific Id  
Contains:
-  members' ids
-  description
-  labels
-  due date

## Update A Card (Moving it from list to list)
Endpoint  
`PUT /1/cards/{id}`  
Url Example  
`https://api.trello.com/1/cards/64761756c338bac930497a59?idList=64760975fbea80d6ef329081&key=9ba27d32be683843dd1ffb346ae07641&token=ATTAd93cf67ec0072d821ff32e199156a675ed9301feea0f899df160829b3f14082dAB1E41AD`  

id is the id of the card, which can be retrieved  

## Add A Comment To A Card
Endpoint  
`POST /1/cards/{id}/actions/comments`  
Url Example  
`https://api.trello.com/1/cards/64761756c338bac930497a59/actions/comments?text=Could you test also for Temperature = 50&key=9ba27d32be683843dd1ffb346ae07641&token=ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193`

## Get The Members Of A Card
Endpoint  
`GET /1/cards/{id}/members/`  
Url  
`https://api.trello.com/1/cards/647614d5def373b831896c2a/members?key=9ba27d32be683843dd1ffb346ae07641&token=ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193`


## Get A Label
(Could be null)  
Endpoint  
`GET /1/labels/{id}`  
Url Example  
`https://api.trello.com/1/labels/647609751afdaf2b05536cd9/?key=9ba27d32be683843dd1ffb346ae07641&token=ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193`

## Get All Comments on a Card
(Could be null/empty)  
Endpoint  
`GET /1/cards/{id}/actions`  
Url Example  
`https://api.trello.com/1/cards/647614d5def373b831896c2a/actions?key=9ba27d32be683843dd1ffb346ae07641&token=ATTA5d17675c2460799412382fd90bfb3b94b0eb355646b6941dd87ac9eb77aa080dD1F24193`





