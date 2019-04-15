using System;

namespace Ndrome.Model
{
    public interface IAuditableEntity : IEntity
    {
        DateTime CreatedDate { get; set; }
        Guid CreatedBy { get; set; }
        DateTime UpdatedDate { get; set; }
        Guid UpdatedBy { get; set; }
    }

    public class AuditableEntity : Entity, IAuditableEntity
    {
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
