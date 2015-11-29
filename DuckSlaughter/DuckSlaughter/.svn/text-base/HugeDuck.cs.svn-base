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
    class HugeDuck : target
    {
        public int points;
        public bool pointsCounted;

        Texture2D godDuck;

        public bool mouthOpen;
        bool changeToOpen;
        int mouthOpenFrame;
        int closedMouthFrame;
        bool midPhase;
        int FrameMouthShotAt;
        public bool mouthOpenAndShot;
        /*
         * Constructor takes in enclosingGame and a Vector2 location which is where it should start on the screen and a speed
         * also takes in an int type and int color which will set what type of duck it is
         */
        public HugeDuck(DuckSlaughterGame enclosingGame, Vector2 location)
        {

            this.enclosingGame = enclosingGame;
            this.location = location;
            this.type = type;
            this.color = color;
            this.isShot = false;
            this.wasShot = false;

            this.mouthOpen = false;

            this.interval = 75;
            this.spriteHeight = spriteHeight = 400;
            this.spriteWidth = spriteWidth = 505;

            godDuck = enclosingGame.contentManager.Load<Texture2D>("duckGodSpriteSheet");

            //do spaceDuck sprite

            points = 300;
            pointsCounted = false;
        }

        // Load necessary content
        public void LoadContent(ContentManager contentManager)
        {
            //Load the sprite textures 
            godDuck = contentManager.Load<Texture2D>("duckGodSpriteSheet");
        }

        /*
         * Update the speed and position of the duck based on time
         */
        public void Update(TimeSpan gameTime, long frameNumber)           //int frameNumber)//
        {

            //Increase the timer by the number of milliseconds since update was last called
            timer += (float)gameTime.TotalMilliseconds;

            //if the duck is shot you have to make it drop down.
            if (this.isShot)
            {
                //this.speed = new Vector2(0.0f, 0.0f);
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
            }
            //have it drop to the bottom
            if (this.wasShot)
            {
                this.speed = new Vector2(0.0f, 100.0f);
            }

            this.location += this.speed * 0.02f;
            //update the location based on what type it is
            //float timeSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //I was using the current time, but a constant has the same effect tried based on framenumber but didn't work to well

            //Check the timer is more than the chosen interval

            if (timer > interval)
            {
                //initail condition mouthed closed get its frame at that moment
                if (!mouthOpen && !midPhase && !changeToOpen && currentFrameForSprite !=0)
                {
                    closedMouthFrame = (int)frameNumber;
                    currentFrameForSprite = 0;

                }
                // if we need to change it to open do it and get its frame and it doesn't need to be changed anymore
                if (changeToOpen)
                {
                    mouthOpen = true;
                    mouthOpenFrame = (int)frameNumber;
                    changeToOpen = false;
                }

                if (this.mouthOpen && this.mouthOpenAndShot)
                {
                    currentFrameForSprite = 3;
                    this.mouthOpenAndShot = false;
                    this.FrameMouthShotAt = (int)frameNumber;
                }
                else if (this.mouthOpen && this.FrameMouthShotAt + 10 <= frameNumber)
                {
                    currentFrameForSprite = 2;
                }

                // if the currentFrame is 1 and we arent' chaning it to open change it to open and move sprite forward
                if (currentFrameForSprite == 1 &&!changeToOpen)
                {
                    currentFrameForSprite = 2;
                    changeToOpen = true;
                }
                // check to see if you hit the mid frame currentFrame = 1 have correct amout of pause
                if (closedMouthFrame + 125 <= frameNumber && !mouthOpen)
                {
                    currentFrameForSprite = 1;
                    midPhase = true;
                }
                // if teh mouthopen and its been open for an large enough pause
                if (mouthOpen && mouthOpenFrame + 250 <= frameNumber)
                {
                    mouthOpen = false;
                    midPhase = false;
                    currentFrameForSprite = 1;
                }

                timer = 0f;
            }
            
            //if it gets shot you only need to show the one frame
            if (this.isShot)
            {
                currentFrameForSprite = 3;
            }
            // if it was shot you only need to show the one frame of it falling
            if (this.wasShot)
            {
                currentFrameForSprite = 4;
            }

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
            spriteBatch.Draw(godDuck, location, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
        }

    }
}
