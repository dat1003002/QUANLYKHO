using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUANLYKHO.model
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int ShelfId { get; set; }

        [ForeignKey(nameof(ShelfId))]
        public Shelf Shelf { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}