using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHO.model
{
    public class InventoryCheckDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int InventoryCheckId { get; set; }
        public InventoryCheck InventoryCheck { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal SystemQuantity { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal Variance => ActualQuantity - SystemQuantity;
        public string Note { get; set; }
    }
}
