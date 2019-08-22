using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.source
{
    // properties loaded from definitions.csv
    class Tile
    {
        // tile id. from 1 to 13k
        public int ID { get; private set; }
        // tile color
        public Color color { get; private set; }
        // is land tile
        public bool isLand { get; private set; }
        // is coastal
        public bool isCoastal { get; private set; }





        // constructor 
        public Tile(int _id, Color _color, bool _isLand, bool _isCoastal) {
            ID = _id;
            color = _color;
            isLand = _isLand;
            isCoastal = _isCoastal;
        }

    }
}
