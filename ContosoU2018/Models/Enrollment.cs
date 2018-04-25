namespace ContosoU2018.Models
{
    public class Enrollment
    {

        public int EnrollmentID { get; set; }//PK

        public int CourseID { get; set; } //FK

        public int StudentID { get; set; }//FK

        public Grade? Grade { get; set; }//? meaning is nullable - because we don't start with a grade

        //Navigation Property :  Each enrollment must be for one student
        public virtual Student Student { get; set; }
    }

    //Grade enumeration
    public enum Grade
    {
        A, B, C, D, F
    }
}