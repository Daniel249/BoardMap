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
            logSize = new Point(100, 100);
            logColor = Color.White;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            // load font
            onlyFont = Content.Load<SpriteFont>("font");

            // init frps counter
            fpsCounter = new Framerate(5);



            // init Frame
            Vector2 framePosition = new Vector2(0, 0);
            // load map as texture
            Texture2D onlyFrame = Content.Load<Texture2D>("provinces");
            frame = new Frame(onlyFrame, framePosition, spriteBatch);

            landscape = new Landscape(onlyFrame);
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


            // move map
            frame.shiftMap(GraphicsDevice.PresentationParameters.BackBufferWidth, 
                           GraphicsDevice.PresentationParameters.BackBufferHeight);

                        
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
            // draw current mouse hover color
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, 15, 15), 
                frame.getColorFrom(Mouse.GetState().X - (int)frame.Position.X, Mouse.GetState().Y - (int)frame.Position.Y));

            // calc string
            string strong = $" X: {Mouse.GetState().X.ToString()} \n Y: {Mouse.GetState().Y.ToString()}";
            // write mouse coords
            spriteBatch.DrawString(onlyFont, strong, new Vector2(15, 15), Color.Black);



            // draw fpsCounter
            fpsCounter.Update(gameTime.ElapsedGameTime.TotalSeconds);
            spriteBatch.DrawString(onlyFont, fpsCounter.framerate.ToString("F"), new Vector2(1920 - 50, 15), Color.Black);


            // draw numbr of tiles
            spriteBatch.DrawString(onlyFont, landscape.tiles.Count.ToString(), new Vector2(15, 50), Color.Black);

            // sprite end
            spriteBatch.End();
            base.Draw(gameTime);
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
