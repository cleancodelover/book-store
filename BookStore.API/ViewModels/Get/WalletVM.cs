﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ViewModels.Get
{
    public partial class WalletVM
    {
        [Key]
        public string Id { get; set; }
        [StringLength(450)]
        public string UserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Balance { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateModifed { get; set; }
        [StringLength(450)]
        public string ModifiedBy { get; set; }
        public virtual AspNetUserVM User { get; set; }
    }
}