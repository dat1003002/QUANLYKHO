using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHO.model
{
    public class InventoryCheck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string CheckCode { get; set; } = null!;
        public DateTime CheckDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime CutOffDate { get; set; } = DateTime.Now;
        public int CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedById { get; set; }
        public User ApprovedBy { get; set; }
        public bool IsApproved { get; set; } = false;
        public string Note { get; set; } = string.Empty;
        public ICollection<InventoryCheckDetail> Details { get; set; } = new List<InventoryCheckDetail>();

        [Required]
        public int FactoryId { get; set; }
        public Factory Factory { get; set; } = null!;
    }
}
