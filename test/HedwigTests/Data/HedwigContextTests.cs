using Xunit;
using Hedwig.Data;
using Moq;
using Moq.Protected;
using Hedwig.Models;
using Microsoft.EntityFrameworkCore;
using HedwigTests.Fixtures;

namespace HedwigTests.Data
{
  public class HedwigContextTests
  {
    [Fact(Skip = "Not working")]
    public void Add_TemporalEntity_AddsAuthor()
    {
      // If a HedwigContext instance exists
      var opts = new DbContextOptionsBuilder<HedwigContext>()
        .UseInMemoryDatabase<HedwigContext>("db");
      var httpContextAccessor = new TestHttpContextAccessorProvider().HttpContextAccessor;
      var contextMock = new Mock<HedwigContext>(opts.Options, httpContextAccessor);
      var child = new Child();
      contextMock.CallBase = true;

      // When a temporal entity is added
      contextMock.Object.Add(child);

      // Then author is added to the entity
      Assert.Equal(1, child.AuthorId.Value);
    }

    [Fact(Skip = "Not working")]
    public void Update_TemporalEntity_AddsAuthor()
    {
      // If a HedwigContext instance exists
      var opts = new DbContextOptionsBuilder<HedwigContext>()
        .UseInMemoryDatabase<HedwigContext>("db");
      var httpContextAccessor = new TestContextProvider().HttpContextAccessor;
      var contextMock = new Mock<HedwigContext>(opts.Options, httpContextAccessor);
      var child = new Child();
      contextMock.Setup(c => c.Update(child)).CallBase();

      // When a temporal entity is updated
      contextMock.Object.Update(child);

      // Then author is added to the entity
      Assert.Equal(1, child.AuthorId.Value);
    }
  }
}