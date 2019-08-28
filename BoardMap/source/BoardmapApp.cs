using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BoardMap.Graphics;
using BoardMap.Common;
using BoardMap.LandscapeNS;
using System.Globalization;
using System;

namespace BoardMap
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BoardmapApp : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public BoardmapApp() {
            // init graphics
            graphics = new GraphicsDeviceManager(this)
            {
                // then set to fullscreen
                IsFullScreen = true,
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };

            Content.RootDirectory = "Content";
        }

        // Frame containing texture colordata and position
        Frame frame;

        // landscape containing counries states and tiles
        Landscape landscape;

        // input states for comparison
        KeyboardState lastKeyboard;
        MouseState lastMouse;

         
        // font - rectangle - log - fps
        #region
        // font
        SpriteFont onlyFont;

        // 1 pixel texture. for: log empty background. 
        Texture2D whiteRectangle;


        // log position and size
        Point logPosition;
        Point logSize;
        // log color
        Color logColor;

        // fps counter
        Framerate fpsCounter;
        #endregion


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            this.IsMouseVisible = true;
            Mouse.WindowHandle = Window.Handle;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // init log
            logPosition = new Point(0, 0);
            logSize = new Point(220, 330);
            logColor = Color.White;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            // load font
            onlyFont = Content.Load<SpriteFont>("font");

            // init fps counter
            fpsCounter = new Framerate(5);

            // init keyboard state stored for comparison
            lastKeyboard = Keyboard.GetState();
            lastMouse = Mouse.GetState();

            // init Frame
            Point framePosition = new Point(0, 0);
            // load map as texture
            Texture2D onlyTexture = Content.Load<Texture2D>("provinces");
            frame = new Frame(onlyTexture, framePosition, spriteBatch);
            // set frame reference in tile class
            Tile.setFrame(frame);

            landscape = new Landscape(onlyTexture);
            // get first tile
            selectedTile = landscape.searchTile(1);
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // esc -> exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            // mouse click
            if (Mouse.GetState().LeftButton == ButtonState.Pressed) {
                // save clicked tile
                selectedTile = landscape.searchTile(frame.getColorFrom(Mouse.GetState().X, Mouse.GetState().Y));
                // draw light state
                //landscape.drawLightState(selectedTile, selectedTile.state.country.color);
                // draw country
                landscape.drawCountry(selectedTile);
            }

            // move map
            frame.shiftMap(GraphicsDevice.PresentationParameters.BackBufferWidth, 
                           GraphicsDevice.PresentationParameters.BackBufferHeight);

            // keyboard
            landscape.changeMapMode(lastKeyboard);
            lastKeyboard = Keyboard.GetState();

            // mouse
            frame.checkZoom(lastMouse);
            lastMouse = Mouse.GetState();
                        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(landscape.waterColor);
            // sprite beginn
            spriteBatch.Begin();


            // draw map !
            frame.Draw(spriteBatch);



            // draw interface

            // draw background for log
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, logSize.X, logSize.Y), logColor);

            // print mouse location
            string strong = $"{Mouse.GetState().X.ToString()}   {Mouse.GetState().Y.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(logPosition.X + 15, logPosition.Y + 10), Color.Black);

            // print current tile hover
            Tile tileHover = landscape.searchTile(frame.getColorFrom(
                Mouse.GetState().X, Mouse.GetState().Y));
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

            



            // Right Corner

            // draw fpsCounter
            fpsCounter.Update(gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.DrawString(onlyFont, "FPS: " + fpsCounter.framerate.ToString("F"), new Vector2(1920 - 100, 15), Color.Black);

            // draw number of loaded tiles
            spriteBatch.DrawString(onlyFont, "Tiles: " + landscape.tiles.Count.ToString(), new Vector2(1920 - 100, 45), Color.Black);

            // draw currentZoom
            spriteBatch.DrawString(onlyFont, $"Zoom: {frame.currentZoom.ToString()}", new Vector2(1920 - 100, 75), Color.Black);


            // sprite end
            spriteBatch.End();
            base.Draw(gameTime);
        }

        Tile selectedTile;

        // variable for all print methods
        // position second row of ui
        int secondRow = 95;

        // print country info
        void printCountryInfo(Point _position, Tile _tile) {
            // get country
            Country country;
            // tile has state, and country
            if(_tile.state != null) {
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
            if(_tile.state != null) {
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
            spriteBatch.DrawString(onlyFont, String.Format("{0:### ### ### ###}", state.population), 
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


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            base.UnloadContent();
            spriteBatch.Dispose();

            whiteRectangle.Dispose();
        }
    }
}
