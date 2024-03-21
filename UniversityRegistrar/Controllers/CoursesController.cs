using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniversityRegistrar.Controllers
{
  public class CoursesController : Controller
  {
    private readonly UniversityRegistrarContext _db;
  
    public CoursesController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Courses.ToList());
    }

    public ActionResult Details(int id)
    {
      Course thisCourse = _db.Courses
        .Include(c => c.JoinEntities)
        .ThenInclude(j => j.Student)
        .FirstOrDefault(c => c.CourseId == id);
      return View(thisCourse);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course)
    {
      _db.Courses.Add(course);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    
    public ActionResult AddStudent(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult AddCourse(Course course, int studentId)
    {
      #nullable enable
      StudentCourse? joinEntity = _db.StudentCourses.FirstOrDefault(j => (j.StudentId == studentId && j.CourseId == course.CourseId));
      #nullable disable
      if (joinEntity == null && studentId != 0)
      {
        _db.StudentCourses.Add(new StudentCourse() { StudentId = studentId, CourseId = course.CourseId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = course.CourseId });
    }
  }
}