using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ViaAPI.Data;
using ViaAPI.Hubs;
using ViaAPI.Jobs;
using ViaAPI.Models;
using ViaAPI.Services.EmailService;
using ViaAPI.Services.FeedbackService;
using ViaAPI.Services.JwtService;
using ViaAPI.Services.LocalizationService;
using ViaAPI.Services.TourismService;
using ViaAPI.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Informe o token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<ApiDbContext>(options =>
   options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHostedService<ViaJob>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApiDbContext>()
    .AddDefaultTokenProviders();

//Pegando o Token e gerando a chave encodada
var JwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(JwtSettingsSection);

var jwtSettings = JwtSettingsSection.Get<JwtSettings>();

var key = Encoding.UTF8.GetBytes(jwtSettings.Segredo!);

builder.Services.AddScoped<IJwtService, JwtService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.Emissor,
        ValidAudience = jwtSettings.Audiencia
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Fail("Token inválido.");
                return;
            }
           
        }

    };
});


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserService, UserService>();


// Services
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<ITourismService, TourismService>();
builder.Services.AddHostedService<ViaJob>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ITravelHistoryService, TravelHistoryService>();
builder.Services.AddSingleton<ViaJob>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());
});

builder.Services.AddSignalR(x =>
{
    x.EnableDetailedErrors = true;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();


app.MapHub<LocalizationHub>("/localization");

app.Run();
