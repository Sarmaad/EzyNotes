using EzyNotes.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EzyNotes.Infrastructure.Auth0
{
    public static class StartupExtensions
    {
        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            var auth0Options = builder.Configuration.GetSection(Auth0Options.Section).Get<Auth0Options>();
            if (auth0Options is null) throw new Exception("Missing auth0 configurations!");
            builder.Services.AddSingleton(auth0Options);

            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.Authority = $"https://{auth0Options.Domain}/";
                    opt.Audience = auth0Options.Audience;

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    opt.Events = new()
                    {
                        OnTokenValidated = async context =>
                        {
                            await using var scope = context.HttpContext.RequestServices.CreateAsyncScope();

                            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                            var repository = scope.ServiceProvider.GetRequiredService<IRepository>();

                            var sub = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);

                            if (string.IsNullOrWhiteSpace(sub))
                            {
                                // no user id in token, this should not happen!
                                context.Fail("invalid user!");
                                return;
                            }

                            var user = await repository.Users.Find(x => x.Id, sub, CancellationToken.None);
                            if (user is null)
                            {
                                user = await userService.EnrollUser(sub);
                            }

                            var claims = new List<Claim>
                            {
                                new(Constants.USER_ID_CLAIM_TYPE, user.Id),
                            };

                            if (context.Principal?.Identity is not ClaimsIdentity identity) return;

                            foreach (var claim in claims.Where(x => !identity.HasClaim(x.Type, x.Value)).ToList())
                            {
                                identity.AddClaim(claim);
                            }
                        }

                    };
                });
        }

        public static void ConfigureAuth0(this WebApplicationBuilder builder)
        {
            var auth0Options = builder.Configuration.GetSection(Auth0Options.Section).Get<Auth0Options>();
            if (auth0Options is null) throw new Exception("Missing auth0 configurations!");
            builder.Services.AddSingleton(auth0Options);

            builder.Services.AddSingleton<IAuthenticationApiClient>(_ => new AuthenticationApiClient(new Uri($"https://{auth0Options.Domain}/")));

            builder.Services.AddScoped<ManagementClientFactory>();
        }


    }
}
