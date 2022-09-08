# CMPG323_API_Project

## How to use JWT_Token_Auth_API From visual studio

### Step 1:
Clone the repository from git

### Step 2:
Open the project in Visual Studio

### Step 3:
Click on the ISS play button to build and run the project(A browser will appear with swagger page showing)

### Step 4:
* Open the **POST**/api/Authenticate/register-admin or **POST**/api/Authenticate/register method and **click** on *"try it out"*
* Then in the request body add a username, password and emailaddress(**Ensure the password has all the diffirent type like "1234Un!x"**)
* **click** on *"Execute"*
* Check if the Response is a **Success** **200** 

### Step 5:
*  Open the **POST**/api/Authenticate/login and **click** on *"try it out"*
*  Enter in the json body your *username* and *password*
*  **click** on *"Execute"*
*  Check if the Response is a **Success** **200**
*  Copy the entire token from the first ** " ** to the ending ** " **

### Step 6:
*  **click** on the green **Authorize** button at the top of the page 
*  Enter **"Bearer"** followed by pasting in the **"token"** copied earlier
*  **click** "Authorize" and close the popup, now all the methods are Authorzed

Step 7:
* 
