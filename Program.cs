using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Reflection;
using Serilog;
using STEPIFY;
using STEPIFY.CloudinarySettings;
using STEPIFY.Repositories;
using STEPIFY.Service;
using STEPIFY.Services;
using STEPIFY.DataAccess;
using STEPIFY.MiddleWare;
using STEPIFY.Interfaces.IAddress;
using STEPIFY.Interfaces.Authentication;
using STEPIFY.Interfaces.IOrder;
using STEPIFY.Interfaces.IProduct;
using STEPIFY.Interfaces.IUser;
using STEPIFY.Interfaces.IWishList;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5175") // React app URL
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// ? Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // ?? Ensures Serilog is used in dependency injection

// ? Load Configuration from appsettings.json
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];

if (string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtSecretKey))
{
    throw new ArgumentNullException("JWT settings (Issuer, Audience, SecretKey) are missing in appsettings.json");
}

// ? Add Services to DI Container
builder.Services.AddControllers();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // ?? AutoMapper setup

// ? Configure Database Connection
builder.Services.AddDbContext<StepifyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// ? Configure Swagger with JWT Support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stepify API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, new string[] {} }
    });
});

// ? Register Application Services (Dependency Injection)
builder.Services.AddScoped<IAuthenticationServ, AuthenticationService>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAddressRepo, AddressRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWishListRepo, WishListRepo>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuthenticationRepo, AuthenticationRepo>();
builder.Services.AddScoped<IOrderRepo, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ? Configure Cloudinary Settings
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

// ? Build Application
var app = builder.Build();
app.UseCors("AllowFrontend"); //for frontend connection
// ? Configure Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GetUserIdMiddleware>(); // ?? Place middleware AFTER Authentication & Authorization

app.MapControllers();
app.Run();
