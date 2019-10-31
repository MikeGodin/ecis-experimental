namespace Hedwig.Models
{
    /// <summary>
    /// Site-Owned entities are a sub-set of models in this app that adhere to certain business logic:
    /// - Entity is associated with a site; only users with appropriate site permissions can access the entity
    /// - Entity contains fields that can be updated by users with appropriate site permissions
    /// - Entity update history is augmented with change author information, and stored in time-versioned temporal tables
    /// </summary>
    public abstract class SiteOwnedEntity
    {
        public int SiteId { get; set; }
        public Site Site { get; set; }
        public int? AuthorId { get; set; }
        public User Author { get; set; }
    }
}
