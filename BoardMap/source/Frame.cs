using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Graphics
{
    // contains frame's texture to draw and its colordata
    class Frame
    {
        // top left corner position
        public Vector2 Position; // { get; private set; }

        // texture printed eachFrame
        Texture2D mapTexture;
        // bitmap color array
        ColorData mapData;
        // after image is to the left or not
        bool afterImageLeft;

        // screen values
        int BBWidth;
        int BBHeight;
        // SpriteBatch spriteBatch;

        // at default zoom moves 1 value to 1 pixel
        public void shiftMap(int shift_x, int shift_y) {

            if (shift_x > 0) {
                if (Position.X < 0) {
                    Position.X += shift_x;
                }
                else {
                    // if out of bound to the left -> teleport to the left and spawn after image to the right
                    Position.X -= mapTexture.Width;
                    afterImageLeft = false;
                }
            }
            if(shift_x < 0) { 
                if (Position.X + mapTexture.Width > BBWidth) {
                    Position.X += shift_x;
                }
                else {
                    // if out of bound to the right -> teleport to the right and spawn after image to the left
                    Position.X += mapTexture.Width;
                    afterImageLeft = true;
                }
            }

            if(shift_y > 0) {
                // up
                if (Position.Y < 0) {
                    Position.Y += shift_y;
                }
            }
            if (shift_y < 0) {
                if (Position.Y + mapTexture.Height > BBHeight) {
                    Position.Y += shift_y;
                }
            }
        }

        // use spriteBatch to draw frame 
        public void Draw(SpriteBatch spriteBatch) {
            // draw only frame
            spriteBatch.Draw(mapTexture, Position, Color.White);
            // draw after image 
            if (afterImageLeft) {
                spriteBatch.Draw(mapTexture, new Vector2(Position.X - mapTexture.Width, Position.Y), Color.White);
            }
            else {
                spriteBatch.Draw(mapTexture, new Vector2(Position.X + mapTexture.Width, Position.Y), Color.White);
            }
        }

        // get Color from mapdata
        public Color getMapData(int pos_x, int pos_y) {
            return mapData.get(pos_x, pos_y);
        }
    

        public Frame(Texture2D _texture, Vector2 _position, SpriteBatch _spriteBatch, int bheight, int bwidth) {
            // set texture and its position
            Position = _position;
            mapTexture = _texture;
            // init color[] from texture
            mapData = new ColorData(mapTexture);

            BBHeight = bheight;
            BBWidth = bwidth;

            // set spritebatch for eventual draw to screen
            // spriteBatch = _spriteBatch;
        }
    }
}
