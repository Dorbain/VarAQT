using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VarAQT.Models
{
    [XmlRoot("StationDetails")]
    public class StationDetails
    {
        [XmlElement("CallSign")]
        public string CallSign { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Street")]
        public string Street { get; set; }

        [XmlElement("City")]
        public string City { get; set; }

        [XmlElement("Country")]
        public string Country { get; set; }

        [XmlElement("State")]
        public string State { get; set; }

        [XmlElement("OProvince")]
        public string Province { get; set; }

        [XmlElement("ZipCode")]
        public string ZipCode { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("License")]
        public string License { get; set; }

        [XmlElement("Equipment")]
        public string Equipment { get; set; }

        [XmlElement("Antennas")]
        public string Antennas { get; set; }

        [XmlElement("Power")]
        public string Power { get; set; }

        [XmlElement("Locator")]
        public string Locator { get; set; }

        [XmlElement("Latitude")]
        public string Latitude { get; set; }

        [XmlElement("Longitude")]
        public string Longitude { get; set; }

        [XmlElement("CQzone")]
        public string CQzone { get; set; }

        [XmlElement("ITU")]
        public string ITU { get; set; }

        [XmlElement("SIG")]
        public string SIG { get; set; }

        [XmlElement("SIGinfo")]
        public string SIGinfo { get; set; }

        [XmlElement("WebPage")]
        public string WebPage { get; set; }

        [XmlElement("Comment")]
        public string Comment { get; set; }
    }
}
