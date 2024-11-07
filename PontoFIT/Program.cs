using Microsoft.EntityFrameworkCore;
using PontoFIT.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar o DbContext usando a string de conexão
builder.Services.AddDbContext<PontoFitContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar outros serviços, como controllers, autenticação, etc.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
