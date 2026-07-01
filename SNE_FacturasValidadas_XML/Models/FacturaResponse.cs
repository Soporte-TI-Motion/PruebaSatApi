namespace SNE_FacturasValidadas_XML.Models
{
    public class FacturaResponse
    {
        public string UUID { get; set; }
        public string RFCEmisor { get; set; }
        public string RFCReceptor { get; set; }
        public decimal Total { get; set; }
        public string EstadoSAT { get; set; }
    }
}
