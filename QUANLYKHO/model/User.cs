using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QUANLYKHO.model
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool IsSuperAdmin { get; set; } = false;

        public bool CanImport { get; set; } = false;
        public bool CanExport { get; set; } = false;
        public bool CanCreateInventoryCheck { get; set; } = false;
        public bool CanApproveInventoryCheck { get; set; } = false;
        public bool CanDeleteProduct { get; set; } = false;

        public ICollection<UserFactory> UserFactories { get; set; } = new List<UserFactory>();
        public ICollection<Export> Exports { get; set; } = new List<Export>();
        public ICollection<Import> Imports { get; set; } = new List<Import>();
        public ICollection<InventoryCheck> CreatedInventoryChecks { get; set; } = new List<InventoryCheck>();
        public ICollection<InventoryCheck> ApprovedInventoryChecks { get; set; } = new List<InventoryCheck>();
    }
}