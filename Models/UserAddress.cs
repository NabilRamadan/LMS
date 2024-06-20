﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CRUDApi.Models;

[Table("UserAddress")]
[Index("ApplicationUserId", Name = "IX_UserAddress_ApplicationUserId", IsUnique = true)]
public partial class UserAddress
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string ApplicationUserId { get; set; }

    [ForeignKey("ApplicationUserId")]
    [InverseProperty("UserAddress")]
    public virtual AspNetUser ApplicationUser { get; set; }
}