using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Graphics
{
    // generic implementation od 2d array using 1d array
    class ColorData<T>
    {
        // the data iteself
        T[] data;
        public T[] Data { get { return data; } }
        // size	
        public int Height { get; private set; }
        public int Width { get; private set; }

        // return Color in coordinates	
        public T get(int pos_x, int pos_y) {
            // check bounds	
            if (pos_x < 0 || pos_x >= Width || pos_y < 0 || pos_y >= Height) {
                return default(T);
                throw new IndexOutOfRangeException("ColorData out of bounds");
            }
            // else return from coordinates	
            return Data[pos_x + pos_y * Width];
        }

        // set Color in coordinates
        public bool set(int pos_x, int pos_y, T toSet) {
            // check bounds	
            if (pos_x < 0 || pos_x >= Width || pos_y < 0 || pos_y >= Height) {
                return false;
                throw new IndexOutOfRangeException("ColorData out of bounds");
            }
            // else set
            data[pos_x + pos_y * Width] = toSet;
            return true;
        }

        // set data array	
        public void setData(Texture2D texture) {
            //data = new Color[texture.Width * texture.Height];
            //texture.GetData<Color>(data);
        }


        // constructor	
        public ColorData(T[] _data, int _width, int _height) {
            data = _data;
            Height = _height;
            Width = _width;
        }
    }
}
