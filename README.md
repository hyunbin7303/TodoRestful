# ToDo ReadMe

## Getting started

### What is ToDo

ToDo is a Restful webservice designed to assist users with building their own ToDo application. This service will provide various methods and request to accommodate general uses of a ToDo task list.  

### Setup
### MongoDB:
- Add your IP Address to the Network Access
  * Log into account, click Network Access on left navigation
  * Click 'ADD IP ADDRESS'
  
### Visual Studio Sln:
- Add MongoDB password to the Environment variable
  * Use label 'MongoDB_Password'

## Todo list
 * [x] Create Application -
 * [x] Dtaahbase
 * [ ] Connection
* Traffic Routing
* Expose Unified end point
* API Composition
* Caching.
* Logging.
Need to set up asap. Investigate Seq Log.


## Planning
| Date | Description |
| --- | --- |
| First testing | List all *new or modified* files |
| Second testing | List all *new or modified* files |
| 2020-08-01 | hahah |
| `Result` | Show file differences that **haven't been** staged |


## Table Schema


### Daily Task
| Name    | DataType       |
|----------|:-------------:|
|    Id    |  left-aligned  |
|    Title      |    string |
|    Description |   string |
|ItemId    |     string      |
|Due       |      Date      |
|  Phone  |   Integer      |
#### API Call

### Home
| Name    | DataType       |
|----------|:-------------:|
|    Id        |  string   |
|    Title     |    string |
|    Description |  string |
|ItemId     |  string   |
#### API Call

### Workout
| Name    | DataType       |
|----------|:-------------:|
|    Id    |   string     |
|    Title      |    string |
|    Description |   string |
#### API Call

### Study
| Name    | DataType       |
|----------|:-------------:|
|    Id        |  string |
|    Title      |    string |
|    Description |   string |
#### API Call

**Title**
----
  <_Additional information about your API call. Try to use verbs that match both request type (fetching vs modifying) and plurality (one vs multiple)._>

* **URL**

### Home
  * api/Home/{id}
  * api/Home/{home}

### Daily Task
  * api/DailyTask/{id}

* **Method:**
  
  <_The request type_>

  `GET` | `POST` | `DELETE` | `PUT`
  
*  **URL Params**

   <_If URL params exist, specify them in accordance with name mentioned in URL section. Separate into optional and required. Document data constraints._> 

   **Required:**
 
   `id=[integer]`

   **Optional:**
 
   `photo_id=[alphanumeric]`

* **Data Params**

  <_If making a post request, what should the body payload look like? URL Params rules apply here too._>

* **Success Response:**
  
  <_What should the status code be on success and is there any returned data? This is useful when people need to to know what their callbacks should expect!_>

  * **Code:** 200 <br />
    **Content:** `{ id : 12 }`
 
* **Error Response:**

  <_Most endpoints will have many ways they can fail. From unauthorized access, to wrongful parameters etc. All of those should be liste d here. It might seem repetitive, but it helps prevent assumptions from being made where they should be._>

  * **Code:** 401 UNAUTHORIZED <br />
    **Content:** `{ error : "Log in" }`

  OR

  * **Code:** 422 UNPROCESSABLE ENTRY <br />
    **Content:** `{ error : "Email Invalid" }`

* **Sample Call:**

  <_Just a sample call to your endpoint in a runnable format ($.ajax call or a curl request) - this makes life easier and more predictable._> 

* **Notes:**

  <_This is where all uncertainties, commentary, discussion etc. can go. I recommend timestamping and identifying oneself when leaving comments here._> 
----

## Features

This project provides API endpoints to operate CRUD functionality to manage a personal To Do list:
* Add daily tasks with due date
* Add contact information and reminder for important phone calls 
* Keep track of items you want to purchase
* Maintain a daily or weekly routine by recording your workout session
* Make a list of groceries to help you save time at the store, save a list for next time

## Contributing

This has been initiaited by a group of 3 developers. Your feedback in appreciated and encourage in how to improve. This service is limited to the available endpoints that we were able to think of and build. We welcome ideas to provide more options and features. 

## Related projects



## Licensing

No license required. Pay requirement of $1M USD to K-Park Foundation
