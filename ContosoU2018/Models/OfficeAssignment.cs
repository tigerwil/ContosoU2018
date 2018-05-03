using System.ComponentModel.DataAnnotations;

namespace ContosoU2018.Models
{
    public class OfficeAssignment
    {

        [Key]
        public int InstructorID { get; set; }//PK and FK to instructor

        /*
         * There is a one-to-zero-or-one relationship between Instructor and OfficeAssignment
         * Entities.
         * An OfficeAssignment only exists in relation to the instructor it's assigned to,
         * and therefor it's PK is also the FK to the Instructor Entity.
         * 
         * Problem:  EF cannot automatically recognize InstructorID as the PK of this
         * entity because it's name doesn't follow the ID or ClassNameID naming convention.
         *
         * Fix:  Use the Key attribute to identity InstructorID as the key
         */

         [StringLength(65)]
         [Display(Name ="Office Location")]
        public string Location { get; set; }

        //Navigation property
        public virtual Instructor Instructor { get; set; }
    }
}