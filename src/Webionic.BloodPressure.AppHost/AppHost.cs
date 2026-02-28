var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Webionic_BloodPressure>("webionic-bloodpressure");

builder.Build().Run();
