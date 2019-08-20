using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// using System;
// using Color = Microsoft.Xna.Framework.Color;

namespace BoardMap
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BoardmapApp : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public BoardmapApp()
        {
            graphics = new GraphicsDeviceManager(this);

            // set to fullscreen
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            Content.RootDirectory = "Content"; 
        }

        // System.Drawing.Bitmap provincemap;
        // only image being printed each frame
        Texture2D onlyFrame;
        // top left corner
        Vector2 mapPosition;
        // bitmap array
        ColorData mapData;

        // font
        SpriteFont onlyFont;
        // log position and size
        Point logPosition;
        Point logSize;
        // log empty background
        Texture2D whiteRectangle;
        // log color
        Color logColor;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            //Mouse.WindowHandle = Window.Handle;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            mapPosition = new Vector2(0, 0);
            // load map as texture
            onlyFrame = Content.Load<Texture2D>("provinces");
            // init bitmap to texture size
            // provincemap = new System.Drawing.Bitmap(@"Content\hoi4province.bmp");
            // get data from texture
            mapData = new ColorData(onlyFrame);

            // init log
            logPosition = new Point(0, 0);
            logSize = new Point(100, 100);
            logColor = Color.White;
            whiteRectangle = new Texture2D(GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            // load font
            onlyFont = Content.Load<SpriteFont>("font");
            

            // test 
            // testFrame = new Texture2D(GraphicsDevice, onlyFrame.Width, onlyFrame.Height);
            // testFrame.SetData<Color>(mapData);
            // testFrame = new Texture2D(GraphicsDevice, onlyFrame.Width, onlyFrame.Height);
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // move map
            #region
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Mouse.GetState().X <= 0) {
                if(mapPosition.X < 0) {
                    mapPosition.X += 20;
                } 
            } else if (Keyboard.GetState().IsKeyDown(Keys.Right) || Mouse.GetState().X >= GraphicsDevice.PresentationParameters.BackBufferWidth - 10) {
                if (onlyFrame.Width + mapPosition.X > GraphicsDevice.PresentationParameters.BackBufferWidth) {
                    mapPosition.X -= 20;
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Up) || Mouse.GetState().Y <= 0) {
                if (mapPosition.Y < 0) {
                    mapPosition.Y += 20;
                }
            } else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Mouse.GetState().Y >= GraphicsDevice.PresentationParameters.BackBufferHeight - 10) {
                if (onlyFrame.Height + mapPosition.Y > GraphicsDevice.PresentationParameters.BackBufferHeight) {
                    mapPosition.Y -= 20;
                }
            }
            #endregion


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);          
            // sprite beginn
            spriteBatch.Begin();


            // draw only frame
            spriteBatch.Draw(onlyFrame, mapPosition, Color.White);
            // draw background for log
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, logSize.X, logSize.Y), logColor);
            // draw current mouse hover color
            spriteBatch.Draw(whiteRectangle, new Rectangle(logPosition.X, logPosition.Y, 15, 15), mapData.get(Mouse.GetState().X - (int)mapPosition.X, Mouse.GetState().Y - (int)mapPosition.Y));

            // calc string
            string strong = $" X: {Mouse.GetState().X.ToString()} \n Y: {Mouse.GetState().Y.ToString()}";
            // write mouse coords
            spriteBatch.DrawString(onlyFont, strong, new Vector2(10, 10), Color.Black);



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
