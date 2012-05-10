using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CPUImgProc
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Bitmap imageToProcess;
        private Bitmap[] images;
        Texture2D greenScreen;
        Texture2D backScreen;
        Effect chroma;
        Effect sobel;
        int scrHeight;
        int scrWidth;
        VertexPositionTexture[] vertices;
        Int32 currentTechnique = 0;
        private Int32 currentImage = 0;
        Microsoft.Xna.Framework.Input.KeyboardState previousState = Keyboard.GetState();
        int screenWidth;
        int screenHeight;
        int techniques = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // Set back buffer resolution
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            Window.Title = "CPU Graphics";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
            images = new Bitmap[10];
          //  images[0] = this.Content.Load<Texture2D>("batman");
           // System.IO.Path;
            images[0] = (Bitmap)Image.FromFile("bathtub.jpg");
            images[1] = new Bitmap("Batman Edge Detect Test");
            images[2] = new Bitmap("chromaback");
            images[3] = new Bitmap("greenscreen");
            images[4] = new Bitmap("knights");
            images[5] = new Bitmap("vampire");
            imageToProcess = new Bitmap("bathtub");


            greenScreen = this.Content.Load<Texture2D>("greenscreen");
            backScreen = this.Content.Load<Texture2D>("chromaback");
            //sobel = this.Content.Load<Effect>("Sobel");
            //chroma = this.Content.Load<Effect>("chromakey");

            imageToProcess = chromaKey(images[currentImage]);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

           // if (Keyboard.GetState().IsKeyDown(Keys.Down) && !previousState.IsKeyDown(Keys.Down))
               // currentTechnique = (currentTechnique + 1) % sobel.Techniques.Count;

            //if (Keyboard.GetState().IsKeyDown(Keys.Up) && !previousState.IsKeyDown(Keys.Up))
                //currentTechnique = (currentTechnique + sobel.Techniques.Count - 1) % sobel.Techniques.Count;

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && !previousState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                currentImage = (currentImage + 1) % 10;
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && !previousState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                currentImage = (currentImage + 9) % 10;
            }

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                this.Exit();
                //chromaKey(images[currentImage]);
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
            PictureBox pbox = new PictureBox();
            pbox.Image = imageToProcess;
           
        }

        private Bitmap chromaKey(Bitmap input)
        {
            for(int i = 0; i < input.Width; i++){
                for(int j = 0; j < input.Height; j++){
                    System.Drawing.Color color = input.GetPixel(i,j);
                    int grey = (int)((color.R * 0.30) + (color.G * 0.59) + (color.B * 0.11)) / 3;
                    color = System.Drawing.Color.FromArgb(color.A,grey,grey,grey);
                    imageToProcess.SetPixel(i,j,color);
                }
            }
            return imageToProcess;// imageToProcess;
        }

        private Bitmap blackWhite(Bitmap input) {
            GraphicsDevice.Textures[0] = null;
            for (int i = 0; i < input.Width; i++) {
                for(int j = 0; j < input.Height; j++){
                    System.Drawing.Color color = input.GetPixel(i,j);
                    if (color.R + color.G + color.R > 127)
                    {
                        imageToProcess.SetPixel(i,j,System.Drawing.Color.FromArgb(255, 255, 255));
                    }
                    else {
                        imageToProcess.SetPixel(i,j,System.Drawing.Color.FromArgb(0, 0, 0));
                    }
                }
            }
            return imageToProcess;
        }
    }
}
