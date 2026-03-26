using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QUANLYKHO.model
{
    public class Factory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = null!; 

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public ICollection<UserFactory> UserFactories { get; set; } = new List<UserFactory>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}