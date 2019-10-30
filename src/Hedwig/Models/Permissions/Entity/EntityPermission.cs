namespace Hedwig.Models
{
    public abstract class EntityPermission
    {
        public int Id { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization;
    }
}