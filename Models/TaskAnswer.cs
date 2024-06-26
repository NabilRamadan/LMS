﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[Table("Task_answer")]
public partial class TaskAnswer
{
    [Key]
    [Column("answer_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string AnswerId { get; set; }

    [Column("task_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string TaskId { get; set; }

    [Column("student_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string StudentId { get; set; }
    
    [Column("name")]
    public string FileName { get; set; }

    [Column("grade")]
    public double? Grade { get; set; }

    [Required]
    [Column("file_path")]
    [StringLength(255)]
    [Unicode(false)]
    public string FilePath { get; set; }

    [Column("status")]
    [StringLength(40)]
    [Unicode(false)]
    public string Status { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("TaskAnswers")]
    public virtual User Student { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskAnswers")]
    public virtual Task Task { get; set; }
}