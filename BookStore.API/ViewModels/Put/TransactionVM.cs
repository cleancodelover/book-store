﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ViewModels.Put
{
    public partial class EditTransactionVM
    {
        [Key]
        public string Id { get; set; }
        [StringLength(450)]
        public string CartId { get; set; }
        [StringLength(450)]
        public string TransactionReference { get; set; }
        [StringLength(150)]
        public string Narration { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Amount { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
    }
}