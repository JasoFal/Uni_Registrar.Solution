using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityRegistrar.Models
{
  public class Course
  {
    public int CourseId { get; set; }

    [Required]
    [StringLength(100)]
    public string CourseName { get; set; }

    [Required]
    public int CourseNumber { get; set; }

    public List<StudentCourse> JoinEntities { get; }
  }
}