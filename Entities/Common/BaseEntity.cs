using System.ComponentModel.DataAnnotations;

namespace Entities.Common
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatedIp { get; set; }
        public string? UpdatedIp { get; set; }
        public bool IsActive { get; set; }
    }
}
