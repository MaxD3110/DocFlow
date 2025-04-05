FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY FileProcessorService.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "FileProcessorService.dll"]

