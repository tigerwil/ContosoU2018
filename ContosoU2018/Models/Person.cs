using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models
{
    public abstract class Person
    {
        //Person properties - characteristics 

        /*
         *  These properties will become database fields within the Person table
         *  by using the Entity Framework Core 
         *  The ID Property will become the Primary Key Column of the database table that correspondes
         *  to this class (Person).  
         *  By default the EF interprets a property that's named ID or ClassNameID as the PK
         *  for example:  
         *      - ID
         *      - PersonID
         */
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
