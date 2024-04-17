using System.Globalization;
using System.Linq;
using System.Text;

namespace FingerPrintManagerApp.ViewModel.Extension
{
    public static class StringExtensions
    {
        public static string NoAccent(this string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD).Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)).Normalize(NormalizationForm.FormC);
        }
    }
}
