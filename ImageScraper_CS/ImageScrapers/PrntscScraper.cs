using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Represents the path to the folder in which the images will be saved.
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Represents the alphabet used in generating image names.
        /// </summary>
        public IList<char> Alphabet { get => _alphabet.AsReadOnly(); }
        /// <summary>
        /// Occurs when a StartDownloading method starts.
        /// </summary>
        public event EventHandler<ScraperEventArgs> OnStarted;
        /// <summary>
        /// Occurs when an image saves successfully.
        /// </summary>
        public event EventHandler<ScraperEventArgs> OnImageDownloaded;
        /// <summary>
        /// Occurs when an image cannot be saved.
        /// </summary>
        public event EventHandler<ScraperEventArgs> OnErrorOccurred;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrntscScraper"/> class.
        /// </summary>
        public PrntscScraper() {
            Path = $"{AppDomain.CurrentDomain.BaseDirectory}DownloadedImages";
            CheckDirectory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrntscScraper"/> class.
        /// </summary>
        /// <param name="path">Represents the path to the folder in which the images will be saved.</param>
        public PrntscScraper(string path) {
            Path = path;
            CheckDirectory();
        }

        /// <summary>
        /// Downloads random images from the prnt.sc site.
        /// </summary>
        public void StartDownloading() {
            OnStarted?.Invoke(this, new ScraperEventArgs($"Task #{Task.CurrentId} has started.", string.Empty));
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
                            webClient.DownloadFile(link, $"{Path}\\{image}.png");
                            OnImageDownloaded?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ + ] Valid:     {linkPattern}{image}", image));
                        }
                        else
                            OnErrorOccurred?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ - ] Invalid:   {linkPattern}{image}", image));
                    }
                    catch (Exception e) {
                        OnErrorOccurred?.Invoke(this, new ScraperEventArgs($"#{Task.CurrentId} [ERR] {e.Message}", string.Empty));
                    }
                }
            }
        }

        /// <summary>
        /// Generates random names for images.
        /// </summary>
        private string GenerateImageName() {
            Random random = new Random();
            string image = string.Empty;
            for (int i = 0; i < 6; i++)
                image += Alphabet[random.Next(Alphabet.Count)];
            return image;
        }

        /// <summary>
        /// Parses html string to find link to image.
        /// </summary>
        private string ParseHtml(string html) {
            string meta = Regex.Match(html, "og:image.+?/>").Value;
            string content = Regex.Match(meta, "content=.+?/>").Value;
            return Regex.Match(content, "\".+?\"").Value.Trim(new char[] { '"' });
        }

        /// <summary>
        /// Checks the directory for a folder for downloaded images.
        /// </summary>
        private void CheckDirectory() {
            DirectoryInfo directory = new DirectoryInfo(Path);
            if (!directory.Exists)
                directory.Create();
        }
    }
}
