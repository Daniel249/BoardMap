using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            pos_x = (pos_x + mapTexture.Width) % mapTexture.Width;
            return mapData.get(pos_x, pos_y);
        }
    
        // get window size and move frame relative to the screen
        // reads static mouse and keyboard
        public void shiftMap(int width, int height) {

            // check left or right, and up or down
            
            // left right movement also controls afterimage location


            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Mouse.GetState().X <= 0) {                      // check left
                if (Position.X < 0) {
                    // if can move
                    Position.X += 20;
                } else {
                    // if out of bound to the left -> teleport to the left and spawn after image to the right
                    Position.X -= mapTexture.Width;
                    afterImageLeft = false;
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Mouse.GetState().X >= width - 10) {     // check right
                if (Position.X + mapTexture.Width > width) {
                    // if can move
                    Position.X -= 20;
                } else {
                    // if out of bound to the right -> teleport to the right and spawn afterimage to the left
                    Position.X += mapTexture.Width;
                    afterImageLeft = true;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Mouse.GetState().Y <= 0) {                        // check up
                if (Position.Y < 0) {
                    Position.Y += 20;
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Mouse.GetState().Y >= height - 10) {     // check down
                if (Position.Y + mapTexture.Height > height) {
                    Position.Y -= 20;
                }
            }
            
        }

        // constructor
        public Frame(Texture2D _texture, Vector2 _position, SpriteBatch _spriteBatch) {
            // set texture and its position
            Position = _position;
            mapTexture = _texture;
            // init color[] from texture
            mapData = new ColorData(mapTexture);
        }
    }
}
