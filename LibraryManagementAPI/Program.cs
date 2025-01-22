/*
Title:Library Management Api
Author : Nivea
Created at: 06/01/2025
Updated at: 16/01/2025
Reviewed by:
Reviewed at:
*/

using LibraryManagementAPI.Data;
using LibraryManagementAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<LibraryService>();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC", policy =>
    {
      policy.AllowAnyOrigin() 
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});

// Configure EF Core with SQL Server
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowMVC");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();