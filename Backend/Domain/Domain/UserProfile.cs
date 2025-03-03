using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Domain
{
    public class UserProfile
    {
        
            [Key]
            public Guid Id { get; set; }
            [Required]
            public string NickName { get; set; }
            public string AvatarFileName { get; set; } // Store filename (path)
            [NotMapped]
            public IFormFile AvatarFile { get; set; }  // For uploading only
            public List<string> ExpertiseAreas { get; set; } = new List<string>();
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public TimeSpan? TimeOfBirth { get; set; }
            public string PlaceOfBirth { get; set; }
            public string CurrentLocation { get; set; }

        [Column(TypeName = "nvarchar(max)")]  // or "json" depending on your database
        public string? AdditionalFieldsJson { get; set; }

        [NotMapped]
        public Dictionary<string, object> AdditionalFields
        {
            get
            {
                if (string.IsNullOrEmpty(AdditionalFieldsJson))
                {
                    return new Dictionary<string, object>();
                }
                return JsonSerializer.Deserialize<Dictionary<string, object>>(AdditionalFieldsJson);
            }
            set
            {
                AdditionalFieldsJson = JsonSerializer.Serialize(value);
            }
        }
    }
}
