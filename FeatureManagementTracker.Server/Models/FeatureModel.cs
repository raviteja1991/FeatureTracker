using System.ComponentModel.DataAnnotations;

namespace FeatureManagementTracker.Server.Models
{
    public class FeatureModel
    {
        public int? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }      
        public required string Complexity { get; set; }
        public required string Status { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }
}
