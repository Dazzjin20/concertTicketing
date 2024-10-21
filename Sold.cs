using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Concertticketing
{
    public class Sold
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EventName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EventDate { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string VIPQuantity { get; set; }
        public string GenAdmissionQuantity { get; set; }
        public string VIPSeats { get; set; }
        public string GenAdmissionSeats { get; set; }
        public double TotalPrice { get; set; }
        
        public bool IsOccupied { get; set; }
        public string TypeOfTicket { get; set; }
        public string VipPrice { get; set; }
        public string GenPrice { get; set; }

        public Sold() { } 

        public Sold(string eventName, string firstName, string lastName, string eventDate, string location, string time, string vIPQuantity, string genAdmissionQuantity, string vIPSeats, string genAdmissionSeats, double totalPrice,bool isOccupied,string typeOfTicket,string vipPrice,string genPrice)
        {
           this.EventName = eventName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EventDate = eventDate;
            this.Location = location;
            this.Time = time;
            this.VIPQuantity = vIPQuantity;
            this.GenAdmissionQuantity = genAdmissionQuantity;
            this.VIPSeats = vIPSeats;
            this.GenAdmissionSeats = genAdmissionSeats;
            this.TotalPrice = totalPrice;
            this.IsOccupied = isOccupied;
            this.TypeOfTicket = typeOfTicket;
           this.VipPrice = vipPrice;
            this.GenPrice = genPrice;
        }
    }
}

