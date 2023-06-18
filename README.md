# Minimal-Api-with-MongoDB Miniprojekt, Maurin Inauen

# Projekt-Dokumentation: .NET7 WebAPI mit Docker und MongoDB

Diese Dokumentation beschreibt die Schritte zur Erstellung eines .NET7 WebAPI-Projekts auf Linux mit der Command Line. Das Projekt wird mithilfe von Docker und MongoDB entwickelt.

## Schritt 1: Installation von .NET7 auf Linux

Um .NET7 auf Linux zu installieren, befolgen Sie die folgenden Schritte:

1. Öffnen Sie das Terminal.

2. Führen Sie den folgenden Befehl aus, um das .NET7 SDK herunterzuladen und zu installieren:
   
   ```shell
    # Get Ubuntu version
    declare repo_version=$(if command -v lsb_release &> /dev/null; then
    lsb_release -r -s; else grep -oP '(?<=ˆVERSION_ID=).+' /etc/os-release
    | tr -d '"'; fi)
    ,→
    ,→
    # Download Microsoft signing key and repository
    wget https://packages.microsoft.com/config/ubuntu/$repo_version/packagesmicrosoft-prod.deb -O
    packages-microsoft-prod.deb
    ,→
    ,→
    # Install Microsoft signing key and repository
    sudo dpkg -i packages-microsoft-prod.deb
    # Clean up
    rm packages-microsoft-prod.deb
    # Update packages
    sudo apt update
    # Install dotnet 7 sdk
    sudo apt install dotnet-sdk-7.0
   ```

3. Überprüfen Sie die erfolgreiche Installation, indem Sie den Befehl `dotnet --version` ausführen. Sie sollten die .NET7-Version angezeigt bekommen.

## Schritt 2: Erstellen des .NET7 WebAPI-Projekts

Um das .NET7 WebAPI-Projekt mit dem Namen "WebAPI" zu erstellen, verwenden Sie den Befehl `dotnet new`:

```shell
dotnet new webapi -n WebAPI
cd WebAPI
```

Dieser Befehl erstellt ein neues .NET7 WebAPI-Projekt mit dem Namen "WebAPI" und wechselt in das Projektverzeichnis.

## Schritt 3: Erstellen eines Multistage Dockerfiles

Ein Multistage Dockerfile ermöglicht es uns, die Anwendung in separaten Stufen zu erstellen und zu optimieren. Verwenden Sie den folgenden Befehl, um das Dockerfile zu erstellen:

```dockerfile
# 1. Build compile image

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# 2. Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /build/out .
ENV ASPNETCORE_URLS=http://*:5001
EXPOSE 5001
ENTRYPOINT ["dotnet", "WebApi.dll"]
```

Das Dockerfile besteht aus zwei Stufen. In der ersten Stufe (`build`) wird das Projekt wiederhergestellt und veröffentlicht. In der zweiten Stufe wird das resultierende veröffentlichte Projekt in das `aspnet`-Image kopiert und als Einstiegspunkt festgelegt.

## Schritt 4: Erstellen der docker-compose.yml-Datei

Die `docker-compose.yml`-Datei ermöglicht es uns, Docker-Container einfach zu verwalten und zu konfigurieren. Erstellen Sie eine Datei mit dem Namen `docker-compose.yml` und fügen Sie den folgenden Inhalt hinzu:

```yaml
version: "3.9"
services:
  webapi:
    build: WebApi
    depends_on:
      - mongodb
    ports:
      - 5001:5001
      
  mongodb:
    image: mongo
    volumes:
      - mongoData:/data/db
volumes:
  mongoData:
```

Dieses Konfigurationsbeispiel definiert zwei Services: `webapi` für unsere .NET7 WebAPI-Anwendung und `mongodb` für die MongoDB-Datenbank. Der `webapi`-Service wird aus dem aktuellen Verzeichnis und dem Dockerfile erstellt. Die Ports 8000 (für die WebAPI) und 27017 (für MongoDB) werden auf den Host weitergeleitet.

## Schritt 5: Zugriff auf MongoDB einrichten

Um auf MongoDB zuzugreifen, müssen wir den entsprechenden MongoDB-Treiber in unser .NET-Projekt hinzufügen. Öffnen Sie die `WebAPI.csproj`-Datei und fügen Sie die folgende Zeile innerhalb des `<ItemGroup>`-Elements hinzu:

```xml
<PackageReference Include="MongoDB.Driver" Version="2.13.3" />
```

Dies fügt den MongoDB-Treiber als Abhängigkeit hinzu.

## Schritt 6: Erstellen eines Connection Strings

Um eine Verbindung zur MongoDB-Datenbank herzustellen, müssen wir einen Connection String konfigurieren. Öffnen Sie die `appsettings.json`-Datei im `WebAPI`-Projekt und fügen Sie den folgenden Abschnitt hinzu:

```json
"MongoDB": {
  "ConnectionString": "mongodb://mongodb:27017"
}
```

Dieser Connection String gibt den Hostnamen (`mongodb`) und den Port (`27017`) der MongoDB-Instanz an.

Das war's! Sie haben erfolgreich ein .NET7 WebAPI-Projekt auf Linux erstellt, es in einem Docker-Container ausgeführt und eine Verbindung zur MongoDB-Datenbank hergestellt.

Für weitere Informationen und detaillierte Anleitungen empfehle ich Ihnen, die offizielle Dokumentation von .NET und Docker zu konsultieren.
