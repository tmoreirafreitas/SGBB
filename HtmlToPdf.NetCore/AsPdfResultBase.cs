using HtmlToPdf.NetCore.Options;
using HtmlToPdf.NetCore.Options.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Size = System.Drawing.Size;

namespace HtmlToPdf.NetCore
{
    public abstract class AsPdfResultBase
    {
        protected AsPdfResultBase()
        {
            this.PageMargins = new Margins();
        }

        [OptionFlag("-s")]
        public Size? PageSize { get; set; }

        [OptionFlag("--page-width")]
        public double? PageWidth { get; set; }

        [OptionFlag("--page-height")]
        public double? PageHeight { get; set; }

        [OptionFlag("-O")]
        public Orientation? PageOrientation { get; set; }

        public Margins PageMargins { get; set; }

        protected string GetContentType()
        {
            return "application/pdf";
        }

        [OptionFlag("-l")]
        public bool IsLowQuality { get; set; }

        [OptionFlag("--copies")]
        public int? Copies { get; set; }

        [OptionFlag("-g")]
        public bool IsGrayScale { get; set; }

        protected string GetConvertOptions()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (this.PageMargins != null)
                stringBuilder.Append(this.PageMargins.ToString());
            stringBuilder.Append(" ");
            stringBuilder.Append(this.GetConvertBaseOptions());
            return stringBuilder.ToString().Trim();
        }

        protected string GetConvertBaseOptions()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (((IEnumerable<object>)property.GetCustomAttributes(typeof(OptionFlag), true)).FirstOrDefault<object>() is OptionFlag optionFlag)
                {
                    object obj = property.GetValue((object)this, (object[])null);
                    if (obj != null)
                    {
                        if (property.PropertyType == typeof(Dictionary<string, string>))
                        {
                            foreach (KeyValuePair<string, string> keyValuePair in (Dictionary<string, string>)obj)
                                stringBuilder.AppendFormat(" {0} {1} {2}", (object)optionFlag.Name, (object)keyValuePair.Key, (object)keyValuePair.Value);
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            if ((bool)obj)
                                stringBuilder.AppendFormat((IFormatProvider)CultureInfo.InvariantCulture, " {0}", (object)optionFlag.Name);
                        }
                        else
                            stringBuilder.AppendFormat((IFormatProvider)CultureInfo.InvariantCulture, " {0} {1}", (object)optionFlag.Name, obj);
                    }
                }
            }
            return stringBuilder.ToString().Trim();
        }
    }
}
