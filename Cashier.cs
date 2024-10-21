using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Concertticketing;

namespace Concertticketing
{
    public partial class Cashier : Form
    {

        private List<string> vipSeats = new List<string>();
        private List<string> genAdsSeats = new List<string>();

        public Cashier()
        {
            InitializeComponent();
            ReadAllDocument();
            
        }

        private List<PurchaseDetails> pendingPurchases = new List<PurchaseDetails>();


        static MongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase db = client.GetDatabase("register");
        static IMongoCollection<ClassOfTicket> collection = db.GetCollection<ClassOfTicket>("Tickets");
        public void ReadAllDocument()
        {
            List<ClassOfTicket> list = collection.AsQueryable().ToList<ClassOfTicket>();
            dataGridView1.DataSource = list;
            dataGridView1.Columns["Id"].Visible = false;

        }

        public void typeofticket()
        {

        }
        public void SearchTicket(string eventName)
        { var filter = Builders<ClassOfTicket>.Filter.Eq("EventName", eventName);
            var ticket = collection.Find(filter).FirstOrDefault();
            if (ticket != null)
            {
                lbleventname.Text = ticket.EventName;
                lblGen.Text = ticket.GenAdsPrice.ToString();
                lblVip.Text = ticket.VIPPrice.ToString();
                lblDate.Text = ticket.EventDate;
                lblLoc.Text = ticket.location;
                lbltime.Text = ticket.time;
            }
        }
        public void refreshseat()
        {
            if (cmbtickets.SelectedItem.ToString() == "VIP")
            {
                seatnumVIP();
            }
            else if (cmbtickets.SelectedItem.ToString() == "General Admission")
            {
                seatnumgen();
            }
        }
        private void Cashier_Load(object sender, EventArgs e)
        {

        }


        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private int vipSeatCount = 0;
        private int genAdsSeatCount = 0;
        private void cmbtickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event from the DataGridView first.");
                cmbtickets.SelectedItem = null; 
                return;
            }
            else
            {
                cmbseat.Items.Clear();
                if (cmbtickets.SelectedItem != null)
                {
                    string selectedValue = cmbtickets.SelectedItem.ToString();
                    if (selectedValue == "General Admission")
                    {

                        seatnumgen();
                    }
                    else if (selectedValue == "VIP")
                    {

                        seatnumVIP();
                    }
                    else
                    {
                        MessageBox.Show("No event available");
                    }
                    return;
                }
            }
           
        }
            private void UpdatePrice()
        {
            decimal totalPrice = 0, genadstotal = 0, viptotal = 0;
            viptotal = vipPrice * vipSeatCount;
            genadstotal = genAdsPrice * genAdsSeatCount;
            totalPrice = genadstotal + viptotal;
            lblTotal.Text = totalPrice.ToString("");
        }
        private HashSet<string> GetOccupiedSeats(string eventName, string ticketType)
        {
            var soldCollection = db.GetCollection<Sold>("Sold");
            var filter = Builders<Sold>.Filter.And(
                Builders<Sold>.Filter.Eq(s => s.EventName, eventName),
                Builders<Sold>.Filter.Eq(s => s.IsOccupied, true)
            );

            var soldSeats = soldCollection.Find(filter).ToList();
            var occupiedSeats = new HashSet<string>();

            foreach (var sale in soldSeats)
            {
                if (ticketType == "VIP" && !string.IsNullOrEmpty(sale.VIPSeats))
                {
                    var seats = sale.VIPSeats.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var seat in seats)
                    {
                        occupiedSeats.Add(seat);
                    }
                }
                else if (ticketType == "General Admission" && !string.IsNullOrEmpty(sale.GenAdmissionSeats))
                {
                    var seats = sale.GenAdmissionSeats.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var seat in seats)
                    {
                        occupiedSeats.Add(seat);
                    }
                }
            }

            return occupiedSeats;
        }

        public void seatnumgen()
        {
            cmbseat.Items.Clear();
            string eventName = lbleventname.Text;
            var occupiedSeats = GetOccupiedSeats(eventName, "General Admission");

            for (int seatNumber = 1; seatNumber <= 10; seatNumber++)
            {
                foreach (char row in "KLMNOPQRST")
                {
                    string seat = $"{row}{seatNumber}";
                    if (!occupiedSeats.Contains(seat))
                    {
                        cmbseat.Items.Add(seat);
                        Console.WriteLine("Available Gen Seat: " + seat); 
                    }
                    else
                    {
                        Console.WriteLine("Occupied Gen Seat: " + seat); 
                    }
                }
            }
        }
        public void seatnumVIP()
        {
            cmbseat.Items.Clear();
            string eventName = lbleventname.Text;
            var occupiedSeats = GetOccupiedSeats(eventName, "VIP");

            for (int seatNumber = 1; seatNumber <= 10; seatNumber++)
            {
                foreach (char row in "ABCDEFGHIJ")
                {
                    string seat = $"{row}{seatNumber}";
                    if (!occupiedSeats.Contains(seat))
                    {
                        cmbseat.Items.Add(seat);
                    }
                }
            }
        }

    
        private int selectedRowIndex = -1;
        private decimal vipPrice;
        private decimal genAdsPrice;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                lbleventname.Text = row.Cells["EventName"].Value.ToString();
                lblGen.Text = row.Cells["GenAdsPrice"].Value.ToString();
                lblVip.Text = row.Cells["VIPPrice"].Value.ToString();
                lblDate.Text = row.Cells["EventDate"].Value.ToString();
                lblLoc.Text = row.Cells["Location"].Value.ToString();
                lbltime.Text = row.Cells["Time"].Value.ToString();
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    selectedRowIndex = dataGridView1.SelectedRows[0].Index;


                    if (selectedRowIndex >= 0)
                    {
                        DataGridViewRow rows = dataGridView1.Rows[selectedRowIndex];
                        vipPrice = Convert.ToDecimal(row.Cells["VIPPrice"].Value);
                        genAdsPrice = Convert.ToDecimal(row.Cells["GenAdsPrice"].Value);

                        UpdatePrice();
                    }
                }

            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTotal_Click(object sender, EventArgs e)
        {

        }

        private void txtammount_TextChanged(object sender, EventArgs e)
        {
            UpdatePrice();



        }
     
        private void button4_Click(object sender, EventArgs e)
        {
            bool hasSelectedVipSeat = !string.IsNullOrEmpty(lblvipseat.Text.Trim());
            bool hasSelectedGenAdSeat = !string.IsNullOrEmpty(lblgenseat.Text.Trim());

           
            if (string.IsNullOrEmpty(lbleventname.Text) || string.IsNullOrEmpty(lblDate.Text) || string.IsNullOrEmpty(lbltime.Text) || string.IsNullOrEmpty(lblLoc.Text)  )
            {
                MessageBox.Show("Please Select a Event that you want to purchase ");
                return;
            }
            if (!hasSelectedVipSeat && !hasSelectedGenAdSeat)
            {
                MessageBox.Show("Please select at least one seat for either VIP or General Admission.");
                return;
            }
            if (cmbtickets.SelectedItem != null)
            {
                
                string vipSeats = string.Empty;
                string genAdmissionSeats = string.Empty;

                if (lblvipseat != null && lblvipseat.Text != null)
                {
                    vipSeats = string.Join(" ", lblvipseat.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }

                if (lblgenseat != null && lblgenseat.Text != null)
                {
                    genAdmissionSeats = string.Join(" ", lblgenseat.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }

                
                string selectedSeat = cmbseat.SelectedItem != null ? cmbseat.SelectedItem.ToString() : string.Empty;

                if (cmbtickets.SelectedItem.ToString() == "VIP")
                {
                    if (!string.IsNullOrEmpty(selectedSeat) && !vipSeats.Contains(selectedSeat))
                    {
                        vipSeats = string.Join(" ", vipSeats, selectedSeat).Trim();
                    }
                }
                else if (cmbtickets.SelectedItem.ToString() == "General Admission")
                {
                    if (!string.IsNullOrEmpty(selectedSeat) && !genAdmissionSeats.Contains(selectedSeat))
                    {
                        genAdmissionSeats = string.Join(" ", genAdmissionSeats, selectedSeat).Trim();
                    }
                }
                int vipSeatCount = vipSeats.Split(' ').Length;
                int genAdSeatCount = genAdmissionSeats.Split(' ').Length;
                int totalSeatCount = vipSeatCount + genAdSeatCount;
                if (totalSeatCount > 8)
                {
                    MessageBox.Show("You can only select up to 8 seats for VIP and General Admission combined.");
                    return;
                }


                string totalText = lblTotal != null && lblTotal.Text != null
                    ? lblTotal.Text.Contains("₱") ? lblTotal.Text.Replace("₱", "").Trim() : lblTotal.Text.Trim()
                    : string.Empty;

                double totalPrice;
                
                if (!double.TryParse(totalText, out totalPrice))
                {
                    MessageBox.Show("Invalid total price format. Please check the amount.");
                    return; 
                }
                else
                {
                    var purchaseDetails = new PurchaseDetails
                    {
                        EventName = lbleventname.Text,
                        EventDate = lblDate.Text,
                        Location = lblLoc.Text,
                        Time = lbltime.Text,
                        TicketType = cmbtickets.SelectedItem.ToString(),
                        VIPQuantity = lblqtyvip.Text,
                        GenAdmissionQuantity = lblgenqty.Text,
                        VIPSeats = vipSeats,
                        GenAdmissionSeats = genAdmissionSeats,
                        TotalPrice = totalPrice,
                        vipprice = Convert.ToDouble(lblVip.Text),
                        genprice = Convert.ToDouble(lblGen.Text)
                    };

                    var confirmPurchase = new confirmpurchase(purchaseDetails);
                    confirmPurchase.ShowDialog();
                    this.Close();
                    ReadAllDocument();
                    cmbseat.Items.Clear();
                    clear();
                }
               
                
               
            }
            else
            {
                MessageBox.Show("Please select a ticket type.");
            }


        }




        private void button5_Click(object sender, EventArgs e)
        {   
          
          

        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }
        public void clear()
        {
            lbleventname.Text = "";
            lblDate.Text = "";
            lblGen.Text = "";
            lblLoc.Text = "";
            lbltime.Text = "";
            lblTotal.Text = "";
            cmbseat.Items.Clear();
            cmbseat.Text = "";
            lblVip.Text = "";
            lblvipseat.Text = "";
            lblgenseat.Text = "";
            vipSeatCount = 0;
            genAdsSeatCount = 0;
            lblqtyvip.Text = vipSeatCount.ToString();
            lblgenqty.Text = genAdsSeatCount.ToString();
            cmbtickets.Text = "";
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            inlog login = new inlog();
            login.Show();
            this.Close();
        }
      

        private void button6_Click(object sender, EventArgs e)
        {
            clear();
           
            MessageBox.Show("Purchase has been canceled.");

           
            if (cmbtickets.SelectedItem.ToString() == "VIP")
            {
                seatnumVIP();
            }
            else
            {
                seatnumgen();
            }
        }
       

        private void button4_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            string eventName = txtsearch.Text.Trim();
            if (string.IsNullOrEmpty(eventName))
            {
                MessageBox.Show("Please enter an NAme of event that you want to find");
            }
            else
            {
                SearchTicket(eventName);
            }
        }
    }
}
    
     

    

