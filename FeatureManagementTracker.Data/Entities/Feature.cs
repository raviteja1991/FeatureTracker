using System.ComponentModel.DataAnnotations;

namespace FeatureManagementTracker.Data
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Title { get; set; }

        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(2)]
        public string EstimatedComplexity { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; }

        public DateTime? TargetCompletionDate { get; set; }
        public DateTime? ActualCompletionDate { get; set; }
    }
}
