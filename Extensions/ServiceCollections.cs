using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuantoEuCobro.Authentication;
using QuantoEuCobro.Infrastructure;
using QuantoEuCobro.Services;
using QuantoEuCobro.Services.Interfaces;
using System;
using System.Text;

namespace QuantoEuCobro.Extensions;

/// <summary>
/// Centraliza o registro de todos os serviços da aplicação.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AdicionarBancoDeDados(
        this IServiceCollection servicos,
        IConfiguration configuracao)
    {
        servicos.AddDbContext<AppDbContext>(opcoes =>
            opcoes.UseSqlite(configuracao.GetConnectionString("Padrao")));

        return servicos;
    }

    public static IServiceCollection AdicionarAutenticacaoJwt(
        this IServiceCollection servicos,
        IConfiguration configuracao)
    {
        var chave = configuracao["Jwt:ChaveSecreta"]
            ?? throw new InvalidOperationException("Chave JWT não configurada.");

        servicos
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opcoes =>
            {
                opcoes.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuracao["Jwt:Issuer"],
                    ValidAudience = configuracao["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(chave))
                };
            });

        servicos.AddAuthorization();
        servicos.AddScoped<IServicoToken, ServicoToken>();

        return servicos;
    }

    public static IServiceCollection AdicionarServicos(this IServiceCollection servicos)
    {
        servicos.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();
        servicos.AddScoped<IServicoProposta, ServicoProposta>();
        servicos.AddScoped<IServicoTemplate, ServicoTemplate>();

        return servicos;
    }

    public static IServiceCollection AdicionarSwaggerProfissional(this IServiceCollection servicos)
    {
        servicos.AddEndpointsApiExplorer();
        servicos.AddSwaggerGen(opcoes =>
        {
            opcoes.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Quanto eu cobro? — API",
                Version = "v1",
                Description = "API SaaS para freelancers criarem propostas profissionais.",
                Contact = new OpenApiContact
                {
                    Name = "Suporte",
                    Email = "suporte@quantoeucobro.com"
                }
            });

            // Habilita autenticação JWT no Swagger UI
            var esquemaSeguranca = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT. Exemplo: Bearer {seu_token}"
            };

            opcoes.AddSecurityDefinition("Bearer", esquemaSeguranca);
            opcoes.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });

            // Documenta com XML se o arquivo existir
            var arquivoXml = Path.Combine(AppContext.BaseDirectory, "QuantoEuCobro.xml");
            if (File.Exists(arquivoXml))
                opcoes.IncludeXmlComments(arquivoXml);
        });

        return servicos;
    }
}