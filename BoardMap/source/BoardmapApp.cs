using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BoardMap.Graphics;
using BoardMap.Common;
using BoardMap.LandscapeNS;

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

        KeyboardState lastKeyboard;

         
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
            logSize = new Point(150, 250);
            logColor = Color.White;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            // load font
            onlyFont = Content.Load<SpriteFont>("font");

            // init frps counter
            fpsCounter = new Framerate(5);

            // init keyboard state stored for comparison
            lastKeyboard = Keyboard.GetState();

            // init Frame
            Vector2 framePosition = new Vector2(0, 0);
            // load map as texture
            Texture2D onlyTexture = Content.Load<Texture2D>("provinces");
            frame = new Frame(onlyTexture, framePosition, spriteBatch);
            // set frame reference in tile class
            Tile.setFrame(frame);

            landscape = new Landscape(onlyTexture);
            maxTile = landscape.getmaxTextures();
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
                maxTile = landscape.searchTile(frame.getColorFrom(Mouse.GetState().X - (int)frame.Position.X, Mouse.GetState().Y - (int)frame.Position.Y));
            }

            // move map
            frame.shiftMap(GraphicsDevice.PresentationParameters.BackBufferWidth, 
                           GraphicsDevice.PresentationParameters.BackBufferHeight);
            landscape.changeMapMode(lastKeyboard);
            lastKeyboard = Keyboard.GetState();

                        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            // sprite beginn
            spriteBatch.Begin();


            // draw map !
            frame.Draw(spriteBatch);



            // draw interface

            // draw background for log
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, logSize.X, logSize.Y), logColor);

            // print mouse location
            string strong = $"{Mouse.GetState().X.ToString()}   {Mouse.GetState().Y.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(logPosition.X + 15, logPosition.Y + 20), Color.Black);

            // print current tile hover
            Tile tileHover = landscape.searchTile(frame.getColorFrom(Mouse.GetState().X - (int)frame.Position.X, Mouse.GetState().Y - (int)frame.Position.Y));
            printTileInfo(new Point(logPosition.X, logPosition.Y + 50), tileHover);



            // Tile tile with most textures
            // draw number
            spriteBatch.DrawString(onlyFont, "Count: " + maxTile.textures.Count.ToString(), new Vector2(logPosition.X + 15, 130), Color.Black);
            // print tile
            printTileInfo(new Point(logPosition.X, logPosition.Y + 150), maxTile);
            


            // Right Corner

            // draw fpsCounter
            fpsCounter.Update(gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.DrawString(onlyFont, fpsCounter.framerate.ToString("F"), new Vector2(1920 - 50, 15), Color.Black);

            // draw number of loaded tiles
            spriteBatch.DrawString(onlyFont, landscape.tiles.Count.ToString(), new Vector2(1920 - 50, 45), Color.Black);


            // sprite end
            spriteBatch.End();
            base.Draw(gameTime);
        }

        Tile maxTile;

        void printTileInfo(Point _position, Tile _tile) {
            // print square of tile's color
            spriteBatch.Draw(whiteRectangle, new Rectangle(_position.X, _position.Y, 15, 15), _tile.color);
            
            // print tile position
            string strong = $"X: {_tile.positions[0].X.ToString()} \nY: {_tile.positions[0].Y.ToString()}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(_position.X + 20, _position.Y), Color.Black);

            // print tile rgb value
            strong = $"R: {_tile.color.R} \nG: {_tile.color.G} \nB: {_tile.color.B}";
            spriteBatch.DrawString(onlyFont, strong, new Vector2(_position.X + 100, _position.Y), Color.Black);
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
