using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class ClassOfTicket
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("eventname")]
    public string EventName { get; set; }

    [BsonElement("genads")]
    public double GenAdsPrice { get; set; }
    [BsonElement("Time")]
    public string time { get; set; }

    [BsonElement("Location")]
    public string location { get; set; }
    [BsonElement("VIPPrice")]
    public double VIPPrice { get; set; }
    [BsonElement("eventDate")]
    
    public string EventDate { get; set; }

    public ClassOfTicket(string eventName, double genAdsPrice, double vipPrice, string eventDate,string Location,string Time)
    {
        EventName = eventName;
        time = Time;
        GenAdsPrice = genAdsPrice;
        VIPPrice = vipPrice;
        location = Location;
        EventDate = eventDate;
    }
}

