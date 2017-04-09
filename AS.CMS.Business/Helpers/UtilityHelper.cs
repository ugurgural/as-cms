using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public static byte[] GenerateZIPFileFromFileList(string[] fileList, string zipFilePath)
        {
            string serverPath = GetEditorFilesServerPath();

            using (ZipArchive archive = ZipFile.Open(string.Format("{0}{1}", serverPath, zipFilePath), ZipArchiveMode.Update))
            {
                foreach (var filePath in fileList)
                {
                    if (!string.IsNullOrWhiteSpace(filePath))
                    {
                        string currentfileName = filePath.Remove(0, filePath.LastIndexOf("/") + 1);
                        string currentfilePath = HttpContext.Current.Server.MapPath(string.Format("~{0}", filePath));

                        var zipEntryContainsThisFile = archive.GetEntry(currentfileName);
                        FileStream currentFile = File.Open(currentfilePath, FileMode.Open);

                        if (zipEntryContainsThisFile != null)
                        {
                            if (zipEntryContainsThisFile.Length != currentFile.Length)
                            {
                                zipEntryContainsThisFile.Delete();
                                archive.CreateEntryFromFile(currentfilePath, currentfileName, CompressionLevel.Fastest);
                            }
                        }
                        else
                        {
                            archive.CreateEntryFromFile(currentfilePath, currentfileName, CompressionLevel.Fastest);
                        }
                    }
                }
            }

            return GetFile(zipFilePath);
        }

        public static bool SendMail(string mail, string subject, string body)
        {
            bool result = false;

            try
            {
                var fromAddress = new MailAddress("b2hrapp@gmail.com");
                var toAddress = new MailAddress(mail);
                string fromPassword = "b2hrcmsapp";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 5000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                result = true;
            }
            catch { }

            return result;
        }
    }
}