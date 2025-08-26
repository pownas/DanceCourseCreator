var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DanceCourseCreator_API>("dancecoursecreator-api");

builder.AddProject<Projects.DanceCourseCreator_Client>("dancecoursecreator-blazorclient")
    .WaitFor(api)
    .WithReference(api);

builder.Build().Run();
