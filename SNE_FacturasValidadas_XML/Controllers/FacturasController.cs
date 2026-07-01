using Microsoft.AspNetCore.Mvc;
using SNE_FacturasValidadas_XML.Models;
using SNE_FacturasValidadas_XML.Repositories;
using SNE_FacturasValidadas_XML.Services;

namespace SNE_FacturasValidadas_XML.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly IXMLService _XMLService;
        private readonly ISatService _satService;
        

        public FacturasController(
            IXMLService XMLService,
            ISatService satService
            )
        {
            _XMLService = XMLService;
            _satService = satService;
            
        }

        [HttpPost("validar")]
        public async Task<IActionResult> Validar(IFormFile archivo)
        {
            //Validación del archivo recibido
            if (archivo is null || archivo.Length == 0)
                return BadRequest("No se recibió ningún archivo.");
            if (!archivo.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                return BadRequest("El archivo debe ser un XML.");
            //Leer el contenido del archivo XML
            using var reader = new StreamReader(archivo.OpenReadStream());
            string XML = await reader.ReadToEndAsync();
            var factura = _XMLService.LeerXML(XML);
            factura.EstadoSAT = await _satService.ValidarAsync(factura.RFCEmisor,factura.RFCReceptor,factura.Total,factura.UUID);
            
            //Aqui vamos a mandar el JSON a la api de Isra 
            //await _httpClient.PostAsJsonAsync("https://api.com/endpoint", factura);

            return Ok(factura);
        }
    }
}