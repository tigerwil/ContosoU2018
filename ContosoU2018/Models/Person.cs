using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models
{
    public abstract class Person
    {
        //Person properties - characteristics 
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }


        //Read Only properties
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

        public string IDFullName
        {
            get
            {
                return "(" + ID + ") " + LastName + ", " + FirstName;
            }
        }


    }
}
