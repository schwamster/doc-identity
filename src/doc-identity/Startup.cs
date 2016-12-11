using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4;
using Microsoft.IdentityModel.Tokens;

namespace doc_identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and resources
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryUsers(Config.GetUsers());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            // cookie middleware for temporarily storing the outcome of the external authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                AutomaticAuthenticate = false,
                AutomaticChallenge = false
            });

            // middleware for google authentication
            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                ClientId = "335717042634-ic4q03s59v6vdctckem1mp9cj407ad65.apps.googleusercontent.com",
                ClientSecret = "HtCpt80rESxczApZN-Jr_00Q"
            });

            // middleware for external openid connect authentication
            app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
            {
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
                SignOutScheme = IdentityServerConstants.SignoutScheme,

                DisplayName = "OpenID Connect",
                Authority = "https://demo.identityserver.io/",
                ClientId = "implicit",

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                }
            });


            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
