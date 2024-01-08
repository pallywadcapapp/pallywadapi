var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PallyWad_Auth>("pallywad.auth");

builder.AddProject<Projects.PallyWad_GateWay>("pallywad.gateway");

builder.AddProject<Projects.PallyWad_Setup>("pallywad.setup");

builder.AddProject<Projects.Pallwad_Accounting>("pallwad.accounting");

builder.AddProject<Projects.PallyWad_UserApi>("pallywad.userapi");

builder.Build().Run();
