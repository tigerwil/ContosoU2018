using ContosoU2018.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2018.Data
{
    public class SchoolContext: DbContext
    {
        //Constructor
        public SchoolContext(DbContextOptions<SchoolContext> options):base(options)
        {

        }

        //Specify the entity sets - corresponding to database tables 
        // each entity corresponds to a row in a table

        public DbSet<Person>People { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        //creating a more complex data model
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        /*
         * Override the default behavior by specifying singular table names using 
         * the Fluent API of the Entity Framework.  Also, we will be mapping our
         * composite PK's using this Fluent API (only way to do it)
         * 
         */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1. Change plural to singular table names
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<Department>().ToTable("Department");


            //2. Map the composite PK's using Fluent API
            //Composite PK on CourseAssignment Table 
            //on CourseID,InstructorID
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(c => new { c.CourseID, c.InstructorID });
        }



        //end creating a more complex data model


        /* Plural names will be used in this case because the EF will create a database with the
         * table names matching the DBSet property names.
         * Property names for collections are typically plural (Students rather than Student),but many 
         * developers disagree about whether table names should be plural or not.
         * Later - we will overrride the naming of the tables
         * 
         */
    }
}
