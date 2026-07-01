using SNE_FacturasValidadas_XML.Models;
using System.Xml.Linq;

namespace SNE_FacturasValidadas_XML.Services
{
    public class XMLService : IXMLService
    {
        public FacturaResponse LeerXML(string XML)
        {
            XDocument doc = XDocument.Parse(XML);

            XNamespace cfdi =
                "http://www.sat.gob.mx/cfd/4";

            XNamespace tfd =
                "http://www.sat.gob.mx/TimbreFiscalDigital";

            var comprobante = doc.Root;

            var emisor =
                comprobante?.Element(cfdi + "Emisor");

            var receptor =
                comprobante?.Element(cfdi + "Receptor");

            var timbre =
                doc.Descendants(
                    tfd + "TimbreFiscalDigital")
                .FirstOrDefault();

            return new FacturaResponse
            {
                UUID =
                    timbre?.Attribute("UUID")?.Value
                    ?? string.Empty,

                RFCEmisor =
                    emisor?.Attribute("Rfc")?.Value
                    ?? string.Empty,

                RFCReceptor =
                    receptor?.Attribute("Rfc")?.Value
                    ?? string.Empty,

                Total =
                    decimal.Parse(
                        comprobante?
                        .Attribute("Total")?.Value
                        ?? "0"),

                EstadoSAT = string.Empty
            };
        }
    }
}