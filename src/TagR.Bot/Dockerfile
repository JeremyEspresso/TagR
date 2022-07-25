#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TagR.Bot/TagR.Bot.csproj", "src/TagR.Bot/"]
COPY ["src/TagR.Application/TagR.Application.csproj", "src/TagR.Application/"]
COPY ["src/TagR.Database/TagR.Database.csproj", "src/TagR.Database/"]
COPY ["src/TagR.Domain/TagR.Domain.csproj", "src/TagR.Domain/"]
RUN dotnet restore "src/TagR.Bot/TagR.Bot.csproj"
COPY . .
WORKDIR "/src/src/TagR.Bot"
RUN dotnet build "TagR.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TagR.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TagR.Bot.dll"]