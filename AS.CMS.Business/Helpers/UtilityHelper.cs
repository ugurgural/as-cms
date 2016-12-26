using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AS.CMS.Business.Helpers
{
    public static class UtilityHelper
    {
        public static string GenerateMD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string GetSiteDomain()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        }

        public static string StripHtml(this string value)
        {
            return Regex.Replace(value, "<(.|\\n)*?>", string.Empty);
        }

        public static byte[] GetFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        public static byte[] GenerateZIPFileFromFileList(string[] fileList, string zipFilePath)
        {
            using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                if (archive.Entries != null && archive.Entries.Count > 0)
                {
                    var fileInZip = (from f in archive.Entries where f.Name == Path.GetFileName(zipFilePath) select f).FirstOrDefault();
                    fileInZip.Delete();
                }

                foreach (var filePath in fileList)
                {
                    archive.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Fastest);
                    
                }

                return GetFile(zipFilePath);
            }
        }
    }
}