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
    class StateReader
    {
        string path;
        // reference to landscape
        Landscape lscape;

        public State[] processFile() {
            // declare to return
            State[] stateArray;
            // get all files in path that end in .txt
            string[] files =
                Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly);


            // init return
            stateArray = new State[files.Length];
            // loop through files
            for(int i = 0; i < files.Length; i++) {
                // get state and set in array
                stateArray[i] = readFile(files[i]);
            }

            return stateArray;
        }

        public State readFile(string fileName) {
            // init state values
            int id = 0;
            int manPower = 0;
            string Tag = "TAG";
            // init to return
            Tile[] stateTiles = new Tile[0];
            // "provinces"  was found and tileIDs are next line, flag
            bool provinceFlag = false;
            // stream reader
            using(StreamReader reader = new StreamReader(fileName) ) {
                while(!reader.EndOfStream) {
                    // all formats lose ='s and spaces
                    // store line to later remove "provinces"
                    string currentLine = reader.ReadLine().
                        Replace("=", " ").
                        Replace("}", string.Empty).
                        Replace("{", string.Empty);
                    string[] _split = currentLine.
                        Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // split

                    // get id
                    if(provinceFlag) {
                        // this is the line after finding "provinces"
                        stateTiles = getTiles(_split, ref provinceFlag);
                        

                    } else if(_split.Length > 0) {
                        // get split[0] and test
                        string firstStr = _split[0];
                        if (firstStr == "id") {
                            id = Int32.Parse(_split[1]);
                        } else if(firstStr == "manpower") {
                            manPower = Int32.Parse(_split[1]);
                        } else if(firstStr == "owner") {
                            Tag = _split[1];
                        } else if(firstStr == "provinces") {
                            provinceFlag = true;
                            _split = currentLine.Replace("provinces", string.Empty).
                                Split(new char[0], StringSplitOptions.RemoveEmptyEntries); // split
                            stateTiles = getTiles(_split, ref provinceFlag);
                        }
                    }
                }
            }
            // get name from fileName
            // from: #-Name.txt to: Name
            string name = fileName.
                Replace(".txt", string.Empty).
                Split('-')[1];

            Color stateColor = Color.Yellow;
            if(stateTiles.Length > 0) {
                stateColor = stateTiles[0].color;
            }
            // out of streamreader and with state variables stored
            return new State(id, manPower, stateTiles, name, lscape.searchCountry(Tag), stateColor);
            
        }

        Tile[] getTiles(string[] split, ref bool provinceFlag) {
            if (split.Length == 0) {
                return new Tile[0];
            }
            // init tile array
            Tile[] stateTiles = new Tile[split.Length];
            // first check at least one number
            int tileID;
            if (!Int32.TryParse(split[0], out tileID)) {
                // throw new Exception("failed loading state tiles in state folder");
                // not a number so we got bamboozled. maybe next line
                return null;
            }
            stateTiles[0] = lscape.searchTile(tileID);
            provinceFlag = false;
            // now go on
            // loop through rest of lines
            for (int i = 1; i < split.Length; i++) {
                // get id
                if (!Int32.TryParse(split[i], out tileID)) {
                    // should not happen
                    throw new Exception("failed loading state tiles in state folder");
                }
                if (stateTiles.Length > 1) {
                    stateTiles[i] = lscape.searchTile(tileID);
                }
                // reset flag
            }
            return stateTiles;
        }

        public StateReader(Landscape _landscape) {
            lscape = _landscape;
            path = "Content\\states";
        }
    }
}
