using Microsoft.EntityFrameworkCore;
using Betonchel.Domain;
using Betonchel.Domain.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DomainContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("BetonchelDB"))
);


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();