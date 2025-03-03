using Swashbuckle.AspNetCore.Annotations;

namespace Web.dto
{
    public class UserProfileCreateDto
    {
        [SwaggerSchema(ReadOnly = true)]  // Hide from create, it's generated on the server
        public Guid Id { get; set; }  // Server-generated

        public string NickName { get; set; }

        public IFormFile AvatarFile { get; set; }

        public List<string> ExpertiseAreas { get; set; } = new List<string>();

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public TimeSpan? TimeOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        public string CurrentLocation { get; set; }

        public string AdditionalFieldsJson { get; set; }  // String JSON containing additional profile fields
    }


    public class UserProfileUpdateDto
    {
        public Guid Id { get; set; }  // REQUIRED - Existing Profile ID

        public string NickName { get; set; }

        public IFormFile AvatarFile { get; set; }

        public List<string> ExpertiseAreas { get; set; } = new List<string>();

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public TimeSpan? TimeOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        public string CurrentLocation { get; set; }

        public string? AdditionalFieldsJson { get; set; }  // String JSON containing additional profile fields
    }


    public class UserProfileAdminUpdateDto
    {
        public Guid Id { get; set; }  // REQUIRED - Existing Profile ID

        public string NickName { get; set; }

        public IFormFile AvatarFile { get; set; }

        public List<string> ExpertiseAreas { get; set; } = new List<string>();

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public TimeSpan? TimeOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        public string CurrentLocation { get; set; }

        public string? AdditionalFieldsJson { get; set; }  // String JSON containing additional profile fields
    }



    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string AvatarFileName { get; set; }
        public List<string> ExpertiseAreas { get; set; } = new List<string>();
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public TimeSpan? TimeOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string CurrentLocation { get; set; }

        //No Json needed for the DTO
        public Dictionary<string, object> AdditionalFields { get; set; }  // deserialized values
    }
}
