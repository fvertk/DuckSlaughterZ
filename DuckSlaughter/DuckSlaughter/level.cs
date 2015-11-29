using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuckSlaughter
{
    public class level
    {
        public Rectangle backgroundRect;
        public bool levelTransition;
        public bool winLose;
        public byte levelNumber;
        public bool playerLose;
        public bool gameRestart;

        Texture2D background;
        Texture2D endLevelScoresTexture;
        
        SpriteFont font;

        public int ducksPerCylce;
        public int nextFrameToMakeDucks;
        public int maxSpeed;

        public int count;

        public level(Texture2D backg, Texture2D endLevelScores, SpriteFont levelfont)
        {
            count = 0;
            background = backg;
            endLevelScoresTexture = endLevelScores;

            font = levelfont;

            backgroundRect = new Rectangle(0, 1795, 190, 253);
            levelTransition = true;


            levelNumber = 1;

            ducksPerCylce = 2;
            nextFrameToMakeDucks = 100;
            maxSpeed = 45;
            winLose = false; 
        }

        public void update()
        {
            if (levelTransition)
            {             
                count++;

                //Level one stall
                if (count == 250 && levelNumber % 4 == 1 && backgroundRect.Y > 1000)
                {
                    count = 0;
                    levelTransition = false;
                }

                //Screen stays still for 600 frames to shoot last ducks
                if (count > 600 )
                {
                    backgroundRect.Y--;

                    if (backgroundRect.Y == 1490)
                    {
                        count = 0;
                        levelTransition = false;
                    }

                    if (backgroundRect.Y == 900)
                    {
                        count = 0;
                        levelTransition = false;
                    }

                    if (backgroundRect.Y == 0)
                    {
                        count = 0;
                        levelTransition = false;
                    }
                }
            }
            else
            {
                count++;

                if (count % 1000 == 0)
                {
                    levelTransition = true;
                    levelNumber++;
                    count = 0;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 600, 800), backgroundRect, Color.White);

            if (playerLose == true && count > 600 && levelTransition) 
            {
                spriteBatch.DrawString(font, "You Lose!", new Vector2(65, 320), Color.Black);
                gameRestart = true;
            }
            else if (levelNumber % 4 == 1 && backgroundRect.Y < 1000 && count > 600 && playerLose == false)
                spriteBatch.DrawString(font, "Wave " + (levelNumber-1)/4 + " complete!\nPress 'r' to advance!", new Vector2(150, 320), Color.Black);

            else if (levelNumber % 4 == 1 && backgroundRect.Y < 1000 && count > 600 && playerLose == true)
            {
                spriteBatch.DrawString(font, "You Lose!", new Vector2(65, 320), Color.Black);
            }
            else
            {
                String place = "";
                if (levelNumber % 4 == 1)
                    place = "SPAWNING GROUNDS";
                if (levelNumber % 4 == 2)
                    place = "     AERIAL WARS";
                if (levelNumber % 4 == 3)
                    place = "  DUCKS IN SPACE";
                if (levelNumber % 4 == 0)
                    place = "BOSS: THE DUCK GOD";
                if (levelTransition && (count > 600 || (levelNumber % 4 == 1 && backgroundRect.Y > 1000)))
                {
                    spriteBatch.Draw(endLevelScoresTexture, new Vector2(-10, 260), Color.White);
                    spriteBatch.DrawString(font, "Level " + levelNumber, new Vector2(240, 300), Color.Black);
                    spriteBatch.DrawString(font, place, new Vector2(120, 340), Color.Black);
                }
            }
        }
    }
}
