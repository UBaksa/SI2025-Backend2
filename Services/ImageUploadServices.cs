using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace carGooBackend.Services
{
    public class ImageUploadResult
    {
        public bool Success { get; set; }
        public string Url { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ImageUploadService
    {
        private readonly Cloudinary _cloudinary;

        public ImageUploadService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            var acc = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(acc)
            {
                // dobijemo HTTPS url
                Api = { Secure = true }
            };
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ImageUploadResult { Success = false, ErrorMessage = "Fajl nije poslat." };

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "cargoo_users" // možeš promeniti folder
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK && uploadResult.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    return new ImageUploadResult { Success = false, ErrorMessage = $"Upload error: {uploadResult.Error?.Message}" };
                }

                return new ImageUploadResult
                {
                    Success = true,
                    Url = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString()
                };
            }
            catch (Exception ex)
            {
                return new ImageUploadResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }
}
