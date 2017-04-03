using System.IO;
using NetworkExtension;

namespace FileReceiver
{
    internal static class FileExtension
    {
        private static readonly string FileNameAttribute = PackageAttributes.FileName.GetPackageAttribute();

        internal static bool IsFileName(string data) => data.Contains(FileNameAttribute);

        internal static FileStream CreateFile(string fileNamePackage)
        {
            var fileName = Path.Combine(Path.GetTempPath(), fileNamePackage.Replace(FileNameAttribute, ""));
            File.Create(fileName).Close();
            return File.OpenWrite(fileName);
        }
    }
}