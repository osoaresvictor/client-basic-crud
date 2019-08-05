using ClientBasicCrud.Repository.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace ClientBasicCrud.Application
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            this.RegisterDependencies(services);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.WithOrigins("http://localhost:5000")
                                                            .AllowAnyMethod()
                                                            .AllowAnyHeader()
                );
            });

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(
                    _configuration.GetConnectionString("ClientCrudDBConnection"),
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly("ClientBasicCrud.Application")
                )
            );

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => options.SerializerSettings.Formatting = Formatting.Indented);

            services.AddSwaggerGen(settings =>
            {
                settings.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Client Basic CRUD API",
                        Version = "v1",
                        Description = "Exemplo de API REST criada com ASP.NET Core + EF + Docker",
                        Contact = new Contact { Name = "Victor Soares", Email = "victor_soares@live.com" }
                    }
                );
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddTransient<IClientRepository, ClientRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client CRUD API");
            });

            app.UseCors();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
        }
    }
}
