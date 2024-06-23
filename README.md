# WebAPIServer

Todo List App with ASP.NET Core 6 and Angular 16
This project is a simple todo list application built using ASP.NET Core 6 on the backend and Angular 16 on the frontend. It allows users to create, view, edit, and delete todo items.

# Features
- Create new todo items: Users can add new tasks to their todo list with a title ,description and planned date.
- View existing todo items: The application displays a list of all currently saved todo items.
- Edit existing todo items: Users can modify the details of existing tasks.
- Delete todo items: Users can remove unwanted tasks from their todo list.

# Prerequisites
-Node.js and npm (or yarn): 
Download and install Node.js (https://nodejs.org/en) which includes npm. Alternatively, you can use a package manager like yarn (https://classic.yarnpkg.com/lang/en/docs/install/).
-.NET 6 SDK: 
Download and install the .NET 6 SDK from the official Microsoft website (https://dotnet.microsoft.com/en-us/download).

# Installation
Clone this repository or download the source code.
Open a terminal or command prompt and navigate to the project directory.
Run npm install (or yarn install) to install the required dependencies for both the frontend and backend.

# Running the Application
Start the backend: 
## Running the Backend with Visual Studio
Open the Solution in Visual Studio:
Double-click the WebAPIServer.sln file to open it in Visual Studio.
### Update localdb connection string in appsettings.json file:
- From the View menu, open SQL Server Object Explorer
- Right click on the localDb and select properties
- In the properties window find the connectionString property
- Copy the value and replace the connectionString value in appsettings.json file with your value
- Change the initial catalog value to “ToDoDb” instead of master
 
 

### Create the database and tables:
On “package manager console” run the command “Update-Database”


Select the Startup Project:

In the Solution Explorer window (usually on the right side), right-click on the project folder named WebAPIServer and select "Set as Startup Project".
Run the Application (Without Debugging):

# There are two ways to run the backend without debugging:
- Using the green "Run" button: Locate the green "Run" button (usually a play symbol) on the top toolbar and click it. This will build and run the backend application.
- Using the context menu: Right-click on the project name in the Solution Explorer and select "Run" without debugging.
Run the Application (Debugging):

# View the Backend Output:

After running the backend application, the Visual Studio console window will display application logs and any debugging information.
You can also access the backend API in your web browser by navigating to the URL specified in the code configuration (https://localhost:7111).



Built with
Backend: ASP.NET Core 6
Frontend: Angular 16
License
This project is licensed under the MIT License (see the LICENSE file for details).

Feel free to customize and extend this project further based on your needs!
