namespace HttpConsumeExamples.DTOs
{
    public class ModeloResponse
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public MarcaResponse Marca { get; set; }
        public List<VersaoResponse> Versoes { get; set; }
    }
}