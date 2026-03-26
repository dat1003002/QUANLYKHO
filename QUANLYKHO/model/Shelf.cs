// Shelf.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUANLYKHO.model
{
    public class Shelf
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        public Warehouse Warehouse { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}