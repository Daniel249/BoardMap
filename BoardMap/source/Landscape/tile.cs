using BoardMap.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.LandscapeNS
{
    // properties loaded from definitions.csv
    class Tile
    {
        // tile id. from 1 to 13k
        public int ID { get; private set; }

        // textures
        // bool[] color atas
        public List<ColorData<bool>> textures { get; private set; }
        // textures' positions 
        public List<Point> positions { get; private set; }
        // and color for those locations
        public Color color { get; private set; }

        // is land tile
        public bool isLand { get; private set; }
        // is coastal
        public bool isCoastal { get; private set; }

        // continent
        public int continent { get; private set; }


        // reference to state
        State state;
        // reference to city | null if not a city
        // Polis city;

        // if 
        public void addTexture(Point _point, ColorData<bool> _texture) {
            if(textures.Any()) {
                textures.Add(_texture);
                positions.Add(_point);
            } else {
                // tile was created without a texture
                throw new NotSupportedException("tile created without texture");
            }
        }

        // constructor 
        public Tile(Point _point, ColorData<bool> _texture, Color _color) {
            color = _color;
            // init lists
            positions = new List<Point>();
            positions.Add(_point);
            textures = new List<ColorData<bool>>();
            textures.Add(_texture);
        }

        // set other attributes, not set in constructor
        // used in DefReader.processFile(dictionary tiles)
        public void setRest(int _id, bool _isLand, bool _isCoastal, int _continent) {
            ID = _id;
            isLand = _isLand;
            isCoastal = _isCoastal;
            continent = _continent;
        }

    }
}
