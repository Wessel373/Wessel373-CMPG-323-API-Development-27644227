using Microsoft.OpenApi.Models;
using CMPG323_API_Project.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CMPG323_API_Project.Models;


namespace CMPG323_API_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration
        {
            get;
        }
        // This method gets called by the runtime. Use this method to add services to the container.  
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            /*services.AddEntityFrameworkSqlServer();
            services.AddDbContextPool<CMPG323_Project_2_DBContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer("Password=ser@WD276;Persist Security Info=False;User ID=WD27644227;Initial Catalog=CMPG323_Project_2_DB;Data Source=serv27644227proj2.database.windows.net");
                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });*/

            services.AddDbContextPool<ApplicationDBContext>(options =>
            options.UseSqlServer("Password=ser@WD276;Persist Security Info=False;User ID=WD27644227;Initial Catalog=CMPG323_Project_2_DB;Data Source=serv27644227proj2.database.windows.net"));
            
            //services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Password=ser@WD276;Persist Security Info=False;User ID=WD27644227;Initial Catalog=CMPG323_Project_2_DB;Data Source=serv27644227proj2.database.windows.net")));
            // For Identity  
            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "JWTToken_Auth_API",
                    Version = "v2"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

            services.AddIdentity<ApplicationUser,
            IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();
            // Adding Authentication  
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ) // Adding Jwt Bearer  
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            }
            );
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            }
            );
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2/swagger.json", "Project 2 API"));

        }
    }
}
