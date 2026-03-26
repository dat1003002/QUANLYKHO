using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUANLYKHO.model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string MaterialCode { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Barcode { get; set; } = null!;
        [StringLength(500)]
        public string Specification { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int UnitId { get; set; }
        public Unit Unit { get; set; } = null!;
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        [Required]
        public int FactoryId { get; set; }
        [ForeignKey(nameof(FactoryId))]
        public Factory Factory { get; set; } = null!;
        public decimal CurrentQuantity { get; set; }
        public decimal TotalImported { get; set; }
        public decimal TotalExported { get; set; }
        public decimal SafetyStock { get; set; }
        public DateTime? LastImportDate { get; set; }
        public int? LastImportById { get; set; }
        public User LastImportBy { get; set; }
        [NotMapped]
        public string FullLocation =>
            Location != null
                ? $"{Location.Shelf?.Warehouse?.Factory?.Name} → {Location.Shelf?.Warehouse?.Name} → {Location.Shelf?.Name} → {Location.Code}"
                : "Chưa phân vị trí";

        [NotMapped]
        public bool IsLowStock => CurrentQuantity <= SafetyStock;
        public ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
        public ICollection<ExportDetail> ExportDetails { get; set; } = new List<ExportDetail>();
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}