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
    class duck : target
    {
        //Sprite Texture

        Texture2D spriteDuck;

        public int points;
        public bool pointsCounted;

        bool backwards;
        /*
         * Constructor takes in enclosingGame and a Vector2 location which is where it should start on the screen and a speed
         * also takes in an int type and int color which will set what type of duck it is
         */
        public duck(DuckSlaughterGame enclosingGame, Vector2 location, Vector2 speed, int type, int color)
        {

            this.enclosingGame = enclosingGame;
            this.location = location;
            this.speed = speed;
            this.type = type;
            this.color = color;
            this.isShot = false;
            this.wasShot = false;

            this.spriteHeight = spriteHeight = 200;
            this.spriteWidth = spriteWidth = 248;

            this.interval = 50f;


            spriteDuck = enclosingGame.contentManager.Load<Texture2D>("duckSpriteSheetv2");

            points = 5; 
        }

        // Load necessary content
        public void LoadContent(ContentManager contentManager)
        {
            //Load the sprite textures
            spriteDuck = contentManager.Load<Texture2D>("duckSpriteSheetv2");
        }

        /*
         * Update the speed and position of the duck based on time
         */
        public void Update(TimeSpan gameTime, long frameNumber)      
        {


            //Increase the timer by the number of milliseconds since update was last called
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
            //wait 20 framse to change animation to was shot
            if ((this.shotAtFrameNumber + 40) < frameNumber && this.isShot)
            {
                this.wasShot = true;
                this.isShot = false;
            }
            //have it drop to the bottom
            if (this.wasShot)
            {
                this.speed = new Vector2(0.0f, 100.0f);
            }

            //I was using the current time, but a constant has the same effect tried based on framenumber but didn't work to well
            this.location += this.speed * 0.02f;

            //Check the timer is more than the chosen interval
            if (!this.wasShot && timer > interval)// && currentFrameForSprite != 4 && currentFrameForSprite != 5)
            {
                //Show the next frame
                if (!backwards)
                {
                    currentFrameForSprite++;
                }
                else
                {
                    currentFrameForSprite--;
                }

                //Reset the timer
                timer = 0f;
            }
            //If we are on the last frame, reset back to the one before the first frame (because currentframe++ is called next so the next frame will be 1!)
            if (currentFrameForSprite == 5)
            {
                currentFrameForSprite = 4;
                backwards = true;
            }
            // if we count too far back count ahead
            if (currentFrameForSprite == -1)
            {
                currentFrameForSprite = 0;
                backwards = false;
            }

            //if it gets shot you only need to show the one frame
            if (this.isShot)
            {
                currentFrameForSprite = 6;
            }
            // if it was shot you only need to show the four frames of it falling
            if (this.wasShot && timer > interval)
            {
                currentFrameForSprite++;
                timer = 0f;
            }
            if (currentFrameForSprite == 11)
            {
                currentFrameForSprite = 7;

            }

            //the selection of the part of sprite sheet
            sourceRect = new Rectangle(currentFrameForSprite * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

        }

        /*
         * This draws duck based on the data recieved on the duck.
         * 
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (this.type == 0)
                spriteBatch.Draw(spriteDuck, location, sourceRect, Color.White, 0f, origin, .20f, SpriteEffects.None, 0);
            // 1 duck goes to left
            if (this.type == 1)
                spriteBatch.Draw(spriteDuck, location, sourceRect, Color.White, 0f, origin, .20f, SpriteEffects.FlipHorizontally, 0);
            // 2 duck goes right angle
            if (this.type == 2)
                spriteBatch.Draw(spriteDuck, location, sourceRect, Color.White, -.35f, origin, .20f, SpriteEffects.None, 0);
            // 3 duck goes left angle l
            if (this.type == 3)
                spriteBatch.Draw(spriteDuck, location, sourceRect, Color.White, .35f, origin, .20f, SpriteEffects.FlipHorizontally, 0);
        }
    }
}

