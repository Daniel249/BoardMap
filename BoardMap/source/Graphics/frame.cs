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
            // = new Point((int)(mapTexture.Width * currentZoom / 100), (int)(mapTexture.Height * currentZoom / 100));

            // init relative position one frame outside to the left of screen
            int relativePosition = Position.X;
            while (relativePosition > 0) {
                relativePosition -= zoomSize.X;
            }

            // draw while still empty in screen
            while(relativePosition < 1920) {
                // print
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

            // estimate location in map relative to map position. apply zoom after
            float search_x = (float)(pos_x - Position.X) * 100 / currentZoom;
            float search_y = (float)(pos_y - Position.Y) * 100 / currentZoom;

            return ogData.get((int)search_x, (int)search_y);
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
        // camera speed
        int cameraSpeed = 15;
        int margin = 100;
        public void shiftMap(int width, int height) {
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
                    // bind to 0 coming from less than 0
                    if (Position.Y > 0) {
                        Position.Y = 0;
                    }
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Mouse.GetState().Y > height - margin) {      // check down
                if (Position.Y + zoomSize.Y > height) {
                    Position.Y -= cameraSpeed;
                    // bind to height coming from more than height
                    if (Position.Y + zoomSize.Y < height) {
                        Position.Y = height - zoomSize.Y;
                    }
                }
            }

            // map bound to screen through 

            // if map bigger than screen and implicit zoom out
            if (zoomSize.Y > height) {

                // if room above
                if (Position.Y > 0) {
                    // and map goes below screen
                    if (Position.Y + zoomSize.Y > height) {
                        // remove room above/blank space
                        Position.Y = 0;
                    }
                } 
                // if room below
                 else if (Position.Y + zoomSize.Y < height) {
                    // and map goes above screen
                    if (Position.Y < 0) {
                        // remove room below
                        Position.Y = height - zoomSize.Y;
                    }
                }
            }
            // if map smaller than screen and implicit zoom in
             else if (zoomSize.Y < height) { 

                // if map goes above screen
                if (Position.Y < 0) {
                    // and room below map
                    if (Position.Y + zoomSize.Y < height) {
                        // remove going above screen
                        Position.Y = 0;
                    }
                } 
                // if map goes below screen
                else if (Position.Y + zoomSize.Y > height) {
                    // and room above map
                    if (Position.Y > 0) {
                        // remove going below screen
                        Position.Y = height - zoomSize.Y;
                    }
                }
            }

            // relocate map modulo mapSize to keep it around the mouse
            // for zooming into mouse
            // has no effect on printing 
            while (Position.X < Mouse.GetState().X) {
                Position.X += zoomSize.X;
            }
            if (Position.X > Mouse.GetState().X) {
                Position.X -= zoomSize.X;
            }

        }

        // zoom control in percentage
        float targetZoom = 100;
        public float currentZoom = 100;
        float zoomSensitivity = 10;
        float minZoom = 10;
        float maxZoom = 500;
        public void checkZoom(MouseState lastState) {
            
            // check mouse wheel and + and - in keyboard 
            if (Mouse.GetState().ScrollWheelValue < lastState.ScrollWheelValue || Keyboard.GetState().IsKeyDown(Keys.Subtract)) {
                // zoom out
                // check if changed direction
                if (targetZoom > currentZoom) {
                    // reset
                    targetZoom = currentZoom;
                } else {
                    // slow down
                    targetZoom += (currentZoom - targetZoom) / 1000;
                }
                // apply zoom proportional to currentZoom
                // zoom currently at 10 in - 500 out 
                targetZoom -= zoomSensitivity * currentZoom / 100;

            } else if(Mouse.GetState().ScrollWheelValue > lastState.ScrollWheelValue || Keyboard.GetState().IsKeyDown(Keys.Add)) {

                // zoom in
                // check change direction
                if (targetZoom < currentZoom) {
                    targetZoom = currentZoom;
                } else {
                    targetZoom += (currentZoom - targetZoom) / 1000;
                }
                // apply zoom
                targetZoom += zoomSensitivity * currentZoom / 100;

            }

            // bound to minZoom - maxZoom
            if (targetZoom > maxZoom) {
                targetZoom = maxZoom;
            } else if (targetZoom < minZoom) {
                targetZoom = minZoom;
            }

            /*
            // before applying zoom
            // estimate location in map relative to map position. apply zoom after
            float search_x = (float)(Mouse.GetState().X - Position.X) * 100 / currentZoom;
            float search_y = (float)(Mouse.GetState().Y - Position.Y) * 100 / currentZoom;
            if (search_x < 0) {
                search_x = 0;
            } else if (search_x > mapTexture.Width) {
                search_x = mapTexture.Width;
            }
            if (search_y < 0) {
                search_y = 0;
            } else if (search_y > mapTexture.Height) {
                search_y = mapTexture.Height;
            } */

            // store currentZoom
            float lastZoom = currentZoom;
            // apply zoom towards target
            currentZoom += (targetZoom - currentZoom) / 10;

            // calculate corrections to maintain mouse static 
            // mouse location is currently calculalted like this in getColorFrom
            // float search_x = (float)(pos_x - Position.X) * 100 / currentZoom;
            // float search_y = (float)(pos_y - Position.Y) * 100 / currentZoom;
            
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            /*
            // bound Y to map height relative to position. as to not zoom into nothing
            if (mouseY > Position.Y) {
                mouseY = Position.Y;
            } else if (mouseY > Position.Y + mapTexture.Height) {
                mouseY = Position.Y + mapTexture.Height;
            } */
            // do calculations
            int rel_x = (int)((lastZoom - currentZoom) * (mouseX - Position.X) / lastZoom);
            int rel_y = (int)((lastZoom - currentZoom) * (mouseY - Position.Y) / lastZoom);
            // translate position to keep mouse static
            Position.X += rel_x;
            Position.Y += rel_y;

            

            //update zoomSize for next drawing
            zoomSize = new Point((int)(mapTexture.Width * currentZoom / 100), (int)(mapTexture.Height * currentZoom / 100));
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
            // init zoom to size*1
            zoomSize = new Point((int)(mapTexture.Width * currentZoom / 100), (int)(mapTexture.Height * currentZoom / 100));
        }
    }
}
