## API Call for Workout

**Title**
----
  <_Additional information about your API call. Try to use verbs that match both request type (fetching vs modifying) and plurality (one vs multiple)._>

* **URL**
### Workout endpoints for GET method
  * api/Workout
  * api/Workout/{userid}
  * api/Workout/{userid}/{since}
  * api/Workout/{userid}/{since}/{until}
  * api/Workout/{userid}/{specific}

* **Method:**
  
  `GET`
  
*  **URL Params**

   <_If URL params exist, specify them in accordance with name mentioned in URL section. Separate into optional and required. Document data constraints._> 

   **Required:**
 
   `userid=[string]`

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

  Currently this is in development process.
  
  
### Workout endpoints for POST method

* **Method:**
  `POST`
  *  **URL Params**

  * api/Workout/{userid}
  
     **Required:**
 
   `userid=[string]`
   
   * **Success Response:**
  When Workout document is successfully created in cloud database, it returns 201 code (Created) with user information and document Id. 
 
  * **Code:** 201 <br />
    **Content:** `{ id : 12 }`
    
    * **Error Response:**

  <_Most endpoints will have many ways they can fail. From unauthorized access, to wrongful parameters etc. All of those should be liste d here. It might seem repetitive, but it helps prevent assumptions from being made where they should be._>

  * **Code:** 401 UNAUTHORIZED <br />
    **Content:** `{ error : "Log in" }`
