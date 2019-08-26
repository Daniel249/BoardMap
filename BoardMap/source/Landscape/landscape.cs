using BoardMap.Externals;
using BoardMap.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.LandscapeNS
{
    class Landscape
    {
        // continents
        Continent[] continents;

        // countries
        Country[] countries;

        // states 
        State[] states;

        // tiles
        Tile[] definitions;
        public Dictionary<Color, Tile> tiles { get; private set; }

        // map modes
        // change map mode with keyboard

        // tile counter for R
        int counter = 1;
        public void changeMapMode(KeyboardState lastState) {
            // get currentState
            KeyboardState currentState = Keyboard.GetState();

            // calc newly pressed keys
            Keys[] newlyPressed = currentState.GetPressedKeys()
                .Except(lastState.GetPressedKeys()).ToArray();
            // init list to use contains
            List<Keys> listKeys = new List<Keys>(newlyPressed);

            // if no newly pressed return immediately
            if(!listKeys.Any<Keys>() || listKeys.Contains(Keys.Escape)) {
                return;
            }

            ColorData<Color> canvas = Tile.frameReference.getBlankCanvas();

            // check keyboard
            if (listKeys.Contains(Keys.Q)) {

                // Q: every tile draws its color 
                for (int i = 1; i < definitions.Length; i++) {
                    definitions[i].drawTile(canvas);
                }
            } else if (listKeys.Contains(Keys.W)) {

                // W: draw every continent 
                for (int i = 1; i < definitions.Length; i++) {
                    // draw tile with continent color
                    definitions[i].drawTile(continents[definitions[i].continent].color, canvas);
                }
            } else if (listKeys.Contains(Keys.E)) {

                // E: draw land
                for (int i = 1; i < definitions.Length; i++) {
                    if (definitions[i].isLand) {
                        definitions[i].drawTile(Color.Brown, canvas);
                    } else {
                        definitions[i].drawTile(Color.Cyan, canvas);
                    }
                }
            } else if (listKeys.Contains(Keys.R)) {

                // R: draw only one tile
                // draw a tile on canvas
                int howMany = 10;
                for(int i = 0; i < howMany; i++) {
                    definitions[i + counter].drawTile(canvas);
                }
                counter += howMany;

            }
            Tile.frameReference.updateTexture(canvas);
            // update texture colodata
            // Tile.frameReference.updateTexture();
        }


        // get tile from dictionary
        public Tile searchTile(Color _color) {
            // if found in dictionary return
            Tile tile;
            if (tiles.TryGetValue(_color, out tile)) {
                return tile;
            } else {
                return definitions[0];
            }
            // else return
        }

        public Tile searchTile(int _id) {
            return definitions[_id];
        }


        // check tile with most textures
        public Tile getmaxTextures() {
            // get noname tile
            Tile currentTile = definitions[0];// = new Tile();
            // to return actual max
            foreach (Tile _tile in tiles.Values) {
                if (currentTile.textures.Count < _tile.textures.Count) {
                    currentTile = _tile;
                }
            }
            return currentTile;
        }

        // init  and assign continents
        void initContinents() {
            continents = new Continent[8] {
                new Continent(0, "Oceans", Color.Cyan),
                new Continent(1, "Europe", new Color(0, 51, 153)),
                new Continent(2, "North America", Color.Green),
                new Continent(3, "South America", Color.Gold),
                new Continent(4, "Australia", Color.HotPink),
                new Continent(5, "Africa", Color.Coral),
                new Continent(6, "Asia", Color.IndianRed),
                new Continent(7, "Middle East", Color.MediumPurple),
            };
        }


        // constructor
        public Landscape(Texture2D _texture) {
            // init loader with map texture
            TileLoader tloader = new TileLoader(_texture);
            // init DefinitionsReader
            DefReader dreader = new DefReader();

            // run loader and set tiles
            tiles = tloader.processMap(50);
            // read file and set definitions
            definitions = dreader.processFile(tiles);

            // init continents
            initContinents();
        }
        // create landscale without loading tiles
        // init stuff to 0
        public Landscape() {
            tiles = new Dictionary<Color, Tile>();
            states = new State[0];
            countries = new Country[0];
            continents = new Continent[0];
        }
    }
}
