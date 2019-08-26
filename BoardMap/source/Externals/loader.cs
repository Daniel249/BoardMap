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
            // create noname tile 
            Tile noname = new Tile(new Point(0, 0), new ColorData<bool>(0, 0), Color.Black);
            dictionary.Add(noname.color, noname);

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
            ColorData<bool> overkill = new ColorData<bool>(2 * range, range);
            // get color from starting pixel
            Color searchColor = mapData.get(pos_x, pos_y);

            // set overkill's relative position in map texture to scan - Note: limit_x first as absolute then as relative
            // set finishing y to {range} or bound by mapdata height
            int limit_y;
            if (pos_y + range < mapData.Height) {
                limit_y = range;
            } else {
                limit_y = mapData.Height - pos_y;
            }

            // set finishing -absolute x in mapdata- to {range} relative to position or bound by mapdata width
            int limit_x;
            if (pos_x + range < mapData.Width) {
                limit_x = pos_x + range;
            } else {
                limit_x = mapData.Width;
            }
            // set scan absolute position to map texture - start {range} to the left and same height
            pos_x -= range;
            if(pos_x < 0) {
                pos_x = 0;
            }
            // get distance from pos_x to limit_x to use on loop
            limit_x = limit_x - pos_x;

            // init margins to max for cut to 0x0
            // where the reduced image already starts
            int topMargin = range; // -1
            int leftMargin = 2 * range; // -1
            int bottomMargin = 0;
            int rightMargin = 0;

            // loop through texture and extract overkill black and white image
            for (int count_y = 0; count_y < limit_y; count_y++) {
                for (int count_x = 0; count_x < limit_x; count_x++) {
                    // if match, delete pixel and draw on return
                    if (mapData.get(pos_x + count_x, pos_y + count_y) == searchColor) {
                        mapData.set(pos_x + count_x, pos_y + count_y, Color.Black);
                        overkill.set(count_x, count_y, true);
                        
                        // then lower margins for later cut
                        if (leftMargin > count_x) {
                            leftMargin = count_x;
                        }
                        if (topMargin > count_y) {
                            topMargin = count_y;
                        }
                        if (rightMargin < count_x) {
                            rightMargin = count_x;
                        }
                        if (bottomMargin < count_y) {
                            bottomMargin = count_y;
                        }
                    }
                }
            }

            // init colordata to return with empty array of cut size
            ColorData<bool> reducedImage = new ColorData<bool>(rightMargin - leftMargin + 1, bottomMargin - topMargin + 1);

            // produce cut version
            for (int count_y = 0; count_y < reducedImage.Height; count_y++) {
                for (int count_x = 0; count_x < reducedImage.Width; count_x++) {
                    // get color
                    bool toPrint = overkill.get(leftMargin + count_x, topMargin + count_y);
                    // draw color
                    reducedImage.set(count_x, count_y, toPrint);
                }
            }

            // update position to aftercut position relative to whole map texture
            pos_x += leftMargin;
            pos_y += topMargin;

            return reducedImage;

            // cut image and update its position relative to frame
            //return cutData(ref pos_x, ref pos_y, reducedImage);
        }

        // flood pixels with same color as point 
        ColorData<bool> scanFlood(ref int pos_x, ref int pos_y) {
            // init black and white datacolor to return
            ColorData<bool> toReturn = new ColorData<bool>(2 , 1);



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
