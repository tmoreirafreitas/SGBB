using System;
using System.IO;
using System.Runtime.InteropServices;

namespace HtmlToPdf.NetCore
{
    public static class RotativaConfiguration
    {
        public static string RotativaPath { get; set; }

        public static bool IsWindows { get; set; }

        public static void Setup(string wkhtmltopdfRelativePath = "Rotativa")
        {
            RotativaConfiguration.IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (RotativaConfiguration.IsWindows)
            {
                RotativaConfiguration.RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);
                if (!Directory.Exists(RotativaConfiguration.RotativaPath))
                    throw new Exception("Folder containing wkhtmltopdf.exe not found, searched for " + RotativaConfiguration.RotativaPath);
            }
            else
            {
                RotativaConfiguration.RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);
                if (!Directory.Exists(RotativaConfiguration.RotativaPath))
                    throw new Exception("Folder containing wkhtmltopdf not found, searched for " + RotativaConfiguration.RotativaPath);
            }
        }
    }
}
