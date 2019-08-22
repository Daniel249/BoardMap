using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap
{
    // holds color bytes and manages array locations
    class ColorData
    {
        // data itself
        Color[] data;
        // size
        public int Height { get; private set; }
        public int Width { get; private set; }

        // return Color in coordinates
        public Color get(int pos_x, int pos_y)
        {
            // check bounds
            if(pos_x < 0 || pos_x >= Width || pos_y < 0 || pos_y >= Height) {
                return data[0];
                throw new IndexOutOfRangeException("ColorData out of bounds");
            }
            // else return from coordinates
            return data[pos_x + pos_y * Width];
        }

        // set data array
        public void setData(Texture2D texture)
        {
            data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);
        }
        // copy data array to texture
        public void getData(Texture2D texture)
        {
            texture.SetData<Color>(data);
        } 

        // constructor
        public ColorData(Texture2D texture)
        {
            // loads data from texture
            setData(texture);
            Height = texture.Height;
            Width = texture.Width;
            
        }
    }
}
