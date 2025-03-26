using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class CertificadoPractica: Certificado
    {
        public string Area { get; set; }
        public string Instructor { get; set; }
        public int CantHours { get; set; }
    }
}
