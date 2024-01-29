using Microsoft.EntityFrameworkCore;
using MindMasterMinds_API.Configurations;
using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MindMasterMinds_DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.WithOrigins(
                              "http://localhost:3000");
                          policy.AllowCredentials();
                      });
});

builder.Services.AddSwagger();
builder.Services.AddDependenceInjection();
builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.AddAutoMapper(typeof(GeneralProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger(c =>
{
    c.RouteTemplate = "/api/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "production");
    c.RoutePrefix = "api/swagger";
});

app.UseCors(MyAllowSpecificOrigins);

app.UseJwt();

app.UseExceptionHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
