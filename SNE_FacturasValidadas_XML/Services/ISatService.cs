using SNE_FacturasValidadas_XML.Models;
using System.Xml.Linq;

namespace SNE_FacturasValidadas_XML.Services
{    
        public interface ISatService
        {
            Task<string> ValidarAsync(
                string rfcEmisor,
                string rfcReceptor,
                decimal total,
                string uuid);
        }        
    
}
