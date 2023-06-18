FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
LABEL description="Minimal Api with MongoDB"
LABEL organisation="KSB St. Gallen"
LABEL author="Manuel Bruelisauer"
WORKDIR /app
COPY --from=build-env /build/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]