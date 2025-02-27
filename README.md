JobBoardApi is the back-end server for my Nashville Software School final Capstone project, JobOpeningBoard.

A C# Web API, built with .NET 8.0 and EF Core, using PostgreSQL
----------------------------------------------------------------------------------------



----------------------------------------------------------------------------------------
API ENDPOINTS
----------------------------------------------------------------------------------------


INDUSTRY
------------------
Get all Industries
=> method `GET` `"/api/industry"`
    {
      [
        { "id": 1, "name": "Technology" },
        { "id": 2, "name": "Retail" }
      ],   
    }
