using BoardMap.Common;
using BoardMap.LandscapeNS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardMap.Interface
{
    class UInterface
    {
        // rectangle - log - fps
        // font
        SpriteFont onlyFont;

        // 1 pixel texture. for: log empty background. 
        Texture2D whiteRectangle;


        // log position and size
        Point logPosition;
        Point logSize;
        // log color
        Color logColor;

        SpriteBatch spriteBatch;

        // main method
        public void DrawInterface(Tile selectedTile, Tile tileHover) {
            // draw interface

            // draw background for log
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, logSize.X, logSize.Y), logColor);

            // print mouse location
            string strong = $"{Mouse.GetState().X.ToString()}   {Mouse.GetState().Y.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(logPosition.X + 15, logPosition.Y + 10), Color.Black);

            // print current tile hover
            printTileInfo(new Point(logPosition.X, logPosition.Y + 30), tileHover);


            // print country
            printCountryInfo(new Point(logPosition.X, logPosition.Y + 120), selectedTile);

            // print selected state
            printStateInfo(new Point(logPosition.X, logPosition.Y + 170), selectedTile);

            // print selectedTile
            printTileInfo(new Point(logPosition.X, logPosition.Y + 220), selectedTile);
            // draw its texture count also
            spriteBatch.DrawString(onlyFont, "Count: " + selectedTile.textures.Count.ToString(),
                new Vector2(logPosition.X + 15, logPosition.Y + 290), Color.Black);
        }


        // print country state and tile info
        #region

        // variable for all print methods
        // position second row of ui for rgb
        int secondRow = 95;

        // print country info
        void printCountryInfo(Point _position, Tile _tile) {
            // get country
            Country country;
            // tile has state, and country
            if (_tile.state != null) {
                country = _tile.state.country;
            } else {
                // dont draw
                return;
            }

            // print square of state's color
            spriteBatch.Draw(whiteRectangle, new Rectangle(_position.X, _position.Y, 15, 15), country.color);

            // print country tag
            spriteBatch.DrawString(onlyFont, country.Tag.ToString(), new Vector2(_position.X + 20, _position.Y), Color.Black);

            // print country name
            spriteBatch.DrawString(onlyFont, country.Name, new Vector2(_position.X + secondRow, _position.Y), Color.Black);

            // go down 20 pixels

            // print number of states
            spriteBatch.DrawString(onlyFont, "#: " + country.states.Count.ToString(), new Vector2(_position.X + 20, _position.Y + 20), Color.Black);

            // print population
            spriteBatch.DrawString(onlyFont, String.Format("{0:### ### ### ###}", country.population),
                new Vector2(_position.X + secondRow, _position.Y + 20), Color.Black);

            // looks like this

            //  []  Name    Tag
            //      #state  Pop
        }

        // print state info
        void printStateInfo(Point _position, Tile _tile) {
            // get state
            State state;
            if (_tile.state != null) {
                state = _tile.state;
            } else {
                // dont draw
                return;
            }
            // print square of state's color
            spriteBatch.Draw(whiteRectangle, new Rectangle(_position.X, _position.Y, 15, 15), state.color);

            // print state id
            spriteBatch.DrawString(onlyFont, "ID: " + state.ID.ToString(), new Vector2(_position.X + 20, _position.Y), Color.Black);

            // print state name
            spriteBatch.DrawString(onlyFont, state.Name, new Vector2(_position.X + secondRow, _position.Y), Color.Black);

            // go down 20 pixels

            // print number of tiles
            spriteBatch.DrawString(onlyFont, "#: " + state.tiles.Length.ToString(), new Vector2(_position.X + 20, _position.Y + 20), Color.Black);

            // print population
            spriteBatch.DrawString(onlyFont, 
                String.Format("{0:### ### ### ###}", state.population.Size),
                new Vector2(_position.X + secondRow, _position.Y + 20), Color.Black);

            // looks like this

            //  []  ID      Name
            //      #tile   Pop
        }

        // print tile info
        void printTileInfo(Point _position, Tile _tile) {
            // print square of tile's color
            spriteBatch.Draw(whiteRectangle, new Rectangle(_position.X, _position.Y, 15, 15), _tile.color);

            // print tile id
            string strong = $"ID: {_tile.ID.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(_position.X + 20, _position.Y), Color.Black);

            // go down 20 pixels

            // print tile position
            strong = $"X: {_tile.positions[0].X.ToString()} \nY: {_tile.positions[0].Y.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(_position.X + 20, _position.Y + 20), Color.Black);

            // print tile rgb value
            strong = $"R: {_tile.color.R} \nG: {_tile.color.G} \nB: {_tile.color.B}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(_position.X + secondRow, _position.Y + 20), Color.Black);

            // looks like this

            //  []  ID
            //      X   R
            //      Y   G
            //          B    
        }
        #endregion

        // constructor 
        // used in game.LoadContent. init all members
        public UInterface(Texture2D _whiteDot, SpriteFont _font, SpriteBatch _spriteBatch) {
            // init log
            logPosition = new Point(0, 0);
            logSize = new Point(220, 330);
            logColor = Color.White;
            whiteRectangle = _whiteDot;
            whiteRectangle.SetData(new[] { Color.White });

            // load font
            onlyFont = _font;
            // store spritebatch reference
            spriteBatch = _spriteBatch;
        }
    }
}
