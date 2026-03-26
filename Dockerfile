FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update && apt-get install git -y
WORKDIR /src
RUN git clone -b postgres-utc --single-branch https://github.com/bmajczak/WebApp.git
WORKDIR /src/WebApp/WebApp
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
EXPOSE 5000
CMD [ "dotnet", "WebApp.dll"]