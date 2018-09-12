
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY TestWebApp.csproj TestWebApp/
RUN dotnet restore TestWebApp/TestWebApp.csproj
WORKDIR /src/TestWebApp
COPY . .
RUN dotnet build TestWebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish TestWebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestWebApp.dll"]
EXPOSE 80
