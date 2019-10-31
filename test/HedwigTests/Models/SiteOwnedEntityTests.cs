using Xunit;
using Hedwig.Models;

namespace HedwigTests.Models
{
    public class SiteOwnedEntityTests
    {
        
        class SiteOwnedModel : SiteOwnedEntity { }
        [Fact]
        public void TemporalEntity_Has_AuthoredBy()
        {
            Assert.NotNull(typeof(SiteOwnedModel).GetProperty("AuthorId"));
        }

        [Fact]
        public void TemporalEntity_Has_SiteId()
        {
            Assert.NotNull(typeof(SiteOwnedModel).GetProperty("SiteId"));
        }

    }
}