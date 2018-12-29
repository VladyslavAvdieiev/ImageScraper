using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImageScrapers
{
    public class PrntscScraper : IScraper {
        private const string linkPattern = "https://prnt.sc/";
        private List<char> _alphabet = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b',
                                                        'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                                        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public IList<char> Alphabet { get => _alphabet.AsReadOnly(); }

        public void StartDownloading() {
            string image;
            string html;
            string link;

            using (WebClient webClient = new WebClient()) {
                image = GenerateImageName();
                webClient.Headers.Add("User-Agent: Other");
                html = webClient.DownloadString($"{linkPattern}{image}");
                link = ParseHtml(html);
            }

            throw new NotImplementedException();
        }

        private string GenerateImageName() {
            Random random = new Random();
            string image = string.Empty;
            for (int i = 0; i < 6; i++)
                image += Alphabet[random.Next(Alphabet.Count)];
            return image;
        }

        private string ParseHtml(string html) {
            string meta = Regex.Match(html, "og:image.+?/>").Value;
            string content = Regex.Match(meta, "content=.+?/>").Value;
            return Regex.Match(content, "\".+?\"").Value.Trim(new char[] { '"' });
        }
    }
}
