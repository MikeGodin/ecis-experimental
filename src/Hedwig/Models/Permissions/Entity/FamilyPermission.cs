namespace Hedwig.Models
{
    public class FamilyPermission : EntityPermission
    {
        public int FamilyId { get; set; }
        public Family Family { get; set; }
    }
}