using FeatureManagementTracker.Data;
using FeatureManagementTracker.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static FeatureManagementTracker.Server.Utilities.Enum;

namespace FeatureManagementTracker.Tests
{
    public class FeatureControllerTests
    {
        private readonly FeatureController _controller;
        private readonly FeatureDBContext _context;

        public FeatureControllerTests()
        {
            var options = new DbContextOptionsBuilder<FeatureDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid().ToString()) // Use a unique database name
                .Options;

            _context = new FeatureDBContext(options);

            // Seed the database with initial data
            _context.Features.AddRange(
                new Feature { Id = 1, Title = "Feature1", Description = "Description1", EstimatedComplexity = "M", Status = "New" },
                new Feature { Id = 2, Title = "Feature2", Description = "Description2", EstimatedComplexity = "L", Status = "Active" }
            );
            _context.SaveChanges();

            _controller = new FeatureController(_context);
        }

        [Fact]
        public async Task GetAllFeaturesAsync_ShouldReturnOkResult_WithListOfFeatures()
        {
            // Act
            var result = await _controller.GetAllFeaturesAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFeatures = Assert.IsType<List<Feature>>(okResult.Value);
            Assert.Equal(2, returnedFeatures.Count);
        }

        [Fact]
        public async Task GetFeatureByIdAsync_ShouldReturnNotFound_WhenFeatureDoesNotExist()
        {
            // Act
            var result = await _controller.GetFeatureByIdAsync(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Feature with ID 999 not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task CreateFeatureAsync_ShouldReturnCreatedAtAction_WhenFeatureIsValid()
        {
            // Arrange
            var newFeature = new Feature { Id = 3, Title = "New Feature", Description = "New Description", EstimatedComplexity = "S", Status = "New" };

            // Act
            var result = await _controller.CreateFeatureAsync(newFeature);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdFeature = Assert.IsType<Feature>(createdResult.Value);
            Assert.Equal(newFeature.Id, createdFeature.Id);
        }

        [Fact]
        public async Task UpdateFeatureAsync_ShouldReturnNoContent_WhenFeatureIsUpdatedSuccessfully()
        {
            // Arrange
            var existingFeature = await _context.Features.FindAsync(1);
            if (existingFeature == null)
            {
                Assert.True(false, "Feature with Id 1 not found.");
            }

            // Update the existing feature properties
            existingFeature.Title = "Updated Feature";
            existingFeature.Description = "Updated Description";
            existingFeature.EstimatedComplexity = "XL";
            existingFeature.Status = "Active";
            existingFeature.TargetCompletionDate = new DateTime(2024, 12, 31);

            // Act
            var result = await _controller.UpdateFeatureAsync(existingFeature.Id, existingFeature);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteFeatureAsync_ShouldReturnNotFound_WhenFeatureDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteFeatureAsync(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
