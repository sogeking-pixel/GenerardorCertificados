using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class CertificacionGeneration
    {
        private readonly PathConfig _paths;
        private FontManager _fontManager;

        public CertificacionGeneration(PathConfig paths)
        {
            _paths = paths;
        }

        public void Generate(CertificadoData data)
        {
            
            using PdfReader reader = new PdfReader(_paths.GetTemplatePath());
            var pageSize = reader.GetPageSize(1);
            float pageWidth = pageSize.Width;
            float pageHeight = pageSize.Height;
            float centerX = pageWidth / 2;

            Console.WriteLine("Plantilla PDF cargada exitosamente");

           
            string outputDir = Path.GetDirectoryName(_paths.GetOutputPath());
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            
            using FileStream fs = new FileStream(_paths.GetOutputPath(), FileMode.Create, FileAccess.Write);
            using PdfStamper stamper = new PdfStamper(reader, fs);
            PdfContentByte overContent = stamper.GetOverContent(1);

           
            _fontManager = new FontManager(_paths);

            
            AddTitle(overContent, centerX : centerX);
            AddRecipientName(overContent, data.RecipientName, centerX);
            AddCertificateBody(overContent, data, pageWidth);
            AddSignatories(overContent, data.Signatories, pageWidth);
            AddImages(overContent, pageWidth);
        }

        private void AddTitle(PdfContentByte content, float centerX, string name = "CERTIFICADO" )
        {
            Phrase title = new Phrase("CERTIFICADO", _fontManager.CreateTitleFont());
            AddTextAligned(content, title, centerX, 450);
        }

        private void AddRecipientName(PdfContentByte content, string name, float centerX)
        {
            Phrase namePhrase = new Phrase(name, _fontManager.CreateNameFont());
            AddTextAligned(content, namePhrase, centerX, 360);
        }

        private void AddCertificateBody(PdfContentByte content, CertificadoData data, float pageWidth)
        {
            Phrase body = new Phrase();
            body.SetLeading(0, 1.8f);
            
            body.Add(new Chunk("Por haber participado como ", _fontManager.CreateBodyFont()));
            body.Add(new Chunk(data.Role + " ", _fontManager.CreateBoldBodyFont()));
            body.Add(new Chunk("en la contribución del desarrollo del ", _fontManager.CreateBodyFont()));
            body.Add(new Chunk(data.ProjectName + ", ", _fontManager.CreateBoldBodyFont()));
            body.Add(new Chunk("para la biblioteca central de la sede Valle Jequetepeque, " + data.ProjectPeriod + ".", _fontManager.CreateBodyFont()));

            AddTextColumn(content, body, 90, 36, pageWidth - 90, 325);
        }

        private void AddSignatories(PdfContentByte content, Signatory[] signatories, float pageWidth)
        {
            if (signatories.Length >= 1)
            {
                Phrase cargo1 = CreateSignatoryPhrase(signatories[0]);
                AddTextColumn(content, cargo1, 150, 36, 350, 125);
            }

            if (signatories.Length >= 2)
            {
                Phrase cargo2 = CreateSignatoryPhrase(signatories[1]);
                AddTextColumn(content, cargo2, 520, 36, pageWidth - 150, 125);
            }
        }

        private Phrase CreateSignatoryPhrase(Signatory signatory)
        {
            Phrase phrase = new Phrase();
            phrase.SetLeading(0, 1.5f);
            phrase.Add(new Chunk(signatory.Name + "\n", _fontManager.CreateSignatoryNameFont()));
            phrase.Add(new Chunk(signatory.Title, _fontManager.CreateSignatoryTitleFont()));
            return phrase;
        }

        private void AddImages(PdfContentByte content, float pageWidth)
        {
            
            AddImgToPdf(content, _paths.GetQrPath(), 60, 485, 70);

            
            AddImgToPdf(content, _paths.GetLogoPath(), pageWidth - 250, 500, 50);
            
            
            AddImgToPdf(content, _paths.GetSignaturePath(), 120, 125, 50);
            AddImgToPdf(content, _paths.GetSignaturePath(), pageWidth - 120, 125, 50, false);
        }

        private void AddTextAligned(PdfContentByte content, Phrase phrase, float x, float y, int alignment = Element.ALIGN_CENTER)
        {
            ColumnText.ShowTextAligned(content, alignment, phrase, x, y, 0);
        }

        private void AddTextColumn(PdfContentByte content, Phrase phrase, float left, float bottom, float right, float top)
        {
            ColumnText column = new ColumnText(content);
            column.SetSimpleColumn(new Rectangle(left, bottom, right, top));
            column.AddElement(phrase);
            column.Go();
        }

        private void AddImgToPdf(PdfContentByte overContent, string pathImagen, float x, float y, float height, bool left = true)
        {

            if (File.Exists(pathImagen))
            {
                Image img = Image.GetInstance(pathImagen);
                var porporcion = img.Width / img.Height;
                var newHeight = height * porporcion;
                img.ScaleAbsolute(newHeight, height);
                
                Console.WriteLine(img.Width);
                img.SetAbsolutePosition(left ? x : x - newHeight, y);
                overContent.AddImage(img);

            }
        }
    }
}
