using EmpInfoInfra.Models;
using EmpInfoInner.Config;
using EmpInfoInner.Middleware;
using EmpInfoService.Mapper;
using EmpInfoService.Services.ServiceImpl;
using EmployeeInformationInner.Services.Interfaces;
using GlobalExceptionHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EmployeeDetailsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddAuthentication(JwtBeareD.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddScoped<IEmployeeDetails, EmployeeDetails>();
builder.Services.AddScoped<EmpService>();
builder.Services.AddScoped<DesignationService>();
builder.Services.AddScoped<EmployeeTypeService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AdminService>();

//builder.Services.AddScoped<AdminService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<SqlEmployeeWatcher>();
builder.Services.AddAutoMapper(cnf => { }, typeof(MappingProfile).Assembly);

// 🧩 1️⃣ Add CORS policy here
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5174") // your React app
              .AllowAnyHeader()
              .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Add controllers with filter globally
builder.Services.AddControllers(options =>
{
    options.Filters.Add<UnifiedResponseFilter>();
});

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
builder.ConfigureSerilog();
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Force initialization of the SqlEmployeeWatcher service.
app.Services.GetRequiredService<SqlEmployeeWatcher>();

var memoryCache = app.Services.GetRequiredService<IMemoryCache>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 🧩 2️⃣ Apply CORS *before* Authorization
app.UseCors("AllowFrontend");
//();

app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();
