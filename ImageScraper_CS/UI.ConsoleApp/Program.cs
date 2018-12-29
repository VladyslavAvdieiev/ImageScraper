using System;
using ImageScrapers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleApp
{
    class Program {
       
        static void Main(string[] args) {
            PrntscScraper scraper = new PrntscScraper();
            scraper.OnStarted += (sender, e) => Console.WriteLine(e.Message);
            scraper.OnImageDownloaded += (sender, e) => Console.WriteLine(e.Message);
            scraper.OnErrorOccurred += (sender, e) => Console.WriteLine(e.Message);

            Task.Run(() => scraper.StartDownloading());
            Task.Run(() => scraper.StartDownloading());
            Task.Run(() => scraper.StartDownloading());

            Console.ReadLine();
        }
    }
}
