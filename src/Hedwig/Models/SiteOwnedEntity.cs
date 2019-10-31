namespace Hedwig.Models
{
    public abstract class SiteOwnedEntity : TemporalEntity
    {
        public int SiteId { get; set; }
        public Site Site { get; set; }
    }
}
