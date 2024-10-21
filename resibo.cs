using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB;

namespace Concertticketing
{
    public partial class resibo : Form
    {
        static MongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase db = client.GetDatabase("register");
        static IMongoCollection<Sold> collection = db.GetCollection<Sold>("Sold");

        public resibo()
        {
            InitializeComponent();
        }

        private void resibo_Load(object sender, EventArgs e)
        {
            var latestSoldTicket = GetLatestSoldTicket();
            if (latestSoldTicket != null)
            {
                lblEventName.Text = latestSoldTicket.EventName;
                
                lblF.Text = latestSoldTicket.FirstName;
                lblL.Text = latestSoldTicket.LastName;
                lblDate.Text = latestSoldTicket.EventDate;
                lblLoc.Text = latestSoldTicket.Location;
                lblTime.Text = latestSoldTicket.Time;
                lblqtygen.Text = latestSoldTicket.GenAdmissionQuantity;
                lblqtyvip.Text = latestSoldTicket.VIPQuantity;
                lblSeatsgen.Text = latestSoldTicket.GenAdmissionSeats;
                lblSeatvip.Text = latestSoldTicket.VIPSeats;
                lblvipprice.Text = latestSoldTicket.VipPrice;
                double viptotal;
                double gvip = Convert.ToDouble(lblvipprice.Text);
                double vipqty = Convert.ToDouble(lblqtyvip.Text);
                viptotal =gvip * vipqty;
                lbltotalvip.Text = viptotal.ToString();
                genprice.Text = latestSoldTicket.GenPrice;
                double gentotal;
                double gen = Convert.ToDouble(genprice.Text);
                double genqty = Convert.ToDouble(lblqtygen.Text);
                gentotal = gen * genqty;
                lbltotalgen.Text = gentotal.ToString();
                double grandtotal;
                grandtotal = gentotal + viptotal;
                grandtot.Text = grandtotal.ToString();
                
                






            }
            else
            {
                MessageBox.Show("No sold tickets found in the database.");
            }
            timer1.Start();
        }


        public Sold GetLatestSoldTicket()
        {
            var sort = Builders<Sold>.Sort.Descending(t => t.Id);
            var latestTicket = collection.Find(FilterDefinition<Sold>.Empty)
                                         .Sort(sort)
                                         .FirstOrDefault();
            return latestTicket;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float printWidth = e.PageBounds.Width;
            float printHeight = e.PageBounds.Height;

           
            float bmpWidth = bmp.Width;
            float bmpHeight = bmp.Height;

           
            float scaleX = printWidth / bmpWidth;
            float scaleY = printHeight / bmpHeight;
            float scale = Math.Min(scaleX, scaleY); // Maintain aspect ratio

        
            RectangleF printArea = new RectangleF(0, 0, bmpWidth * scale, bmpHeight * scale);

         
            float offsetX = (printWidth - printArea.Width) / 2;
            float offsetY = (printHeight - printArea.Height) / 2;

           
            e.Graphics.DrawImage(bmp, new RectangleF(offsetX, offsetY, printArea.Width, printArea.Height));
        }
        Bitmap bmp;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           

          
        }
      

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            this.Hide();
            Cashier cashier = new Cashier();
            cashier.Show();
        }
        public void StartPrinting()
        {
            Graphics g = this.CreateGraphics();
            bmp = new Bitmap(this.Size.Width, this.Size.Height, g);
            Graphics mg = Graphics.FromImage(bmp);
            mg.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            printPreviewDialog1.ShowDialog();
        }


        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void printDocument1_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {

        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            
            StartPrinting();
        }
        public void TriggerPrinting()
        {
            timer1.Interval = 7000; 
            timer1.Start(); 
        }
    }

}

