namespace EvAldarado.Extensions
{
    public static class IFormFileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            // Ensure to check for null file and handle common image content types
            return file != null && (
                file.ContentType.ToLower() == "image/jpg" ||
                file.ContentType.ToLower() == "image/jpeg" ||
                file.ContentType.ToLower() == "image/png" ||
                file.ContentType.ToLower() == "image/gif");
        }

        public static bool IsSmaller(this IFormFile file, int mb)
        {
            // Ensure to check for null file
            return file != null && (file.Length / 1024 / 1024 <= mb);
        }

        public static async Task<string> SaveFileAsync(this IFormFile file, string uploadsFolderPath)
        {
            // Generate a unique filename to avoid conflicts
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var path = Path.Combine(uploadsFolderPath, filename);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filename;
        }

        public static void Delete(string webroot, string folder, string filename)
        {
            var path = Path.Combine(webroot, folder, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
