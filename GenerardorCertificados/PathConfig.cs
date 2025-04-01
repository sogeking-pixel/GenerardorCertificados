using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class PathConfig
    {
        private readonly string _basePath;

        public PathConfig(string basePath)
        {
            _basePath = basePath;
        }

        public string TemplatePath { get; set; }
        public string OutputFileName { get; set; }
        public string SignaturePath { get; set; }
        public string LogoPath { get; set; }
        public string QrPath { get; set; }
        public string NameFontPath { get; set; }
        public string BodyFontPath { get; set; }
        public string TitleFontPath { get; set; }

        public string GetFullPath(string relativePath)
        {
            return Path.Combine(_basePath, relativePath);
        }

        public string GetTemplatePath() => GetFullPath(TemplatePath);
        public string GetOutputPath() => GetFullPath($"PdfCreated\\{OutputFileName}");
        public string GetSignaturePath() => GetFullPath(SignaturePath);
        public string GetLogoPath() => GetFullPath(LogoPath);
        public string GetQrPath() => GetFullPath(QrPath);
        public string GetNameFontPath() => GetFullPath(NameFontPath);
        public string GetBodyFontPath() => GetFullPath(BodyFontPath);
        public string GetTitleFontPath() => GetFullPath(TitleFontPath);
    }
}
