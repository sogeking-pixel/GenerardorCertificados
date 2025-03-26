using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;


class Program
{
    static void Main()
    {
        string basePath = "D:\\Programming\\C#\\Aprendiendo\\Consolitas\\GenerardorCertificados\\GenerardorCertificados\\";

        string templatePath = Path.Combine(basePath, "Template\\Plantilla.pdf");

        string outputPath = Path.Combine(basePath, $"PdfCreated\\Diploma_{"aluxdmno"}.pdf");

        string logoPath = Path.Combine(basePath, "Imagen\\images.png");

        try
        {
            // Abrir la plantilla PDF
            PdfReader reader = new PdfReader(templatePath);
            var pageSize = reader.GetPageSize(1);
            float pageWidth = pageSize.Width;
            float pageHeight = pageSize.Height;

            // Calcular la mitad del ancho
            float centerX = pageWidth / 2;
           
            Console.WriteLine("Plantilla PDF cargada exitosamente");
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            {
                PdfStamper stamper = new PdfStamper(reader, fs);
                PdfContentByte overContent = stamper.GetOverContent(1); // Se utiliza la primera página
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                AddTextToPdf(overContent, "Diploma de Reconocimiento", baseFont, 55, centerX, 500);
                AddTextToPdf(overContent, "Juan Perez", baseFont, 24, centerX, 470);


                // 2. Agregar una imagen PNG (por ejemplo, un logo)
                if (File.Exists(logoPath))
                {
                    Image logo = Image.GetInstance(logoPath);
                    logo.ScaleAbsolute(100, 100); // Escala de la imagen (ancho, alto)
                    // Posicionar la imagen (por ejemplo, en la esquina superior izquierda)
                    logo.SetAbsolutePosition(120, 100);
                    overContent.AddImage(logo);
                }

                // 3. Agregar un enlace interactivo (link)
                // Dibujar el texto del enlace
                string linkText = "Visita nuestro sitio web";
                float linkX = 300;
                float linkY = 440;
                overContent.BeginText();
                overContent.SetFontAndSize(baseFont, 12);
                overContent.SetColorFill(BaseColor.BLUE);
                overContent.ShowTextAligned(Element.ALIGN_CENTER, linkText, linkX, linkY, 0);
                overContent.EndText();

                // Definir el área activa del enlace (ajusta según el tamaño del texto)
                Rectangle linkRect = new Rectangle(linkX - 75, linkY - 5, linkX + 75, linkY + 10);
                PdfAnnotation linkAnnotation = PdfAnnotation.CreateLink(
                    stamper.Writer,
                    linkRect,
                    PdfAnnotation.HIGHLIGHT_INVERT,
                    new PdfAction("https://www.ejemplo.com")
                );
                stamper.AddAnnotation(linkAnnotation, 1);

                // Finalizar y cerrar el PDF
                stamper.Close();
            }
            reader.Close();

            Console.WriteLine("✅ PDF generado exitosamente en: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error: " + ex.Message);
        }
    }

    public static void AddTextToPdf(PdfContentByte overContent, string text, BaseFont baseFont,int sizeFont,  float x, float y)
    {
        overContent.BeginText();
        overContent.SetFontAndSize(baseFont, sizeFont);
        overContent.ShowTextAligned(Element.ALIGN_CENTER, text, x, y, 0);
        overContent.EndText();
           
    }
}
