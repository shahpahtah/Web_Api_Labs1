using Data.EF;
using Domain;
using Domain.Interfaces;

namespace Web.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserProfileService(AppDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<UserProfile> CreateUserProfileAsync(UserProfile profile)
        {
            profile.Id = Guid.NewGuid();
            await _context.UserProfiles.AddAsync(profile);

            if (profile.AvatarFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(profile.AvatarFile.FileName);
                    if (!IsValidImageFileExtension(fileExtension))
                    {
                        throw new Exception("Invalid image file type. Only .jpg, .jpeg, and .png are allowed.");
                    }
                    if (profile.AvatarFile.Length > 100 * 1024)
                    {
                        throw new Exception("Avatar size exceeds the limit of 100KB.");
                    }

                    profile.AvatarFileName = await SaveFileAsync(profile.AvatarFile, $"{profile.Id}_{Guid.NewGuid()}{fileExtension}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading avatar: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task<UserProfile> UpdateUserProfileAsync(UserProfile profile)
        {
            var existingProfile = await _context.UserProfiles.FindAsync(profile.Id);
            if (existingProfile == null)
            {
                return null;
            }

            // Update properties
            existingProfile.NickName = profile.NickName;
            existingProfile.Gender = profile.Gender;
            existingProfile.DateOfBirth = profile.DateOfBirth;
            existingProfile.TimeOfBirth = profile.TimeOfBirth;
            existingProfile.PlaceOfBirth = profile.PlaceOfBirth;
            existingProfile.CurrentLocation = profile.CurrentLocation;
            existingProfile.ExpertiseAreas = profile.ExpertiseAreas;

            // Update AdditionalFields
            if (profile.AdditionalFields != null)  // check if admin is sending the additional fields
            {
                existingProfile.AdditionalFields = profile.AdditionalFields;
            }

            if (profile.AvatarFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(profile.AvatarFile.FileName);
                    if (!IsValidImageFileExtension(fileExtension))
                    {
                        throw new Exception("Invalid image file type. Only .jpg, .jpeg, and .png are allowed.");
                    }
                    if (profile.AvatarFile.Length > 100 * 1024)
                    {
                        throw new Exception("Avatar size exceeds the limit of 100KB.");
                    }

                    // Delete existing avatar
                    if (!string.IsNullOrEmpty(existingProfile.AvatarFileName))
                    {
                        DeleteFile(existingProfile.AvatarFileName);
                    }

                    existingProfile.AvatarFileName = await SaveFileAsync(profile.AvatarFile, $"{profile.Id}_{Guid.NewGuid()}{fileExtension}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading avatar: {ex.Message}");
                }
            }

            _context.UserProfiles.Update(existingProfile);
            await _context.SaveChangesAsync();
            return existingProfile;
        }

        public async Task<bool> DeleteUserProfileAsync(Guid id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(profile.AvatarFileName))
            {
                DeleteFile(profile.AvatarFileName);
            }

            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserProfile> GetUserProfileAsync(Guid id)
        {
            return await _context.UserProfiles.FindAsync(id);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string fileName)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
            string filePath = Path.Combine(uploadPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private bool IsValidImageFileExtension(string fileExtension)
        {
            fileExtension = fileExtension.ToLowerInvariant();
            return fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png";
        }
    }
}
