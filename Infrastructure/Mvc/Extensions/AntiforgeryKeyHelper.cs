using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Infrastructure.Mvc.Extensions
{
    public class AntiforgeryKeyHelper
    {

        public const string ATTRIBUTE_VALUE_NAME = "value";

        public const string ATTRIBUTE_TOKEN_NAME = "antiForgeryToken";
        public static MvcHtmlString AntiForgeryToken()
        {
            dynamic htmlTag = AntiForgery.GetHtml().ToString();
            XElement element = XElement.Parse(htmlTag);

            dynamic attValue = element.Attribute(ATTRIBUTE_VALUE_NAME);
            dynamic tokenValue = attValue.Value;
            //attValue.Remove()

            element.Add(new XAttribute("ng-model", ATTRIBUTE_TOKEN_NAME));
            element.Add(new XAttribute("ng-init", string.Format("{0}='{1}'", ATTRIBUTE_TOKEN_NAME, tokenValue)));

            htmlTag = element.ToString();

            return new MvcHtmlString(htmlTag);
        }
    }
}
