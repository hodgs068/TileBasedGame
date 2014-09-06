using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileBasedGame
{

    class UserControlledSprite: Sprite
    {

        //bool downPress = false;
        //bool leftPress = false;
        //bool upPress = false;
        //bool rightPress = false;

        //bool downRelease = false;
        //bool leftRelease = false;
        //bool upRelease = false;
        //bool rightRelease = false;

        public Vector2 initialSpriteDirection = new Vector2(0, 1);

        public Vector2 inputDirection = new Vector2(0, 1);
        public Vector2 prevInputDirection = new Vector2(0, 1);

        KeyboardState oldState;

        KeyboardState originalState  = Keyboard.GetState();

        KeyboardState newState = Keyboard.GetState();

        //public bool hasMovedThisStep = false;
        public int lastMovedDuringStep = -1;
        //public Vector2 initialSpriteDirection = new Vector2(0, 1);
        public Queue<Vector2> moveQueue = new Queue<Vector2>();



        //public Game1 game;



        public UserControlledSprite(Game1 game, Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, String collisionCueName, int scoreValue)
            : base(game, textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, null, 0)
        {
        }

        public UserControlledSprite(Game1 game, Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, String collisionCueName, int scoreValue)
            : base(game, textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, null, 0)
        {
            //this.game = game;
        }




        public Vector2 direction ()
        {
            //get 
            //{
                newState = Keyboard.GetState();

                //Vector2 inputDirection = Vector2.Zero;
                if (newState.IsKeyDown(Keys.Left) && !oldState.IsKeyDown(Keys.Left) && inputDirection.X != 1)
                {
                    //inputDirection.X -= 75;
                    //inputDirection.X -= game.tileSize;
                    inputDirection.X = -1;
                    inputDirection.Y = 0;
                    oldState = newState;
                    //game.incrementGameStep();
                }
                else if (!newState.IsKeyDown(Keys.Left) && oldState.IsKeyDown(Keys.Left))
                    oldState = originalState;

                if (newState.IsKeyDown(Keys.Right) && !oldState.IsKeyDown(Keys.Right) && inputDirection.X != -1)
                {
                    //inputDirection.X += 75;
                    //inputDirection.X += game.tileSize;
                    inputDirection.X = 1;
                    inputDirection.Y = 0;
                    oldState = newState;
                    //game.incrementGameStep();
                }
                else if (!newState.IsKeyDown(Keys.Right) && oldState.IsKeyDown(Keys.Right))
                    oldState = originalState;

                if (newState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up) && inputDirection.Y != 1)
                {
                    //inputDirection.Y -= 75;
                    //inputDirection.Y -= game.tileSize;
                    inputDirection.Y = -1;
                    inputDirection.X = 0;
                    oldState = newState;
                    //game.incrementGameStep();
                }
                else if (!newState.IsKeyDown(Keys.Up) && oldState.IsKeyDown(Keys.Up))
                    oldState = originalState;

                if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down) && inputDirection.Y != -1)
                {
                    //inputDirection.Y += 75;
                    //inputDirection.Y += game.tileSize;
                    inputDirection.Y = 1;
                    inputDirection.X = 0;
                    oldState = newState;
                    //game.incrementGameStep();
                }
                else if (!newState.IsKeyDown(Keys.Down) && oldState.IsKeyDown(Keys.Down))
                    oldState = originalState;

                


//                the best way to implement this is to cache the keyboard/gamepad state from the update statement that just passed.

//KeyboardState oldState;
//...

//var newState = Keyboard.GetState();

//if (newState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
//{
//    // the player just pressed down
//}
//else if (newState.IsKeyDown(Keys.Down) && oldState.IsKeyDown(Keys.Down))
//{
//    // the player is holding the key down
//}
//else if (!newState.IsKeyDown(Keys.Down) && oldState.IsKeyDown(Keys.Down))
//{
//    // the player was holding the key down, but has just let it go
//}

//oldState = newState;
//In your case, you probably only want to move "down" in the first case above, when the key was just pressed.






                //GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                //if (gamePadState.ThumbSticks.Left.X != 0)
                //    inputDirection.X += gamePadState.ThumbSticks.Left.X;
                //if (gamePadState.ThumbSticks.Left.Y != 0)
                //    inputDirection.Y -= gamePadState.ThumbSticks.Left.Y;


                //return inputDirection * speed;

                if (inputDirection != prevInputDirection)
                {
                    moveQueue.Enqueue(inputDirection);
                }
                prevInputDirection = inputDirection;
                return inputDirection;
            //}
        }

        ////Commented out MOUSE support:
        //MouseState prevMouseState;





        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (game.gameStep == 0)
            {
                moveQueue.Enqueue(initialSpriteDirection);
            }

            //position += new Vector2(direction.X * game.tileSize, direction.Y * game.tileSize);

            //if (hasMovedThisStep == false && lastMovedDuringStep != game.gameStep)

            //if (lastMovedDuringStep != game.gameStep)
            //{
            //    position += direction() * game.tileSize;
            //    //direction();
            //    //if (moveQueue.Count > 0)
            //    //{
            //    //    position += moveQueue.Dequeue() * game.tileSize;
            //    //    //hasMovedThisStep = true;
            //    //}
            //    lastMovedDuringStep = game.gameStep;
            //}
            direction();
            if (lastMovedDuringStep != game.gameStep)
            {
                //position += direction() * game.tileSize;
                //direction();
                if (moveQueue.Count > 0)
                {
                    position += moveQueue.Dequeue() * game.tileSize;
                }
                else
                {
                    position += direction() * game.tileSize;
                }
                lastMovedDuringStep = game.gameStep;
            }
            else
            {
                //direction();
            }


            game.newPlayerPosition = this.position;

            ////Commented out MOUSE support:
            //MouseState currMouseState = Mouse.GetState();
            //if (currMouseState.X != prevMouseState.X ||
            //    currMouseState.Y != prevMouseState.Y)
            //{
            //    position = new Vector2(currMouseState.X, currMouseState.Y);
            //}

            //prevMouseState = currMouseState;

            //if (position.X < 0)
            //    position.X = 0;
            //if (position.Y < 0)
            //    position.Y = 0;
            //if (position.X > clientBounds.Width - frameSize.X)
            //    position.X = clientBounds.Width - frameSize.X;
            //if (position.Y > clientBounds.Height - frameSize.Y)
            //    position.Y = clientBounds.Height - frameSize.Y;



            if (position.X < 0)
                game.setToGameOver();
            if (position.Y < 0)
                game.setToGameOver();
            if (position.X > clientBounds.Width - frameSize.X)
                game.setToGameOver();
            if (position.Y > clientBounds.Height - frameSize.Y)
                game.setToGameOver();


            game.debugMessage = "Input Direction: " + inputDirection.ToString();


            base.Update(gameTime, clientBounds);

        }

        public override bool isBlockExpired()
        {
            return false;
        }
        public override bool isBlockNew()
        {
            return false;
        }
    }
}
