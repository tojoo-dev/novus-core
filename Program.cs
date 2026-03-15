using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Novus.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Register all IEndpoint implementations automatically
builder.Services.AddEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Automatically map all endpoints discovered in the assembly
app.MapEndpoints();

app.Run();
