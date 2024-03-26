### Feedback

*Please add below any feedback you want to send to the team*

### Intro

You can access Cinema API via http (http://localhost:32772) or https (https://localhost:32773) endpoints

### Swagger Endpoins

http://localhost:32772/index.html
https://localhost:32773/index.html

### Features

Besides requested features I have added correspondig HttpGet endpoints following the HttpPost best practices by providing the resource location in the response.
I also added API versioning.
I have modified docker-compose.yaml to adopt it with visual studio DockerCompose profile so it could be easily launched via Visual Studio during debug.
Also I have added the newly create Cinema API to the docker-compose definition so it would launch alongside with two other services.

### Cinema API communication with Provided Movies API

I have configured GRPC client and used it to communicate with the Movies API using GRPC.

### Execution Tracking

To not reimplement the Wheel I have added loggers to track request execution time in the appsettings.json:
- "Microsoft.AspNetCore.Hosting": "Information"
- "Microsoft.AspNetCore.Mvc": "Information"

There is also Open Telemetry package that can provided additional information via metrics endpoints.

### Movies API issues

There was a configuration issue that enabled FailureMiddleware which produced failures. I have set environment variable to 0% failures -> FailurePercentage=0
Another issue was in the naming of the file 'amovies-db.json' that contains data about the movies. The name should be 'movies-db.json'. I have addressed this issue by creating a docker file 'MoviesApi.Dockerfile', In 'MoviesApi.Dockerfile' I used your dcoker image (lodgify/movies-api:3) and fixed the naming for the 'amovies-db.json'. I have modified docker-compose.yml file accordingly to build a new image with fix. 
Also I have noticed that condition for enabling the FailureMiddleware is overcomplicated and might not work as expected.

### Things to improve

Since the requirments haven't mentioned updating the .NET version, I would recommend to update to the latest .NET version since .NET Core 3.1 is no longer supported and expose application to security vulnerabilities.
In the real world application I would add Authentication and Authorization.
I would add Localization and moved all the hardcoded strings to the Resource files.
Also I would have moved entity framework models configuration to separate classes implementing IEntityTypeConfiguration.
Etc.

