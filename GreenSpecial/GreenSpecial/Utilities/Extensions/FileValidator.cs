using GreenSpecial.Utilities.Enums;

namespace GreenSpecial.Utilities.Extensions
{
    public static class FileValidator
    {
        public static bool ValidateFileType(this IFormFile file,FileHelper type)
        {
            if (type==FileHelper.Image)
            {
                if (!file.ContentType.Contains("image/"))
                {
                    return false;
                }
                return true;
            }
            if (type == FileHelper.Video)
            {
                if (!file.ContentType.Contains("video/"))
                {
                    return false;
                }
                return true;
            }
            if (type == FileHelper.Audio)
            {
                if (!file.ContentType.Contains("audio/"))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public static bool ValidateFileSize(this IFormFile file, SizeHelper size)
        {
            long filesize = file.Length;
            switch (size)
            {
                case SizeHelper.kb:
                    return filesize <= 1024;
                case SizeHelper.mb:
                    return filesize <= 1024*1024;
                case SizeHelper.gb:
                    return filesize <= 1024*1024*1024;
            }
            return false;
        }
        public static void DeleteFile(this string filename,string root,params string[] folders)
        {
            string path = root;
          
            for (int i = 0; i < folders.Length; i++)
            {
                Path.Combine(folders[i]);
            }
            path = Path.Combine(root, filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
