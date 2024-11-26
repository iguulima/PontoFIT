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
            //Instancia o contexto como um servi�o, ou seja, o asp.core ficar� respons�vel
            //por instanciar o Context
            services.AddDbContext<Context>(options =>
                options.
                    //Implementa o conceito de LazyLoading dos objetos
                    UseLazyLoadingProxies().
                    //Define SQL Server como banco de dados e busca URL de conex�o no AppSettings
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

            //Incluir servi�o de Sess�o
            services.AddSession();
            //Incluir servi�o de Authoruza��o
            services.AddAuthorization();

            //Incluir HttpContextAccessor (Acessar valores relacionados com contexto da conex�o HTTP)
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
            //Instancia servi�o de sess�o
            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            //Esse m�todo adiciona um middleware ao pipeline de processamento
            //de requisi��es da aplica��o para gerenciar a pol�tica de cookies.
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
                //SameSiteMode.Strict: Define a pol�tica SameSite para Strict,
                //que � a configura��o mais restritiva. Com essa configura��o,
                //o cookie s� ser� enviado se a requisi��o for originada do
                //mesmo site que configurou o cookie. Isso ajuda a proteger
                //contra ataques de falsifica��o de solicita��o entre sites (CSRF).
                HttpOnly = HttpOnlyPolicy.Always
                //Define que todos os cookies ter�o o atributo HttpOnly
                //definido.O atributo HttpOnly indica que o cookie n�o deve
                //ser acess�vel via JavaScript(por meio da document.cookie),
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
