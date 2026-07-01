using SNE_FacturasValidadas_XML.Models;

namespace SNE_FacturasValidadas_XML.Services
{

    public interface IXMLService
    {
        FacturaResponse LeerXML(string XML);
    }
}
