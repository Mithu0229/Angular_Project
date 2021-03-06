using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

     services.AddCors(o => o.AddPolicy("MyPolicy", builder =>{
        builder
        .WithOrigins(new[]{"http://localhost:4200"})
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));
            services.AddControllers();
            services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DataContext")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DatingApp.API", Version = "v1" });
            });
           // services.AddCors();
        //     services.AddCors(options =>
        // {
        //     options.AddPolicy(name: "MyPolicy",
        //                       builder =>
        //                       {
        //                           builder.WithOrigins("http://localhost:4200",
        //                                               "http://www.contoso.com");
        //                       });
        // });


   

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=> {
            options.TokenValidationParameters= new TokenValidationParameters(){
                ValidateIssuerSigningKey=true,
                IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                ValidateIssuer=false,
                ValidateAudience=false
            };
        });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DatingApp.API v1"));
            }

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
             
            app.UseAuthentication();
           // app.UseMvc();
        }
    }
}
