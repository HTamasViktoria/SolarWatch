using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch;
using SolarWatch.Model;
using SolarWatch.Service;
using SolarWatch.Service.Authentication;
using SolarWatch.Service.Repository;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);
        ConfigureSwagger(builder.Services);
        ConfigureAuthentication(builder.Services, builder.Configuration);
        ConfigureDatabaseContexts(builder.Services, builder.Configuration);
        ConfigureCustomServices(builder.Services);
        ConfigureIdentity(builder.Services);

        var app = builder.Build();
        using var scope = app.Services.CreateScope(); // AuthenticationSeeder is a scoped service, therefore we need a scope instance to access it
        var authenticationSeeder = scope.ServiceProvider.GetRequiredService<AuthenticationSeeder>();
        authenticationSeeder.AddRoles();
        authenticationSeeder.AddAdmin();
        ConfigureMiddleware(app);
        
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var key = jwtSettings["IssuerSigningKey"];
        var issuer = jwtSettings["ValidIssuer"];
        var audience = jwtSettings["ValidAudience"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                };
            });
    }

    private static void ConfigureDatabaseContexts(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MSSQL_CONNECTION");
        
        services.AddDbContext<SolarWatchApiContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddDbContext<UsersContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureCustomServices(IServiceCollection services)
    {
        services.AddSingleton<ICoordinateProvider, CoordinateProviderApi>();
        services.AddSingleton<ISunsetSunriseProvider, SunsetSunriseProvider>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ISunDataRepository, SunDataRepository>();
        services.AddSingleton<IJsonProcessor, JsonProcessor>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<AuthenticationSeeder>();
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services
            .AddIdentityCore<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>() //Enable Identity roles 
            .AddEntityFrameworkStores<UsersContext>();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}
