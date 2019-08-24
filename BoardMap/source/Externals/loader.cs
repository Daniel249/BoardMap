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
        ColorData<Color> colorData;

        // main method
        // process whole frame's colordata and get dictionary from color to colordata
        // evtl init tiles with those colordatas
        public Dictionary<Color, ColorData<bool>> processData(int range) {
            Dictionary<Color, ColorData<bool>> dict = new Dictionary<Color, ColorData<bool>>();

            // search for a tile's pixel
            for (int pos_y = 0; pos_y < colorData.Height; pos_y++) {
                for (int pos_x = 0; pos_x < colorData.Width; pos_x++) {
                    // get color
                    Color color = colorData.get(pos_x, pos_y);
                    // if not black -> tile of that color still not processed
                    if (color != Color.Black) {
                        dict.Add(color, scanRange(pos_x, pos_y, range));
                    }

                }
            }
            return dict;
        }

        // search range pixel to every side for the color in that position
        ColorData<bool> scanRange(int pos_x, int pos_y, int range) {
            // init black and white datacolor to return
            ColorData<bool> toReturn = new ColorData<bool>(new bool[2 * range * range], 2 * range, range);
            // get color from starting pixel
            Color searchColor = colorData.get(pos_x, pos_y);
            // set boundaries based on position and range
            pos_x -= range;
            if(pos_x < 0) {
                pos_x = 0;
            }

            // set black and white overkill image
            for (int count_y = 0; count_y < range; count_y++) {
                for (int count_x = 0; count_x < 2*range; count_x++) {
                    // if match
                    if (colorData.get(pos_x + count_x, pos_y + count_y) == searchColor) {
                        colorData.set(pos_x + count_x, pos_y + count_y, Color.Black);
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
            int right = -1;
            int down = -1;

            // run from left to right and up down
            for (int count_y = 0; count_y < toCut.Height; count_y++) {
                for (int count_x = 0; count_x < toCut.Width; count_x++) {
                    // if match
                    if (toCut.get(count_x, count_y) == true) {
                        // expected to always set once to 0
                        if(up > count_y) {
                            up = count_y;
                        }
                        // set left to minimun distance from left frame
                        if(left > count_x) {
                            left = count_x;
                        }
                    }
                }
            }
            // run from right to left and down up
            for (int count_y = toCut.Height - 1; count_y >= 0; count_y--) {
                for (int count_x = toCut.Width - 1; count_x >= 0; count_x--) {
                    // if match
                    if (toCut.get(count_x, count_y) == true) {
                        // expected to always set once to 0
                        if (down < count_y) {
                            down = count_y;
                        }
                        // set left to minimun distance from left frame
                        if (right < count_x) {
                            right = count_x;
                        }
                    }
                }
            }
            // init empty newReturn data
            ColorData<bool> newReturn = new ColorData<bool>(new bool[(right - left) * (down - up)], 
                (right - left), (down - up));

            // load newReturn with data from toCut
            for (int count_y = 0; count_y < newReturn.Height; count_y++) {
                for (int count_x = 0; count_x < newReturn.Width; count_x++) {
                    // print to newReturn with up and left as offset
                    bool toPrint = toCut.get(count_x + left, count_y + up);
                    newReturn.set(count_x, count_y, toPrint);
                }
            }

            // update position relative to frame and return
            pos_x += left;
            pos_y += up;
            return newReturn;
        }

        // constructor from texture
        public TileLoader(Texture2D _texture) {
            Color[] _colorData = new Color[_texture.Width * _texture.Height];
            _texture.GetData<Color>(_colorData);
            colorData = new ColorData<Color>(_colorData, _texture.Width, _texture.Height);
        }
    }
}
