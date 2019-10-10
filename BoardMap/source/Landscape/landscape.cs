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
        public Dictionary<string, Country> countries { get; private set; }

        // states 
        State[] states;

        // tiles
        Tile[] definitions;
        public Dictionary<Color, Tile> tiles { get; private set; }

        // map modes
        // change map mode with keyboard
        #region

        public Color waterColor = Color.White;// new Color(163, 192, 245); //Color.CornflowerBlue; // = new Color(128, 255, 255);
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
                    // if not ocean
                    if(definitions[i].isLand) {
                        definitions[i].drawTile(canvas);
                    }
                }
            } else if (listKeys.Contains(Keys.W)) {

                // W: draw states 
                for(int i = 0; i < states.Length; i++) {
                    State _state = states[i];
                    drawStateDiffcolor(_state, _state.color, 0.3f, 0.3f, -1f, canvas);
                } 
            } else if (listKeys.Contains(Keys.E)) {

                // E: draw countries
                for (int i = 1; i < definitions.Length; i++) {
                    if (definitions[i].isLand) {
                        definitions[i].drawTile(definitions[i].state.country.color, canvas);
                    } else {
                        // definitions[i].drawTile(waterColor, canvas);
                    }
                }
            } else if (listKeys.Contains(Keys.R)) {

                // R: draw continents
                for (int i = 1; i < definitions.Length; i++) {
                    // if not ocean
                    if(definitions[i].isLand) {
                        // draw tile with continent color
                        definitions[i].drawTile(continents[definitions[i].continent].color, canvas);
                    }
                }

            } else if (listKeys.Contains(Keys.T)) {

                // T: draw draw land
                for (int i = 1; i < definitions.Length; i++) {
                    if (definitions[i].isLand) {
                        definitions[i].drawTile(Color.Brown, canvas);
                    } else {
                        // definitions[i].drawTile(waterColor, canvas);
                    }
                }

            } else {
                // if no match dont print blank canvas
                return;
            }
            // update frame colordata
            Tile.frameReference.setDataColor(canvas);
            // update texture colodata
            Tile.frameReference.updateTexture(canvas);
        }
        #endregion

        // get tile from dictionary
        public Tile searchTile(Color _color) {
            // if found in dictionary return
            Tile tile;
            if (tiles.TryGetValue(_color, out tile)) {
                return tile;
            } else {
                // else return null tile
                return definitions[0];
            }
        }

        // rgb parameters as fraction closer to 255 relative to actual state color
        // like this: difR = 1/5 then new color is 4/5 the way to 255
        // can be negative to get far from 255
        public void drawStateDiffcolor(State lighttState, Color _color, float difR, float difG, float difB, ColorData<Color> currentTexture) {
            // if null is ocean probably
            if (lighttState != null) {
                // get color and make lighter
                // set to fraction of distance to 255 from lightState.color
                Color lightColor = new Color(

                    (int)(255 - (255 - _color.R) * (1 - difR) ),
                    (int)(255 - (255 - _color.G) * (1 - difG) ),
                    (int)(255 - (255 - _color.B) * (1 - difB) ));

                // draw each tile in state with light color
                for (int i = 0; i < lighttState.tiles.Length; i++) {
                    lighttState.tiles[i].drawTile(lightColor, currentTexture);
                }
            }
        }


        // draw 
        public void drawCountry(Tile _tile) {
            // get state
            State _state = _tile.state;

            // if null, is ocean probably
            if (_state != null) {
                // get country color
                Color color = _tile.state.country.color;
                // get data from frame once
                ColorData<Color> currentTexture = Tile.frameReference.colorData.getCopy();

                // loop through states in country
                for(int state_count = 0; state_count < _tile.state.country.states.Count; state_count++) {
                    // draw each tile in state with 
                    State currentState = _state.country.states[state_count];

                    // dont draw if is selected state. it will be drawn later
                    if (currentState == _tile.state) {
                        continue;
                    }
                    // loop through tiles
                    for (int i = 0; i < currentState.tiles.Length; i++) {
                        // if land
                        if(currentState.tiles[i].isLand) {
                            currentState.tiles[i].drawTile(color, currentTexture);
                        }
                    }

                }

                float relativeDiff = 1f/5f;
                // draw state lighter
                drawStateDiffcolor(_tile.state, _tile.state.country.color, relativeDiff, relativeDiff, relativeDiff, currentTexture);
                // drawLightState(_tile, _tile.state.color, currentTexture);
                // redraw selected tile with actual state color
                _tile.drawTile(_tile.state.color, currentTexture);

                // update texture
                Tile.frameReference.updateTexture(currentTexture);
            }
        }


        public Tile searchTile(int _id) {
            return definitions[_id];
        }
        public Country searchCountry(string _tag) {
            return countries[_tag];
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
                new Continent(0, "Oceans", waterColor),
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
            genLandscape();
            // init loader with map texture
            TileLoader tloader = new TileLoader(_texture);
            // init reader for definitions.csv
            DefReader dreader = new DefReader();
            // init reader for countries.txt
            LandReader lreader = new LandReader();
            // init reader for states folder
            StateReader sreader = new StateReader(this); // needs initialized tiles 

            // run loader and set tiles
            tiles = tloader.processMap(50);
            // read file and set definitions
            definitions = dreader.processFile(tiles);
            // read file and set countries
            countries = lreader.processFile();
            // read files and set states
            states = sreader.processFile();
            State.initStates(states);


            // init continents
            initContinents();
        }
        // create landscale without loading tiles
        // init stuff to 0
        void genLandscape() {
            tiles = new Dictionary<Color, Tile>();
            definitions = new Tile[0];
            states = new State[0];
            countries = new Dictionary<string, Country>();
            continents = new Continent[0];
        }
    }
}
