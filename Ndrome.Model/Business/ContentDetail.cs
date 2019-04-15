using System;
using System.ComponentModel.DataAnnotations;

namespace Ndrome.Model.Business
{
    public class ContentDetail : AuditableEntity
    {
        public Guid ContentID { get; set; }
        public Guid Version { get; set; }
        [MaxLength(10000)]
        public string Body { get; set; }
    }
}
