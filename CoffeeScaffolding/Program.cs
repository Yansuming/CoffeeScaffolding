using CoffeeScaffolding.CoffeeHostServices;
using CoffeeScaffolding.CoffeeScaffoldingData;
using CoffeeScaffolding.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CoffeeScaffolding.CoffeeScaffoldingUtil.Json;
using CoffeeScaffolding.CoffeeScaffoldingUtil.Quartz;

// Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/CoffeeScaffoldingLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();
    // Add services to the container.
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<CoffeeScaffolding.Controllers.Filters.ActionFilter>();
    })
    .AddJsonOptions(o => {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
    });
    // builder.Services.AddScoped<CoffeeScaffolding.Filters.ActionLogFilter>();
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
        builder.Services.AddDbContext<CoffeeScaffoldingDBContext>(x => x.UseInMemoryDatabase("InMem"));
        builder.Services.AddDbContext<CoffeeIdentityDbContext>(x => x.UseInMemoryDatabase("InMem"));
    }
    builder.Services.AddQuartz();
    builder.Services.AddDataProtection();
    builder.Services.AddIdentityCore<CoffeeUser>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequiredLength = 10;
        opt.Password.RequireNonAlphanumeric = true;

        opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;
        opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
    });
    IdentityBuilder identityBuilder = new IdentityBuilder(typeof(CoffeeUser), typeof(CoffeeRole), builder.Services);
    identityBuilder.AddEntityFrameworkStores<CoffeeIdentityDbContext>().AddDefaultTokenProviders().AddUserManager<UserManager<CoffeeUser>>().AddRoleManager<RoleManager<CoffeeRole>>();

    //hostService
    builder.Services.AddHostedService<ExportDataHostService>();

    //MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
    //JWT
    builder.Services.AddAuthentication(options =>
    {
        //认证middleware配置
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "CoffeeScaffolding.com",
            ValidAudience = "CoffeeScaffolding",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("_@999888CoffeeScaffolding-CoffeeScaffolding-CoffeeScaffolding#$%"))
        };
    });
    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAuthorization();

    app.MapControllers();

    //pre database
    PreDb.PrepPopulation(app, builder.Environment.IsProduction());

    app.RunQuartzJob("");//Cron表达式

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Coffee Scaffolding terminated unexpectedly");    
}
finally
{
    Log.CloseAndFlush();
}



