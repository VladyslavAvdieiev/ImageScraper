﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageScrapers
{
    public class PrntscScraper : IScraper {
        private List<char> _alphabet = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b',
                                                        'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                                                        'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public IList<char> Alphabet { get => _alphabet.AsReadOnly(); }

        public void StartDownloading() {
            throw new NotImplementedException();
        }
    }
}