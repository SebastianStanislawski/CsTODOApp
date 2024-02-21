# ToDo application
Server created in C#. It will be used as backend for future ToDo application.

## Features
- Connecting to the server with multiple clients. Each client can independently communicate with the server via sockets.
- Server communicates with the local database. Apart from connecting, it performs basic activities such as inserting, updating, deleting and selecting data.

## Database
Application was tested in connection with MySQL database. For everything to work properly, you need to add a [MySQL reference](https://dev.mysql.com/downloads/connector/net/6.1.html) to the server project.
Connection to the database is defined in `Database.cs` file and contains url to database, database name, username, password and table name:
```
private string serverName = "localhost";
private string databaseName = "databaseName";
private string username = "root";
private string password = "";
private string tableName = "tableName";
```
Values ​​should be changed according to the current configuration.

Currently, the application only uses one table that contains three columns: `id` of the entry, `Entry Date` that is created when the user adds a new entry and `Message` which contains message added to the entry.
