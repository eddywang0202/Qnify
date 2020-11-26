using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qnify.Service;
using Qnify.Service.Interface;
using Qnify.Utility;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Serilog.Events;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Qnify
{
    public class Startup
    {
        //public static IConfigurationRoot ConfigurationRoot { get; set; }

        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            Configuration = builder.Build();
            Config.AppSettings = Configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILayoutService, LayoutService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerSetService, AnswerSetService>();
            services.AddScoped<IAnswerActionService, AnswerActionService>();
            services.AddScoped<ICasePropertyService, CasePropertyService>();
            services.AddScoped<ICaseService, CaseService>();
            services.AddScoped<ITestCasePropertyService, TestCasePropertyService>();
            services.AddScoped<ITestCaseService, TestCaseService>();
            services.AddScoped<ITestSetService, TestSetService>();
            services.AddScoped<IUserTestSetAnswerService, UserTestSetAnswerService>();
            services.AddScoped<IUserTestCaseAnswerService, UserTestCaseAnswerService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserTestQuestionAnswerService, UserTestQuestionAnswerService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IDemographicQuestionService, DemographicQuestionService>();
            services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();
            services.AddScoped<ICellService, CellService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITestService, TestService>();

            //Allow cross origin resource sharing
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .SetIsOriginAllowed(x => x == "http://localhost:8080" || x == "http://104.248.150.50")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌)
                    .RequireAuthenticatedUser()
                    .RequireRole("admin", "member")
                    .Build());
            });

            var key = Encoding.ASCII.GetBytes(Configuration["Token:Issuer"].ToString());
            var secretSigningKey = Encoding.ASCII.GetBytes(Configuration["Token:Secret"].ToString());
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretSigningKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(lc =>
                    lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information)
                        .WriteTo.RollingFile("Log/Qnify_Information.txt", outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(lc =>
                    lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error)
                        .WriteTo.RollingFile("Log/Qnify_Error.txt", outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .WriteTo.Logger(lc =>
                    lc.Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Fatal)
                        .WriteTo.RollingFile("Log/Qnify_Critical.txt", outputTemplate: "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"))
                .CreateLogger();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Qnify WebAPI", Version = "v1" });
                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var modelXmlPath = Path.Combine(basePath, "Qnify.Model.xml");
                c.IncludeXmlComments(modelXmlPath);
            });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Qnify WebAPI v1");
                });

                app.UseCors(
                    "AllowAll"
                );

                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Use production cors settings
                app.UseCors(
                    "AllowAll"
                );

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
