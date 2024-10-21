using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Concertticketing
{
    class archieve
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public long contact { get; set; }
        public string position { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string password { get; set; }

      
        public archieve(string fname, string lname, long Contact, string Position, string Username, string Gender, string Password)
        {
            Fname = fname;
            Lname = lname;
            contact = Contact;
            position = Position;
            username = Username;
            gender = Gender;
            password = Password;
        }
    }
}
