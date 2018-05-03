using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoU2018.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        /*
         * You can turn off IDENTITY (on by default) by using the option above
         * You have the following 3 options
         * None - Database does not generata a value
         * Identity - Database generates a value when a row is inserted
         * Computed - Databas generates a value when a row is inserted or updated
         */

        [Display(Name ="Course Number")]
        public int CourseID { get; set; }//PK

        [StringLength(50,MinimumLength =3)]
        [Required]
        public string Title { get; set; }

        [Range(0,5)]//possible values are:  0,1,2,3,4,5
        public int Credits { get; set; }

        [Required]
        [Display(Name ="Department")]
        public int DepartmentID { get; set; }//FK: 1 department : many courses

        //Navigation properties
        //1 course: many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //1 course: many instructors
        public virtual  ICollection<CourseAssignment> Assignments { get; set; }

        //A course can only have at most one department, so the department property holds
        //a single Department Entity (which may  by null if no department is assigned
        //for a course)
        public  virtual Department Department { get; set; }

        //Calculated (read only) returning CourseID and Course Title
        public string CourseIdTitle {
            get {
                return CourseID + ": " + Title;
                //1: Chemistry
                //2: Math
            }
        }//End of CourseIdTitle property
    }//End of Class
}//End of namespace