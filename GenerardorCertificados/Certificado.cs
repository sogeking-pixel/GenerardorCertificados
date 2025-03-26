using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class Certificado
    {
        public static readonly string basePath = "D:\\Programming\\C#\\Aprendiendo\\Consolitas\\GenerardorCertificados\\GenerardorCertificados\\";
        public static readonly string templatePath = Path.Combine(basePath, "PlantillaCertificado.pdf");
        public string FullNames { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string link { get; set; }
        public List<Person> Persons { get; set; }
        public DateOnly DateEnd { get; set; }
        public DateOnly DateStart { get; set; }

    }


}
