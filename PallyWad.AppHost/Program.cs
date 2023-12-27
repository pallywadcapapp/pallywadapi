var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PallyWad_Auth>("pallywad.auth");

builder.AddProject<Projects.PallyWad_GateWay>("pallywad.gateway");

builder.Build().Run();
