using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using BoardMap.Graphics;
using BoardMap.Common;
using BoardMap.LandscapeNS;
using System.Globalization;
using System;
using BoardMap.Interface;
using BoardMap.Economy;

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

        // manages all current interface
        UInterface uinterface;

        // input states for comparison
        KeyboardState lastKeyboard;
        MouseState lastMouse;

        // font
        SpriteFont onlyFont;

        // fps counter
        Framerate fpsCounter;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            Mouse.WindowHandle = Window.Handle;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

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

            // load font
            onlyFont = Content.Load<SpriteFont>("font");

            // fps counter
            fpsCounter = new Framerate(5);

            // init uinterface
            uinterface = new UInterface(new Texture2D(GraphicsDevice, 1, 1), onlyFont, spriteBatch);

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

            // init german economy as test
            germany = landscape.searchCountry("GER");
            // init grain and cotton firm in all states
            for (int i = 0; i < germany.states.Count; i++)
            {
                // state ref
                State currentState = germany.states[i];
                // add grain and cotton firms
                new Firm(1, currentState.population.Size / 2, currentState);
                new Firm(2, currentState.population.Size / 2, currentState);
            }
        }
        Country germany;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // esc -> exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // mouse click
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
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

            // run economy every half second
            if (timer > TimeSpan.Zero)
            {
                // pass time from last update
                timer -= gameTime.ElapsedGameTime;
                // check if timer ran out
                if (timer <= TimeSpan.Zero)
                {
                    // run economy
                    germany.runEconomy();
                    // set timer to t - half second
                    timer = new TimeSpan(0, 0, 0, 0, 500);
                }

            }

            base.Update(gameTime);
        }
        TimeSpan timer = new TimeSpan(0, 0, 3);

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(landscape.waterColor);
            // sprite beginn
            spriteBatch.Begin();


            // draw map !
            frame.Draw(spriteBatch);

            // print current tile hover
            Tile tileHover = landscape.searchTile(frame.getColorFrom(
                Mouse.GetState().X, Mouse.GetState().Y));

            // interface draw
            uinterface.DrawInterface(selectedTile, tileHover);


            // Right Corner

            // draw fpsCounter
            fpsCounter.Update(timeSinceLastFrame: gameTime.ElapsedGameTime.TotalSeconds);
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


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
        }
    }
}
