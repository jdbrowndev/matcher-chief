# Build from solution directory:
# docker build -f tools/MatcherChief.ClientRunner/Dockerfile -t matcherchief.clientrunner . 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . ./
WORKDIR tools/MatcherChief.ClientRunner
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/tools/MatcherChief.ClientRunner/out .
ENTRYPOINT ["dotnet", "MatcherChief.ClientRunner.dll"]
