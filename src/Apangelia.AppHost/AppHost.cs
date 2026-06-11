var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "postgres", secret: true);

var postgres = builder.AddPostgres("apangelia-postgres", username, password,port: 5434)
    .WithDataVolume(isReadOnly: false);

builder.AddProject<Projects.Apangelia_WebApi>("apangelia-webapi")
    .WithReference(postgres)
    .WaitFor(postgres);

builder.Build().Run();