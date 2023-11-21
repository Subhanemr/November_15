﻿using System.ComponentModel.DataAnnotations;

namespace _15_11_23.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name must be entered mutled")]
        [MaxLength(25, ErrorMessage = "It should not exceed 25 characters")]
        public string Name { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
    }
}
