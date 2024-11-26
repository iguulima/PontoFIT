using Domain.EF;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Sistema
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
            //Instancia o contexto como um serviço, ou seja, o asp.core ficará responsável
            //por instanciar o Context
            services.AddDbContext<Context>(options =>
                options.
                    //Implementa o conceito de LazyLoading dos objetos
                    UseLazyLoadingProxies().
                    //Define SQL Server como banco de dados e busca URL de conexão no AppSettings
                    UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Home/Login");//401 - Unauthorized
                });

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            //Incluir serviço de Sessão
            services.AddSession();
            //Incluir serviço de Authoruzação
            services.AddAuthorization();

            //Incluir HttpContextAccessor (Acessar valores relacionados com contexto da conexão HTTP)
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //Instancia serviço de sessão
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            //Esse método adiciona um middleware ao pipeline de processamento
            //de requisições da aplicação para gerenciar a política de cookies.
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                //SameSiteMode.Strict: Define a política SameSite para Strict,
                //que é a configuração mais restritiva. Com essa configuração,
                //o cookie só será enviado se a requisição for originada do
                //mesmo site que configurou o cookie. Isso ajuda a proteger
                //contra ataques de falsificação de solicitação entre sites (CSRF).
                HttpOnly = HttpOnlyPolicy.Always
                //Define que todos os cookies terão o atributo HttpOnly
                //definido.O atributo HttpOnly indica que o cookie não deve
                //ser acessível via JavaScript(por meio da document.cookie),
                //o que ajuda a mitigar ataques de script entre sites(XSS).
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
