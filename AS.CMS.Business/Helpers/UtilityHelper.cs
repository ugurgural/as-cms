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
            return File.ReadAllBytes(string.Format("{0}{1}", GetEditorFilesServerPath(), filePath));
        }

        public static string GetEditorFilesServerPath()
        {
            return HttpContext.Current.Server.MapPath("~/Dosyalar");
        }

        //TODO: Eğer zip dosyası belirtilen yolda daha önce üretildiyse içindeki dosyalar ile yeni kaydedilecek dosyaların tek tek hash karşılaştırmalarının yapılıp aynı değil ise update edilmesi gerekiyor. şu an logic eğer kaydedilecek dosyayı zipin içinde bulduysa silip yenisini ekliyor.
        public static byte[] GenerateZIPFileFromFileList(string[] fileList, string zipFilePath)
        {
            string serverPath = GetEditorFilesServerPath();

            using (ZipArchive archive = ZipFile.Open(string.Format("{0}{1}", serverPath, zipFilePath), ZipArchiveMode.Update))
            {
                foreach (var filePath in fileList)
                {
                    string fileName = filePath.Remove(0, filePath.LastIndexOf("/") + 1);

                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        var zipEntryContainsThisFile = archive.GetEntry(fileName);

                        if (zipEntryContainsThisFile != null)
                        {
                            zipEntryContainsThisFile.Delete();
                            archive.CreateEntryFromFile(HttpContext.Current.Server.MapPath(string.Format("~{0}", filePath)), filePath.Remove(0, filePath.LastIndexOf("/") + 1), CompressionLevel.Fastest);
                        }
                    }
                }  
            }

            return GetFile(zipFilePath);
        }
    }
}