using CoffeeScaffolding.CoffeeScaffoldingData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db
if (builder.Environment.IsProduction())
{
    builder.Services.AddDbContext<CoffeeScaffoldingDBContext>(x => x.UseSqlServer(""));
}
else
{
    builder.Services.AddDbContext<CoffeeScaffoldingDBContext>(x=>x.UseInMemoryDatabase("InMem"));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//pre database
PreDb.PrepPopulation(app,builder.Environment.IsProduction());

app.Run();

