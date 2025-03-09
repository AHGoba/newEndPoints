using AssetsManagementEG.Context.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AssetsManagementEG.Models.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.Repositories.Many_ManyRepo;

namespace AssetsManagementEG.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<DBSContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DB")));
            services.AddScoped<TaskRepository>();
            services.AddScoped<DistrictRepository>();
            services.AddScoped<CarRepository>();
            services.AddScoped<EquipmentRepository>();
            services.AddScoped<LaborsRepository>();
            services.AddScoped<CompanyLRepository>();
            services.AddScoped<ContractCarRepository>();


            services.AddScoped<MDistrictCarRepo>();
            services.AddScoped<MDistrictEquipmentRepo>();
            services.AddScoped<MDistrictLaborsRepo>();
            services.AddScoped<MCompanyLaborsRepo>();


            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["jwt:key"])),
                    ValidateIssuerSigningKey = true,
                    //ValidAudience = builder.Configuration["jwt:audience"],
                    //ValidateAudience = true,
                    //ValidIssuer = builder.Configuration["jwt:issuer"],
                    //ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(op => op.SignIn.RequireConfirmedAccount = false)
               .AddEntityFrameworkStores<DBSContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
