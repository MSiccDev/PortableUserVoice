using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortableUserVoice.Utilities
{
    public class HtmlHelper
    {
        public static string DecodeHtmlToStringElements(string text)
        {

            string result = text;

            result = DecodeListingsToStringElements(result);

            result = DecodeNumerationsToStringElements(result);

            try
            {
                result =
                    Regex.Replace
                    (result.
                    Replace("<div>", "").
                    Replace("</div>", "").
                    Replace("<br>", "\r\n").
                    Replace("<p>", "").
                    Replace("</p>", "\r\n\r\n").
                    Replace("&ndash;", "-").
                    Replace("&nbsp;", " ").
                    Replace("&rsquo;", "'").
                    Replace("&amp;", "&").
                    Replace("&#038;", "&").
                    Replace("&quot;", "\"").
                    Replace("&#039;", "'").
                    Replace("&#39;", "'").
                    Replace("&#8230;", "...").
                    Replace("&#8212;", "—").
                    Replace("&#8211;", "-").
                    Replace("&#8220;", "“").
                    Replace("&#8221;", "”").
                    Replace("&#8217;", "'").
                    Replace("&#160;", " ").
                    Replace("&gt;", ">").
                    Replace("&rdquo;", "\"").
                    Replace("&ldquo;", "\"").
                    Replace("&lt;", "<").
                    Replace("&#215;", "×").
                    Replace("&#8242;", "′").
                    Replace("&#8243;", "″").
                    Replace("&#8216;", "'").
                    Replace("u201c", "\"").
                    Replace("u201d", "\"").
                    Replace("u2019", "′").
                    Replace("\r\n\r\n\r\n", "\r\n\r\n"),
                    "<[^<>]+>", "");

                return result;
            }
            catch
            {
                return "";
            }
        }

        public static string DecodeListingsToStringElements(string text)
        {
            var result = text;

            var listings = Regex.Matches(result, "<ul>(.*?)</ul>");
            var sortedListings = listings.Cast<Match>().OrderByDescending(i => i.Index);

            foreach (var list in sortedListings)
            {
                result = result.Replace("<ul>", "\r\n").Replace("</ul>", "\r\n");

                var listElements = Regex.Matches(list.ToString(), "<li>(.*?)</li>");

                foreach (var le in listElements)
                {
                    var listElement = string.Format("{0}" + le.ToString().Substring(4, le.ToString().Length - 9) + "{1}", "- ", "\r\n");
                    result = Regex.Replace(result, le.ToString(), listElement);
                }
            }
            return result;
        }



        public static string DecodeNumerationsToStringElements(string text)
        {
            var result = text;

            var numerations = Regex.Matches(result, "<ol>(.*?)</ol>");
            var sortedNumerations = numerations.Cast<Match>().OrderByDescending(i => i.Index);

            foreach (var numeration in sortedNumerations)
            {
                result = result.Replace("<ol>", "\r\n").Replace("</ol>", "\r\n");

                var listElements = Regex.Matches(numeration.ToString(), "<li>(.*?)</li>");

                for (int i = 0; i < listElements.Count; i++)
                {
                    var listElement = string.Format("{0}. " + listElements[i].ToString().Substring(4, listElements[i].ToString().Length - 9) + "{1}", i + 1, "\r\n");
                    result = Regex.Replace(result, listElements[i].ToString(), listElement);
                }
            }
            return result;
        }


        public static string EncodeStringElementsToHtml(string text)
        {
            var result = "<p>" + text + "\r\n\r\n";

            result = EncodeListingsToHtml(result);

            result = EncodeNumerationsToHmtl(result);

            var paragraphs = Regex.Matches(result, "\r\n\r\n");
            var sortedParagraphs = paragraphs.Cast<Match>().OrderByDescending(i => i.Index);

            result = sortedParagraphs.Aggregate(result, (current, match) => Regex.Replace(current, "\r\n\r\n", @"</p><p>"));

            var linebreaks = Regex.Matches(result, "\r\n");
            var sortedLinebreaks = linebreaks.Cast<Match>().OrderByDescending(i => i.Index);

            result = sortedLinebreaks.Aggregate(result, (current, match) => Regex.Replace(current, "\r\n", @"<br>"));

            return result;
        }

        private static string EncodeListingsToHtml(string text)
        {
            string result = text;

            var listings = Regex.Matches(text, "(-(.*?)\r\n)+\r\n");
            foreach (var listingItem in listings)
            {
                result = result.Insert(result.IndexOf(listingItem.ToString(), StringComparison.Ordinal), "<ul>");
                result = result.Insert((result.IndexOf(listingItem.ToString(), StringComparison.Ordinal) + listingItem.ToString().Length), "</ul>");

                var listingsItems = Regex.Matches(listingItem.ToString(), "-(.*?)\r\n");
                if (listingsItems.Count > 0)
                {
                    var hintHyphens = Regex.Matches(text, "-(.*?)-(.*?)");
                    var sortedHintHyphens = (from object item in hintHyphens select hintHyphens[0].Value.Trim()).ToList();

                    var sortedListingStrings = (from object item in listingsItems select item.ToString().Trim()).Except(sortedHintHyphens).ToList();

                    foreach (var item in sortedListingStrings)
                    {
                        var listelement = string.Format(CultureInfo.InvariantCulture, "{0}" + item.Substring(1) + "{1}", "<li>", "</li>");

                        result = Regex.Replace(result, item, listelement);
                    }
                }
            }

            var leftLineBreaksAfterListConversion = Regex.Matches(result, "\r\n<li>");
            var sortedLeftLineBreaksAfterListConversion = leftLineBreaksAfterListConversion.Cast<Match>().OrderByDescending(lb => lb.Index);

            return sortedLeftLineBreaksAfterListConversion.Aggregate(result, (current, match) => Regex.Replace(current, "\r\n<li>", "<li>"));
        }

        private static string EncodeNumerationsToHmtl(string text)
        {
            string result = text;

            var numerations = Regex.Matches(result, "([0-9]+.(.*?)\r\n)+\r\n");
            foreach (var numerationItem in numerations)
            {
                if (!numerationItem.ToString().Contains("<li>"))
                {
                    result = result.Insert(result.IndexOf(numerationItem.ToString(), StringComparison.Ordinal), "<ol>");
                    result =
                        result.Insert(
                            result.IndexOf(numerationItem.ToString(), StringComparison.Ordinal) +
                            numerationItem.ToString().Length, "</ol>");

                    var numerationItems = Regex.Matches(numerationItem.ToString(), "[0-9]+.(.*?)\r\n");
                    if (numerationItems.Count > 0)
                    {
                        var sortedNumerations = numerationItems.Cast<Match>();
                        foreach (var item in sortedNumerations)
                        {
                            var numerationElement = string.Format(CultureInfo.InvariantCulture,
                                "{0}" +
                                item.ToString().Substring(item.ToString().IndexOf(".", StringComparison.Ordinal) + 1) +
                                "{1}", "<li>", "</li>");
                            result = Regex.Replace(result, item.ToString(), numerationElement);

                        }
                    }
                }
            }

            var leftLineBreaksAfterNumerationConversion = Regex.Matches(result, "\r\n</li>");
            var sortedLeftLineBreaksAfterNumerationConversion = leftLineBreaksAfterNumerationConversion.Cast<Match>().OrderByDescending(lb => lb.Index);

            return sortedLeftLineBreaksAfterNumerationConversion.Aggregate(result, (current, match) => Regex.Replace(current, "\r\n</li>", "</li>"));
        }
    }
}
