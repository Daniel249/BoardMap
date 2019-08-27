using BoardMap.LandscapeNS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Externals
{
    // file reader for definitions.csv
    class DefReader //: Reader
    {
        // path 
        string path;

        // main method
        // return tiles ordered in array
        public Tile[] processFile(Dictionary<Color, Tile> tiles) {
            // init readline buffer
            List<string[]> split = new List<string[]>();

            // ReadLine format: ID;R;G;B;land?;coastal?;terrain;continent

            // stream reader
            using (StreamReader reader = new StreamReader(path)) {
                // loop through lines
                while (!reader.EndOfStream) {
                    // get partition by semicolon
                    string[] _split = reader.ReadLine().Split(';');
                    // save to list
                    split.Add(_split);
                }
            }

            // init to return
            Tile[] definitions;
            // check size
            if (split.Count == tiles.Count) {
                definitions= new Tile[split.Count];
            } else {
                throw new NotSupportedException("definitions and mapdata don't match");
            }

            // produce tiles from: id, rgb, land?, coastal?, continent
            foreach (Tile tile in tiles.Values) {
                // loop through split for match for testTile
                for (int count_tile = 0; count_tile < split.Count; count_tile++) {
                    string[] currentLine = split[count_tile];
                    // if rgb matches
                    if (tile.color.R.ToString() == currentLine[1]
                    &&  tile.color.G.ToString() == currentLine[2]
                    &&  tile.color.B.ToString() == currentLine[3]) {
                        // init values for tile
                        bool _isCoastal;
                        bool _isLand;
                        int _id;
                        int _continent;

                        // set values for tile
                        #region
                        // set id
                        if (!Int32.TryParse(currentLine[0], out _id)) {
                            throw new NotSupportedException($"id is not a number at {count_tile}");
                        }

                        // set continent
                        if (!Int32.TryParse(currentLine[7], out _continent)) {
                            throw new NotSupportedException($"continent is not a number at {count_tile}");
                        }

                        // set is land or sea
                        if (currentLine[4] == "land") {
                            _isLand = true;
                        } else if (currentLine[4] == "sea") {
                            _isLand = false;
                        } else if (currentLine[4] == "lake") {
                            _isLand = false;
                        } else {
                            throw new NotSupportedException($"not land or sea at {count_tile}");
                        }

                        // set is coastal
                        if (currentLine[5] == "true") {
                            _isCoastal = true;
                        }
                        else if (currentLine[5] == "false") {
                            _isCoastal = false;
                        } else {
                            throw new NotSupportedException($"not true or false at {count_tile}");
                        }
                        #endregion

                        // break from for loop and into next tile in foreach
                        count_tile = split.Count;

                        // set rest of properties in tile
                        tile.setRest(_id, _isLand, _isCoastal, _continent);
                        // add tile to Tile[] definitions
                        definitions[_id] = tile;
                    } else if (count_tile == split.Count - 1) {
                        // error no match found for a tile
                        throw new NotSupportedException("no match found for tile in definitions");
                    }
                }
            }
            return definitions;
        }

        string[] readFile() {
            throw new NotImplementedException();
        }

        public DefReader() {
            // string relativePatch = "\\Content\\definitions.csv";
            path = "Content\\definitions.csv";
            //path = Path.Combine(Environment.CurrentDirectory, relativePatch);
        }
    }
}
