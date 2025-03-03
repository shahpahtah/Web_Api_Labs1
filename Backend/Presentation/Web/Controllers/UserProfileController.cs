using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Web.dto;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        private string GetUserRole()
        {
            return HttpContext.Items["UserRole"] as string;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var profile = await _profileService.GetUserProfileAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            // Convert to DTO for response
            var profileDto = MapToUserProfileDto(profile);
            return Ok(profileDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserProfileCreateDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Generate Id on the server if it's empty
                    Guid profileId = Guid.NewGuid();

                    // Map DTO to Domain Model
                    var profile = MapToUserProfile(createDto, profileId); // Pass in the generated ID

                    var createdProfile = await _profileService.CreateUserProfileAsync(profile);

                    // Convert back to DTO for response
                    var createdProfileDto = MapToUserProfileDto(createdProfile);
                    return CreatedAtAction(nameof(Get), new { id = createdProfileDto.Id }, createdProfileDto);
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError("AdditionalFieldsJson", "Invalid JSON format for AdditionalFields.");
                    return BadRequest(ModelState);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while creating the profile.");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfileByUser(Guid id, [FromForm] UserProfileUpdateDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map DTO to Domain Model
                    var profile = MapToUserProfile(updateDto);


                    var updatedProfile = await _profileService.UpdateUserProfileAsync(profile);
                    if (updatedProfile == null)
                    {
                        return NotFound();
                    }

                    // Convert back to DTO for response
                    var updatedProfileDto = MapToUserProfileDto(updatedProfile);
                    return Ok(updatedProfileDto);
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError("AdditionalFieldsJson", "Invalid JSON format for AdditionalFields.");
                    return BadRequest(ModelState);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while updating the profile.");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("Admin/{id}")] // New route for admin updates
        public async Task<IActionResult> UpdateProfileByAdmin(Guid id, [FromForm] UserProfileAdminUpdateDto updateDto)
        {
            string userRole = GetUserRole();
            if (userRole != "admin")
            {
                return Forbid("Only admins can update profiles.");
            }

            if (id != updateDto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map DTO to Domain Model
                    var profile = MapToUserProfile(updateDto);


                    var updatedProfile = await _profileService.UpdateUserProfileAsync(profile);
                    if (updatedProfile == null)
                    {
                        return NotFound();
                    }

                    // Convert back to DTO for response
                    var updatedProfileDto = MapToUserProfileDto(updatedProfile);
                    return Ok(updatedProfileDto);
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError("AdditionalFieldsJson", "Invalid JSON format for AdditionalFields.");
                    return BadRequest(ModelState);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "An error occurred while updating the profile.");
                }
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string userRole = GetUserRole();
            if (userRole != "admin")
            {
                return Forbid("Only admins can delete profiles.");
            }

            var deleted = await _profileService.DeleteUserProfileAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Helper methods to map between DTO and Domain Model
        private UserProfile MapToUserProfile(UserProfileCreateDto dto, Guid profileId) // Modified to accept profileId
        {
            return new UserProfile
            {
                Id = profileId, // use generated ID
                NickName = dto.NickName,
                AvatarFile = dto.AvatarFile,
                ExpertiseAreas = dto.ExpertiseAreas,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                TimeOfBirth = dto.TimeOfBirth,
                PlaceOfBirth = dto.PlaceOfBirth,
                CurrentLocation = dto.CurrentLocation,
                AdditionalFields = string.IsNullOrEmpty(dto.AdditionalFieldsJson)
                                    ? new Dictionary<string, object>()
                                    : JsonSerializer.Deserialize<Dictionary<string, object>>(dto.AdditionalFieldsJson)
            };
        }


        private UserProfile MapToUserProfile(UserProfileUpdateDto dto)
        {
            return new UserProfile
            {
                Id = dto.Id,
                NickName = dto.NickName,
                AvatarFile = dto.AvatarFile,
                ExpertiseAreas = dto.ExpertiseAreas,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                TimeOfBirth = dto.TimeOfBirth,
                PlaceOfBirth = dto.PlaceOfBirth,
                CurrentLocation = dto.CurrentLocation,
                AdditionalFields = string.IsNullOrEmpty(dto.AdditionalFieldsJson)
                                    ? new Dictionary<string, object>()
                                    : JsonSerializer.Deserialize<Dictionary<string, object>>(dto.AdditionalFieldsJson)
            };
        }

        private UserProfile MapToUserProfile(UserProfileAdminUpdateDto dto)
        {
            return new UserProfile
            {
                Id = dto.Id,
                NickName = dto.NickName,
                AvatarFile = dto.AvatarFile,
                ExpertiseAreas = dto.ExpertiseAreas,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                TimeOfBirth = dto.TimeOfBirth,
                PlaceOfBirth = dto.PlaceOfBirth,
                CurrentLocation = dto.CurrentLocation,
                AdditionalFields = string.IsNullOrEmpty(dto.AdditionalFieldsJson)
                                    ? new Dictionary<string, object>()
                                    : JsonSerializer.Deserialize<Dictionary<string, object>>(dto.AdditionalFieldsJson)
            };
        }

        private UserProfileDto MapToUserProfileDto(UserProfile profile)
        {
            return new UserProfileDto
            {
                Id = profile.Id,
                NickName = profile.NickName,
                AvatarFileName = profile.AvatarFileName,
                ExpertiseAreas = profile.ExpertiseAreas,
                Gender = profile.Gender,
                DateOfBirth = profile.DateOfBirth,
                TimeOfBirth = profile.TimeOfBirth,
                PlaceOfBirth = profile.PlaceOfBirth,
                CurrentLocation = profile.CurrentLocation,
                AdditionalFields = profile.AdditionalFields
            };
        }
    }
}