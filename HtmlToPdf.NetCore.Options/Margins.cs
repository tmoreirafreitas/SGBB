using System.Globalization;
using System.Linq;
using System.Text;

namespace HtmlToPdf.NetCore.Options
{
    public class Margins
    {
        [OptionFlag("-B")]
        public int? Bottom;
        [OptionFlag("-L")]
        public int? Left;
        [OptionFlag("-R")]
        public int? Right;
        [OptionFlag("-T")]
        public int? Top;

        public Margins()
        { }

        public Margins(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var field in GetType().GetFields())
            {
                if (!(field.GetCustomAttributes(typeof(OptionFlag), true).FirstOrDefault() is OptionFlag
                    optionFlag)) continue;
                var obj = field.GetValue(this);
                if (obj != null)
                    stringBuilder.AppendFormat(CultureInfo.InvariantCulture, " {0} {1}", optionFlag.Name, obj);
            }
            return stringBuilder.ToString().Trim();
        }
    }
}
