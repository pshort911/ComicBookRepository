using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComicBookRepository
{
    public static class HelperExtensions
    {
        public static HtmlString YesNo(this IHtmlHelper htmlHelper, bool? yesNo)
        {
            var text = yesNo != null && (bool)yesNo ? "Yes" : "No";
            return new HtmlString(text);
        }
    }
}
