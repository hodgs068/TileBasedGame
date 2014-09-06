using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileBasedGame
{

    class Food : Sprite
    {

        public int gameStepsRemaining;
        private int gameStepAtCreation;



        public Food(Game1 game, Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, String collisionCueName, int scoreValue, int gameStepsRemaining)
            : base(game, textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, null, 0)
        {
            this.gameStepsRemaining = gameStepsRemaining;
            gameStepAtCreation = game.gameStep;
        }

        public Food(Game1 game, Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, String collisionCueName, int scoreValue, int gameStepsRemaining)
            : base(game, textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, null, 0)
        {
            this.gameStepsRemaining = gameStepsRemaining;
            gameStepAtCreation = game.gameStep;
        }



        //public override Vector2 direction
        //{
        //    get
        //    {
        //        return new Vector2(0, 0);
        //    }
        //}



        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            base.Update(gameTime, clientBounds);

        }


        public override bool isBlockExpired()
        {
            if ((gameStepAtCreation + gameStepsRemaining) > game.gameStep)
                return false;
            else return true;
        }

        public override bool isBlockNew()
        {
            if (gameStepAtCreation == game.gameStep)
                return true;
            else return false;
        }
    }
}
