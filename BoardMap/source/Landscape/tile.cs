﻿using Microsoft.Xna.Framework;
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
        // bool[] datas
        public List<bool[]> textures { get; private set; }
        // and color for those locations
        public Color color { get; private set; }

        // is land tile
        public bool isLand { get; private set; }
        // is coastal
        public bool isCoastal { get; private set; }


        // reference to state
        State state;
        // reference to city | null if not a city
        Polis city;

        // if 
        public void addTexture(bool[] _texture) {
            if(textures.Any()) {
                textures.Add(_texture);
            } else {
                // list wasnt created with a texture
                throw new NotSupportedException("tile created without texture");
            }
        }

        // constructor 
        public Tile(Color _color, bool[] _texture) {
            textures.Add(_texture);
            color = _color;
        }

        // set other attributes, not set in constructor
        public void setRest(int _id, bool _isLand, bool _isCoastal) {
            throw new NotImplementedException();
        }

    }
}