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

namespace BreakOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Paddle paddle;
        Ball ball;
        Rectangle screenRectangle;

        int bricksWide = 10;
        int bricksHigh = 5;
        Texture2D brickImage;
        Brick[,] bricks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 750;
            graphics.PreferredBackBufferHeight = 600;

            screenRectangle = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
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

            Texture2D tempTexture = Content.Load<Texture2D>("paddle");
            paddle = new Paddle(tempTexture, screenRectangle);

            tempTexture = Content.Load<Texture2D>("ball");
            ball = new Ball(tempTexture, screenRectangle);

            brickImage = Content.Load<Texture2D>("brick");

            StartGame();    
        }

        private void StartGame()
        {
            paddle.SetInStartPosition();
            ball.SetInStartPosition(paddle.GetBounds());

            bricks = new Brick[bricksWide, bricksHigh];

            for (int y = 0; y < bricksHigh; y++)
            {
                Color tint = Color.White;

                switch (y)
                {
                    case 0:
                        tint = Color.Blue;
                        break;
                    case 1:
                        tint = Color.Red;
                        break;
                    case 2:
                        tint = Color.Green;
                        break;
                    case 3:
                        tint = Color.Yellow;
                        break;
                    case 4:
                        tint = Color.Purple;
                        break;
                }

                for (int x = 0; x < bricksWide; x++)
                {
                    bricks[x, y] = new Brick(
                        brickImage,
                        new Rectangle(
                            x * brickImage.Width,
                            y * brickImage.Height,
                            brickImage.Width,
                            brickImage.Height),
                            tint);
                }
            }
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

            paddle.Update();
            ball.Update();

            foreach (Brick brick in bricks)
            {
                brick.CheckCollision(ball);
            }

            ball.PaddleCollision(paddle.GetBounds());

            if (ball.OffBottom())
                StartGame();
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            foreach (Brick brick in bricks)
                brick.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
