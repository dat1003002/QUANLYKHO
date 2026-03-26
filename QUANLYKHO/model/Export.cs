using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHO.model
{
    public class Export
    {
        public int Id { get; set; }
        public string ExportCode { get; set; } = null!;
        public DateTime ExportDate { get; set; } = DateTime.Now;
        [Required] public string Reason { get; set; } = null!;
        public string? RecipientName { get; set; }
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
        public ICollection<ExportDetail> Details { get; set; } = new List<ExportDetail>();

        // Mới
        [Required]
        public int FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;

    }
}
