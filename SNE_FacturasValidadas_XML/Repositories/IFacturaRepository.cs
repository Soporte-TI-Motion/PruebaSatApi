using SNE_FacturasValidadas_XML.Models;
namespace SNE_FacturasValidadas_XML.Repositories
{

    public interface IFacturaRepository
    {
        Task GuardarAsync(Factura factura);
    }
}
