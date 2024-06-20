﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[Table("Lecture_file")]
public partial class LectureFile
{
    [Key]
    [Column("Lecture_file_ID")]
    public int LectureFileId { get; set; }

    [Required]
    [Column("lecture_ID")]
    [StringLength(50)]
    [Unicode(false)]
    public string LectureId { get; set; }

    [Required]
    [Column("file_path")]
    [StringLength(255)]
    [Unicode(false)]
    public string FilePath { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; }

    [ForeignKey("LectureId")]
    [InverseProperty("LectureFiles")]
    public virtual Lecture Lecture { get; set; }
}