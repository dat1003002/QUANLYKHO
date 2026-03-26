using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QUANLYKHO.model
{
    public class Import
    {
        public int Id { get; set; }
        [Required] public string ImportCode { get; set; } = null!;
        public DateTime ImportDate { get; set; } = DateTime.Now;
        public string SupplierName { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
        public ICollection<ImportDetail> Details { get; set; } = new List<ImportDetail>();

        // Mới
        [Required]
        public int FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;
    }
}