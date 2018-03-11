using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MD5Gen;
using SteamAppPathTool;
using Newtonsoft.Json;

namespace Secret_of_Mana_Music_Picker
{
    public partial class PrimaryForm : Form
    {
        //The constructor sets strSavePath
        const string AppDataFolderName = "Secret of Mana Music Selector";
        string SavePath;

        public PrimaryForm()
        {
            SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataFolderName);
            InitializeComponent();
            
        }

        private void PrimaryForm_Load(object sender, EventArgs e)
        {

        }

        private void btnFindPath_Click(object sender, EventArgs e)
        {

            // MessageBox.Show(JsonConvert.SerializeObject(objTest, Formatting.Indented));
            //  FileVersionPicker fv2;
            //fv2 = JsonConvert.DeserializeObject<FileVersionPicker>(JsonConvert.SerializeObject(fvpTest));
            TTest();
        }

        public void TTest()
        {
            
            MessageBox.Show(SteamAppPath.GetSteamPath());
            MessageBox.Show(SteamAppPath.GetSteamAppPath("637670"));
        }
        
    }
}
