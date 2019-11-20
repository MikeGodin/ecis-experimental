using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hedwig.Controllers;
using Hedwig.Repositories;
using Hedwig.Models;

namespace HedwigTests.Controllers
{
    public class ChildrenControllerTests
    {
        [Theory]
        [InlineData(new string[]{}, false, false)]
        [InlineData(new string[]{"family"}, true, false)]
        [InlineData(new string[]{"family", "determinations"}, true, true)]
        [InlineData(new string[]{"determinations"}, false, false)]
        public async Task Get_IncludeEntities_GetsChildrenForOrganization_WithEntities(
            string[] include,
            bool shouldGetFamilies,
            bool includeDeterminations
        )
        {
            var organizationId = 1;

            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildrenForOrganizationAsync(organizationId))
                .Returns(Task.FromResult(new List<Child>{new Child{}}));

            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(organizationId, include);
            _children.Verify(c => c.GetChildrenForOrganizationAsync(organizationId), Times.Once());
            var times = shouldGetFamilies ? Times.Once() : Times.Never();
            _families.Verify(f => f.GetFamiliesByIdsAsync(It.IsAny<IEnumerable<int>>(), includeDeterminations), times);

        }

        [Theory]
        [InlineData(new string[]{}, false, false)]
        [InlineData(new string[]{"family"}, true, false)]
        [InlineData(new string[]{"family", "determinations"}, true, true)]
        [InlineData(new string[]{"determinations"}, false, false)]
        public async Task Get_Id_IncludeEntities_GetsChildForOrganization_WithEntities(
            string[] include,
            bool shouldGetFamilies,
            bool includeDeterminations
        )
        {
            var organizationId = 1;

            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildForOrganizationAsync(It.IsAny<Guid>(), organizationId))
                .Returns(Task.FromResult(new Child{FamilyId = 1}));

            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(Guid.NewGuid(), organizationId, include);
            _children.Verify(c => c.GetChildForOrganizationAsync(It.IsAny<Guid>(), organizationId), Times.Once);
            var times = shouldGetFamilies ? Times.Once() : Times.Never();
            _families.Verify(f => f.GetFamilyByIdAsync(It.IsAny<int>(), includeDeterminations), times);
        }

        [Theory]
        [InlineData(false, true, typeof(CreatedAtActionResult))]
        [InlineData(true, false, typeof(BadRequestResult))]
        public async Task Post_AddsChild_IfValid(
            bool hasId,
            bool shouldAddChild,
            Type resultType
        )
        {
            var organizationId = 1;

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var child = new Child();
            if(hasId) child.Id = Guid.NewGuid();

            var result = await controller.Post(organizationId, child);
            var times = shouldAddChild ? Times.Once() : Times.Never();
            _children.Verify(c => c.AddChild(It.IsAny<Child>()), times);
            Assert.IsType(resultType, result.Result);
        }

        [Theory]
        [InlineData(true, false, true, typeof(NoContentResult))]
        [InlineData(false, false, false, typeof(BadRequestResult))]
        [InlineData(true, true, true, typeof(NotFoundResult))]
        public async Task Put_UpdatesChild_IfValid_AndExists(
            bool idsMatch,
            bool shouldNotFind,
            bool shouldUpdateChild,
            Type resultType
        )
        {
            var organizationId = 1;

            var _children = new Mock<IChildRepository>();
            if(shouldNotFind) {
                _children.Setup(c => c.SaveChangesAsync())
                    .Throws(new DbUpdateConcurrencyException());
            }
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var pathId = Guid.NewGuid();
            var child = new Child();
            if(idsMatch) child.Id = pathId;

            var result = await controller.Put(pathId, organizationId, child);
            var times = shouldUpdateChild ? Times.Once() : Times.Never();
            _children.Verify(c => c.UpdateChild(It.IsAny<Child>()), times);
            Assert.IsType(resultType, result.Result);
        }
    }
}