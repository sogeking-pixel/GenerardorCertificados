
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;



class Program
{
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        string basePath = "D:\\Programming\\C#\\Aprendiendo\\Consolitas\\GenerardorCertificados\\GenerardorCertificados\\";

        string templatePath = Path.Combine(basePath, "Template\\Plantilla.pdf");

        string outputPath = Path.Combine(basePath, $"PdfCreated\\Diploma_{"aluxdmno"}.pdf");

        string firmaPath = Path.Combine(basePath, "Imagen\\Firma.png");
        string logoPath = Path.Combine(basePath, "Imagen\\LogoEmpresa.png");
        string qrPath = Path.Combine(basePath, "Imagen\\Qr.png");

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

                
                Font fuenteTitulo = new Font(baseFontTitle, 45, Font.BOLD, ColorHexadecimal("1f628e") );
                Font fuenteTexto = new Font(baseFontBody, 15, Font.NORMAL, new BaseColor(50, 50, 50));
                Font fuenteTextoBold = new Font(baseFontTitle, 15, Font.NORMAL, BaseColor.BLACK);

                Font fuenteName = new Font(baseFontName, 50, Font.NORMAL, BaseColor.BLACK);

                Font fuentaPersonNombres= new Font(baseFontBody, 14, Font.BOLDITALIC, new BaseColor(10,10,10));
                Font fuentaPersonCargo = new Font(baseFontBody, 13, Font.NORMAL, new BaseColor(80, 80, 80));

                Phrase title = new Phrase();
                Phrase body = new Phrase();                
                Phrase name = new Phrase();
                Phrase cargo1 = new Phrase();
                Phrase cargo2 = new Phrase();

               


                title.Add(new Chunk("CERTIFICADO", fuenteTitulo));
                name.Add(new Chunk("Yerson Omar Sanchez Leiva", fuenteName));

                ColumnText.ShowTextAligned(overContent, Element.ALIGN_CENTER, title, centerX, 450, 0);
                ColumnText.ShowTextAligned(overContent, Element.ALIGN_CENTER, name, centerX, 360, 0);

                body.SetLeading(0, 1.8f);
                body.Add(new Chunk("Por haber participado como ", fuenteTexto));
                body.Add(new Chunk("DESARROLLADOR DE SOFTWARE ", fuenteTextoBold));
                body.Add(new Chunk("en la contribución del desarrollo del ", fuenteTexto));
                body.Add(new Chunk("SOFTWARE WEB DE GESTION BIBLIOTECARIA CON ARQUITECTURA CLIENTE SERVIDOR, ", fuenteTextoBold));
                body.Add(new Chunk("para la biblioteca central de la sede Valle Jequetepeque, desde 09 de octubre 2023 al 29 de marzo del 2024.", fuenteTexto));

                

                ColumnText column = new ColumnText(overContent);
                column.SetSimpleColumn(new Rectangle(90, 36, pageWidth - 90, 325));
                column.AddElement(body);
                column.Go();

                cargo1.SetLeading(0, 1.5f);
                cargo1.Add(new Chunk("Sanchez Leiva, Yerson\n", fuentaPersonNombres));
                cargo1.Add(new Chunk("Director Ejecutivo", fuentaPersonCargo));

                cargo2.SetLeading(0, 1.5f);
                cargo2.Add(new Chunk("Sanchez Leiva, Yerson\n", fuentaPersonNombres));
                cargo2.Add(new Chunk("Director Ejecutivo Interino", fuentaPersonCargo));

                ColumnText columnCargo1 = new ColumnText(overContent);
                column.SetSimpleColumn(new Rectangle(150, 36, 350, 125));
                column.AddElement(cargo1);
                column.Go();

                ColumnText columnCargo2 = new ColumnText(overContent);
                column.SetSimpleColumn(new Rectangle(520, 36, pageWidth - 150, 125));
                column.AddElement(cargo2);
                column.Go();


                addImgToPdf(overContent, qrPath, 60, 485, 70);
                addImgToPdf(overContent, logoPath, pageWidth - 250, 500, 50);
                addImgToPdf(overContent, firmaPath, 120, 125, 50);
                addImgToPdf(overContent, firmaPath, pageWidth - 120, 125, 50, false);


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
    public static void addImgToPdf(PdfContentByte overContent, string pathImagen,  float x, float y, float height, bool left = true)
    {

        if (File.Exists(pathImagen))
        {
            Image img = Image.GetInstance(pathImagen);
            var porporcion = img.Width/ img.Height;
            var newHeight = height * porporcion;
            img.ScaleAbsolute(newHeight, height);
            // Posicionar la imagen (por ejemplo, en la esquina superior izquierda)
            Console.WriteLine(img.Width);
            img.SetAbsolutePosition( left? x : x- newHeight, y);
            overContent.AddImage(img);

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
