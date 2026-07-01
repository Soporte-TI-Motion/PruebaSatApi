using Microsoft.EntityFrameworkCore;
using SNE_FacturasValidadas_XML.Models;

namespace SNE_FacturasValidadas_XML.Data
{
    // Data/AppDbContext.cs
    public class AppDbContext : DbContext
    {
        

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Factura> Facturas { get; set; }
    }

}
