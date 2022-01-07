

using System.IO;

namespace Fiorella_second.Utilities.File
{
    public class Helper
    {
        public static void RemoveFile(string root,string folder,string image)
        {
            string path = Path.Combine(root, folder, image);
            if (System.IO.File.Exists("path"))
            {
                System.IO.File.Delete(path);
            }
        }
        public enum UserRoles
        {
            SuperAdmin,
            Admin,
            Member,
            Moderator

        }
    }
}
