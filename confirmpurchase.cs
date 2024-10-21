using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using Concertticketing;
using System.Globalization;
using MongoDB.Bson;


namespace Concertticketing
{
    public partial class confirmpurchase : Form
    {
        private PurchaseDetails purchaseDetails;

        public confirmpurchase(PurchaseDetails details)
        {
            InitializeComponent();
            purchaseDetails = details;
            txtammount.TextChanged += txtammount_TextChanged;
            txtFname.KeyPress += new KeyPressEventHandler(txtFname_KeyPress);
            txtLname.KeyPress += new KeyPressEventHandler(txtLname_KeyPress);

        }
        static MongoClient client = new MongoClient();
        static IMongoDatabase db = client.GetDatabase("register");
        static IMongoCollection<Sold> collection = db.GetCollection<Sold>("Sold");
       
        private void button4_Click(object sender, EventArgs e)
        {
          
         
          
           string vipprice = lblvipprice.Text;
            string genprice = lblgenprice.Text;

            if (string.IsNullOrEmpty(txtFname.Text) ||
                string.IsNullOrEmpty(txtLname.Text) ||
                string.IsNullOrEmpty(lbleventname.Text) ||
                string.IsNullOrEmpty(lblDate.Text) ||
                string.IsNullOrEmpty(lbltime.Text) ||
                string.IsNullOrEmpty(lblLoc.Text) ||
                string.IsNullOrEmpty(lblTotal.Text))
            {
                MessageBox.Show("Please fill up all the blank fields.");
                return;
            }

          
            double totalPrice;
            double amountPaid;
            double change = 0;

            if (!double.TryParse(lblTotal.Text, out totalPrice))
            {
                MessageBox.Show("Invalid total price format.");
                return;
            }

            if (!double.TryParse(txtammount.Text, out amountPaid))
            {
                MessageBox.Show("Invalid amount paid format.");
                return;
            }

        
            if (amountPaid < totalPrice)
            {
                change = amountPaid - totalPrice;
                txtchange.Text = change.ToString();
            }
            else {
                change = amountPaid - totalPrice;
                txtchange.Text = change.ToString(); 

              
                var soldTicket = new Sold
                (
                     lbleventname.Text,
                    txtFname.Text,
                    txtLname.Text,
                    lblDate.Text,
                    lblLoc.Text,
                    lbltime.Text,
                    lblqtyvip.Text,
                    lblqtygen.Text,
                    lblseatsvip.Text,
                    lblseatgen.Text,
                    totalPrice,
                    true,
                    "VIP", 
                    lblvipprice.Text,
                    lblgenprice.Text

                );

                collection.InsertOne(soldTicket);
               
                this.Close();
                
                resibo resibo = new resibo();
                resibo.ShowDialog();
               

            }




        }

        private void confirmpurchase_Load(object sender, EventArgs e)
        {
            lbleventname.Text = purchaseDetails.EventName;
            txtFname.Text = purchaseDetails.FirstName;
            txtLname.Text = purchaseDetails.LastName;
            lblDate.Text = purchaseDetails.EventDate;
            lblLoc.Text = purchaseDetails.Location;
            lbltime.Text = purchaseDetails.Time;
            lblqtyvip.Text = purchaseDetails.VIPQuantity.ToString();
            lblqtygen.Text = purchaseDetails.GenAdmissionQuantity.ToString();
            lblseatsvip.Text = purchaseDetails.VIPSeats;
            lblseatgen.Text = purchaseDetails.GenAdmissionSeats;
            lblTotal.Text = purchaseDetails.TotalPrice.ToString();
            lblgenprice.Text = purchaseDetails.genprice.ToString();
            lblvipprice.Text = purchaseDetails.vipprice.ToString();

        }

        private void txtammount_TextChanged(object sender, EventArgs e)
        {
            double totalPrice;
            double amountPaid;
            double change = 0;
            if (!double.TryParse(lblTotal.Text, out totalPrice))
            {
                MessageBox.Show("Invalid total price format.");
                return;
            }
            if (!double.TryParse(txtammount.Text, out amountPaid))
            {
                txtchange.Text = string.Empty;
                return;
            }

            if (amountPaid >= totalPrice)
            {
                change = amountPaid - totalPrice;
                txtchange.Text = change.ToString(); 
            }
            else
            {
                txtchange.Text = "Insufficient amount.";
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cashier cashier = new Cashier();
            cashier.Show();
            this.Close();
        }

        private void txtFname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void txtLname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
