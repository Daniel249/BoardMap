using BoardMap.Graphics;
using BoardMap.LandscapeNS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Externals
{
    class TileLoader
    {
        // data to process
        ColorData<Color> mapData;

        // main method
        // process whole frame's colordata and return dictionary from color to colordata
        // evtl init tiles with those colordatas
        public Dictionary<Color, Tile> processMap(int range) {
            Dictionary<Color, Tile> dictionary = new Dictionary<Color, Tile>();

            // loop through pixels left right and up down
            for (int count_y = 0; count_y < mapData.Height; count_y++) {
                for (int count_x = 0; count_x < mapData.Width; count_x++) {
                    // get color
                    Color color = mapData.get(count_x, count_y);
                    // if not black -> tile of that color still not processed
                    if (color != Color.Black) {
                        // copy current location to use by reference
                        int pos_x = count_x;
                        int pos_y = count_y;
                        // generate texture
                        ColorData<bool> _tile = scanRange(ref pos_x, ref pos_y, range);

                        // check tile not already in dictionary
                        Tile tile;
                        if (!dictionary.TryGetValue(color, out tile)) {
                            // init new tile and add it to return dictionary
                            tile = new Tile(new Point(pos_x, pos_y), _tile, color);
                            dictionary.Add(color, tile);
                        } else {
                            // add texture to tile found
                            tile.addTexture(new Point(pos_x, pos_y), _tile);
                        }
                    }

                }
            }
            return dictionary;
        }

        // search pixels in range to every side and downwards for the color in that position
        ColorData<bool> scanRange(ref int pos_x, ref int pos_y, int range) {
            // init black and white datacolor to return
            ColorData<bool> toReturn = new ColorData<bool>(new bool[2 * range * range], 2 * range, range);
            // get color from starting pixel
            Color searchColor = mapData.get(pos_x, pos_y);
            // set boundaries based on position and range
            pos_x -= range;
            if(pos_x < 0) {
                pos_x = 0;
            }

            // set black and white overkill image
            for (int count_y = 0; count_y < range; count_y++) {
                for (int count_x = 0; count_x < 2*range; count_x++) {
                    // if match, delete pixel and draw on return
                    if (mapData.get(pos_x + count_x, pos_y + count_y) == searchColor) {
                        mapData.set(pos_x + count_x, pos_y + count_y, Color.Black);
                        toReturn.set(count_x, count_y, true);
                    }
                }
            }
            // cut image and update its position relative to frame
            return cutData(ref pos_x, ref pos_y, toReturn);
        }

        // cut toReturn color data from scanRange() to lowest possible size 
        // and change position relative to frame accordingly
        ColorData<bool> cutData(ref int pos_x, ref int pos_y, ColorData<bool> toCut) {
            // init all sides' offset to move
            int up = toCut.Height;
            int left = toCut.Width;
            int right = 0;
            int down = 0;

            // run from left to right and up down
            for (int count_y = 0; count_y < toCut.Height; count_y++) {
                for (int count_x = 0; count_x < toCut.Width; count_x++) {
                    // if find
                    if (toCut.get(count_x, count_y) == true) {
                        // expected to always set once to 0
                        if(up > count_y) {
                            up = count_y;
                        }
                        // minimun distance from left frame
                        if(left > count_x) {
                            left = count_x;
                        }
                    }
                }
            }
            // run from right to left and down up
            for (int count_y = toCut.Height - 1; count_y >= 0; count_y--) {
                for (int count_x = toCut.Width - 1; count_x >= 0; count_x--) {
                    // if find
                    if (toCut.get(count_x, count_y) == true) {
                        // minimum distance from bottom frame
                        if (down < count_y) {
                            down = count_y;
                        }
                        // minimum distance from right frame
                        if (right < count_x) {
                            right = count_x;
                        }
                    }
                }
            }
            // init colordata to return with empty array of cut size
            ColorData<bool> toReturn = new ColorData<bool>(new bool[(right - left) * (down - up)], 
                (right - left), (down - up));

            // load toReturn with data from toCut
            for (int count_y = 0; count_y < toReturn.Height; count_y++) {
                for (int count_x = 0; count_x < toReturn.Width; count_x++) {
                    // get color
                    bool toPrint = toCut.get(count_x + left, count_y + up);
                    // draw color
                    toReturn.set(count_x, count_y, toPrint);
                }
            }

            // update position relative to frame and return
            pos_x += left;
            pos_y += up;
            return toReturn;
        }

        // constructor from texture
        public TileLoader(Texture2D _texture) {
            // set mapData from texture
            Color[] _mapData = new Color[_texture.Width * _texture.Height];
            _texture.GetData<Color>(_mapData);

            mapData = new ColorData<Color>(_mapData, _texture.Width, _texture.Height);
        }
    }
}
