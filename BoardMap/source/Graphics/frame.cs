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

            // if map bigger than screen
            if (zoomSize.Y > height) {

                // room above not aloud
                if (Position.Y > 0) {
                    // remove room above/blank space
                    Position.Y = 0;
                } 
                // room below not aloud
                 else if (Position.Y + zoomSize.Y < height) {
                    // remove room below
                    Position.Y = height - zoomSize.Y;
                }
            }
            // if map smaller than screen
             else if (zoomSize.Y < height) { 

                // map cant go above screen
                if (Position.Y < 0) {
                    // remove going above screen
                    Position.Y = 0;
                } 
                // map cant go below screen
                else if (Position.Y + zoomSize.Y > height) {
                    // remove going below screen
                    Position.Y = height - zoomSize.Y;
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
        // float accumulator for int calculations each frame
        float float_x = 0;
        float float_y = 0;
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

            } else if (Mouse.GetState().ScrollWheelValue > lastState.ScrollWheelValue || Keyboard.GetState().IsKeyDown(Keys.Add)) {

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


            // store currentZoom
            float lastZoom = currentZoom;
            // apply zoom towards target
            currentZoom += (targetZoom - currentZoom) / 10;

            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            // calculate offset to keep mouse in place
            float_x += ( (mouseX - Position.X) * (lastZoom - currentZoom)/lastZoom);
            float_y += ( (mouseY - Position.Y) * (lastZoom - currentZoom)/lastZoom);
            // cast to int if bigger than 1
            int rel_x = (int)float_x;
            int rel_y = (int)float_y;
            // apply
            Position.X += rel_x;
            Position.Y += rel_y;
            // accumulate float smaller than 1. cast to int and apply next time if bigger than 1 
            float_x -= rel_x;
            float_y -= rel_y;
            

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
