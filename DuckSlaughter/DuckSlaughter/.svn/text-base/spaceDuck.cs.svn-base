using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace DuckSlaughter
{
    class spaceDuck : target
    {   
        public int points;
        public bool pointsCounted;
        bool up;
        int frame_up;
        int frame_down;
        Texture2D spriteDuckAstronaut;
        public bool frozen;
        int frozenFrame;
        bool frozenHasntBeenSet;
        Vector2 oldSpeed;
        

        float RotationAngle;


        bool beingFrozen;
        /*
         * Constructor takes in enclosingGame and a Vector2 location which is where it should start on the screen and a speed
         * also takes in an int type and int color which will set what type of duck it is
         */
        public spaceDuck(DuckSlaughterGame enclosingGame, Vector2 location, Vector2 speed, int type)
        {

            this.enclosingGame = enclosingGame;
            this.location = location;
            this.speed = speed;
            this.type = type;
            this.isShot = false;
            this.wasShot = false;

            this.up = true;
            this.frame_up = 100000;
            this.frame_up = 100000;
            
            this.spriteHeight = spriteHeight = 400;
            this.spriteWidth = spriteWidth = 505;
            this.frozenHasntBeenSet = true;
            //LoadContent(enclosingGame.contentManager);
            spriteDuckAstronaut = enclosingGame.contentManager.Load<Texture2D>("duckAstronautSpriteSheet");

            //do spaceDuck sprite
            points = 10;
            //pointsCounted = false;
        }

        // Load necessary content
        public void LoadContent(ContentManager contentManager)
        {
            //Load the sprite textures 
            spriteDuckAstronaut = contentManager.Load<Texture2D>("duckAstronautSpriteSheet");
        }

        /*
         * Update the speed and position of the duck based on time
         */
        public void Update(TimeSpan gameTime, long frameNumber)
        {

            //next idea with spinning
            if (this.up && this.frame_down + 25 <= frameNumber)
            {
                this.speed = new Vector2(this.speed.X, (this.speed.Y + 15.0f));
                this.up = false;
                this.frame_up = (int)frameNumber;
            }
            else if (!this.up && (this.frame_up + 25 <= frameNumber))
            {
                this.speed = new Vector2((this.speed.X), (this.speed.Y - 15.0f));
                this.up = true;
                this.frame_down = (int)frameNumber;
            }

            timer += (float)gameTime.TotalMilliseconds;

            //if the duck is shot you have to make it drop down.
            if (this.isShot)
            {
                this.speed = new Vector2(0.0f, 0.0f);
                if (this.shotcount == 0)
                {
                    this.shotAtFrameNumber = frameNumber;
                    this.shotcount++;
                }
            }
            //wait 40 framse to change animation to was shot
            if ((this.shotAtFrameNumber + 50) < frameNumber && this.isShot)
            {
                this.wasShot = true;
            }
            //in space don't drop
            if (this.wasShot)
            {
                this.speed =oldSpeed;
            }


            this.currentFrameForSprite = 0;

            this.location += this.speed * 0.005f;

            if (this.isShot)
            {
                this.currentFrameForSprite = 3;
            }
            if (this.wasShot)
            {
                this.currentFrameForSprite = 4;
            }
            if (this.frozen && this.frozenHasntBeenSet)
            {
                this.oldSpeed = speed;
                this.currentFrameForSprite = 4;
                this.speed = new Vector2(0, 0);
                this.frozenFrame = (int)frameNumber;
                this.frozenHasntBeenSet = false;
                this.beingFrozen = true;
            }
            //if it is being frozen keep it that way
            if (this.beingFrozen)
            {
                this.currentFrameForSprite = 4;
            }
            // once the time is up unfrozen it and put it's speed back to normal
            if (this.frozen && this.frozenFrame + 30 <= frameNumber)
            {
                this.frozen = false;
                frozenHasntBeenSet = true;
                this.currentFrameForSprite = 0;
                this.speed = oldSpeed;
                this.beingFrozen = false;
            }


            //rotation of the sprite
            RotationAngle += .012f;
            float circle = MathHelper.Pi * 2;
            RotationAngle = RotationAngle % circle;


            //helps with the selection of the part of sprite sheet
            sourceRect = new Rectangle(currentFrameForSprite * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);


        }

        /*
         * This draws duck based on the data recieved on the duck.
         * 
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.type == 0 || this.type == 2)
            {
                spriteBatch.Draw(spriteDuckAstronaut, location, sourceRect, Color.White, RotationAngle, origin, .10f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(spriteDuckAstronaut, location, sourceRect, Color.White, RotationAngle, origin, .10f, SpriteEffects.FlipHorizontally, 0);
            }

        }
    }
}
