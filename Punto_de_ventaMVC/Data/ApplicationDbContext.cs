using Microsoft.EntityFrameworkCore;
using Punto_de_ventaMVC.Models;
using System.Collections.Generic;

namespace Punto_de_ventaMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Punto_de_ventaMVC.Models.Venta> Venta { get; set; } = default!;
        // Añade más DbSets para otras tablas
    }
}