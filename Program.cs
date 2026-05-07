using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using QuantoEuCobro.Infrastructure;
using QuantoEuCobro.Services;
using QuantoEuCobro.Services.Interfaces;
using QuantoEuCobro.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ==========================================================
// 1️⃣ Controllers + Swagger
// ==========================================================

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ChaveSecreta"]!)
        )
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quanto eu cobro? API",
        Version = "v1",
        Description = "API para geração e gerenciamento de propostas profissionais."
    });

    // XML Comments (Swagger bonito)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// ==========================================================
// 2️⃣ Database
// ==========================================================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

// ==========================================================
// 3️⃣ Services
// ==========================================================

builder.Services.AddScoped<IServicoProposta, ServicoProposta>();
builder.Services.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();
builder.Services.AddScoped<IServicoTemplate, ServicoTemplate>();
builder.Services.AddScoped<IServicoToken, ServicoToken>();

// ==========================================================
// 4️⃣ CORS
// ==========================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ==========================================================
// 5️⃣ Build app
// ==========================================================

var app = builder.Build();

// ==========================================================
// 6️⃣ Database Migration
// ==========================================================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
}

// ==========================================================
// 7️⃣ Middleware pipeline
// ==========================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quanto eu cobro? API v1");
        c.RoutePrefix = "swagger";
    });
}

//app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();