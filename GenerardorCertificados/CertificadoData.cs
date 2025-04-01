using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerardorCertificados
{
    class CertificadoData
    {
        public string RecipientName { get; set; }
        public string Role { get; set; }
        public string ProjectName { get; set; }
        public string ProjectPeriod { get; set; }
        public Signatory[] Signatories { get; set; }
    }
}
