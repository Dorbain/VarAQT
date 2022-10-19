using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace VarAQT
{
    public partial class Settings : Form
    {
        private bool _savedSettings = false;
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveSettings();
            Thread.Sleep(2000);
            loadSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadSettings();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_savedSettings)
                this.Close();
            else
            {
                string message = "You posible forgot to save your changes, do you want to save them now?";
                string caption = "Exit without saving?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    saveSettings();
                    this.Close();
                }
                else if (result == DialogResult.No)
                {
                    this.Close();
                }
            }
        }
        private void loadSettings()
        {
            Values.stationDetails = Functions.readStationDetailsXML();
            callSignTextBox.Text = Values.stationDetails.CallSign;
            nameTextBox.Text = Values.stationDetails.Name;
            streetTextBox.Text = Values.stationDetails.Street;
            cityTextBox.Text = Values.stationDetails.City;
            countryTextBox.Text = Values.stationDetails.Country;
            stateTextBox.Text = Values.stationDetails.State;
            provinceTextBox.Text = Values.stationDetails.Province;
            zipTextBox.Text = Values.stationDetails.ZipCode;
            eMailTextBox.Text = Values.stationDetails.Email;
            webPageTextBox.Text = Values.stationDetails.WebPage;
            locatorTextBox.Text = Values.stationDetails.Locator;
            latitudeTextBox.Text = Values.stationDetails.Latitude;
            longitudeTextBox.Text = Values.stationDetails.Longitude;
            cqZoneTextBox.Text = Values.stationDetails.CQzone;
            ituTextBox.Text = Values.stationDetails.ITU;
            licenseTextBox.Text = Values.stationDetails.License;
            equipmentTextBox.Text = Values.stationDetails.Equipment;
            antennasTextBox.Text = Values.stationDetails.Antennas;
            powerTextBox.Text = Values.stationDetails.Power;
            commentTextBox.Text = Values.stationDetails.Comment;
        }

        private void saveSettings()
        {
            Values.stationDetails.CallSign = callSignTextBox.Text;
            Values.stationDetails.Name = nameTextBox.Text;
            Values.stationDetails.Street = streetTextBox.Text;
            Values.stationDetails.City = cityTextBox.Text;
            Values.stationDetails.Country = countryTextBox.Text;
            Values.stationDetails.State = stateTextBox.Text;
            Values.stationDetails.Province = provinceTextBox.Text;
            Values.stationDetails.ZipCode = zipTextBox.Text;
            Values.stationDetails.Email = eMailTextBox.Text;
            Values.stationDetails.WebPage = webPageTextBox.Text;
            Values.stationDetails.Locator = locatorTextBox.Text;
            Values.stationDetails.Latitude = latitudeTextBox.Text;
            Values.stationDetails.Longitude = longitudeTextBox.Text;
            Values.stationDetails.CQzone = cqZoneTextBox.Text;
            Values.stationDetails.ITU = ituTextBox.Text;
            Values.stationDetails.License = licenseTextBox.Text;
            Values.stationDetails.Equipment = equipmentTextBox.Text;
            Values.stationDetails.Antennas = antennasTextBox.Text;
            Values.stationDetails.Power = powerTextBox.Text;
            Values.stationDetails.Comment = commentTextBox.Text;
            Functions.WriteStationDetailsXML(Values.stationDetails);
            _savedSettings = true;
        }

    }
}
