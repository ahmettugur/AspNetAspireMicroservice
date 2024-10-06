using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//var username = builder.AddParameter("username", secret: true);
//var password = builder.AddParameter("password", secret: true);

var rabbitUser = builder.AddParameter("Username");
var rabbitPass = builder.AddParameter("Password", true);

var postgresUsername = builder.AddParameter("PostgresUsername");
var postgresPassword = builder.AddParameter("PostgresPassword", true);

var postgres = builder.AddPostgres("postgres", postgresUsername, postgresPassword, 5432);

var clientsDb = postgres
    .WithHealthCheck()
    .AddDatabase("ClientsDb");

var accountsDb = postgres
    .WithHealthCheck()
    .AddDatabase("AccountsDb");
    

var messaging = builder.AddRabbitMQ("rabbitmq-broker", rabbitUser, rabbitPass,port: 5672)
    .WithHealthCheck()
    .WithManagementPlugin();

var riskevaluator = builder.AddProject<Projects.RiskEvaluator_Grpc>("riskevaluator-grpc");

builder.AddProject<Projects.Clients_Api>("clients-api")
    .WithReference(clientsDb)
    .WithReference(messaging)
    .WithReference(riskevaluator)
    .WaitFor(clientsDb)
    .WaitFor(messaging)
    .WaitFor(riskevaluator)
    .WithExternalHttpEndpoints();



builder.AddProject<Projects.Accounts_Api>("accounts-api")
    .WithReference(accountsDb)
    .WithReference(messaging)
    .WaitFor(accountsDb)
    .WaitFor(messaging);



builder.AddProject<Projects.Notifications_Api>("notifications-api")
    .WithReference(messaging)
    .WaitFor(messaging);



builder.Build().Run();
