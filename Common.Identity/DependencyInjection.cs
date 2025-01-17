using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Common.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            if (!jwtSettingsSection.Exists())
            {
                throw new InvalidOperationException("JwtSettings section is missing in configuration.");
            }

            var key = configuration["JwtSettings:Key"];
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("JWT Key is not configured.");
            }

            var issuer = configuration["JwtSettings:Issuer"];
            var audience = configuration["JwtSettings:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                    ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };

                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = async c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "text/plain";
                        await c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = async context =>
                    {
                        if (context.Response.HasStarted)
                        {
                            await context.Response.WriteAsync("JWT token key expired");
                        }
                        else
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new { message = "401 Not authorized" });
                            await context.Response.WriteAsync(result);
                        }
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new { message = "403 Forbidden" });
                        await context.Response.WriteAsync(result);
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentityServices(configuration);
            services.AddTransient<IIdentityService, IdentityService>();
            return services;
        }
    }
}
