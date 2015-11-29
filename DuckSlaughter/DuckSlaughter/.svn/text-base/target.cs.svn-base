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
    /*
     * This is a super class for all targets that are in the game.
     * It's basic info is 
     * ints location, speed, type, color
     * bools isShot, wasShot, etc.
     * 
     */
    class target
    {
        //public bool variable if shot
        public bool isShot;
        //public bool variable if it was shot
        public bool wasShot;
        //A Timer variable
        public float timer = 0f;
        //The interval (100 milliseconds)
        public float interval = 100f;
        //Current frame holder (start at 1)
        public int currentFrameForSprite = 1;
        //Width of a single sprite image, not the whole Sprite Sheet
        public int spriteWidth;
        //Height of a single sprite image, not the whole Sprite Sheet
        public int spriteHeight;
        //A rectangle to store which 'frame' is currently being shown
        public Rectangle sourceRect;
        //The centre of the current 'frame'
        public Vector2 origin;

        public int type;       //0 = toRight, 1 = toLeft, 2 = rightAngle, 3 = leftAngle
        public int color;      //0 = blue, 1 = red, 2 = green

        public int howManyHits;

        public long shotAtFrameNumber;
        public int shotcount = 0;

        public Vector2 location;
        public Vector2 speed;

        //game in which duck is in
        public DuckSlaughterGame enclosingGame;

        public target(DuckSlaughterGame enclosingGame, Vector2 location, Vector2 speed, int type, int color)
        {
            
            this.enclosingGame = enclosingGame;
            this.location = location;
            this.speed = speed;
            this.type = type;
            this.color = color;
            this.isShot = false;
            this.wasShot = false;
        }

        public target()
        {
        }
    }
}
