using CoffeeScaffolding.CoffeeHostServices;
using CoffeeScaffolding.CoffeeScaffoldingData;
using CoffeeScaffolding.Identity;
using Microsoft.AspNetCore.Identity;
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
    builder.Services.AddDbContext<CoffeeIdentityDbContext>(x => x.UseSqlServer(""));
}
else
{
    builder.Services.AddDbContext<CoffeeScaffoldingDBContext>(x=>x.UseInMemoryDatabase("InMem"));
    builder.Services.AddDbContext<CoffeeIdentityDbContext>(x => x.UseInMemoryDatabase("InMem"));
}

builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<CoffeeUser>(opt => { 
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequiredLength = 10;
    opt.Password.RequireNonAlphanumeric = true;

    opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
    opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
});
IdentityBuilder identityBuilder = new IdentityBuilder(typeof(CoffeeUser), typeof(CoffeeRole),builder.Services);
identityBuilder.AddEntityFrameworkStores<CoffeeIdentityDbContext>().AddDefaultTokenProviders().AddUserManager<UserManager<CoffeeUser>>().AddRoleManager<RoleManager<CoffeeRole>>();

//hostService
builder.Services.AddHostedService<ExportDataHostService>();


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

