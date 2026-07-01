using SNE_FacturasValidadas_XML.Data;
using SNE_FacturasValidadas_XML.Models;

namespace SNE_FacturasValidadas_XML.Repositories
{
    public class FacturaRepository : IFacturaRepository
    {
        private readonly AppDbContext _context;

        public FacturaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task GuardarAsync(Factura factura)
        {
            await _context.Facturas.AddAsync(factura);

            await _context.SaveChangesAsync();
        }
    }
}