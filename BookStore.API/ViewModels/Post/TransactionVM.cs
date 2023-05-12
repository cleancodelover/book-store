﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.ViewModels.Post
{
    public partial class CreateTransactionVM
    {
        [JsonIgnore]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(450)]
        public string CartId { get; set; }
        [StringLength(450)]
        public string TransactionReference { get; set; }
        [StringLength(150)]
        public string Narration { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
    }
}