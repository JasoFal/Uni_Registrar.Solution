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
      if (!ModelState.IsValid)
      {
        return View(course);
      }
      else
      {
        _db.Courses.Add(course);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
    
    public ActionResult AddStudent(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      ViewBag.StudentId = new SelectList(_db.Students, "StudentId", "Name");
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult AddStudent(Course course, int studentId)
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

    public ActionResult Edit(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult Edit(Course course)
    {
      _db.Courses.Update(course);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Course thisCourse = _db.Courses.FirstOrDefault(c => c.CourseId == id);
      _db.Remove(thisCourse);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      StudentCourse joinEntry = _db.StudentCourses.FirstOrDefault(e => e.StudentCourseId == joinId);
      _db.StudentCourses.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}