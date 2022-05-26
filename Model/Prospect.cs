using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccounts.Model
{
    public class Prospect
    {
       
        public string Location { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Invite { get; set; }
        public string Category { get; set; }
        public string Remark { get; set; }
        public string PhoneNumber { get; set; }


        public Prospect()
        {
            // default values for each property.
            Location = string.Empty;
            Name = string.Empty; 
            Info = string.Empty;
            Invite = string.Empty;
            Category = string.Empty;
            PhoneNumber = string.Empty;
            Remark = string.Empty;
        }
     

    }
}
