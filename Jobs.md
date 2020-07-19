-------------------
**Title**
----
  Get items for the jobs
* **URL**
  /api/Jobs/
  
* **Method:**
  `GET` 
  
*  **URL Params**

   **Optional:** 
   `id=[integer]`
  
* **Success Response:**  
  <_What should the status code be on success and is there any returned data? This is useful when people need to to know what their callbacks should expect!_>
  * **Code:** 200 <br />
    **Content:** `{ id : 12, title:"job title", description:"job description", goal:"job goal", startTime:"2020-05-05", endTime:"2020-05-06", expectedEndTime:"2020-05-05", jobType:"Others" }` 
    
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

--------------------------

**Title**
----
    Create an item for the jobs
* **URL**
    /api/Jobs/
    
* **Method:**
   `POST` 

* **Data Params**
  `{ id : 12, title:"job title", description:"job description", goal:"job goal", startTime:"2020-05-05", endTime:"2020-05-06", expectedEndTime:"2020-05-05", jobType:"Others" }`

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

---------------------------------

**Title**
----
    Delete an item for the jobs
* **URL**
    /api/Jobs/id
    
* **Method:**
 `DELETE`
  
*  **URL Params**

   **Required:**
   `id=[integer]`


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

---------------------------------

**Title**
----
    Update an item for the jobs
* **URL**
    /api/Jobs/id
    
* **Method:**
 `PUT`
  
*  **URL Params**

   **Required:**
   `id=[integer]`
   
* **Data Params**
  `{ id : 12, title:"job title", description:"job description", goal:"job goal", startTime:"2020-05-05", endTime:"2020-05-06", expectedEndTime:"2020-05-05", jobType:"Others" }`

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
