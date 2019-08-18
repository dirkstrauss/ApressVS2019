using ShipmentTracking;
using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ShipmentLocator
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void Form1_Load(object sender, EventArgs e)
        {
            var frmLogin = new Login();
            _ = frmLogin.ShowDialog();

            txtWaybill.Text = GenerateWaybill(WBPartA(), WBPartB(100,2000));
        }

        private void BtnTrack_Click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(txtWaybill.Text)))
            {
                var waybillNum = txtWaybill.Text;
                if (WaybillValid())
                {
                    var package = new Package(waybillNum);
                                                                                                                                    
                    var packLoc = package.TrackPackage();
                    if (packLoc != null)
                    {
                        txtLocationDetails.Text = $"Package location: " +
                            $"{packLoc.LocationName} with coordinates " +
                            $"Long: {packLoc.Long} and " +
                            $"Lat: {packLoc.Lat}";
                    }
                }
                else
                {
                    _ = MessageBox.Show("You have entered an invalid Waybill number");
                }
            }

        }

        //private bool WaybillValid() => txtWaybill.Text.ToLower().Contains("acme-");

        private string GenerateWaybill(string partA, int rndNum) => $"{partA}-{rndNum}-{DateTime.Now.Year}-{DateTime.Now.Month}";
        private string WBPartA() => "acme-";

        private int WBPartB(int min, int max)
        {
            var rngCrypto = new RNGCryptoServiceProvider();
            var bf = new byte[4];

            rngCrypto.GetBytes(bf);
            var result = BitConverter.ToInt32(bf, 0);

            return new Random(result).Next(min, max);
        }
        
        private bool WaybillValid()
        {
            return txtWaybill.Text.ToLower().Contains("acme-");
        }
    }
}
