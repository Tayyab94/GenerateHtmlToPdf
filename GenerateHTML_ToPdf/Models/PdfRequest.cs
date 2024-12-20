namespace GenerateHTML_ToPdf.Models
{
    public class PdfRequest
    {
        public string Name { get; set; }
        public string Std { get; set; }
        public List<FeesStructure> fees { get; set; }
    }
}
