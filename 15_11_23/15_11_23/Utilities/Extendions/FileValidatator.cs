using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _15_11_23.Utilities.Extendions
{
    public static class FileValidatator
    {

        public static bool ValidateType(this IFormFile file, string type = "image/")
        {
            if (file.ContentType.Contains(type)) return true;
            return false;
        }
        public static bool ValidataSize(this IFormFile file, int limitMb)
        {
            if (file.Length <= limitMb * 1024 * 1024) return true;
            return false;
        }
        public static async Task<string> CreateFile(this IFormFile file, string root, params string[] folder)
        {
            string originalFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            // Extract Guid-based portion
            string guidBasedFileName = ExtractGuidFileName(originalFileName);

            // Get the file format (extension)
            string fileFormat = GetFileFormat(originalFileName);

            // Combine Guid-based portion and file format
            string finalFileName = guidBasedFileName + fileFormat;

            string path = root;
            for (int i = 0; i < folder.Length; i++)
            {
                path = Path.Combine(path, folder[i]);
            }
            path = Path.Combine(path, finalFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return finalFileName;

        }
        public static async void DeleteFile(this string fileName,string root, params string[] folders)
        {
            string path = root;
            for(int i = 0;i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string ExtractGuidFileName(string fullFileName)
        {
            // Assuming the Guid-based portion is followed by an underscore
            int underscoreIndex = fullFileName.IndexOf('_');

            // Check if underscore is found
            if (underscoreIndex != -1)
            {
                // Extract Guid-based portion
                string guidBasedFileName = fullFileName.Substring(0, underscoreIndex);

                return guidBasedFileName;
            }
            else
            {
                // If no underscore is found, return the full filename
                return fullFileName;
            }
        }
        public static string GetFileFormat(string fullFileName)
        {
            // Get the file format (extension) by finding the last dot in the filename
            int lastDotIndex = fullFileName.LastIndexOf('.');

            // Check if dot is found
            if (lastDotIndex != -1)
            {
                // Extract file format
                string fileFormat = fullFileName.Substring(lastDotIndex);

                return fileFormat;
            }
            else
            {
                // If no dot is found, return an empty string or handle it as needed
                return string.Empty;
            }
        }

    }
}
