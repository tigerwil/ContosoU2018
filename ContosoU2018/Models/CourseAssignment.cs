namespace ContosoU2018.Models
{
    public class CourseAssignment
    {
        //Creating a more complex data model

        //Composite PK, FK to Instructor Entity
        public int InstructorID { get; set; }

        //Composite PK, FK to Course Entity
        public int CourseID { get; set; }

        /*
         * Note:  the only way to identify composite primary keys to EntityFramework is by
         * using the fluent API  within the SchoolContext Class.
         * It cannot be done with attributes
         */

        //Navigation properties:
        //1 Course to many course assignments (each assigned course belongs to 1 Course) 
        //1 instructor to many course assignments (each assigned instructor belongs to 1 Instructor)
        public virtual Instructor Instructor { get; set; }

        public virtual Course Course { get; set; }

    }
}