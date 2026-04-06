FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["ProjetoCNP.csproj", "."]

RUN dotnet restore "ProjetoCNP.csproj"

COPY . .

RUN dotnet build "ProjetoCNP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjetoCNP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProjetoCNP.dll"]
