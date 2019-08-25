﻿using BoardMap.Externals;
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
    class Landscape
    {
        // continents
        Continent[] continents;

        // countries
        Country[] countries;

        // states 
        State[] states;

        // tiles
        // Tile[] tiles;
        public Dictionary<Color, Tile> tiles { get; private set; }



        // constructor
        public Landscape(Texture2D _texture) {
            // init loader with map texture
            TileLoader tloader = new TileLoader(_texture);
            // run loader and set tiles
            tiles = tloader.processData(40);
        }
    }
}
