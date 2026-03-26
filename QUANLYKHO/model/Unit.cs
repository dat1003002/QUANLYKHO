using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYKHO.model
{
    public class Unit
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}
