using BoardMap.LandscapeNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Externals
{
    // read files in Countries folder
    class LandReader
    {
        // path
        string tagPath;
        string folderPath;

        // main method
        // get name and tag from tagPath = Content\\countries.txt
        public Dictionary<string, Country> processFile() {
            // init return
            Dictionary<string, Country> countriesDict = new Dictionary<string, Country>();
            // init readline buffer
            List<string[]> split = new List<string[]>();

            // readLine format: TAG = "countries/CountryName.txt"

            // stream reader 
            using (StreamReader reader = new StreamReader(tagPath)) {
                // loop through lines
                while(!reader.EndOfStream) {
                    // get line as string[]
                    string[] _split = reader.ReadLine().
                        // into format: [TAG, CountryName]
                        Replace("=",    string.Empty).
                        Replace("\"",   string.Empty).
                        Replace(@"countries/", string.Empty).
                        Replace(".txt", string.Empty).
                        Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // split by white space
                    // save to list
                    split.Add(_split);
                }
            }


            // loop through lines
            for (int i = 0; i < split.Count; i++) {
                // get name and tag
                string[] line = split[i];
                string _tag = line[0];
                // build name
                string _name = line[1];
                for (int from = 2; from < line.Length; from++) {
                    _name += " " + line[from];
                }
                // add new country to list
                countriesDict.Add(_tag, new Country(_name, _tag));
            }

            // read countries folder
            readCountries(countriesDict.Values.ToArray());

            return countriesDict;

        }

        // foreach Country read countries/Country.txt
        public void readCountries(Country[] countries) {
            for(int i = 0; i < countries.Length; i++) {
                // get country
                Country currentCountry = countries[i];

                // generate path from country name
                string fileName = currentCountry.Name + ".txt";
                string path = Path.Combine(folderPath, fileName);

                // stream reader
                using (StreamReader reader = new StreamReader(path)) {
                    // loop through lines
                    while (!reader.EndOfStream) {
                        // format to: [color, R, G, B]
                        string[] _split = reader.ReadLine().
                            Replace("rgb", string.Empty).
                            Replace("{", string.Empty).
                            Replace("}", string.Empty).
                            Replace("#", string.Empty).
                            Replace("=", string.Empty).
                            Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // split by white space

                        // find line with color
                        if (_split.Length > 3 &&_split[0] == "color") {
                            currentCountry.setColor(Int32.Parse(_split[1]), Int32.Parse(_split[2]), Int32.Parse(_split[3]));
                            break;
                        }
                    }
                }
            }
        }

        // constructor
        public LandReader() {
            tagPath = "Content\\countries.txt";
            folderPath = "Content\\countries";
        }
    }
}
