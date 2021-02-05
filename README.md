# Colossus (pseudo) File Manager by Derek Hampton

## What is it?
This is a technical project example intended for Colossus. If you don't know what I'm talking about, then this page isn't for you.

Visitors may freely copy code and concepts from this project without attribution, just don't try to pass it off as your own...


## Getting Started

1. Clone the repo
```
git clone https://github.com/Augmensis/ColossusFileManager.git
```

2. Enter the repo and the Main Solution Folder
```
cd ColossusFileManager/ColossusFileManager
```

3. Build the WebApi project if needed
```
dotnet build ColossusFileManager.WebApi
```
4. Run the test suite
```
dotnet test
```

5. Run the WebApi project
```
dotnet run --project ColossusFileManager.WebApi
```

6. In a separate console with or API client such as [Postman](https://www.postman.com/product/api-client/) (recommended) try running against some endpoints

List all folders and files recursively (See BUG comments)
```
curl -Method Get -Uri https://localhost:5001/ListStructure
```

List files and folders starting from a specific folder
```
curl -Method Get -Uri https://localhost:5001/ListStructure?folderpath=/Test2
```

Search for a file that starts with a search term
```
curl -Method Get -Uri https://localhost:5001/FindFile?searchTerm=demo
```

Search for a file that starts with a search term and narrow it down by a folder path
```
curl -Method Get -Uri https://localhost:5001/FindFile?searchTerm=demo&folderPath
```

Create a new Folder
```
curl -Method Post -Uri https://localhost:5001/CreateFolder?newfolderpath=Test2/SubFolder9
```

Create a new File (note ampersand (&) is wrapped in quotes for some calls in powershell
```
curl -Method Post -Uri https://localhost:5001/CreateFile?folderpath=Test2"&"filename=demofile14.txt
```

7. ???

8. Profit


## Troubleshooting
If you are getting errors about SSL/TLS certs try running the following command before restarting from Step 4 above:
```
dotnet dev-certs https --trust
```

The Sqlite connection string can be replaced in the appsettings.json file if needed.

Any other major problems, just shout.

## Known Bugs & missing features
1. /FindFile with both searchTerm and folderPath returns a 200 result, but an empty Data payload.
2. /ListStructure doesn't currently iterate through all levels of recursion when there is more than one folder
3. /ListStructure doesn't list files held in subfolders
4. Integration Tests have been excluded
5. Unit tests have been mostly completed for the API Controller (FileManagerController) and is missing some use cases.


## Cool stuff
Swagger has been enabled and is available at `https://localhost:5001/swagger/v1/swagger.json`
Since this is a pure WebAPI, it is only available as a JSON object but can be loaded into IDEs and other builders to generate client connections automatically.

## Closing notes
It's taken a while to complete as I'm a bit rusty with NUnit and pure EntityFrameworkCore. However, it was some good, challenging, fun. Given more time or incentive I would investigate the recursion problems with /ListStructure further and add integration tests that spin-up and tear-down their own database connections. 

## License
[MIT](https://choosealicense.com/licenses/mit/)
