using FeatureManagementTracker.Data;
using FeatureManagementTracker.Server.Controllers;
using FeatureManagementTracker.Server.Models;
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
            var returnedFeatures = Assert.IsType<List<FeatureModel>>(okResult.Value);
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
        public async Task CreateFeatureAsync_ShouldReturnOkResult_WhenFeatureIsValid()
        {
            // Arrange
            var newFeature = new FeatureModel { Id = 3, Title = "New Feature", Description = "New Description", Complexity = "S", Status = "New" };

            // Act
            var result = await _controller.CreateFeatureAsync(newFeature);

            // Assert
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            var createdFeature = Assert.IsType<FeatureModel>(createdResult.Value);
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
            var updatedFeatureModel = new FeatureModel
            {
                Id = existingFeature.Id,
                Title = "Updated Feature",
                Description = "Updated Description",
                Complexity = "XL",
                Status = "Active",
                TargetDate = new DateTime(2024, 12, 31),
                ActualDate = existingFeature.ActualCompletionDate // Assuming this remains unchanged
            };

            // Act
            var result = await _controller.UpdateFeatureAsync(updatedFeatureModel.Id.Value, updatedFeatureModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            // Check the response for a success message and status
            Assert.Equal("1", (string)response.GetType().GetProperty("status").GetValue(response, null));
            Assert.Equal("Feature updated successfully.", (string)response.GetType().GetProperty("message").GetValue(response, null));

            // Assert the updated feature details
            var returnedFeature = response.GetType().GetProperty("feature").GetValue(response, null) as Feature;
            Assert.NotNull(returnedFeature);
            Assert.Equal(updatedFeatureModel.Id, returnedFeature.Id);
            Assert.Equal(updatedFeatureModel.Title, returnedFeature.Title);
            Assert.Equal(updatedFeatureModel.Description, returnedFeature.Description);
            Assert.Equal(updatedFeatureModel.Complexity, returnedFeature.EstimatedComplexity);
            Assert.Equal(updatedFeatureModel.Status, returnedFeature.Status);
            Assert.Equal(updatedFeatureModel.TargetDate, returnedFeature.TargetCompletionDate);
            Assert.Equal(updatedFeatureModel.ActualDate, returnedFeature.ActualCompletionDate);
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
