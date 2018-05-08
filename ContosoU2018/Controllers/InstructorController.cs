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
                .Include(i=>i.Courses)//include courses
                .ThenInclude(i=>i.Course)//get the course entity as well
                .ThenInclude(i=>i.Department)//get the department entity as well
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

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.SingleOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HireDate,ID,FirstName,LastName,Email,Address,City,PostalCode,Province")] Instructor instructor)
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }

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
