using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using UIStuff;

namespace UIStuff
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;
        int width = 1920;
        int height = 1080;
        bool fullscreen = false;

        UIController controller;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullscreen;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            controller = new UIController();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(GraphicsDevice);

            //Adds a base which contains one control, an image.
            controller.Add(
                new UIBase("splash", UIBase.Type.over, UIBase.Overlaytype.Menu,
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        Point.Zero,
                        new Size(100, 100),
                        Content.Load<Texture2D>("testimg")
                    ),
                    new UIText(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopCenter,
                        UIControl.Alignment.MiddleCenter,
                        new Point(0, 10),
                        "text that is placed near the top",
                        Content.Load<SpriteFont>("testfont"),
                        Color.Red
                    ),
                    new UIImage(
                        UIControl.Positioning.Square,
                        UIControl.Origin.BottomRight,
                        UIControl.Alignment.BottomRight,
                        Point.Zero,
                        new Size(25),
                        Content.Load<Texture2D>("testimg")
                    )
                )
            );
            controller.Add(
                new UIBase("menu", UIBase.Type.over, UIBase.Overlaytype.Menu,
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        Point.Zero,
                        new Size(100, 100),
                        Content.Load<Texture2D>("testimg")
                    ),
                    new UIText(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.BottomLeft,
                        UIControl.Alignment.BottomLeft,
                        new Point(10, -10),
                        "this is a different menu",
                        Content.Load<SpriteFont>("testfont"),
                        Color.Green
                    ),
                    new UIImage(
                        UIControl.Positioning.Square,
                        UIControl.Origin.BottomCenter,
                        UIControl.Alignment.BottomCenter,
                        Point.Zero,
                        new Size(50),
                        Content.Load<Texture2D>("testimg")
                    ),
                    new UIBGText(UIControl.Positioning.Absolute, 
                        UIControl.Origin.MiddleLeft, 
                        UIControl.Alignment.BottomRight, 
                        new Point(200, -10), 
                        new Size(100), 
                        Content.Load<Texture2D>("testimg"), 
                        "bgtext", 
                        Content.Load<SpriteFont>("testfont"), 
                        Color.Black
                    ),
                    new UIBGImg(UIControl.Positioning.Relative, 
                        UIControl.Origin.TopLeft, 
                        UIControl.Alignment.TopLeft, 
                        new Point(10, 10),
                        Content.Load<Texture2D>("testimg"),
                        "text goes here\"",
                        Content.Load<SpriteFont>("testfont"),
                        Color.Violet
                    )
                )
            );
            controller.Switchto("splash");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }


        bool splashchanged = true;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (gameTime.TotalGameTime.TotalSeconds > 3 && splashchanged)
            {
                controller.Switchto("menu");
                splashchanged = false;
            }

            UIBase.Overlaytype olt = controller.Update();
            if (olt == UIBase.Overlaytype.Game || olt == UIBase.Overlaytype.Running)
            {
                //Pausing Code
            }
            //Non-Pausing Code

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sb.Begin();
            controller.Draw(sb, graphics.GraphicsDevice.Viewport);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
