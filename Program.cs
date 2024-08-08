using Microsoft.EntityFrameworkCore;

// This is the entry point of application. It is where the execution of the program starts.
// Using the WebApplication.CreateBuilder method to create a new instance of the builder.
var builder = WebApplication.CreateBuilder(args);

// The builder is used to configure and build the WebApplication, also add services to the container using the Services property.
// In this case, I'm adding two services: Controllers and DbContext.

// The Controllers service is used to configure and add controllers to the pipeline. Controllers are responsible for handling HTTP requests and returning HTTP responses.

// Add services to the container.
builder.Services.AddControllers();

// The DbContext service is used to configure and add a database context to the pipeline. The WarehouseContext is responsible for managing the database connection and mapping the entities to the database tables.
builder.Services.AddDbContext<WarehouseContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// The EndpointsApiExplorer service is used to configure the API endpoints. It provides information about the available endpoints to tools like Swagger.
builder.Services.AddEndpointsApiExplorer();

// The SwaggerGen service is used to configure Swagger/OpenAPI documentation. It generates the API documentation based on the controllers and their attributes.
builder.Services.AddSwaggerGen();

// The builder is used to build the WebApplication. This step initializes the services and configures the HTTP request pipeline.
var app = builder.Build();


// The HTTP request pipeline is configured here. It determines how HTTP requests are handled by the application.

// If the application is in development environment, the Swagger UI is enabled. Swagger UI is a web interface for viewing the API documentation.
// HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// The HTTPS redirection is enabled. This ensures that all HTTP requests are redirected to HTTPS.
app.UseHttpsRedirection();

// Authorization is enabled. 
// This allows the application to authenticate and authorize users based on their roles and permissions.
app.UseAuthorization();

// The controllers are mapped to the HTTP request pipeline. 
// This means that incoming HTTP requests are checked for matching routes and corresponding controller actions are executed.
app.MapControllers();

app.Run();

