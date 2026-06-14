var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "postgres", secret: true);

var postgres = builder.AddPostgres("apangelia-postgres", username, password,port: 5434)
    .WithDataVolume(isReadOnly: false);

var webapi = builder.AddProject<Projects.Apangelia_WebApi>("apangelia-webapi")
    .WithReference(postgres)
    .WaitFor(postgres);

var smee = builder.AddExecutable(
    name: "smee-github",
    command: "smee",
    workingDirectory: builder.AppHostDirectory,
    "-u", builder.Configuration["Smee:FromUrl"]!,
    "-t", builder.Configuration["Smee:ToUrl"]!)
    .WaitFor(webapi);

builder.Build().Run();
