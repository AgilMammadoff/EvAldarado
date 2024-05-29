﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvAldarado.Models
{
    public class Category
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParentId")]
        public Category ParentCategory { get; set; }

        public List<UserProducts> UserProducts { get; set; }
    }
}
