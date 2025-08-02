using Microsoft.EntityFrameworkCore;
using Vicporsy.Application;
using Vicporsy.Application.Mapper;
using Vicporsy.Data;
using Vicporsy.Data.Context;

var builder = WebApplication.CreateBuilder(args);

#region [Services]
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjectionRepositories();
builder.Services.AddDependencyInjectionServices();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
#endregion

#region [SQL]
var defaultConnString = builder.Configuration.GetConnectionString("DefaultConnection");
var logConnString = builder.Configuration.GetConnectionString("LogConnection");

var defaultOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlServer(defaultConnString)
    .Options;

var logOptions = new DbContextOptionsBuilder<LogDbContext>()
    .UseSqlServer(logConnString)
    .Options;

builder.Services.AddSingleton(new AppDbContext(defaultOptions));
builder.Services.AddSingleton(new LogDbContext(logOptions));

#endregion

#region [Redis]

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis") ??
                            builder.Configuration["Redis:ConnectionString"];
});

#endregion

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
