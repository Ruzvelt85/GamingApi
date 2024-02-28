# YLD Gaming API  

The solution consists of 5 projects:

**WebApi**: Contains the main API controller, query handler, validator, filter and mapping configuration.

**Dto**: Contains DTO models for request and response

**Patterns**: Contains core interfaces.

**Integration**: Contains logics of interaction with 3rd party service, including DTO and configuration.

**Tests**: Contains unit tests for controller, query handler, validator, mapping and integration service.


This structure can be a little bit extensive or even redundant for the scope of the given task, but it will be able to accomodate the following features well.

In case of appearing a specific business logics, a separate domain model should be created

###  Build and Run

To build and run the solution, please perform the following steps:

1) Open the solution in MS Visual Studio
2) Execute command Build - Build solution
3) To run API press F5

API will be explored through Swagger at https://localhost:59613/swagger/index.html

Alternatively, the solution can be built and run through CLI:
1) Go to the solution folder and make build with command 'dotnet build'
2) Run API with command 'dotnet run --project src/WebApi'
