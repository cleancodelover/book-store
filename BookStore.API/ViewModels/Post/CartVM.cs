﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BookStore.API.ViewModels.Get;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ViewModels.Post
{
    public partial class CreateCartVM
    {
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(450)]
        public string? UserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalAmount { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public List<CreateCartItemVM> Items { get; set; }
    }
}