using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GPUImgProc
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D imageToProcess;
        Effect sobel;
        int scrHeight;
        int scrWidth;
        VertexPositionTexture[] vertices;
        Int32 currentTechnique = 0;
        KeyboardState previousState = Keyboard.GetState();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // Set back buffer resolution  
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            

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

            imageToProcess = this.Content.Load<Texture2D>("batman");
            sobel = this.Content.Load<Effect>("Sobel");

            vertices = new VertexPositionTexture[4];
            vertices[0].Position = new Vector3(-1, 1, 0);
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].Position = new Vector3(1, 1, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].Position = new Vector3(-1, -1, 0);
            vertices[2].TextureCoordinate = new Vector2(0, 1);
            vertices[3].Position = new Vector3(1, -1, 0);
            vertices[3].TextureCoordinate = new Vector2(1, 1);

            scrHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            scrWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down))
                currentTechnique = (currentTechnique + 1) % sobel.Techniques.Count;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up))
                currentTechnique = (currentTechnique + sobel.Techniques.Count - 1) % sobel.Techniques.Count;

            previousState = Keyboard.GetState();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            DrawImage();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawImage(){
            Rectangle rec = new Rectangle(0, 0, scrWidth, scrHeight);
            
            sobel.Parameters["xRenderedScene"].SetValue(imageToProcess);
            sobel.CurrentTechnique = sobel.Techniques[currentTechnique];

            sobel.CurrentTechnique.Passes[0].Apply();
            GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
            //spriteBatch.Draw(imageToProcess, rec, Color.White);
        }
    }
}
