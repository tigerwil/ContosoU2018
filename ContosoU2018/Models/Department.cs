using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models
{
    public class Department
    {
        //Creating a more complex data model
        public int DepartmentID { get; set; }//PK

        [Required]
        [StringLength(50, MinimumLength =3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]//client only
        [Column(TypeName ="money")] //SQL Server money data type
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]//removes the time
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]//change date format
        [Display(Name="Date Created")]
        public DateTime CreatedDate { get; set; }

        //Relationship to Instructor
        //A department MAY have an administrator (Instructor), and an
        //administrator is always an Instructor
        [Display(Name = "Administrator")]
        public int? InstructorID { get; set; }//? denotes nullable

        //Navigation properties
        //Administrator is always an Instructor
        public virtual Instructor Administrator { get; set; }

        //One department with many courses
        public virtual ICollection<Course> Courses { get; set; }







    }
}
