using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Contact.API.Data;
using Contact.API.Event;
using Contact.API.Infrastructure;
using Contact.API.Service;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Contact.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddContactDBContext<ContactDBContext>(Configuration.GetSection("ContactDBContextSettings"));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "contact_api";
                    options.Authority = "http://localhost";
                    options.SaveToken = true;
                });
            services.AddTransient<ISubscriberService, SubscriberService>();
            services.AddCap(x =>
            {
                x.UseMySql("server=127.0.0.1;port=3306;database=user_cap;uid=root;pwd=password;");
                x.UseRabbitMQ("localhost");
                x.UseDashboard();
                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5002;
                    d.NodeId = "2";
                    d.NodeName = "CAP No.2 Node";
                });
            });
            services.AddServiceDiscovery(Configuration.GetSection("ServiceDiscovery"));
            services.AddSingleton(new HttpClient());
            services.AddScoped<IUserService, UserService>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
