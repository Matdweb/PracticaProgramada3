// Program.cs del proyecto MVC (PracticaProgramada3) cuando la API es un proyecto APARTE (opción A).
// El MVC no usa base de datos: solo consume la API con HttpClient.
using Microsoft.EntityFrameworkCore;
using PracticaProgramada3.Infrastructure.Data;
using PracticaProgramada3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cliente HTTP tipado: cada vez que se pide IVotacionApiClient, llega con el HttpClient ya configurado.
// La dirección de la API se lee de appsettings.json (ApiSettings:BaseUrl).
builder.Services.AddHttpClient<IVotacionApiClient, VotacionApiClient>(client =>
{
    var urlBase = builder.Configuration["ApiSettings:BaseUrl"]
        ?? throw new InvalidOperationException("Falta ApiSettings:BaseUrl en appsettings.json.");

    client.BaseAddress = new Uri(urlBase);
    client.Timeout = TimeSpan.FromSeconds(15);
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
