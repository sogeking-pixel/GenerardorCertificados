
using System.Text;
using GenerardorCertificados;
using iTextSharp.text;
using iTextSharp.text.pdf;



class Program
{
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string basePath = "D:\\Programming\\C#\\Aprendiendo\\Consolitas\\GenerardorCertificados\\GenerardorCertificados\\";

        
        CertificadoData data = new CertificadoData
        {
            RecipientName = "Yerson Omar Sanchez Leiva",
            Role = "DESARROLLADOR DE SOFTWARE",
            ProjectName = "SOFTWARE WEB DE GESTION BIBLIOTECARIA CON ARQUITECTURA CLIENTE SERVIDOR",
            ProjectPeriod = "desde 09 de octubre 2023 al 29 de marzo del 2024",
            Signatories = new[]
            {
                    new Signatory { Name = "Sanchez Leiva, Yerson", Title = "Director Ejecutivo" },
                    new Signatory { Name = "Sanchez Leiva, Yerson", Title = "Director Ejecutivo Interino" }
                }
        };

        
        PathConfig paths = new PathConfig(basePath)
        {
            TemplatePath = "Template\\Plantilla.pdf",
            OutputFileName = $"Diploma_{data.RecipientName.Replace(" ", "_")}.pdf",
            SignaturePath = "Imagen\\Firma.png",
            LogoPath = "Imagen\\LogoEmpresa.png",
            QrPath = "Imagen\\Qr.png",
            NameFontPath = "Fonts\\AlexBrush-Regular.ttf",
            BodyFontPath = "Fonts\\Josefin_Sans\\static\\JosefinSans-Regular.ttf",
            TitleFontPath = "Fonts\\Josefin_Sans\\static\\JosefinSans-Bold.ttf"
        };

        // Generate certificate
        CertificacionGeneration generator = new CertificacionGeneration(paths);
        try
        {
            generator.Generate(data);
            Console.WriteLine("PDF generado exitosamente en: " + paths.GetOutputPath());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
