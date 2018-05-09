using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2018.Data;
using ContosoU2018.Models;
using ContosoU2018.Models.SchoolViewModels;

namespace ContosoU2018.Controllers
{
    public class InstructorController : Controller
    {
        private readonly SchoolContext _context;

        public InstructorController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index(int? id,//Add param for Selected Instructor
                                               int? courseID)//Add param for Selected Course
        {
            //Original code - converted to using a ViewModel
            //return View(await _context.Instructors
            //    .Include(i=>i.OfficeAssignment)//Include offices assigned to instructor
            //    .ToListAsync());

            //Create instance of our InstructorIndexData ViewModel
            var viewModel = new InstructorIndexData();

            //Populate the ViewModel Instructors from database context
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)//include offices assigned
                .Include(i => i.Courses)//include courses
                .ThenInclude(i => i.Course)//get the course entity as well
                .ThenInclude(i => i.Department)//get the department entity as well
                .ToListAsync();


            //================ INSTRUCTOR HAS BEEN SELECTED ===============//
            //check if id param has been passed in
            if (id != null)
            {
                //Get the single instructor's data
                Instructor instructor = viewModel.Instructors
                    .Where(i => i.ID == id.Value)
                    .SingleOrDefault();

                //check if instructor was found
                if (instructor == null)
                {
                    return NotFound(); //return not found page
                }

                //if we get this far, ok to get courses that instructor teaches
                //populate view model with instructor courses
                viewModel.Courses = instructor.Courses.Select(s => s.Course);

                //Get the Instructor name for display in view
                ViewData["InstructorName"] = instructor.FullName;

                //Get the instructor id for use within view (highlighting selected row)
                ViewData["InstructorID"] = id.Value;  //the id that was passed in url
                                                      //or get it from instructor object
                                                      //ViewData["InstructorID"] = instructor.ID;

            }
            //================== END INSTRUCTOR SELECTED ==================//

            //===================== COURSE SELECTED =======================//
            if (courseID != null)
            {
                //If we have the courseID param - get all enrollments for this course
                //using explicit loading this time: loading only if required
                _context.Enrollments//get all enrollments
                    .Include(i => i.Student)//including student
                                            //Only for selected course
                .Where(c => c.CourseID == courseID.Value).Load(); //explicit loading

                //get enrollments for this specic course
                var enrollments = viewModel.Courses
                    .Where(e => e.CourseID == courseID).SingleOrDefault();

                if (enrollments == null)
                {
                    return NotFound();
                }

                //Populate viewModel with enrollments
                viewModel.Enrollments = enrollments.Enrollments;

                //Pass CourseID back to view (for selected course highlight)
                ViewData["CourseID"] = courseID.Value;
            }

            //================== END COURSE SELECTED ======================//
            return View(viewModel);


        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,ID,FirstName,LastName,Email,Address,City,PostalCode,Province")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

        /* The code in the PopulateAssignedCourseData method reads through all Course entities in order to load 
         * a list of courses using the view model class. For each course, the code checks whether the course 
         * exists in the instructor's Courses navigation property. To create efficient lookup when checking whether 
         * a course is assigned to the instructor, the courses assigned to the instructor are put into a HashSet collection. 
         * The Assigned property is set to true for courses the instructor is assigned to. The view will use this property 
         * to determine which check boxes must be displayed as selected. Finally, the list is passed to the view in ViewData.
         */
        private void PopulateAssignedCourseData(Instructor instructor)
        {
            //get all courses
            var allCourses = _context.Courses;

            //create a hashset of instructor courses (HashSet of integers populated with course id)
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));

            //Create and populate the AssignedCourseData ViewModel
            var viewModel = new List<AssignedCourseData>();//create


            //populate it once for each of the courses within allCourses
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }

            //Save the viewModel within the ViewData object for use within View
            ViewData["Courses"] = viewModel;

        }


        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)//mwilliams:  added office assignemnts
                .Include(i => i.Courses)//mwilliams:  must include courses for assignedcoursedata
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            //mwilliams:  Populate the Assigned Course Data for this instructor
            PopulateAssignedCourseData(instructor);


            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("HireDate,ID,FirstName,LastName,Email,Address,City,PostalCode,Province")] Instructor instructor)
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            //mwilliams:  take care of overposting attacks
            //            also added string array for course assignments
            if (id == null)
            {
                return NotFound();
            }

            //First find the instructor to update
            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)//include office assignments
                .Include(i => i.Courses)//include courses for course assignments
                .ThenInclude(i => i.Course)//for update of course
                .SingleOrDefaultAsync(i => i.ID == id);//only this instructor

            //bind the form inputs to the instructor to update object
            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate, "",
                i => i.FirstName,
                i => i.LastName,
                i => i.HireDate,
                i => i.OfficeAssignment,
                i => i.Address,
                i => i.City,
                i => i.PostalCode,
                i => i.Province))
            {
                //Check for empty string in office location
                if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;//remove the complete record
                }

                //update the instructor (along with assigned courses
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);

                if (ModelState.IsValid)
                {
                    try
                    {
                        await _context.SaveChangesAsync();//this is the update table stmt.
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Unable to save changes.");
                    }
                    return RedirectToAction("Index");//Back to index page for this controller
                }
            }
            //return view with model data
            return View(instructorToUpdate);        
        }



        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                //If no checkboxes were selected, initialize the Courses navigation property
                //with an empty collection and return
                instructorToUpdate.Courses = new List<CourseAssignment>();
                return;
            }

            //To facilitate efficient lookups, 2 collections will be stored in HashSet objects
            //: selectedCourseHS ->  selected course (hashset of checkboxe selections)
            //: instructorCourses -> instructor courses (hashset of courses assigned to instructor)
            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.Courses.Select(c => c.Course.CourseID));

            //Loop through all courses in the database and check each course against the ones
            //currently assigned to the instructor versus the ones that were selected in the
            //view
            foreach (var course in _context.Courses)//Loop all courses
            {
                //CONDITION 1:
                //If the checkbox for a course was selected but the course isn't in the 
                //Instructor.Courses navigation property, the course is added to the collection
                //in the navigation property
                if (selectedCourseHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        instructorToUpdate.Courses.Add(new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.CourseID
                        });
                    }
                }
                //CONDITION 2:
                //If the check box for a course wasn't selected, but the course is in the 
                //Instructor.Courses navigation property, the course is removed 
                //from the navigation property.
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        CourseAssignment courseToRemove =
                            instructorToUpdate.Courses
                            .SingleOrDefault(i => i.CourseID == course.CourseID);
                        _context.Remove(courseToRemove);
                    }
                }

            }//end foreach
        }//End of UpdateInstructorCourse


        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.SingleOrDefaultAsync(m => m.ID == id);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
