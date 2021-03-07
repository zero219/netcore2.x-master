using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore2.Web.Models.TagHelpersModels
{
    [HtmlTargetElement(Attributes = "bold")]//Bold元素
    [HtmlTargetElement("bold")]//Bold标签(两者满足其一即可)
    public class BoldTagHelper : TagHelper
    {


        [HtmlAttributeName("my-style")]
        public MyStyle MyStyle { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("bold");
            output.PreContent.SetHtmlContent("<strong>");
            output.PostContent.SetHtmlContent("</strong>");

            output.Attributes.SetAttribute("style", $"color:{MyStyle.Color};font-size:{MyStyle.FontSize}px; font-family:{MyStyle.FontFamily}");
        }
    }

    public class MyStyle
    {
        public string Color { get; set; }
        public int FontSize { get; set; }
        public string FontFamily { get; set; }
    }
}
