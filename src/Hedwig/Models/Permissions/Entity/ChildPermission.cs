using System;

namespace Hedwig.Models
{
    public class ChildPermission : EntityPermission
    {
        public Guid ChildId { get; set; }
        public Child Child { get; set; }
    }
}