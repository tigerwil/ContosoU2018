using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Models.SchoolViewModels
{
    public class AssignedCourseData
    {
        /*
         * This viewmodel will be used within the course assignments section
         * of the Instructor Edit and Create.  It will provide a list of course
         * checkboxes with CourseID, title as well as an indicator that the
         * instructor is assigned or not assigned to a particular course         * 
         */
        public int CourseID { get; set; }//For the Course ID
        public string Title { get; set; }//For the Course Title
        public bool Assigned { get; set; }//For checkbox (assigned|not assigned)
    }
}
