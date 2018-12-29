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
        private const string imageIsNotExist = "//st.prntscr.com/2018/10/13/2048/img/0_173a7b_211be8ff.png";
        private List<char> _alphabet = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b',
                                                        'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                                        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public IList<char> Alphabet { get => _alphabet.AsReadOnly(); }
        public event EventHandler<ScraperEventArgs> OnStarted;
        public event EventHandler<ScraperEventArgs> OnImageDownloaded;
        public event EventHandler<ScraperEventArgs> OnErrorOccurred;

        public void StartDownloading() {
            OnStarted?.Invoke(this, new ScraperEventArgs($"Task #{Task.CurrentId} has started...", string.Empty));
            string image;
            string html;
            string link;

            using (WebClient webClient = new WebClient()) {
                while (true) {
                    try {
                        image = GenerateImageName();
                        webClient.Headers.Add("User-Agent: Other");
                        html = webClient.DownloadString($"{linkPattern}{image}");
                        link = ParseHtml(html);

                        if (link != imageIsNotExist) {
                            webClient.DownloadFile(link, $"{image}.png");
                            OnImageDownloaded?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ + ] {image}", image));
                        }
                        else
                            OnErrorOccurred?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ - ] {image}", image));
                    }
                    catch (Exception e) {
                        OnErrorOccurred?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ERR] {e.Message}", string.Empty));
                    }
                }
            }
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
