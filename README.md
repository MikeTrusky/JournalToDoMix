<div align="center">
  <h1 align="center">Journal&ToDo Mix</h1>  
  <br/>
</div>

## Table of Contents
* [About The Project](#about-the-project)
* [Built with](#built-with)
* [Features](#features)
* [Screenshots](#screenshots)
* [Usage](#usage)
* [Room for Improvement](#room-for-improvement)
* [Contact with author](#contact-with-author)

## About The Project

![Main screen][main-screenshot]

It's an application designed to help users organize and track their daily activities and preserve memories of past events. With this project you can add new activities 
to specific days and then look at them based on their time frame: current, past or future. These activities can be viewed in seperate tables or visualized on a timeline, 
which supports both daily and weekly views.

The app provides intuitive features to add, edit or delete activities, empowering users to manager their schedules. Additionaly, users can access charts that display 
activity statistics, such as the total hours spent on specific activities and their number.

Users can create their own account through registration process, and upon logging in, they gain access to their own private set of activities.

## Built With

* Backend:
  - ASP.NET Core MVC
  - C#
  - SQL Server
  - EntityFrameworkCore
  - AspNetCore.Identity
 
* Frontend:
  - HTML with Razor syntax
  - CSS and Bootstrap
  - Google Charts
  - Vue.js
  - jQuery

## Features

* Activity Management:
  - Add, edit and delete activity
  - Assign activity to specific days
  - Assign activity category
  - Autocomplete suggestions for activity titles
* Time frame organization:
  - View activities in tables categorized as current, previous or planned
  - Visualize activities on a timeline in week or day mode
* Statistics:
  - Pie chart with activities number values
  - Bar chart with activities time values
* User accounts:
  - Register and login
  - Show activities associated with account
* Dynamic Front-End:
  - Responsive UI with real-time updates, such as showing current activity time elapsed 

## Screenshots

### Activity details
![details screen][details-screenshot]
### Adding activity
![adding screen][adding-screenshot]
### Tables view
![tables screen][tables-screenshot]
### Activities Count chart
![count chart screen][count-chart-screenshot]
### Activities Time chart
![time chart screen][time-chart-screenshot]
### Register view
![register screen][register-screenshot]
### Failed login view
![failed login screen][failed-login-screenshot]

## Usage

1. External Dependecies
   * Ensure that all required NuGet packages are installed. You can find the list of dependencies in the .csproj file.
   * Restore missing packages by running `dotnet restore` in the project directory or through Visual Studio.
2. Preparing the Database
   * Configure the connection string in `appsettings.json` file to point to your local SQL server.
   * Use the Package Manager Console or CLI to apply migrations:
     - Run Add-Migration <MigrationName> if there are changes to the data model
     - Run Update-Database to apply migrations to the database
3. Running the App locally
   * Open project in Visual Studio
   * Click Run to start the app
   * The app will open in your browser at `http://localhost:5106` or `https://localhost:7261`
4. User Management and Loggin In
   * Register new user
   * Log in to the app

## Room for Improvement

- [ ] Add new statistics charts
- [ ] Admin panel: adding new categories through the app
- [ ] Admin panel: manage users
- [ ] Improve views on different resolutions

## Contact with author

Maciej Trzuskowski

- ![Gmail][gmail-icon] - maciej.trzuskowski@gmail.com
- ![LinkedIn][linkedin-icon] - https://www.linkedin.com/in/maciej-trzuskowski/

[main-screenshot]: screenshots/main.png
[details-screenshot]: screenshots/activity_details.png
[adding-screenshot]: screenshots/adding_activity.png
[tables-screenshot]: screenshots/tables_view.png
[count-chart-screenshot]: screenshots/activities_count_chart.png
[time-chart-screenshot]: screenshots/activities_time_chart.png
[register-screenshot]: screenshots/register_view.png
[failed-login-screenshot]: screenshots/failedloginview.png
[gmail-icon]: https://img.shields.io/badge/Gmail-D14836?style=for-the-badge&logo=gmail&logoColor=white
[linkedin-icon]: https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white
