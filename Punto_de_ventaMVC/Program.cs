using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Data;
using Punto_de_ventaMVC.ModelBinders;

var builder = WebApplication.CreateBuilder(args);

// Agregar la cadena de conexión al contexto de base de datos (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PuntoDeVentaConnection"))
);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Esto inserta el Model Binder personalizado para forzar el uso del PUNTO como separador decimal (cultura Invariante)
    // para todos los tipos 'decimal'.
    options.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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