using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models
{
    public class Student:Person//Student inherits from Person
    {
        //Student specific property
        public DateTime EnrollmentDate { get; set; }

        //1 student: many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }



    }
}
