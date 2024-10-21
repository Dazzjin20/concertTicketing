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


namespace Concertticketing

{
    public partial class records : Form
    {
        public records()
        {
            InitializeComponent();
            ReadAllDocument();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Cashier cashier = new Cashier();
            cashier.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        static MongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase db = client.GetDatabase("register");
        static IMongoCollection<Sold> collection = db.GetCollection<Sold>("Sold");
        public void ReadAllDocument()
        {
            List<Sold> list = collection.AsQueryable().ToList<Sold>();
            dataGridView1.DataSource = list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string eventName = selectedRow.Cells["EventName"].Value.ToString();
                var filter = Builders<Sold>.Filter.And(
                    Builders<Sold>.Filter.Eq(s => s.EventName, eventName));
                var result = collection.DeleteOne(filter);
                if (result.DeletedCount > 0)
                {
                    MessageBox.Show("Record deleted successfully.");

                    ReadAllDocument();
                }
                else
                {
                    MessageBox.Show("Record not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete.");
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txttickets_Click(object sender, EventArgs e)
        {
            Tickets ticket = new Tickets();
            ticket.Show();
            this.Hide();
        }

        private void txtdata_Click(object sender, EventArgs e)
        {
           
        }

        private void txtcashier_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            inlog login = new inlog();
            login.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            inlog log = new inlog();
            log.Show();
            this.Hide();
        }
    }
}
