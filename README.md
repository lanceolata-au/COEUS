# CARBON EVENT (Scout)

### Major Events Framwork

#### Getting setup

##### Required software and frameworks

* [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [node.js](https://nodejs.org/en/) (the LTS version)
* [MySQL](https://dev.mysql.com/downloads/) or [MariaDB](https://downloads.mariadb.org/)

##### Recommended software

A good IDE, I personally like [Rider](https://www.jetbrains.com/rider/) from JetBrains however you can use [Visual Studio](https://visualstudio.microsoft.com/downloads/) Community Edition if licensing is a problem. I am going to work on getting some licenses for Rider for those contributing to the project under their OpenSource  

[MySQL workbench](https://dev.mysql.com/downloads/workbench/) is probably a good idea so you can work with the data

[SourceTree](https://www.sourcetreeapp.com/) is a really solid GIT management tool


##### Starting the Project

1. Clone down this repo into a known folder
2. Open your IDE of choice


###### Example with Mac OS X and Rider

3.
4.
5.
6.
7.
8.

###### Example with Mac OS X and Visual Studio

##### Example db startup

```connectionString="server=zeryter.xyz;user=#######;password=#########" dropAll=true startingData=true dbName=carbonTest```

##### Getting your way around the project

There are three executable projects

###### carbon.api
```Application Programming Interface```

This one is the backend, it deals with authentication and is the middleman between the frontend (the website) and the database.

The main places of interest are
- ```controllers``` (this is where the endpoints live)
- ```modules``` currently contains the automapper setup (a handy tool for converting between data types)



###### carbon.app

###### carbon.runner.database

#### TODO
- [ ] Full application setup
- [ ] Contingents
- [ ] Approval Process

#### Currently Targeting  Moot2023 Project

#### Works leveraged

Entity Framework methods primarily [this](https://cpratt.co/generic-entity-base-class/) tutorial.

Angular authentication for Identity Server 4 [from here](https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver) and [from here](https://github.com/manfredsteyer/angular-oauth2-oidc) and finally [from here](https://christianlydemann.com/openid-connect-with-angular-8-oidc-part-7/)
