﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Color = Microsoft.Xna.Framework.Color;

#endregion

namespace RaceGame
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RaceGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World world;
        private KeyboardState _oldState;
        private Keys _menuKey;
        private Keys _exitKey;
        private Menu _menu;

        private const int NR_OF_MAPS = 5;
        private const int NR_OF_CARS = 5;
        private const int NR_OF_PAUSE_BUTTONS = 3;
        private const int MAP_INDEX = 0;
        private Map[] _maps;
        private Texture2D[] _cars;
        private int _nr_of_laps = 1;
        private ComputerPlayer computerPlayer;
        //Screen State variables to indicate what is the current screen
        private bool _isGameMenuShowed;

        public RaceGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _menuKey = Keys.P;
            _exitKey = Keys.Escape;

            //Initialize screen size to an ideal resolution for the projector
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            //graphics.IsFullScreen = true;
            graphics.IsFullScreen = false;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _oldState = Keyboard.GetState();

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
            computerPlayer = new ComputerPlayer();
            Texture2D[] pauseButtons = new Texture2D[NR_OF_PAUSE_BUTTONS];
            pauseButtons[0] = Content.Load<Texture2D>("menu_continue");
            pauseButtons[1] = Content.Load<Texture2D>("menu_mainmenu");
            pauseButtons[2] = Content.Load<Texture2D>("menu_exit");

            _menu = new Menu(Content.Load<Texture2D>("transparentBackground"), pauseButtons);

            Texture2D[] mapCollisions = new Texture2D[NR_OF_MAPS];
            Texture2D[] mapBackgrounds = new Texture2D[NR_OF_MAPS];
            Texture2D[] mapForegrounds = new Texture2D[NR_OF_MAPS];
            Bitmap[] bitmaps = new Bitmap[NR_OF_MAPS];
            _maps = new Map[NR_OF_MAPS];
            _cars = new Texture2D[NR_OF_CARS];

            _cars[0] = Content.Load<Texture2D>("car1");
            _cars[1] = Content.Load<Texture2D>("car2");
            _cars[2] = Content.Load<Texture2D>("car3");
            _cars[3] = Content.Load<Texture2D>("car4");
            _cars[4] = Content.Load<Texture2D>("car6");

            mapCollisions[0] = Content.Load<Texture2D>("map1_collision");
            mapCollisions[1] = Content.Load<Texture2D>("map2_collision");
            mapCollisions[2] = Content.Load<Texture2D>("map3_collision");
            mapCollisions[3] = Content.Load<Texture2D>("map4_collision");
            mapCollisions[4] = Content.Load<Texture2D>("map6_collision");

            mapBackgrounds[0] = Content.Load<Texture2D>("map1_background");
            mapBackgrounds[1] = Content.Load<Texture2D>("map2_background");
            mapBackgrounds[2] = Content.Load<Texture2D>("map3_background");
            mapBackgrounds[3] = Content.Load<Texture2D>("map4_background");
            mapBackgrounds[4] = Content.Load<Texture2D>("map6_background");

            mapForegrounds[0] = Content.Load<Texture2D>("map1_foreground1");
            mapForegrounds[1] = Content.Load<Texture2D>("default_foreground");
            mapForegrounds[2] = Content.Load<Texture2D>("default_foreground");
            mapForegrounds[3] = Content.Load<Texture2D>("default_foreground");
            mapForegrounds[4] = Content.Load<Texture2D>("default_foreground");

            for (int i = 0; i < bitmaps.Length; i++)
            {
                MemoryStream stream = new MemoryStream();

                mapCollisions[i].SaveAsPng(stream, mapCollisions[i].Bounds.Width, mapCollisions[i].Bounds.Height);
                bitmaps[i] = new Bitmap(stream);
            }

            for (int i = 0; i < _maps.Length; i++)
            {
                _maps[i] = new Map(mapBackgrounds[i], mapForegrounds[i], bitmaps[i], Content.Load<Texture2D>("clouds"), _nr_of_laps, 80, 270, 8.0f);
            }

            List<Player> players = new List<Player>();
            Player player1 = new Player(new Control(Keys.W, Keys.S, Keys.A, Keys.D), _cars[0], new Vector2(_maps[MAP_INDEX].StartX, _maps[MAP_INDEX].StartY), _maps[MAP_INDEX].StartRotation);
            Player player2 = new Player(new Control(Keys.Up, Keys.Down, Keys.Left, Keys.Right), _cars[1], new Vector2(_maps[MAP_INDEX].StartX, _maps[MAP_INDEX].StartY), _maps[MAP_INDEX].StartRotation);
            Player player3 = new Player(new Control(Keys.PageDown, Keys.PageDown, Keys.PageDown, Keys.PageDown), _cars[3], new Vector2(_maps[MAP_INDEX].StartX, _maps[MAP_INDEX].StartY), _maps[MAP_INDEX].StartRotation);
            Player player4 = new Player(new Control(Keys.PageDown, Keys.PageDown, Keys.PageDown, Keys.PageDown), _cars[4], new Vector2(_maps[MAP_INDEX].StartX, _maps[MAP_INDEX].StartY), _maps[MAP_INDEX].StartRotation);
            players.Add(player1);
            players.Add(player2);
            players.Add(player3);
            players.Add(player4);
            computerPlayer.Players.Add(player3);
            computerPlayer.Players.Add(player4);
            world = new World(_maps[MAP_INDEX], players, Content.Load<SpriteFont>("spritefont1"), Content.Load<Texture2D>("HUD"));
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
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(_exitKey))
                this.Exit();

            if (newState.IsKeyDown(_menuKey))
            {
                if (_isGameMenuShowed && _oldState.IsKeyUp(_menuKey))
                {
                    _isGameMenuShowed = false;
                }
                else if (_oldState.IsKeyUp(_menuKey))
                {
                    _isGameMenuShowed = true;
                }
            }

            if (!_isGameMenuShowed)
            {
                foreach (Player player in world.Players)
                {
                    if (newState.IsKeyDown(player.Control.Accelerate))
                    {
                        player.Car.Accelerate();
                    }
                    if (newState.IsKeyDown(player.Control.Decelearte))
                    {
                        player.Car.Break();
                    }
                    if (newState.IsKeyDown(player.Control.Left))
                    {
                        player.Car.TurnLeft();
                    }
                    if (newState.IsKeyDown(player.Control.Right))
                    {
                        player.Car.TurnRight();
                    }

                }
                computerPlayer.Update();
                world.Update();
            }
            else
            {
                if (newState.IsKeyDown(Keys.Up))
                {
                    if (_oldState.IsKeyUp(Keys.Up))
                    {
                        _menu.ScrollUp();
                    }
                }
                if (newState.IsKeyDown(Keys.Down))
                {
                    if (_oldState.IsKeyUp(Keys.Down))
                    {
                        _menu.ScrollDown();
                    }
                }
                if (newState.IsKeyDown(Keys.Enter))
                {
                    if (_oldState.IsKeyUp(Keys.Enter))
                    {
                        switch (_menu.Index)
                        {
                            case (int)PauseMenuItems.Continue:
                                _isGameMenuShowed = false;
                                break;
                            case (int)PauseMenuItems.MainMenu:
                                //go to main menu
                                break;
                            case (int)PauseMenuItems.Exit:
                                this.Exit();
                                break;
                        }
                    }
                }
            }

            if (world.Winner != null)
            {
                this.Exit();
                //Spelet är över... spara tiden till highscoren och celebrate
            }

            _oldState = newState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            world.Draw(spriteBatch);

            if (_isGameMenuShowed)
            {
                _menu.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
