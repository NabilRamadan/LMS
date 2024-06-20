﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[Table("Faculty")]
public partial class Faculty
{
    [Key]
    [Column("faculty_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string FacultyId { get; set; }

    [Column("university_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string UniversityId { get; set; }

    [Required]
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; }

    [Column("levels")]
    public int Levels { get; set; }

    [Required]
    [Column("Logo_path")]
    [StringLength(255)]
    [Unicode(false)]
    public string LogoPath { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("last_semester_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string LastSemesterId { get; set; }

    [InverseProperty("Faculty")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [InverseProperty("Faculty")]
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    [ForeignKey("LastSemesterId")]
    [InverseProperty("Faculties")]
    public virtual Semester LastSemester { get; set; }

    [InverseProperty("Faculty")]
    public virtual ICollection<News> News { get; set; } = new List<News>();

    [InverseProperty("Faculty")]
    public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();

    [ForeignKey("UniversityId")]
    [InverseProperty("Faculties")]
    public virtual University University { get; set; }

    [InverseProperty("Faculty")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}