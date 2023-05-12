﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DAL.Models
{
    public partial class CartItem
    {
        [Key]
        public string Id { get; set; }
        [StringLength(450)]
        public string BookId { get; set; }
        [StringLength(450)]
        public string CartId { get; set; }
        public int? Quantity { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateCreated { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("CartItems")]
        public virtual Book Book { get; set; }
        [ForeignKey("CartId")]
        [InverseProperty("CartItems")]
        public virtual Cart Cart { get; set; }
    }
}