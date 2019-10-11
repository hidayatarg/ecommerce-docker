# build stage
FROM microsoft/aspnetcore-build:2.0.0 AS build
# set working directory
# if not exist docker will create
WORKDIR code/

# copy file from current directory
COPY . .

# build and publish
# publish to directory out
RUN dotnet restore
RUN dotnet publish --output /out --configuration Release

FROM microsoft/aspnetcore:2.0.0
# copy files from one out directory to app
# create directory in new image called app
COPY --from=build /out /app/

# work from
WORKDIR /app

# run when container stand up
ENTRYPOINT ["dotnet", "ProductCatalogApi.dll"]

# run the docker from cli
# docker build -t shoes/catalog .
