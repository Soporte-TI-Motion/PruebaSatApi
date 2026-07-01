using System.ComponentModel.DataAnnotations.Schema;

namespace SNE_FacturasValidadas_XML.Models
{
    [Table("SNE_FacturasValidadas_XML")]
    public class Factura
    {
        public int Id { get; set; }
        public string UUID { get; set; }
        public string RFCEmisor { get; set; }
        public string RFCReceptor { get; set; }
        public decimal Total { get; set; }
        public string EstadoSAT { get; set; }
        public DateTime FechaValidacion { get; set; }
    }
}
