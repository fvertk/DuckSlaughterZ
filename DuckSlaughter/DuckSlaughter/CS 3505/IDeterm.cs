using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/**
 * Contains implementation for GameState class as well as nextInputs
 */
namespace Project7.CS_3505
{
    // Everything in objects built from this class represent the critical game state
    public class GameState
    {
        public long frameNumber;

        private long checksum;
        public long Checksum { get { return checksum; } }

        public bool[] isGameOver;
        public bool[] isTerminated;

        // Since this is not a game, I'll just keep track of the user inputs as the game state.

        public int[] mouseLocationX;
        public int[] mouseLocationY;
        public bool[] mouseButton;
        public List<Keys>[] keysDown;
        public int[] keypressCount;

        public long elapsedTime;

        /* Constructor */
        public GameState()
        {
            frameNumber = 0;
            checksum = 0;

            isGameOver = new bool[4];
            isTerminated = new bool[4];

            mouseLocationX = new int[4];
            mouseLocationY = new int[4];
            mouseButton = new bool[4];
            keysDown = new List<Keys>[4];
            for (int i = 0; i < 4; i++)
                keysDown[i] = new List<Keys>();
            keypressCount = new int[4];

            elapsedTime = 0;

            checksum = 0;
        }

        /* The game engine! */
        public void advanceFrame(NextInputs inputs, long milliseconds)
        {
            // Advance frame number
            frameNumber++;

            // Advance game - for the test harness, just record statistics and input states.

            elapsedTime += milliseconds;

            for (int player = 0; player < 4; player++)
            {
                if (isGameOver[player])
                    continue;

                if (inputs.mousePressedChanged[player])
                    mouseButton[player] = inputs.mousePressed[player];

                if (inputs.mouseLocationChanged[player])
                {
                    mouseLocationX[player] = inputs.mouseLocationX[player];
                    mouseLocationY[player] = inputs.mouseLocationY[player];
                }

                foreach (Keys k in inputs.keysPressed[player])
                    if (!keysDown[player].Contains(k))
                    {
                        keysDown[player].Add(k);
                        keypressCount[player]++;
                    }

                foreach (Keys k in inputs.keysReleased[player])
                        keysDown[player].Remove(k);                 

                // If the mouse was pressed for a certain player, activate game over or terminated states as appropriate

                if (inputs.mousePressed[player])
                    for (int p = 0; p < 4; p++)
                    {
                        int x = 200 * p + 10;
                        int y = 220;

                        if (mouseLocationX[player] >= x && mouseLocationY[player] >= y &&
                            mouseLocationX[player] < x + 25 && mouseLocationY[player] < y + 25)
                        {
                            //isGameOver[p] = true;
                        }
                        y += 25;
                        if (mouseLocationX[player] >= x && mouseLocationY[player] >= y &&
                            mouseLocationX[player] < x + 25 && mouseLocationY[player] < y + 25)
                        {
                            //isGameOver[p] = true;
                            //isTerminated[p] = true;
                        }
                    }

            }
            
        }

    }

    // This class encapsulates inputs from the players.
    public class NextInputs
    {
        public List<Keys>[] keysPressed;
        public List<Keys>[] keysReleased;
        public int[] mouseLocationX;
        public int[] mouseLocationY;
        public bool[] mouseLocationChanged;
        public bool[] mousePressed;
        public bool[] mousePressedChanged;

        public NextInputs()
        {
            keysPressed = new List<Keys>[4];
            keysReleased = new List<Keys>[4];
            mouseLocationX = new int[4];
            mouseLocationY = new int[4];
            mouseLocationChanged = new bool[4];
            mousePressed = new bool[4];
            mousePressedChanged = new bool[4];
            for (int i = 0; i < 4; i++)
                keysPressed[i] = new List<Keys>();
            for (int i = 0; i < 4; i++)
                keysReleased[i] = new List<Keys>();
        }
    }

}
