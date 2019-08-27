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
        public Point Position; // { get; private set; }

        // texture printed eachFrame
        Texture2D mapTexture;
        // <color> data
        public ColorData<Color> colorData { get; private set; }
        // original data
        ColorData<Color> ogData;

        // get size
        public int Size_x { get { return mapTexture.Width; } }
        public int Size_y { get { return mapTexture.Height; } }

        // width and height of texture after applying zoom
        Point zoomSize;
        // use spriteBatch to draw frame 
        public void Draw(SpriteBatch spriteBatch) {
            // update frame zoom size
            zoomSize = new Point((int)(mapTexture.Width * currentZoom / 100), (int)(mapTexture.Height * currentZoom / 100));

            // init relative position one frame outside to the left of screen
            int relativePosition = Position.X;
            while (relativePosition > 0) {
                relativePosition -= zoomSize.X;
            }

            // draw while still empty in screen
            while(relativePosition < 1920) {
                // print afterimage at appropriate location to fill currentcheck
                spriteBatch.Draw(mapTexture, new Rectangle(new Point(relativePosition, Position.Y), zoomSize), Color.White);
                // move check to next modulo zoomsize width
                relativePosition += zoomSize.X;
            }
        }

        // set color from coord in colordata
        public void setColorFrom(int pos_x, int pos_y, Color _color) {
            colorData.set(pos_x, pos_y, _color);
        }

        // get Color from coord in colordata
        public Color getColorFrom(int pos_x, int pos_y) {
            // bind to screen
            pos_x = pos_x % 1920;

            // calc mod
            //int mod = (int)(mapTexture.Width * 100 / currentZoom);
            // make positive
            //pos_x = (int)(pos_x + (colorData.Width * mod) % colorData.Width * 100 / currentZoom);

            // move pos_x frames to the left
            while (zoomSize.X + Position.X < pos_x) {
                pos_x -= zoomSize.X;
            }
            // move pos_x frames to the right
            while(Position.X > pos_x) {
                pos_x += zoomSize.X;
            }

            // get location click without zoom
            /* Point comparePoint = new Point (
                 (pos_x - Position.X) * 100 / (int)currentZoom, 
                 (pos_y - Position.Y) * 100 / (int)currentZoom); */

            pos_x = (pos_x - Position.X) * 100 / (int)currentZoom;
            pos_y = (pos_y - Position.Y) * 100 / (int)currentZoom;


            // apply zoom
            //int loc_x = (int)(pos_x * 100/currentZoom);
            //int loc_y = (int)(pos_y * 100/currentZoom);

            return ogData.get(pos_x, pos_y);
        }

        // update mapTexture with colorData
        public void updateTexture() {
            mapTexture.SetData<Color>(colorData.Data);
        }
        public void updateTexture(ColorData<Color> canvas) {
            mapTexture.SetData<Color>(canvas.Data);
            // colorData = canvas;
        }

        // copy colordata to texture
        public void setDataColor(ColorData<Color> canvas) {
            colorData = canvas;
        }
        // get data from texture as colordata
        public ColorData<Color> getAllData() {
            /* get data from texture
            // init datacolor
            ColorData<Color> returnData = new ColorData<Color>(mapTexture.Width, mapTexture.Height);
            // save texture to its data array
            mapTexture.GetData<Color>(returnData.Data);
            return returnData;
            */
            // get data from datacolor
            return colorData.getCopy();
        }



    
        // get window size and move frame relative to the screen
        // reads static mouse and keyboard
        int cameraSpeed = 10;
        int margin = 300;
        public void shiftMap(int width, int height) {
            // camera speed
            // distance from edge to trigger movement

            // check left or right, and up or down
            
            // left right movement also controls afterimage location

            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Mouse.GetState().X < margin) {                      // check left
                // always move horizontally because infinite
                Position.X += cameraSpeed;
            } else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Mouse.GetState().X > width - margin) {      // check right
                // always move horizontally because infinite
                Position.X -= cameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Mouse.GetState().Y < margin ) {                       // check up
                if (Position.Y < 0) {
                    Position.Y += cameraSpeed;
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Mouse.GetState().Y > height - margin) {      // check down
                if (zoomSize.Y > height) {
                    Position.Y -= cameraSpeed;
                }
            }

            // stop from going out of bound during rapid zoom

            // relocate frame

            // relative to zoomSize
            /*
            Position.X = Position.X % (int)(mapTexture.Width * currentZoom / 100);
            if((int)(mapTexture.Width * currentZoom / 100) < 1920) {
                // if zoomSize smaller than screen always positive
                if (Position.X < 0) {
                    Position.X += (int)(mapTexture.Width * currentZoom / 100);
                }
            } else {
                // if zoomSize smaller than screen always negative
                if (Position.X < 0) {
                    Position.X -= (int)(mapTexture.Width * currentZoom / 100);
                }
            }*/

            // avoid go below map if Position.Y < 0 (still map not shown above)
            if (Position.Y + zoomSize.Y < height) {
                if (Position.Y < 0) {
                    if (height - zoomSize.Y <= 0) {
                        Position.Y = height - zoomSize.Y;
                    }
                }
            }

            // bound to range 0 - zoomSize.width
            if (Position.X < 0) {
                Position.X += zoomSize.X;
            } else if(Position.X > zoomSize.X) {
                Position.X -= zoomSize.X;
            }


        }

        // zoom control in percentage
        float targetZoom = 100;
        public float currentZoom = 100;
        float zoomSensitivity = 10;
        float minZoom = 20;
        float maxZoom = 200;
        public void checkZoom(MouseState lastState) {
            // check mouse wheel
            if (Mouse.GetState().ScrollWheelValue < lastState.ScrollWheelValue) {

                // zoom out
                // check if changed direction
                if (targetZoom > currentZoom) {
                    // reset
                    targetZoom = currentZoom;
                }
                else {
                    // slow down
                    targetZoom += (currentZoom - targetZoom) / 1000;
                }
                // apply zoom
                targetZoom -= zoomSensitivity;
                if (targetZoom < minZoom) {
                    targetZoom = minZoom;
                } 
            } else if(Mouse.GetState().ScrollWheelValue > lastState.ScrollWheelValue) {

                // zoom in
                // check change direction
                if (targetZoom < currentZoom) {
                    targetZoom = currentZoom;
                } else {
                    targetZoom += (currentZoom - targetZoom) / 1000;
                }
                // apply zoom
                targetZoom += zoomSensitivity;
                if (targetZoom > maxZoom) {
                    targetZoom = maxZoom;
                }
            }

            // zoom towards target
            currentZoom += (targetZoom - currentZoom) / 10;

        }

        // get blank canvas
        public ColorData<Color> getBlankCanvas() {
            return new ColorData<Color>(colorData.Width, colorData.Height);
        }

        // constructor
        public Frame(Texture2D _texture, Point _position, SpriteBatch _spriteBatch) {
            // set texture and its position
            Position = _position;
            mapTexture = _texture;

            // init color[] data from texture
            Color[] _colorData = new Color[_texture.Width * _texture.Height];
            _texture.GetData<Color>(_colorData);
            // then save it to colordata
            colorData = new ColorData<Color>(_colorData, _texture.Width, _texture.Height);
            // also save ogData
            ogData = colorData.getCopy();

            zoomSize = new Point((int)(mapTexture.Width * currentZoom / 100), (int)(mapTexture.Height * currentZoom / 100));
        }
    }
}
