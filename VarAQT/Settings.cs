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
            if(_savedSettings)
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
            textBox1.Text = Properties.Settings.Default.CallSign;
            textBox2.Text = Properties.Settings.Default.GridLocator;
        }

        private void saveSettings()
        {
            
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            _savedSettings = true;
        }

    }
}
