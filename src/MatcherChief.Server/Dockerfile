# Build from solution directory:
# docker build -f src/MatcherChief.Server/Dockerfile -t matcherchief.server . 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY . ./
WORKDIR src/MatcherChief.Server
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/src/MatcherChief.Server/out .
ENTRYPOINT ["dotnet", "MatcherChief.Server.dll"]
