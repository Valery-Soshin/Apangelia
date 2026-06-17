var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", "postgres", secret: true);
var password = builder.AddParameter("password", "postgres", secret: true);

var postgres = builder.AddPostgres("apangelia-postgres", username, password,port: 5434)
    .WithDataVolume(isReadOnly: false);

var webapi = builder.AddProject<Projects.Apangelia_WebApi>("apangelia-webapi")
    .WithReference(postgres)
    .WaitFor(postgres);

var githubSmee = builder.AddExecutable(
    name: "github-smee",
    command: "smee",
    workingDirectory: builder.AppHostDirectory,
    "-u", builder.Configuration["Smee:GithubFromUrl"]!,
    "-t", builder.Configuration["Smee:GithubToUrl"]!)
    .WaitFor(webapi);

var telegramSmee = builder.AddExecutable(
    name: "telegram-smee",
    command: "smee",
    workingDirectory: builder.AppHostDirectory,
    "-u", builder.Configuration["Smee:TelegramFromUrl"]!,
    "-t", builder.Configuration["Smee:TelegramToUrl"]!)
    .WaitFor(webapi);

builder.Build().Run();
