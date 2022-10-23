using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VarAQT.Models
{
    [XmlRoot("ConnectedStationDetails")]
    public class ConnectedStationDetails
    {
        [XmlElement("CallSign")]
        public string CallSign { get; set; }

        [XmlElement("Locator")]
        public string Locator { get; set; }

        [XmlElement("qth")]
        // “qth” refer to information about the station being worked in this contact.
        public string qth { get; set; }

        [XmlElement("rcv")]
        public string rcv { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Equipment")]
        public string Equipment { get; set; }

        [XmlElement("Antennas")]
        public string Antennas { get; set; }

        [XmlElement("Power")]
        public string Power { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }
    }
}
