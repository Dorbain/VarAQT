using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VarAQT.Models
{
    // <?xml version="1.0" encoding="utf-8"?>
    // N1MM XML Format external UDP broadcasts logging.
    [XmlRoot("contactinfo")]
    public class contactinfo
    {
        [XmlElement("app")]
        public string app { get; set; } = "VarAQT";

        [XmlElement("contestname")]
        public string contestname { get; set; }

        [XmlElement("contestnr")]
        public string contestnr { get; set; }

        [XmlElement("timestamp")]
        public string timestamp { get; set; } = DateTime.UtcNow.ToString();

        [XmlElement("mycall")]
        public string mycall { get; set; }

        [XmlElement("band")]
        // “band” is composed of 2 or 3 characters that may include localized delimiters.
        // For example, 80 meters may be “3.5” or “3,5”; 160 meters as “1.8” or “1,8” The user’s
        // Windows setting will determine which delimiter is present in the band tag
        public string band { get; set; }

        [XmlElement("rxfreq")]
        public string rxfreq { get; set; }

        [XmlElement("txfreq")]
        public string txfreq { get; set; }

        [XmlElement("operator")]
        public string Operator { get; set; }

        [XmlElement("mode")]
        public string mode { get; set; }

        [XmlElement("call")]
        public string call { get; set; }

        [XmlElement("countryprefix")]
        public string countryprefix { get; set; }

        [XmlElement("wpxprefix")]
        public string wpxprefix { get; set; }

        [XmlElement("stationprefix")]
        public string stationprefix { get; set; }

        [XmlElement("continent")]
        public string continent { get; set; }

        [XmlElement("snt")]
        public string snt { get; set; }

        [XmlElement("sntnr")]
        public string sntnr { get; set; }

        [XmlElement("rcv")]
        public string rcv { get; set; }

        [XmlElement("rcvnr")]
        public string rcvnr { get; set; }

        [XmlElement("gridsquare")]
        public string gridsquare { get; set; }

        [XmlElement("exchangel")]
        public string exchangel { get; set; }

        [XmlElement("section")]
        public string section { get; set; }

        [XmlElement("comment")]
        public string comment { get; set; }

        [XmlElement("qth")]
        // “qth” refer to information about the station being worked in this contact.
        public string qth { get; set; }

        [XmlElement("name")]
        // “name” refer to information about the station being worked in this contact.
        public string name { get; set; }

        [XmlElement("power")]
        // ”power” refer to information about the station being worked in this contact.
        // In a contest where transmit power is part of the exchange,
        // “power” will contain the received power exchange from the other station.
        public string power { get; set; }

        [XmlElement("misctext")]
        public string misctext { get; set; }

        [XmlElement("zone")]
        public string zone { get; set; }

        [XmlElement("prec")]
        public string prec { get; set; }

        [XmlElement("ck")]
        public string ck { get; set; }

        [XmlElement("ismultiplierl")]
        public string ismultiplierl { get; set; }

        [XmlElement("ismultiplier2")]
        public string ismultiplier2 { get; set; }

        [XmlElement("ismultiplier3")]
        public string ismultiplier3 { get; set; }

        [XmlElement("points")]
        public string points { get; set; }

        [XmlElement("radionr")]
        public string radionr { get; set; }

        [XmlElement("run1run2")]
        // “run1run2” refer to the run radio number is a multi-2 arrangement.
        public string run1run2 { get; set; }

        [XmlElement("RoverLocation")]
        public string RoverLocation { get; set; }

        [XmlElement("RadioInterfaced")]
        public string RadioInterfaced { get; set; }

        [XmlElement("NetworkedCompNr")]
        public string NetworkedCompNr { get; set; }

        [XmlElement("IsOriginal")]
        // IsOriginal indicates that this is the station on which this contact was initially logged
        // – to differentiate from another station that may be forwarding the contact record.
        // StationName is the netbios name of the station that sent this packet,
        // not necessarily the name of the station that logged this contact.
        public string IsOriginal { get; set; }

        [XmlElement("NetBiosName")]
        public string NetBiosName { get; set; }

        [XmlElement("IsRunQSO")]
        public string IsRunQSO { get; set; }

        [XmlElement("StationName")]
        public string StationName { get; set; }

        [XmlElement("ID")]
        // ID is a 32 byte unique GUID identifier for each contact in the log. Note that it is sent as 2 hex characters per byte.
        public string ID { get; set; }

        [XmlElement("IsClaimedQso")]
        // IsClaimedQso will default to True for all contacts and set to False when a contact is declared to be an X-QSO
        public string IsClaimedQso { get; set; }
    }
}
