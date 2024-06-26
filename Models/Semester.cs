﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[Table("Semester")]
public partial class Semester
{
    [Key]
    [Column("Semester_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string SemesterId { get; set; }

    [Column("faculty_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string FacultyId { get; set; }

    [Column("start_Date")]
    public DateOnly StartDate { get; set; }

    [Column("end_Date")]
    public DateOnly EndDate { get; set; }

    [Required]
    [Column("years")]
    [StringLength(50)]
    [Unicode(false)]
    public string Years { get; set; }

    [Column("number")]
    public int Number { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Semester")]
    public virtual ICollection<CourseSemester> CourseSemesters { get; set; } = new List<CourseSemester>();

    [InverseProperty("LastSemester")]
    public virtual ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();

    [ForeignKey("FacultyId")]
    [InverseProperty("Semesters")]
    public virtual Faculty Faculty { get; set; }
}