using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace UniversityRegistrar.Models
{
  public class Student
  {
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DOE { get; set; }
    
    public List<StudentCourse> JoinEntities { get; }
  }
}