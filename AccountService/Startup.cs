using AccountService.DatastoreSettings;
using AccountService.Messaging;
using AccountService.Repositories;
using AccountService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AccountService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            
            services.AddTransient<IAccountService, Services.AccountService>();
            
            services.AddTransient<IAccountRepository, AccountRepository>();
            
            services.Configure<AccountDatabaseSettings>(
                Configuration.GetSection(nameof(AccountDatabaseSettings)));

            services.AddSingleton<IAccountDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<AccountDatabaseSettings>>().Value);
            
            services.AddMessagePublisher();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}