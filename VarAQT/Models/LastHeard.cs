using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VarAQT.Models
{
    [XmlRoot("LastHeard")]
    public class station
    {
        [XmlElement("UTCTime")]
        public string UTCTime { get; set; }
        [XmlElement("Call")]
        public string Call { get; set; }
        [XmlElement("SID")]
        public string SID { get; set; }
        [XmlElement("SNR")]
        public string SNR { get; set; }
        [XmlElement("SM")]
        public string SM { get; set; }
        [XmlElement("Cnt")]
        public string Cnt { get; set; }
    }
}
