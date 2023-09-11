using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Use_Wheels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Use_Wheels.Auth;
using Use_Wheels.Services;
using Microsoft.AspNetCore.Diagnostics;

// Sets up the builder object for creating a web application
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Also creates a cache profile named as 'Default 30'
builder.Services.AddControllers(option => {
    option.CacheProfiles.Add(Constants.Configurations.CACHE_PROFILE_NAME,
       new CacheProfile()
       {
           Duration = 20
       });
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Adds response caching to the application's services
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

// Gets the secret key from appsettings file
var key = builder.Configuration.GetValue<string>(Constants.Configurations.JWT_SECRET_CONFIGURATION_KEY);

// Configures and adds the API Explorer is a tool that generates documentation for API, including information about routes, controllers, actions, request and response models
builder.Services.AddEndpointsApiExplorer();

// Configures and adds identity management services
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddTokenProvider<DataProtectorTokenProvider<User>>(Constants.Configurations.TOKEN_PROVIDER_NAME);

// Gets connection string from appsettings file
var connectionString = builder.Configuration.GetConnectionString(Constants.Configurations.SQL_CONFIGURATION_KEY);

// Adds DbContext to the application
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Configure authentication services
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
}
);

// Connects to Redis DB
var redis = ConnectionMultiplexer.Connect(Constants.Configurations.REDIS_CONNECTION_KEY);
builder.Services.AddScoped(s => redis.GetDatabase());

// Configures authorization services
builder.Services.AddAuthorization();

// Registers a service with a scoped lifetime in the application's dependency injection (DI) container
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAdminCarServices, AdminCarServices>();
builder.Services.AddScoped<IAdminCategoriesServices, AdminCategoriesServices>();
builder.Services.AddScoped<IUserCarServices, UserCarServices>();
builder.Services.AddScoped<IUserCategoriesServices, UserCategoriesServices>();
builder.Services.AddScoped<IUserOrderServices, UserOrderServices>();
builder.Services.AddScoped<IUserWishlistServices, UserWishlistServices>();

// Sets up Auto mapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Configures logger
Log.Logger = new LoggerConfiguration().
    MinimumLevel.Information()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Host.UseSerilog();
builder.Logging.AddSerilog();

// Configures Swagger to document the security requirement
builder.Services.AddSwaggerGen(options =>
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    })
 );

builder.Services.AddSwaggerGen(options =>
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = Constants.Swagger.JWT_SECURITY_DESCRIPTION,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    })
);

// Prepares the application to start handling HTTP requests after all setup work is completed
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Automatically redirect HTTP requests to their equivalent HTTPS thus implementing security
app.UseHttpsRedirection();

// Authenticate the user before they're allowed access to secure resources
app.UseAuthentication();

// Authorizes a user to access secure resources
app.UseAuthorization();

// Exception handler
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

        if (exception is BadHttpRequestException)
        {
            var obj = (BadHttpRequestException)exception;
            context.Response.StatusCode = obj.StatusCode;
            await context.Response.WriteAsJsonAsync(obj.Message);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var response = new { error = exception.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// Adds middleware thats validates the token
app.UseMiddleware<TokenValidator>();

// Maps and routes HTTP requests to the appropriate controller action methods based on the URL and HTTP verb (GET, POST,)
app.MapControllers();

// Enables response caching
app.UseResponseCaching();
app.Run();

