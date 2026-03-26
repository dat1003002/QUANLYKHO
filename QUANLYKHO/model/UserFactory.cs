using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUANLYKHO.model
{
    public class UserFactory
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int FactoryId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(FactoryId))]
        public Factory Factory { get; set; } = null!;

        public bool IsDefault { get; set; } = false;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public string? AssignedByUsername { get; set; }

        public bool CanManageProducts { get; set; } = true;
        public bool CanImport { get; set; } = true;
        public bool CanExport { get; set; } = true;
        public bool CanApproveInventoryCheck { get; set; } = true;
    }
}