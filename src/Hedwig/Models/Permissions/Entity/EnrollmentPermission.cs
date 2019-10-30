namespace Hedwig.Models
{
    public class EnrollmentPermission : EntityPermission
    {
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
    }
}