﻿using BoardMap.Externals;
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

        // tile counter for R
        int counter = 1;

        public Color waterColor = new Color(163, 192, 245); //Color.CornflowerBlue; // = new Color(128, 255, 255);
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
                for (int i = 1; i < definitions.Length; i++) {
                    if (definitions[i].isLand) {
                        // make color less blue
                        Color stateColor = definitions[i].state.color;
                        // calc blue fraction as 1/4
                        byte blueFraction = (byte)(stateColor.B / 4);
                        // add to R and G and take from B
                        stateColor.R += blueFraction;
                        stateColor.G += blueFraction;
                        stateColor.B -= blueFraction;
                        if(stateColor.B < 0) {
                            stateColor.B = 0;
                        }
                        // draw
                        definitions[i].drawTile(stateColor, canvas);
                    } else {
                        // definitions[i].drawTile(waterColor, canvas);
                    }
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

            } else if (listKeys.Contains(Keys.P)) {

                // P: draw only one tile
                // draw a tile on canvas
                int howMany = 10;
                for(int i = 0; i < howMany; i++) {
                    definitions[i + counter].drawTile(canvas);
                }
                counter += howMany;
            } else {
                // if no match dont print blank canvas
                return;
            }
            // update frame colordata
            Tile.frameReference.setDataColor(canvas);
            // update texture colodata
            Tile.frameReference.updateTexture(canvas);
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

        public void drawLightState(Tile _tile, Color _color) {
            // get state
            State lighttState = _tile.state;

            // if null is ocean probably
            if(lighttState != null) {
                // get color and make lighter
                // set to fraction of distance to 255 from lightState.color
                Color lightColor = new Color(
                    255 - (255 - _color.R) * 4 / 5,
                    255 - (255 - _color.G) * 4 / 5,
                    255 - (255 - _color.B) * 4 / 5);

                // get data from frame once
                ColorData<Color> currentTexture = Tile.frameReference.colorData.getCopy();
                // draw each tile in state with light color
                for (int i = 0; i < lighttState.tiles.Length; i++) {
                    lighttState.tiles[i].drawTile(lightColor, currentTexture);
                }
                // redraw selected tile with state color
                _tile.drawTile(_color, currentTexture);
                // update texture
                Tile.frameReference.updateTexture(currentTexture);
            }

        }

        public void drawCountry(Tile _tile) {
            // get state
            State _state = _tile.state;

            // if null is ocean probably
            if (_state != null) {
                // get country color
                Color color = _tile.state.country.color;
                // get data from frame once
                ColorData<Color> currentTexture = Tile.frameReference.colorData.getCopy();

                // loop through states in country
                for(int state_count = 0; state_count < _tile.state.country.states.Count; state_count++) {
                    // draw each tile in state with 
                    State currentState = _state.country.states[state_count];
                    for (int i = 0; i < currentState.tiles.Length; i++) {
                        // if land
                        if(currentState.tiles[i].isLand) {
                            currentState.tiles[i].drawTile(color, currentTexture);
                        }
                    }

                }

                // redraw selected tile's state
                // draw lighter
                color = new Color(
                    255 - (255 - color.R) * 4 / 5,
                    255 - (255 - color.G) * 4 / 5,
                    255 - (255 - color.B) * 4 / 5);
                for (int i = 0; i < _state.tiles.Length; i++) {
                    _state.tiles[i].drawTile(color, currentTexture);
                }
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

            // run loader and set tiles
            tiles = tloader.processMap(50);
            // read file and set definitions
            definitions = dreader.processFile(tiles);
            // read file and set countries
            countries = lreader.processFile();

            // init reader for states folder
            StateReader sreader = new StateReader(this); // needs initialized tiles 
            // read files and set states
            states = sreader.processFile();
            State.initStates(states);


            // init continents
            initContinents();
        }
        // create landscale without loading tiles
        // init stuff to 0
        public void genLandscape() {
            tiles = new Dictionary<Color, Tile>();
            definitions = new Tile[0];
            states = new State[0];
            countries = new Dictionary<string, Country>();
            continents = new Continent[0];
        }
    }
}
