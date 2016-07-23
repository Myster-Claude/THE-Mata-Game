﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using Microsoft.Xna.Framework.Media;
using TrebleSketchGameUtils;

namespace Apocalyptic_Sunrise
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Game1 theGame;
        public DevLogging Debug;
        public Player player;
        public Camera camera;
        public GameStates gameStates;
        Level level;
        HealthBar healthBar;

        string GameVersionBuild;
        private int levelIndex = -1;
        private const int numberOfLevels = 2;

        Texture2D thing;
        Video video;
        VideoPlayer vidplayer;
        Texture2D videoTexture;

        Vector2 spawningPosition;

        public Game1()
        {
            Debug = new DevLogging();
            File.Delete(Debug.GetCurrentDirectory());
            Debug.WriteToFile("This game proudly uses the TrebleSketch Utilities Debugger v6.2", true, false);
            GameVersionBuild = "v0.3.6.126 ";
            DateTime thisDay = DateTime.Now;
            Debug.WriteToFile("Starting Apocalyptic Sunrise " + GameVersionBuild + thisDay.ToString("dd-MM-yyyy HH:mm:ss zzz"), true, false);

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            
            theGame = this;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Debug.WriteToFile("Started Initializing Game", true, false);

            gameStates = new GameStates();
            gameStates.Debug = Debug;
            level = new Level();
            level.Debug = Debug;
            level.gameState = gameStates;
            player = new Player(level.playerSpawningPosition);
           // level.LoadNextMap(Content);
            camera = new Camera();
            healthBar = new HealthBar(Content);
            gameStates.InitGame();

            VideoPlayer vidPlayer = new VideoPlayer();

            //map = new TiledMap(GraphicsDevice, 110, 110, 32, 32, TiledMapOrientation.Orthogonal);
            base.Initialize();

            Debug.WriteToFile("Finished Initializing Game", true, false);
        }

        protected override void LoadContent()
        {
            Debug.WriteToFile("Started Loading Game Textures", true, false);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameStates.LoadContent(Content);
            player.LoadContent(Content);
            gameStates.enemytexture = Content.Load<Texture2D>("DroneSprite");
            gameStates.GameOver = Content.Load<Texture2D>("GameOver");
            gameStates.WinScreen = Content.Load<Texture2D>("WinScreen");

            vidplayer = new VideoPlayer();
            thing = Content.Load<Texture2D>("start");
            video = Content.Load<Video>("MenuBackground");
            

            vidplayer.Play(video);

            level.LoadNextMap(Content);
            blackScreen = Content.Load<Texture2D>("Black screen");
            level.font = Content.Load<SpriteFont>("scoreFont");

            gameStates.Mainframe = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            Debug.WriteToFile("Finished Loading Game Textures", true, false);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Debug.WriteToFile("Ending Game...", true, false);
                Exit();
            }

            if (vidplayer.State != MediaState.Stopped)
            {
                vidplayer.Play(video);
                vidplayer.IsLooped = false;
            }

            gameStates.player = player;
            level.player = player;
            player.level = level;
            player.gameStates = gameStates;
            gameStates.level = level;
            
            camera.graphics = graphics;
            camera.Update(player.sPosition);
            gameStates.healthBar = healthBar;   
            gameStates.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.End))
            {
                level.LoadNextMap(Content);
            }

            base.Update(gameTime);
        }

        public bool isloadingLevel = false;
        Texture2D blackScreen;
        public const float delay = 5;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);           

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, new Matrix?(this.camera.viewMatrix));
            
            if (gameStates.isInMenu == true && gameStates.isGame != true)
            {
                if (vidplayer.State != MediaState.Stopped) // Only call GetTexture if a video is playing or paused
                    videoTexture = vidplayer.GetTexture();

                Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                    GraphicsDevice.Viewport.Y,
                    GraphicsDevice.Viewport.Width,
                    GraphicsDevice.Viewport.Height);
                    // Drawing to the rectangle will stretch the video to fill the screen

                if (videoTexture != null) // Draw the video, if we have a texture to draw.
                {
                    spriteBatch.Draw(videoTexture, screen, Color.White);
                }
            }

            gameStates.Draw(gameTime, spriteBatch);

            spriteBatch.End();


            spriteBatch.Begin();

            if (gameStates.isGame)
            { 
                healthBar.Draw(spriteBatch);
            }
            gameStates.Draw2(gameTime, spriteBatch);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.viewMatrix);
            if (isloadingLevel == true)
            {
                spriteBatch.Draw(blackScreen, new Vector2(player.m_position.X - 300, player.m_position.Y - 0), Color.White);
                level.info(spriteBatch, gameTime);

                var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                level.remainingdelay -= timer;
                if (level.remainingdelay <= 0)
                {
                    isloadingLevel = false;
                    level.remainingdelay = delay;
                    level.LoadNextMap(Content);
                }

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
    