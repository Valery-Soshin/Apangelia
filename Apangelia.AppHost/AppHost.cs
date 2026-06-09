var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Apangelia_WebApi>("apangelia-webapi");

builder.Build().Run();