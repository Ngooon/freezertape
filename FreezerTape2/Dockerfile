#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FreezerTape2/FreezerTape2.csproj", "FreezerTape2/"]
RUN dotnet restore "FreezerTape2/FreezerTape2.csproj"
COPY . .
WORKDIR "/src/FreezerTape2"
RUN dotnet build "FreezerTape2.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "FreezerTape2.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreezerTape2.dll"]