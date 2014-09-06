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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace TileBasedGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;

        //AudioEngine audioEngine;
        //WaveBank waveBank;
        //SoundBank soundBank;
        //Cue trackCue;
        public Random rnd { get; private set; }

        //int currentScore = 0;
        public string debugMessage = "";
        SpriteFont scoreFont;

        Texture2D backgroundTexture;
        public int tileSizePrivate = 25; // default 75
        public int numberOfHorizontalTiles = 14 * 3; // default 14
        public int numberOfVerticalTiles = 11 * 3; // default 11
        public int tileSize
        {
            get { return tileSizePrivate; }
        }
        public int gameStep = 0;
        public int numberOfTailBlocks = 2; ///default to around 7
        public Vector2 originalPlayerPosition = new Vector2(0, 0);
        public Vector2 oldPlayerPosition = new Vector2(0, 0);
        public Vector2 newPlayerPosition = new Vector2(0, 0);
        public int timeSinceLastStep = 0;
        public int millisecondsPerStep = 30;

        public enum GameState { Start, InGame, GameOver };
        public GameState currentGameState = GameState.Start;

        int numberLivesRemaining = 3;

        public bool isFoodSpawned = false;

        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    currentGameState = GameState.GameOver;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                }
            }
        }







        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random { };

            graphics.PreferredBackBufferHeight = 825;
            graphics.PreferredBackBufferWidth = 1050;

            graphics.PreferredBackBufferHeight = tileSize * numberOfVerticalTiles;
            graphics.PreferredBackBufferWidth = tileSize * numberOfHorizontalTiles;
            //((Game1)Game).tileSize
        }

        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTexture = Content.Load<Texture2D>(@"Images\background2");

            scoreFont = Content.Load<SpriteFont>(@"fonts\Score");

            //audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            //waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            //soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            // Start the soundtrack audio
            //trackCue = soundBank.GetCue("track");
            //trackCue.Play();

            // Play the start sound
            //soundBank.PlayCue("start");

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {



            // Only perform certain actions based on
            // the current game state
            switch (currentGameState)
            {


                case GameState.Start:
                    //if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentGameState = GameState.InGame;
                        resetGameState();
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;

                    }
                    break;



                case GameState.InGame:

                    timeSinceLastStep += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastStep > millisecondsPerStep)
                    {
                        this.incrementGameStep();
                        timeSinceLastStep = 0;
                    }

                    break;
                case GameState.GameOver:
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {

                        //oldPlayerPosition = new Vector2(100, 100);
                        //newPlayerPosition = new Vector2(200, 200);

                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                        currentGameState = GameState.Start;

                    }

                    break;
            }


            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
              ButtonState.Pressed)
                this.Exit();


            //this helps the audio engine stay in sync to update every frame
           //audioEngine.Update();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {



            switch (currentGameState)
            {
                case GameState.Start:

                    GraphicsDevice.Clear(Color.AliceBlue);

                    // Draw text for intro splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(backgroundTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 0);


                    string text = "HUNGRY BLUE FERRET";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2)),
                        Color.Orange);

                    text = "(Press SPACEBAR to begin)";
                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2) + 50),
                        Color.Orange);

                    spriteBatch.End();
                    break;






                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);

                    spriteBatch.Begin();


                    //// Draw Background
                    spriteBatch.Draw(backgroundTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                    SpriteEffects.None, 0);


                    //// Draw Fonts
                    spriteBatch.DrawString(scoreFont, "Score: " + (numberOfTailBlocks - 2),
                        new Vector2(10, 10), Color.Orange, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);

                    // Draw Debug Text
                    //spriteBatch.DrawString(scoreFont, "Debug Message: " + debugMessage,
                    //    new Vector2(10, 10), Color.Red, 0, Vector2.Zero,
                    //    1, SpriteEffects.None, 1);

                    spriteBatch.End();
                    break;





                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    spriteBatch.Begin();

                    spriteBatch.Draw(backgroundTexture,
                    new Rectangle(0, 0, Window.ClientBounds.Width,
                        Window.ClientBounds.Height), null,
                        Color.White, 0, Vector2.Zero,
                        SpriteEffects.None, 0);



                    string gameover = "Game Over! You Died!";
                    spriteBatch.DrawString(scoreFont, gameover,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(gameover).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(gameover).Y / 2)),
                        Color.Orange);

                    //gameover = "Your score: " + currentScore;
                    //spriteBatch.DrawString(scoreFont, gameover,
                    //    new Vector2((Window.ClientBounds.Width / 2)
                    //    - (scoreFont.MeasureString(gameover).X / 2),
                    //    (Window.ClientBounds.Height / 2)
                    //    - (scoreFont.MeasureString(gameover).Y / 2) + 30),
                    //    Color.SaddleBrown);

                    gameover = "(Press ESCAPE to exit, or ENTER to restart)";
                    spriteBatch.DrawString(scoreFont, gameover,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(gameover).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(gameover).Y / 2) + 60),
                        Color.Orange);

                    spriteBatch.End();

                    break;



            }



            base.Draw(gameTime);
        }

        //public void PlayCue(string cueName)
        //{


        //    Cue playCueObject = soundBank.GetCue(cueName);
        //    playCueObject.Play();


        //}

        //public void AddScore(int score)
        //{
        //    currentScore += score;
        //}

        public void incrementGameStep()
        {
            gameStep++;
        }

        public void setToGameOver()
        {
            //spriteManager.Enabled = false;
            //spriteManager.Visible = false;
            currentGameState = GameState.GameOver;
        }

        public void resetGameState()
        {
            //spriteManager.
            newPlayerPosition = originalPlayerPosition;
            oldPlayerPosition = originalPlayerPosition;
            gameStep = 0;
            numberOfTailBlocks = 2;
            timeSinceLastStep = 0;
            numberLivesRemaining = 3;
            isFoodSpawned = false;
            spriteManager.resetSprites();

        }


    }
}