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


namespace TileBasedGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {


        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<Sprite> spriteList = new List<Sprite>();
        public Game1 game;

        //public Point prevPlayerPosition;

        //int enemySpawnMinMilliseconds = 1000;
        //int enemySpawnMaxMilliseconds = 2000;
        //int enemyMinSpeed = 2;
        //int enemyMaxSpeed = 6;

        //int nextSpawnTime = 0;


        //int likelihoodAutomated = 75;
        //int likelihoodChasing = 20;
        //int likelihoodevading = 5;  // never used!

        //int automatedSpritePointValue = 10;
        //int chasingSpritePointValue = 20;
        //int evadingSpritePointValue = 0;

        //List<AutomatedSprite> livesList = new List<AutomatedSprite>();

        //int nextSpawnTimeChange = 5000;
        //int timeSinceLastSpawnTimeChange = 0;

        //int powerUpExpiration = 0;





        public SpriteManager(Game1 game)
            : base(game)
        {
            this.game = game;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            //ResetSpawnTime();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here



            //nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            //if (nextSpawnTime < 0)
            //{
            //    SpawnEnemy();
            //    // Reset Spawn Timer
            //    ResetSpawnTime();
            //}

            if (game.gameStep == 0)
            {
                spriteList.Add(
                        new TailBlock(game,
                Game.Content.Load<Texture2D>(@"Images/playerSquare"),
                game.oldPlayerPosition, new Point(((Game1)Game).tileSize, ((Game1)Game).tileSize), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(1, 1), null, 0, game.numberOfTailBlocks));

                game.oldPlayerPosition = game.newPlayerPosition;
            }
            else if (!game.oldPlayerPosition.Equals(game.newPlayerPosition))
            {
                spriteList.Add(
                        new TailBlock(game,
                Game.Content.Load<Texture2D>(@"Images/playerSquare"),
                game.newPlayerPosition, new Point(((Game1)Game).tileSize, ((Game1)Game).tileSize), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(1, 1), null, 0, game.numberOfTailBlocks));

                game.oldPlayerPosition = game.newPlayerPosition;
            }

            if (game.isFoodSpawned == false)
            {
                spriteList.Add(
                        new Food(game,
                Game.Content.Load<Texture2D>(@"Images/Food"),
                getNewFoodPosition(), new Point(((Game1)Game).tileSize, ((Game1)Game).tileSize), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(1, 1), null, 0, 1));

                game.isFoodSpawned = true;
            }


            
            UpdateSprites(gameTime);

            //AdjustSpawnTimes(gameTime);

            //CheckPowerUpExpiration(gameTime);

            //game.oldPlayerPosition = game.newPlayerPosition;

            base.Update(gameTime);
        }

        protected void UpdateSprites(GameTime gameTime)
        {

        //    // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            for (int i = 0; i < spriteList.Count; ++i)
            {
                Sprite s = spriteList[i];
                s.Update(gameTime, Game.Window.ClientBounds);


                if (s is TailBlock)
                {
                    if (s.isBlockExpired())
                    {
                        spriteList.RemoveAt(i);
                    }
                    if (player.GetPosition == s.GetPosition && s.isBlockNew() == false)
                    {
                        game.setToGameOver();
                    }

                }

                if (s is Food)
                {
                    if (player.GetPosition == s.GetPosition)
                    {
                        spriteList.RemoveAt(i);
                        game.isFoodSpawned = false;
                        game.numberOfTailBlocks++;

                    }
                }





                




        //        // Check for collisions
        //        if (s.collisionRect.Intersects(player.collisionRect))
        //        {
        //            // Play collision sound
        //            if (s.collisionCueName != null)
        //                ((Game1)Game).PlayCue(s.collisionCueName);

        //            // If collided with Automated Sprite, 
        //            // remove a life from the player
        //            if (s is AutomatedSprite)
        //            {
        //                if (livesList.Count > 0)
        //                {
        //                    livesList.RemoveAt(livesList.Count - 1);
        //                    --((Game1)Game).NumberLivesRemaining;
        //                }
        //            }
        //            else if (s.collisionCueName == "pluscollision")
        //            {
        //                //Colllided with plus - start plus powerup
        //                powerUpExpiration = 5000;
        //                player.ModifyScale(2);
        //            }
        //            else if (s.collisionCueName == "skullcollision")
        //            {
        //                //Colllided with skull - start skull powerup
        //                powerUpExpiration = 5000;
        //                player.ModifySpeed(.5f);
        //            }

        //            else if (s.collisionCueName == "boltcollision")
        //            {
        //                //Colllided with bolt - start bolt powerup
        //                powerUpExpiration = 5000;
        //                player.ModifySpeed(2);
        //            }




        //            // Remove collided sprite from the game
        //            spriteList.RemoveAt(i);
        //            --i;
        //        }
        //        // Remove out-of-bounds objects
        //        if (s.IsOutOfBounds(Game.Window.ClientBounds))
        //        {
        //            ((Game1)Game).AddScore(spriteList[i].scoreValue);
        //            spriteList.RemoveAt(i);
        //            --i;
        //        }

            }

        //    foreach (Sprite sprite in livesList)
        //        sprite.Update(gameTime, Game.Window.ClientBounds);




        }


        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            player.Draw(gameTime, spriteBatch);

            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            //foreach (Sprite sprite in livesList)
            //    sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }




        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Load the player sprite, remember to change the image if the tile size is changed
            player = new UserControlledSprite(game,
                Game.Content.Load<Texture2D>(@"Images/playerSquare"),
                game.oldPlayerPosition, new Point(((Game1)Game).tileSize, ((Game1)Game).tileSize), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(1, 1), null, 0);


            //for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
            //{
            //    int offset = 10 + i * 40;
            //    livesList.Add(new AutomatedSprite(
            //        Game.Content.Load<Texture2D>(@"images\threerings"),
            //        new Vector2(offset, 35), new Point(75, 75), 10,
            //        new Point(0, 0), new Point(6, 8), Vector2.Zero,
            //        null, 0, .5f));
            //}



            base.LoadContent();
        }

        //private void ResetSpawnTime()
        //{
        //    nextSpawnTime = ((Game1)Game).rnd.Next(
        //        enemySpawnMinMilliseconds,
        //        enemySpawnMaxMilliseconds);
        //}

        //private void SpawnEnemy()
        //{
        //    Vector2 speed = Vector2.Zero;
        //    Vector2 position = Vector2.Zero;

        //    // Default frame size
        //    Point frameSize = new Point(75, 75);

        //    // Randomly choose which side of the screen to place enemy,
        //    // then randomly create a position along that side of the screen
        //    // and randomly choose a speed for the enemy
        //    switch (((Game1)Game).rnd.Next(4))
        //    {
        //        case 0: // LEFT to RIGHT
        //            position = new Vector2(
        //                -frameSize.X, ((Game1)Game).rnd.Next(0,
        //                Game.GraphicsDevice.PresentationParameters.BackBufferHeight
        //                - frameSize.Y));

        //            speed = new Vector2(((Game1)Game).rnd.Next(
        //                enemyMinSpeed,
        //                enemyMaxSpeed), 0);
        //            break;

        //        case 1: // RIGHT to LEFT
        //            position = new
        //                Vector2(
        //                Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
        //                ((Game1)Game).rnd.Next(0,
        //                Game.GraphicsDevice.PresentationParameters.BackBufferHeight
        //                - frameSize.Y));

        //            speed = new Vector2(-((Game1)Game).rnd.Next(
        //               enemyMinSpeed, enemyMaxSpeed), 0);
        //            break;

        //        case 2: // BOTTOM to TOP
        //            position = new Vector2(((Game1)Game).rnd.Next(0,
        //                Game.GraphicsDevice.PresentationParameters.BackBufferWidth
        //                - frameSize.X),
        //                Game.GraphicsDevice.PresentationParameters.BackBufferHeight);

        //            speed = new Vector2(0,
        //               -((Game1)Game).rnd.Next(enemyMinSpeed,
        //               enemyMaxSpeed));
        //            break;

        //        case 3: // TOP to BOTTOM
        //            position = new Vector2(((Game1)Game).rnd.Next(0,
        //                Game.GraphicsDevice.PresentationParameters.BackBufferWidth
        //                - frameSize.X), -frameSize.Y);

        //            speed = new Vector2(0,
        //                ((Game1)Game).rnd.Next(enemyMinSpeed,
        //                enemyMaxSpeed));
        //            break;
        //    }

        //    // Create the sprite
        //    ////////////spriteList.Add(
        //    ////////////    new EvadingSprite(Game.Content.Load<Texture2D>(@"images\skullball"),
        //    ////////////    position, new Point(75, 75), 10, new Point(0, 0),
        //    ////////////    new Point(6, 8), speed, "skullcollision", this, .75f, 150, 0));


        //    // Get random number

        //    int random = ((Game1)Game).rnd.Next(100);
        //    if (random < likelihoodAutomated)
        //    {
        //        //create automatedSprite
        //        if (((Game1)Game).rnd.Next(2) == 0)
        //        {
        //            spriteList.Add(
        //                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\fourblades"),
        //                position, new Point(75, 75), 10, new Point(0, 0),
        //                new Point(6, 8), speed, "fourbladescollision", automatedSpritePointValue));
        //        }
        //        else
        //        {
        //            spriteList.Add(
        //                new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\threeblades"),
        //                position, new Point(75, 75), 10, new Point(0, 0),
        //                new Point(6, 8), speed, "threebladescollision", automatedSpritePointValue));
        //        }


        //    }
        //    else if (random < likelihoodAutomated + likelihoodChasing)
        //    {
        //        //create ChasingSprite
        //        if (((Game1)Game).rnd.Next(2) == 0)
        //        {
        //            spriteList.Add(
        //                new ChasingSprite(Game.Content.Load<Texture2D>(@"images\skullball"),
        //                position, new Point(75, 75), 10, new Point(0, 0),
        //                new Point(6, 8), speed, "skullcollision", this, chasingSpritePointValue));
        //        }
        //        else
        //        {
        //            spriteList.Add(
        //                new ChasingSprite(Game.Content.Load<Texture2D>(@"images\plus"),
        //                position, new Point(75, 75), 10, new Point(0, 0),
        //                new Point(6, 4), speed, "pluscollision", this, chasingSpritePointValue));
        //        }


        //    }
        //    else
        //    {
        //        //create EvadingSprite
        //        spriteList.Add(
        //            new EvadingSprite(Game.Content.Load<Texture2D>(@"images\bolt"),
        //            position, new Point(75, 75), 10, new Point(0, 0),
        //            new Point(6, 8), speed, "boltcollision", this, 0.75f, 150, evadingSpritePointValue));
        //    }


        //}


        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }



        //protected void AdjustSpawnTimes(GameTime gameTime)
        //{
        //    //If the spawn max time is > 500 milliseconds,
        //    // decrease the spawn time if it is time to do 
        //    // so based on the spawn-timeer variable
        //    if (enemySpawnMaxMilliseconds > 500)
        //    {
        //        timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
        //        if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
        //        {
        //            timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
        //            if (enemySpawnMaxMilliseconds > 1000)
        //            {
        //                enemySpawnMaxMilliseconds -= 100;
        //                enemySpawnMinMilliseconds -= 100;
        //            }
        //            else
        //            {
        //                enemySpawnMaxMilliseconds -= 10;
        //                enemySpawnMinMilliseconds -= 10;
        //            }

        //        }
        //    }
        //}



        //protected void CheckPowerUpExpiration(GameTime gameTime)
        //{
        //    // is a power-up active?
        //    if (powerUpExpiration > 0)
        //    {
        //        // Decrement power-up timer
        //        powerUpExpiration -= gameTime.ElapsedGameTime.Milliseconds;
        //        if (powerUpExpiration <= 0)
        //        {
        //            // If power-up timer has expired, end all power-ups
        //            powerUpExpiration = 0;
        //            player.ResetScale();
        //            player.ResetSpeed();
        //        }

        //    }
        //}



        public Vector2 getNewFoodPosition()
        {
            bool isUnoccupied = false;
            Vector2 newFoodPosition = new Vector2(0, 0);
            while (isUnoccupied == false)
            {
                newFoodPosition = new Vector2(((Game1)Game).rnd.Next(0, game.numberOfHorizontalTiles) * game.tileSize,
                                                        ((Game1)Game).rnd.Next(0, game.numberOfVerticalTiles) * game.tileSize);
                bool cleanPass = true;
                foreach (Sprite s in spriteList)
                {
                    if (s.GetPosition == newFoodPosition && s is TailBlock)
                    {
                        cleanPass = false;
                    }
                }
                isUnoccupied = cleanPass;

            }
            //isUnoccupied = false;
            return newFoodPosition;
        }


        public void resetSprites()
        {
            player = new UserControlledSprite(game,
                Game.Content.Load<Texture2D>(@"Images/playerSquare"),
                game.oldPlayerPosition, new Point(((Game1)Game).tileSize, ((Game1)Game).tileSize), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(1, 1), null, 0);



            spriteList = new List<Sprite>();
        }

    }
}
