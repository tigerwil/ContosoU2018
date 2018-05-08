using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        /*The instructors index view (page) will need to show data from 
         * from 3 different tables (entities), so for this reason we are
         * creating this ViewModel class.  It will include the data for 
         * the Instructor, Course, and Enrollment
         * 
         */

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
