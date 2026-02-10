using EmpInfoInfra.CatalogModels;
using EmpInfoInfra.ConncectionStrings;
using EmpInfoInfra.Models;
using EmpInfoInner.Config;
using EmpInfoInner.Middleware;
using EmpInfoService.Common;
using EmpInfoService.Mapper;
using EmpInfoService.Services.ServiceImpl;
using EmployeeInformationInner.Services.Interfaces;
using GlobalExceptionHandler;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using EmpInfoService.Constant;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Environment.IsDevelopment();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<DbConnectionStrings>(serviceProvider =>
{
    // 1. Create a scope to resolve Scoped services like MemoryCacheService
    using IServiceScope scope = serviceProvider.CreateScope();
    MemoryCacheService memoryCacheService = scope.ServiceProvider.GetRequiredService<MemoryCacheService>();

    // 2. Fetch the base connection strings from your cache
    string empBase = memoryCacheService.GetConnectionString(BussinessConstant.EMPLOYEE_CONNECTION_STRING_KEY).GetAwaiter().GetResult();
    string catBase = memoryCacheService.GetConnectionString(BussinessConstant.CATALOG_CONNECTION_STRING_KEY).GetAwaiter().GetResult();

    // 3. Build the final strings with credentials
    string empFull = new SqlConnectionStringBuilder(empBase)
    {
        UserID = builder.Configuration["DB_USER"],
        Password = builder.Configuration["DB_PASSWORD"]
    }.ConnectionString;

    string catFull = new SqlConnectionStringBuilder(catBase)
    {
        UserID = builder.Configuration["DB_USER"],
        Password = builder.Configuration["DB_PASSWORD"]
    }.ConnectionString;

    return new DbConnectionStrings
    {
        MainDb = empFull,
        CatalogDb = catFull
    };
});

builder.Services.AddDbContext<EmployeeDetailsContext>((serviceProvider, options) =>
{
    DbConnectionStrings connStrings = serviceProvider.GetRequiredService<DbConnectionStrings>();
    options.UseSqlServer(connStrings.MainDb);
});

builder.Services.AddDbContext<CatalogContext>((serviceProvider, options) =>
{
    DbConnectionStrings connStrings = serviceProvider.GetRequiredService<DbConnectionStrings>();
    options.UseSqlServer(connStrings.CatalogDb);
});

builder.Services.AddScoped<IEmployeeDetails, EmployeeDetails>();
builder.Services.AddScoped<EmpService>();
builder.Services.AddScoped<DesignationService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<EmployeeTypeService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<OrgChartService>();
builder.Services.AddScoped<UserPermissionService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<SqlEmployeeWatcher>();
builder.Services.AddAutoMapper(cnf => { }, typeof(MappingProfile).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CommonService>();
builder.Services.AddSingleton<MemoryCacheService>();

// Add CORS policy to allow requests from the remote application
string[] allowedOrigins = isDevelopment
    ? builder.Configuration.GetSection("CorsSettings:DevelopmentOrigins").Get<string[]>() ?? ["*"]
    : builder.Configuration.GetSection("CorsSettings:ProductionOrigins").Get<string[]>()
        ?.Where(origin => !string.IsNullOrWhiteSpace(origin)).ToArray() 
        ?? throw new InvalidOperationException("CORS production origins are missing.");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.WithOrigins(allowedOrigins)
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

WebApplication app = builder.Build();

app.UsePathBase("/emsapi");

// Force initialization of the SqlEmployeeWatcher service.
app.Services.GetRequiredService<SqlEmployeeWatcher>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
