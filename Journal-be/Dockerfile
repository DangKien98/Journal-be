#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 31081
EXPOSE 31081
ENV ASPNETCORE_URLS=http://+:31081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Journal-be/Journal-be.csproj", "Journal-be/"]
RUN dotnet restore "Journal-be/Journal-be.csproj"
COPY . .
WORKDIR "/src/Journal-be"
RUN dotnet build "Journal-be.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Journal-be.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Journal-be.dll"]