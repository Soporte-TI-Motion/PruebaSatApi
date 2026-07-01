using System.Text;
using System.Xml.Linq;

namespace SNE_FacturasValidadas_XML.Services
{
    public class SatService : ISatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SatService> _logger;

        private const string ServiceUrl =
            "https://consultaqr.facturaelectronica.sat.gob.mx/ConsultaCFDIService.svc";

        public SatService(HttpClient httpClient, ILogger<SatService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> ValidarAsync(
            string rfcEmisor,
            string rfcReceptor,
            decimal total,
            string uuid)
        {
            try
            {
                string expresion = BuildExpresion(rfcEmisor, rfcReceptor, total, uuid);
                string soapBody = BuildSoapEnvelope(expresion);

                var content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction",
                    "http://tempuri.org/IConsultaCFDIService/Consulta");

                var response = await _httpClient.PostAsync(ServiceUrl, content);

                // ─── DEBUG TEMPORAL ──────────────────────────────────────
                string rawResponse = await response.Content.ReadAsStringAsync();
                _logger.LogError("SAT Status: {Status} | Body: {Body}",
                    response.StatusCode, rawResponse);
                // ────────────────────────────────────────────────────────


                response.EnsureSuccessStatusCode();

                string xmlResponse = await response.Content.ReadAsStringAsync();
                return ParseEstado(xmlResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el SAT para UUID {Uuid}", uuid);
                return $"Error: {ex.Message}";
            }
        }

        // ─── Helpers ────────────────────────────────────────────────────────────

        private static string BuildExpresion(
            string rfcEmisor, string rfcReceptor, decimal total, string uuid)
        {
            // El total debe ir sin ceros no significativos, ej: "1234.50" no "1234.500"
            string totalStr = total.ToString("0.##########");

            return $"?re={Uri.EscapeDataString(rfcEmisor)}" +
                   $"&rr={Uri.EscapeDataString(rfcReceptor)}" +
                   $"&tt={totalStr}" +
                   $"&id={uuid}";
        }
        private static string BuildSoapEnvelope(string expresion)
        {
            // ✅ Escapar los caracteres especiales que rompen el XML
            string expresionEscapada = System.Security.SecurityElement.Escape(expresion);

            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                  xmlns:tem=""http://tempuri.org/"">
  <soapenv:Header/>
  <soapenv:Body>
    <tem:Consulta>
      <tem:expresionImpresa>{expresionEscapada}</tem:expresionImpresa>
    </tem:Consulta>
  </soapenv:Body>
</soapenv:Envelope>";
        }
        private static string ParseEstado(string xmlResponse)
        {
            var doc = XDocument.Parse(xmlResponse);
            XNamespace ns = "http://schemas.datacontract.org/2004/07/Sat.Cfdi.Negocio.ConsultaCfdi.Servicio";

            var result = doc.Descendants(ns + "ConsultaResult").FirstOrDefault();

            if (result is null)
                return "No encontrado";

            string estado = result.Element(ns + "Estado")?.Value ?? "Desconocido";
            string codigoEstatus = result.Element(ns + "CodigoEstatus")?.Value ?? "";
            string efos = result.Element(ns + "ValidacionEFOS")?.Value ?? "";

            return $"{estado} | {codigoEstatus} | EFOS: {efos}";
        }
    }
}