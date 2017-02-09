using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
            controller = new UIController(this, false);

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
                new UIBase("splash", UIBase.Type.over, "menu", false, 3, "menu",
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
                    ),
                    new UIImage(
                        UIControl.Positioning.Square,
                        UIControl.Origin.BottomLeft,
                        UIControl.Alignment.BottomLeft,
                        Point.Zero,
                        new Size(25),
                        controller.GetColor(Color.SeaGreen)
                    )
                )
            ); 
            controller.Add(
                 new UIBase("buttontarg", UIBase.Type.over, "menu", true, 0, null,
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
                         "Button targ.",
                         Content.Load<SpriteFont>("testfont"),
                         Color.Blue
                     ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.BottomCenter,
                        UIControl.Alignment.BottomCenter,
                        new Point(0, -10),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "none",
                        true,
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopCenter,
                        UIControl.Alignment.TopCenter,
                        new Point(0, 40),
                        new ButtonData(controller.GetColor(Color.Peru), "Cinematic", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(controller.GetColor(Color.Purple), "Cinematic", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "cine",
                        true,
                        new ButtonData(controller.GetColor(Color.LemonChiffon), "Cinematic", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.BottomCenter,
                        UIControl.Alignment.BottomCenter,
                        new Point(0, -5),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "exit",
                        true,
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    )
                 )
             );
            tb1 = new UIVar<string>();
            controller.Add(
                new UIBase("menu", UIBase.Type.over, "menu", true, 0, null,
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
                        Color.Black,
                        UIBGText.TAlign.Center
                    ),
                    new UIBGImg(UIControl.Positioning.Relative, 
                        UIControl.Origin.TopLeft, 
                        UIControl.Alignment.TopLeft, 
                        new Point(10, 10),
                        Content.Load<Texture2D>("testimg"),
                        "text goes here\"",
                        Content.Load<SpriteFont>("testfont"),
                        Color.Violet,
                        0.5f
                    ),
                    new UIButton(
                        UIControl.Positioning.Absolute,
                        UIControl.Origin.MiddleCenter,
                        UIControl.Alignment.MiddleCenter,
                        Point.Zero,
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Button", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Button_hov", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "buttontarg",
                        false,
                        ButtonData.Empty,
                        new UIGVar<bool>()
                    ),
                    new UITextBox(
                        UIControl.Positioning.Absolute,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        new Point(10),
                        new Size(100, 20),
                        controller.GetColor(Color.White),
                        "",
                        Content.Load<SpriteFont>("testfont"),
                        Color.Black,
                        UIBGText.TAlign.Left,
                        true,
                        6,
                        tb1
                    )
                )
            );
            controller.Add(
                new UIBase("cine", UIBase.Type.over, "menu", false, 5, "none",
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        Point.Zero,
                        new Size(100, 50),
                        controller.GetColor(Color.SkyBlue)
                    ),
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        new Point(0, 50),
                        new Size(100, 50),
                        controller.GetColor(Color.ForestGreen)
                    ),
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        Point.Zero,
                        new Size(100, 10),
                        controller.GetColor(Color.Black)
                    ),
                    new UIImage(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopLeft,
                        UIControl.Alignment.TopLeft,
                        new Point(0, 90),
                        new Size(100, 10),
                        controller.GetColor(Color.Black)
                    )
                )
            );
            controller.Add(
                 new UIBase("pausetarg", UIBase.Type.over, "pause", true, 0, null,
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
                         "Pause targ.",
                         Content.Load<SpriteFont>("testfont"),
                         Color.Blue
                     ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.BottomCenter,
                        UIControl.Alignment.BottomCenter,
                        new Point(0, -10),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "none",
                        true,
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Play", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.TopCenter,
                        UIControl.Alignment.TopCenter,
                        new Point(0, 40),
                        new ButtonData(controller.GetColor(Color.Peru), "Red", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(controller.GetColor(Color.Purple), "Red", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "redswitch",
                        true,
                        new ButtonData(controller.GetColor(Color.LemonChiffon), "Red", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    ),
                    new UIButton(
                        UIControl.Positioning.Relative,
                        UIControl.Origin.BottomCenter,
                        UIControl.Alignment.BottomCenter,
                        new Point(0, -5),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.Green),
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.Red),
                        "exit",
                        true,
                        new ButtonData(Content.Load<Texture2D>("testimg"), "Exit", Content.Load<SpriteFont>("testfont"), Color.PaleGoldenrod),
                        new UIGVar<bool>()
                    )
                 )
             );
            controller.Add(
                new UIBase("redswitch", UIBase.Type.over, "red", false, 0, null
                )
            );
            controller.Switchto("splash");
            ostate = Keyboard.GetState();
        }
        UIVar<string> tb1;
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        KeyboardState ostate;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            string olt = controller.Update(gameTime);
            if (olt == "game")
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.Escape) && ostate.IsKeyUp(Keys.Escape)))
                    controller.Switchto("pausetarg");
                //Game code
            }
            else if (olt == "pause")
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (Keyboard.GetState().IsKeyDown(Keys.Escape) && ostate.IsKeyUp(Keys.Escape)))
                    controller.Switchto("none");
                //Pause code
            }
            if (olt == "red")
            {
                redbg = true;
                controller.Switchto("pausetarg");
            }
            ostate = Keyboard.GetState();
            //All code
            if (tb1.Value == "cooL")
            {
                tb1.Value = "";
                controller.Switchto("splash");
            }
            base.Update(gameTime);
        }

        bool redbg = false;
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (redbg)
            {
                GraphicsDevice.Clear(Color.PaleVioletRed);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            sb.Begin();
            controller.Draw(sb);
            sb.End();

            base.Draw(gameTime);
        }
    }
}
