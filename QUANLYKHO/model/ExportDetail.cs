using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHO.model
{
    public class ExportDetail
    {
        public int Id { get; set; }
        public int ExportId { get; set; }
        public Export Export { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
    }
}
