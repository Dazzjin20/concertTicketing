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
using System.Text.RegularExpressions;

namespace Concertticketing
{
    public partial class Tickets : Form
    {
        private System.Windows.Forms.Timer cleanupTimer;
        public Tickets()
        {
            InitializeComponent();
            DoubleBuffered = true;
            ReadAllDocument();
            ClearFields();
            txtDate.Format = DateTimePickerFormat.Custom;
            txtDate.CustomFormat = "MM/dd/yyyy";
            DateTime today = DateTime.Today;
            cmbloc.Text = "SM Pampanga";

            cleanupTimer = new System.Windows.Forms.Timer();
            cleanupTimer.Interval = 24 * 60 * 60 * 1000; // Set interval to 24 hours
            cleanupTimer.Tick += new EventHandler(timer1_Tick);
            cleanupTimer.Start();

        }
        static MongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase db = client.GetDatabase("register");
        static IMongoCollection<ClassOfTicket> collection = db.GetCollection<ClassOfTicket>("Tickets");
        private void txtdata_Click(object sender, EventArgs e)
        {
           
        }
        private bool CheckForOutdatedRecords()
        {
            DateTime today = DateTime.Today;
            var filter = Builders<ClassOfTicket>.Filter.Lt("EventDate", today.ToString("MM/dd/yyyy"));
            var count = collection.CountDocuments(filter);
            return count > 0;
        }

        private void DeleteOutdatedRecords()
        {
            DateTime today = DateTime.Today;
            var filter = Builders<ClassOfTicket>.Filter.Lt("EventDate", today.ToString("MM/dd/yyyy"));
            var result = collection.DeleteMany(filter);
            ReadAllDocument();
        }

        private void txttickets_Click(object sender, EventArgs e)
        {
            Tickets ticket = new Tickets();
            ticket.Show();
            this.Hide();
        }

        private void Tickets_Load(object sender, EventArgs e)
        {

        }
       
        public void ReadAllDocument()
        {
           

            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    List<ClassOfTicket> list = collection.AsQueryable().ToList();
                    dataGridView1.DataSource = list;
                }));
            }
            else
            {
                List<ClassOfTicket> list = collection.AsQueryable().ToList();
                dataGridView1.DataSource = list;
            }
        }
        private bool EventAlreadyScheduled(string eventDate, string eventTime)
        {
            var filter = Builders<ClassOfTicket>.Filter.And(
                Builders<ClassOfTicket>.Filter.Eq(e => e.EventDate, eventDate),
                Builders<ClassOfTicket>.Filter.Eq(e => e.time, eventTime)
            );

            var existingEvent = collection.Find(filter).FirstOrDefault();
            return existingEvent != null;
        }
        private bool EventNameExists(string eventName)
        {
            var existingEvent = collection.Find(ticket => ticket.EventName == eventName ).FirstOrDefault();
            return existingEvent != null;
        }
        private bool EventDateExistss(string eventDate)
        {
            var existingDate = collection.Find(ticket => ticket.EventDate == eventDate).FirstOrDefault();
            return existingDate != null;
        }
        private bool EventimeExistss( string eventTime)
        {
            var existingDate = collection.Find(ticket => ticket.time == eventTime).FirstOrDefault();
            return existingDate != null;
        }
        private bool EventNameExistsOnDate(string eventName, string eventDate)
        {
            var filter = Builders<ClassOfTicket>.Filter.And(
                Builders<ClassOfTicket>.Filter.Eq(e => e.EventName, eventName),
                Builders<ClassOfTicket>.Filter.Eq(e => e.EventDate, eventDate)
            );

            var existingEvent = collection.Find(filter).FirstOrDefault();
            return existingEvent != null;
        }
        private void Add_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(txtEventName.Text) || string.IsNullOrEmpty(cmbtime.Text) || string.IsNullOrEmpty(txtgenadsprice.Text) || string.IsNullOrEmpty(txtvipprice.Text) || string.IsNullOrEmpty(cmbloc.Text))
            {
                MessageBox.Show("Please Fill up all the blank!*");

            }
            else if (!double.TryParse(txtgenadsprice.Text, out double genAdsPrice) || genAdsPrice <= 0)
            {
                MessageBox.Show("Please enter a valid number greater than zero for General Admission Prices.");
            }
            else if (!double.TryParse(txtvipprice.Text, out double vipPrice) || vipPrice <= 0)
            {
                MessageBox.Show("Please enter a valid number greater than zero for VIP Prices.");
            }

            else if (txtDate.Value < DateTime.Today )
            {
                MessageBox.Show("You can set you date in Present and Future only");
            }
            else if( txtDate.Value > DateTime.Today.AddYears(2)) {
                MessageBox.Show("You cannot set your date in more than 2 years");
            }
            else if (EventNameExistsOnDate(txtEventName.Text, txtDate.Text))
            {
                MessageBox.Show("An event with the same name already exists on this date. Please choose a different name or date.");
            }

            else if (EventNameExists(txtEventName.Text)&& EventDateExistss(txtDate.Text)&& EventimeExistss(cmbtime.Text))
            
            {
                MessageBox.Show("An event with the same name and time already exists on this date. Please schedule your event for a different of this (Name / Time / Date).");
                 if (EventAlreadyScheduled(txtDate.Text, cmbtime.Text))
                {
                    MessageBox.Show("An event with the same date and time already exists. Please choose a different date or time.");
                   
                }
            }
          

            else if (!IsValidEventName(txtEventName.Text))
            {
                MessageBox.Show("Event name must contain only letters, numbers, and spaces, and should be understandable.");
            }
            else if (genAdsPrice > vipPrice)
            {
                var result = MessageBox.Show("The General Admission price is higher than the VIP price. Are you sure you want to continue?",
                                             "Price Validation",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
               
                var newTicket = new ClassOfTicket(
                  txtEventName.Text,
                  Convert.ToDouble(txtgenadsprice.Text),
                  Convert.ToDouble(txtvipprice.Text),
                    txtDate.Text,
                  cmbloc.Text,
                 cmbtime.Text

              );
                collection.InsertOne(newTicket);
                ReadAllDocument();
                ClearFields();

            }

        }
        bool IsValidEventName(string eventName)
        {
            string pattern = @"^[a-zA-Z0-9\s]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(eventName);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)

        {

            collection.DeleteOne(newTicket => newTicket.Id == ObjectId.Parse(txtEventID.Text));
            MessageBox.Show("Deleted sucessfuly");
            ReadAllDocument();
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventName.Text) || string.IsNullOrEmpty(cmbtime.Text) || string.IsNullOrEmpty(txtgenadsprice.Text) || string.IsNullOrEmpty(txtvipprice.Text) || string.IsNullOrEmpty(cmbloc.Text))
            {
                MessageBox.Show("Please Fill up all the blank!*");

            }
            else if (!double.TryParse(txtgenadsprice.Text, out double genAdsPrice) || genAdsPrice <= 0)
            {
                MessageBox.Show("Please enter a valid number greater than zero for General Admission Prices.");
            }
            else if (!double.TryParse(txtvipprice.Text, out double vipPrice) || vipPrice <= 0)
            {
                MessageBox.Show("Please enter a valid number greater than zero for VIP Prices.");
            }
            else if (txtDate.Value < DateTime.Today)
            {
                MessageBox.Show("You can set you date in Present and Future only");
            }
            else if (txtDate.Value > DateTime.Today.AddYears(2))
            {
                MessageBox.Show("You cannot set your date in more than 2 years");
            }
           
            else if (!IsValidEventName(txtEventName.Text))
            {
                MessageBox.Show("Event name must contain only letters, numbers, and spaces, and should be understandable.");
            }
            else if (genAdsPrice > vipPrice)
            {
                var result = MessageBox.Show("The General Admission price is higher than the VIP price. Are you sure you want to continue?",
                                             "Price Validation",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                var update = Builders<ClassOfTicket>.Update
                   .Set("EventName", txtEventName.Text)
                   .Set("GenAdsPrice", genAdsPrice)
                   .Set("VIPPrice", vipPrice)
                   .Set("EventDate", txtDate.Text)
                   .Set("location", cmbloc.Text)
                   .Set("time", cmbtime.Text);
                collection.UpdateOne(newTicket => newTicket.Id == ObjectId.Parse(txtEventID.Text), update);
                ReadAllDocument();
                MessageBox.Show("Edit Successful");
                ClearFields();
            }
        }

       
        private void ClearFields()
        {
            txtEventID.Text = "";
            txtEventName.Text = "";
            txtgenadsprice.Text = "";
            txtvipprice.Text = "";
            txtDate.Value = DateTime.Now;
            cmbloc.Text = "SM Pampanga";
            cmbtime.SelectedIndex = -1;
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtcashier_Click(object sender, EventArgs e)
        {
            records records = new records();
            records.Show();
            this.Hide();
        }

        private void txtDate_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbloc_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtEventID.Text = row.Cells["Id"].Value.ToString();
                txtEventName.Text = row.Cells["EventName"].Value.ToString();
                txtgenadsprice.Text = row.Cells["GenAdsPrice"].Value.ToString();
                txtvipprice.Text = row.Cells["VIPPrice"].Value.ToString();
                txtDate.Text = row.Cells["EventDate"].Value.ToString();
                cmbloc.Text = row.Cells["Location"].Value.ToString();
                cmbtime.Text = row.Cells["Time"].Value.ToString();
            }

        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            bool hasOutdatedRecords = await Task.Run(() => CheckForOutdatedRecords());

            if (hasOutdatedRecords)
            {
                await Task.Run(() => DeleteOutdatedRecords());
                ReadAllDocument(); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cashier cashier = new Cashier();
            cashier.Show();
            this.Hide();
        }
    }
}
