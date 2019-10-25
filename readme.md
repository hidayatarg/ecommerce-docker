## Docker Images:
```docker
docker pull microsoft/mssql-server-linux
// Run the docker container with name catalogDB in port 1433 expose to outside
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ProductApi(!)' -e 'MSSQL_PID=Express' -p 1433:1433 --name=catalogdb microsoft/mssql-server-linux
docker start catalogdb
```

## Create the initial Migration
Put the migrations under the Data folder
```csharp
dotnet ef migrations add  InitialMigration -o Data/Migrations
dotnet ef database update
```

## Documentation using Swagger
Swashbuckle.AspNetCore

## ASPNETCORE Docker image
`docker pull microsoft/aspnetcore:2.0.0` and `docker pull microsoft/aspnetcore-build` install images from docker Hub.

## Created the Docker File
in the directory of ProductCatalogApi `docker build -t shoes/catalog .`

## Remove a docker image
`docker rmi shoes/catalog` and clean the dependent images with name none `docker system prune`

## Build Docker Compose
`docker-compose build` run where docker compose is placed.

`docker-compose up mssqlserver` run the mssqlserver container.

`docker-compose up catalog` run the catalog container.

## Generic Class to Call HTTP REQ. of
This class is used to call all projects using HTTP Request.

## Creating Service Class
The service classes will consume the IHttpClient and ApiPaths classes. These classes are specific classes.
We created two model classes (Catalog and CatalogItem) that will be used in the service classes.
