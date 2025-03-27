
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Utilities.Encoders;
using static System.Collections.Specialized.BitVector32;


class Program
{
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string basePath = "D:\\Programming\\C#\\Aprendiendo\\Consolitas\\GenerardorCertificados\\GenerardorCertificados\\";

        string templatePath = Path.Combine(basePath, "Template\\Plantilla.pdf");

        string outputPath = Path.Combine(basePath, $"PdfCreated\\Diploma_{"aluxdmno"}.pdf");

        string logoPath = Path.Combine(basePath, "Imagen\\images.png");

        string fontPathName = Path.Combine(basePath, "Fonts\\AlexBrush-Regular.ttf");
        string fontPathBody = Path.Combine(basePath, "Fonts\\Josefin_Sans\\static\\JosefinSans-Regular.ttf");
        string fontPathTitle = Path.Combine(basePath, "Fonts\\Josefin_Sans\\static\\JosefinSans-Bold.ttf");



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
                PdfContentByte overContent = stamper.GetOverContent(1);


                BaseFont baseFontBody = GetBaseFont(fontPathBody);
                BaseFont baseFontTitle = GetBaseFont(fontPathTitle);

                BaseFont baseFontName = GetBaseFont(fontPathName);

                
                Font fuenteTitulo = new Font(baseFontTitle, 50, Font.BOLD, ColorHexadecimal("1f628e") );
                Font fuenteTexto = new Font(baseFontBody, 15, Font.NORMAL, BaseColor.BLACK);
                Font fuenteTextoBold = new Font(baseFontTitle, 15, Font.NORMAL, BaseColor.BLACK);

                Font fuenteName = new Font(baseFontName, 48, Font.NORMAL, BaseColor.BLACK);

                Phrase title = new Phrase();
                Phrase body = new Phrase();                
                Phrase name = new Phrase(); 

               


                title.Add(new Chunk("CERTIFICADO", fuenteTitulo));
                name.Add(new Chunk("Yerson Omar Sanchez Leiva", fuenteName));

                ColumnText.ShowTextAligned(overContent, Element.ALIGN_CENTER, title, centerX, 480, 0);
                ColumnText.ShowTextAligned(overContent, Element.ALIGN_CENTER, name, centerX, 400, 0);

                body.SetLeading(0, 1.8f);
                body.Add(new Chunk("Por haber participado como ", fuenteTexto));
                body.Add(new Chunk("DESARROLLADOR DE SOFTWARE ", fuenteTextoBold));
                body.Add(new Chunk("en la contribución del desarrollo del ", fuenteTexto));
                body.Add(new Chunk("SOFTWARE WEB DE GESTION BIBLIOTECARIA CON ARQUITECTURA CLIENTE SERVIDOR, ", fuenteTextoBold));
                body.Add(new Chunk("para la biblioteca central de la sede Valle Jequetepeque, desde 09 de octubre 2023 al 29 de marzo del 2024.", fuenteTexto));

                

                ColumnText column = new ColumnText(overContent);
                column.SetSimpleColumn(new Rectangle(90, 36, pageWidth - 90, 360));
                column.AddElement(body);
                column.Go();
                

                if (File.Exists(logoPath))
                {
                    Image logo = Image.GetInstance(logoPath);
                    logo.ScaleAbsolute(100, 100);
                    // Posicionar la imagen (por ejemplo, en la esquina superior izquierda)
                    logo.SetAbsolutePosition(120, 100);
                    overContent.AddImage(logo);

                    logo.SetAbsolutePosition(pageWidth - 220, 100);
                    overContent.AddImage(logo);
                }

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

    public static void AddTextToPdf(PdfContentByte overContent, string text, BaseFont baseFont,int sizeFont, string? color, float x, float y)
    {
        overContent.BeginText();
        overContent.SetFontAndSize(baseFont, sizeFont);
        overContent.SetColorFill(BaseColor.BLACK);
        if (color != null)
        {
            var colorHex = ColorHexadecimal(color);
            overContent.SetColorFill(colorHex);
            colorHex = null;
        }        
        overContent.ShowTextAligned(Element.ALIGN_CENTER, text, x, y, 0);
        
        overContent.EndText();
           
    }

    public static BaseColor ColorHexadecimal(string colorHexadecimal)
    {
        if (colorHexadecimal.StartsWith("#"))
            colorHexadecimal = colorHexadecimal.Substring(1);

        int r = int.Parse(colorHexadecimal.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(colorHexadecimal.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(colorHexadecimal.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new BaseColor(r, g, b);
    }

    public static BaseFont GetBaseFont(string fontPath)
    {
        try
        {
            return BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading font: {ex.Message}");
            // Fallback to default font if loading fails
            return BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        }
    }

}
