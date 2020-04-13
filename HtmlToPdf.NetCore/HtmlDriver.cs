using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HtmlToPdf.NetCore
{
    public abstract class HtmlDriver
    {
        public static byte[] Convert(string wkhtmlPath, string switches, string html)
        {
            switches = "-q " + switches + " -";
            if (!string.IsNullOrEmpty(html))
            {
                switches += " -";
                html = HtmlDriver.SpecialCharsEncode(html);
            }
            wkhtmlPath = !RotativaConfiguration.IsWindows ? Path.Combine(wkhtmlPath, "Linux") : Path.Combine(wkhtmlPath, "Windows");
            string path = Path.Combine(wkhtmlPath, RotativaConfiguration.IsWindows ? "wkhtmltopdf.exe" : Path.Combine(wkhtmlPath, "wkhtmltopdf"));
            if (!File.Exists(path))
                throw new Exception("wkhtmltopdf not found, searched for " + path);
            Process process = new Process();
            try
            {
                process.StartInfo = new ProcessStartInfo()
                {
                    FileName = path,
                    Arguments = switches,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                };
                process.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (!string.IsNullOrEmpty(html))
            {
                using (StreamWriter standardInput = process.StandardInput)
                    standardInput.WriteLine(html);
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Stream baseStream = process.StandardOutput.BaseStream)
                {
                    byte[] buffer = new byte[4096];
                    int count;
                    while ((count = baseStream.Read(buffer, 0, buffer.Length)) > 0)
                        memoryStream.Write(buffer, 0, count);
                }
                string end = process.StandardError.ReadToEnd();
                if (memoryStream.Length == 0L)
                    throw new Exception(end);
                process.WaitForExit();
                return memoryStream.ToArray();
            }
        }

        private static string SpecialCharsEncode(string text)
        {
            char[] charArray = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder(text.Length + (int)((double)text.Length * 0.1));
            foreach (char ch in charArray)
            {
                int int32 = System.Convert.ToInt32(ch);
                if (int32 > (int)sbyte.MaxValue)
                    stringBuilder.AppendFormat("&#{0};", (object)int32);
                else
                    stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }
    }
}
