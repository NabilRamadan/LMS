﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[PrimaryKey("StudentId", "QuizId")]
[Table("Student_Quiz_Grade")]
public partial class StudentQuizGrade
{
    [Key]
    [Column("student_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string StudentId { get; set; }

    [Key]
    [Column("quiz_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string QuizId { get; set; }

    [Column("grade")]
    public double? Grade { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("StudentQuizGrades")]
    public virtual Quiz Quiz { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("StudentQuizGrades")]
    public virtual User Student { get; set; }
}