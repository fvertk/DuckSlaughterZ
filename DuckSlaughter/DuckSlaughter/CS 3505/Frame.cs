using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Project7.CS_3505
{
    public class Frame
    {
        int frameN;

        // Inputs
        public short[] mouseLocationX;
        public short[] mouseLocationY;
        public bool[] mouseButton;
        public List<Keys>[] keysDown;

        // Events list
        public List<Event> eventsList = new List<Event>();

        // Constructor
        public Frame(int readframeN, short[] readMouseLocationX, short[] readMouseLocationY,
                     bool[] readMouseButton, List<Keys>[] readKeysDown,
                     bool[] readLastButtonPressedArray)
        {
            this.frameN = readframeN;

            mouseLocationX = new short[4];
            mouseLocationY = new short[4];
            mouseButton = new bool[4];
            keysDown = new List<Keys>[4];

            for (int i = 0; i < 4; i++)
                keysDown[i] = new List<Keys>();
            
            int playerAmount = mouseLocationX.Length;

            for (int i = 0; i < playerAmount; i++)
            {
                mouseLocationX[i] = readMouseLocationX[i];
                mouseLocationY[i] = readMouseLocationY[i];
                mouseButton[i] = readMouseButton[i];

                keysDown[i] = new List<Keys>(readKeysDown[i]);
            }
        }
    }
}
