using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Minimal API nach Arbeitsauftrag 2 (test)");

/*
    docker run --name mongodb -d -p 27017:27017 -v data:/data/db mongo
    docker run --name mongodb -d -p 27017:27017 -v data:/data/db -e MONGO_INITDB_ROOT_USERNAME=gbs -e MONGO_INITDB_ROOT_PASSWORD=geheim mongo

*/
app.MapGet("/check", () => {
    
    try
    {
        var mongoDbConnectionString = "mongodb://gbs:geheim@localhost:27017";
        var mongoClient = new MongoClient(mongoDbConnectionString);
        var databaseNames = mongoClient.ListDatabaseNames().ToList();

        return "Zugriff auf MongoDB ok. Vorhandene DBs: " + string.Join(",", databaseNames);
    }
    catch (System.Exception e)
    {
        return "Zugriff auf MongoDB funktioniert nicht: " + e.Message;
    }         

});

app.Run();
