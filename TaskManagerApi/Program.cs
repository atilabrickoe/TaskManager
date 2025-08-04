using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.DataManagers.Tasks;
using Repository.DataManagers.Users;
using System.Text;
using TaskManagerApplication.Users.Commands.CreateRandomUsers;
using TaskManagerDomain.Interfaces;
using TaskManagerMessaging.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

#region JWT Bearer
// configure the application to authenticate users using JWT tokens,
// verifying the issuer, audience, lifetime, and signing key
// of the issuer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //defines the valid issuer and audience for the token
            //JWT obtained from the application
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //Defines the signing key used to sign and
            //verify the JWT token.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerApi", Version = "v1" });

    // Define a secure schema for JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization using Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Implements authentication on all API endpoints
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

#endregion

// Add services to the container.
builder.Services.AddControllers();

// Register MediatR for CQRS pattern (add any assembly from application)
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateRandomUsersCommandHandler).Assembly);
});

//adding logged in user to context
builder.Services.AddHttpContextAccessor();

#region Ioc
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<INotificationService, NotificationService>();


#endregion

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Sql"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JSON serialization options (add custom enums descriptions to swagger documentation)
//builder.Services.AddControllersWithViews().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

var app = builder.Build();

#region Run EF Core migrations automatically at startup
using (var scope = app.Services.CreateScope())
{
    //try once per minute, maximum number of attempts 10
    var maxAttempts = 10;
    var delay = TimeSpan.FromSeconds(60);
    var connected = false;

    for (int attempt = 1; attempt <= maxAttempts && !connected; attempt++)
    {
        try
        {
            Console.WriteLine($"Attempt {attempt}/{maxAttempts} to connect to SQL Server...");

            using (var innerScope = app.Services.CreateScope())
            {
                var db = innerScope.ServiceProvider.GetRequiredService<TaskManagerDbContext>();

                db.Database.Migrate();
                // Check if the database is ready
                if (db.Database.CanConnect())
                {
                    Console.WriteLine("SQL Server is ready!");
                    connected = true;
                }
                else
                {
                    throw new Exception("Database not ready");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to connect: {ex.Message}");
            if (attempt == maxAttempts) throw;// give up after last attempt
            Thread.Sleep(delay);
        }
    }
}

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.Run();
}

// Required for integration testing
public partial class Program { }
