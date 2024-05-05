#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 32772:80
EXPOSE 32773:443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiApplication.csproj", "."]
RUN dotnet restore "./ApiApplication.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ApiApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet dev-certs https -ep /app2/aspnetapp.pfx -p pw
RUN dotnet publish "ApiApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiApplication.dll"]