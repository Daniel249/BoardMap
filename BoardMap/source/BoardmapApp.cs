﻿using Microsoft.Xna.Framework;
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

            spriteBatch.Draw(onlyFrame, mapPosition, Color.White);

            spriteBatch.End();
            // sprite end

            base.Draw(gameTime);
        }    


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() { }
    }
}
