using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class SeoHelper
    {
        public static string ConverToSeo(this string url, string? lang)
        {

            switch (lang)
            {
                case "az":
                    url = SeoAz(url);
                    break;
                case "en":
                    url = SeoEn(url);
                    break;
                case "ru":
                    url = SeoRu(url);
                    break;
                case "tr":
                    url = SeoTr(url);
                    break;
                default:
                    break;
            }

            return url;
        }
        public static string SeoAz(string url)
        {
            url = url.ToLowerInvariant();
            url = url.Replace("ə", "e").Replace("ı", "i").Replace("ö", "o").Replace("ş", "s").Replace("ü", "u").Replace("ç", "c").Replace("ğ", "g");

            return Regex.Replace(url, @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoEn(string url)
        {
            return Regex.Replace(url, @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoTr(string url)
        {
            url = url.ToLowerInvariant();
            url = url.Replace("ç", "c")
                     .Replace("ğ", "g")
                     .Replace("ı", "i")
                     .Replace("ö", "o")
                     .Replace("ş", "s")
                     .Replace("ü", "u");

            return Regex.Replace(url, @"[^a-z0-9]", "-").Trim('-');
        }

        public static string SeoRu(string url)
        {
            url = url.ToLowerInvariant();
            url = url
            .Replace("а", "a")
            .Replace("б", "b").Replace("в", "v")
            .Replace("г", "g").Replace("д", "d")
            .Replace("е", "e").Replace("ё", "yo")
            .Replace("ж", "zh").Replace("з", "z")
            .Replace("и", "i").Replace("й", "y")
            .Replace("к", "k").Replace("л", "l")
            .Replace("м", "m").Replace("н", "n")
            .Replace("о", "o").Replace("п", "p")
            .Replace("р", "r").Replace("с", "s")
            .Replace("т", "t").Replace("у", "u")
            .Replace("ф", "f").Replace("х", "kh")
            .Replace("ц", "ts").Replace("ч", "ch")
            .Replace("ш", "sh").Replace("щ", "sch")
            .Replace("ъ", "").Replace("ы", "y")
            .Replace("ь", "").Replace("э", "e")
            .Replace("ю", "yu").Replace("я", "ya");

            return Regex.Replace(url, @"[^a-z0-9]", "-").Trim('-');
        }
    }
}
