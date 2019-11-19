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
        [Fact]
        public async Task Get_GetsChildrenForOrganization_WithoutFamily()
        {
            var organizationId = 1;
            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(organizationId, new string[]{});
            _children.Verify(c => c.GetChildrenForOrganizationAsync(organizationId), Times.Once);
            _families.Verify(f => f.GetFamiliesByIdsAsync_OLD(new int[]{}, null), Times.Never);
        }

        [Fact]
        public async Task Get_IncludesFamily_GetsChildrenForOrganization_WithFamily()
        {
            var organizationId = 1;
            var familyId = 1;

            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildrenForOrganizationAsync(organizationId))
                .Returns(Task.FromResult(new List<Child>{new Child{FamilyId = familyId}}));

            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(organizationId, new string[]{"family"});
            _children.Verify(c => c.GetChildrenForOrganizationAsync(organizationId), Times.Once);
            _families.Verify(f => f.GetFamiliesByIdsAsync_OLD(new int[]{familyId}, null), Times.Once);

        }

        [Fact]
        public async Task Get_Id_GetsChildForOrganization_WithoutFamily()
        {
            var organizationId = 1;
            var childId = Guid.NewGuid();

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(childId, organizationId, new string[]{});
            _children.Verify(c => c.GetChildForOrganizationByIdAsync(childId, organizationId), Times.Once);
            _families.Verify(f => f.GetFamilyByIdAsync(0, null), Times.Never);
        }

        [Fact]
        public async Task GetId_IncludesFamily_GetsChildForOrganization_WithFamily()
        {
            var organizationId = 1;
            var familyId = 1;
            var childId = Guid.NewGuid();

            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.GetChildForOrganizationByIdAsync(childId, organizationId))
                .Returns(Task.FromResult(new Child{FamilyId = familyId}));

            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            await controller.Get(childId, organizationId, new string[]{"family"});
            _children.Verify(c => c.GetChildForOrganizationByIdAsync(childId, organizationId), Times.Once);
            _families.Verify(f => f.GetFamilyByIdAsync(familyId, null), Times.Once);
        }

        [Fact]
        public async Task Post_AddsChild_ReturnsCreatedAtAction()
        {
            var organizationId = 1;
            var child = new Child();

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var result = await controller.Post(organizationId, child);
            _children.Verify(c => c.AddChild(child), Times.Once);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Post_DoesNotAddChild_ReturnsBadRequest_WhenChildHasId()
        {
            var organizationId = 1;
            var child = new Child{
                Id = Guid.NewGuid()
            };

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);
            
            var result = await controller.Post(organizationId, child);
            _children.Verify(c => c.AddChild(child), Times.Never);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Put_UpdatesChild_ReturnsNoContent()
        {
            var organizationId = 1;
            var childId = Guid.NewGuid();
            var child = new Child{ Id = childId };

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var result = await controller.Put(childId, organizationId, child);
            _children.Verify(c => c.UpdateChild(child), Times.Once);
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task Put_DoesNotUpdateChild_ReturnsBadRequest_WhenIdDoesNotMatch()
        {
            var organizationId = 1;
            var childId = Guid.NewGuid();
            var child = new Child { Id = Guid.NewGuid() };

            var _children = new Mock<IChildRepository>();
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var result = await controller.Put(childId, organizationId, child);
            _children.Verify(c => c.UpdateChild(child), Times.Never);
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Put_DoesNotUpdateChild_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var organizationId = 1;
            var childId = Guid.NewGuid();
            var child = new Child { Id = Guid.NewGuid() };

            var _children = new Mock<IChildRepository>();
            _children.Setup(c => c.SaveChangesAsync())
                .Throws(new DbUpdateConcurrencyException());
            var _families = new Mock<IFamilyRepository>();

            var controller = new ChildrenController(_children.Object, _families.Object);

            var result = await controller.Put(childId, organizationId, child);
            _children.Verify(c => c.UpdateChild(child), Times.Never);
            Assert.IsType<BadRequestResult>(result.Result);
        }

    }
}