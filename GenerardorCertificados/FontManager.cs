using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class FontManager
    {
        private readonly PathConfig _paths;
        private readonly Dictionary<string, BaseFont> _baseFonts = new Dictionary<string, BaseFont>();

        public FontManager(PathConfig paths)
        {
            _paths = paths;
            InitializeFonts();
        }

        private void InitializeFonts()
        {
            _baseFonts["name"] = GetBaseFont(_paths.GetNameFontPath());
            _baseFonts["body"] = GetBaseFont(_paths.GetBodyFontPath());
            _baseFonts["title"] = GetBaseFont(_paths.GetTitleFontPath());
        }

        public Font CreateTitleFont()
        {
            return new Font(_baseFonts["title"], 45, Font.BOLD, ColorHexadecimal("1f628e"));
        }

        public Font CreateNameFont()
        {
            return new Font(_baseFonts["name"], 50, Font.NORMAL, BaseColor.BLACK);
        }

        public Font CreateBodyFont()
        {
            return new Font(_baseFonts["body"], 15, Font.NORMAL, new BaseColor(50, 50, 50));
        }

        public Font CreateBoldBodyFont()
        {
            return new Font(_baseFonts["title"], 15, Font.NORMAL, BaseColor.BLACK);
        }

        public Font CreateSignatoryNameFont()
        {
            return new Font(_baseFonts["body"], 14, Font.BOLDITALIC, new BaseColor(10, 10, 10));
        }

        public Font CreateSignatoryTitleFont()
        {
            return new Font(_baseFonts["body"], 13, Font.NORMAL, new BaseColor(80, 80, 80));
        }

        private static BaseFont GetBaseFont(string fontPath)
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

        private static BaseColor ColorHexadecimal(string colorHexadecimal)
        {
            if (colorHexadecimal.StartsWith("#"))
                colorHexadecimal = colorHexadecimal.Substring(1);

            int r = int.Parse(colorHexadecimal.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(colorHexadecimal.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(colorHexadecimal.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return new BaseColor(r, g, b);
        }
    }
}
