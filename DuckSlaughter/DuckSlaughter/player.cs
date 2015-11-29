using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using Project7;

/**
 * This object represents a player in a game
 */
namespace CS_3505_Project_06
{
    class player
    {
        public int numberOfFS;
        public int avOWD;
        public int lastFrameNumber;
        public object me;
        IDeterministicGame deterministicGame;
        public Microsoft.Xna.Framework.Net.NetworkGamer networkPlayer;
        public int begFrame;

        public Dictionary<int, frame> framesOfPlayer;

        public player(Microsoft.Xna.Framework.Net.NetworkGamer p, object me, IDeterministicGame game)
        {
            networkPlayer = p;
            deterministicGame = game;
            this.me = me;
            framesOfPlayer = new Dictionary<int, frame>();
            framesOfPlayer.Add(0, new frame());
        }

        /*
         * Updates the game/gui on the current frame number with the data that we already have
         */
        public void update(int frameNum)
        {
            // Console.WriteLine("Player: " + networkPlayer.Gamertag);
            //Console.WriteLine(framesOfPlayer[frameNum].toString());

            if (framesOfPlayer[frameNum].pressedKeys != null)
            {
                foreach (Keys k in framesOfPlayer[frameNum].pressedKeys)
                    deterministicGame.ApplyKeyInput(me, k, true);
            }

            if (framesOfPlayer[frameNum].releasedKeys != null)
            {
                foreach (Keys k in framesOfPlayer[frameNum].releasedKeys)
                    deterministicGame.ApplyKeyInput(me, k, false);
            }

            if (!(framesOfPlayer[frameNum].mouseX == 0 && framesOfPlayer[frameNum].mouseY == 0))
                deterministicGame.ApplyMouseLocationInput(me, framesOfPlayer[frameNum].mouseX, framesOfPlayer[frameNum].mouseY);
            else
            {
                if (frameNum != 0)
                {
                    framesOfPlayer[frameNum].mouseX = framesOfPlayer[frameNum - 1].mouseX;
                    framesOfPlayer[frameNum].mouseY = framesOfPlayer[frameNum - 1].mouseY;
                    deterministicGame.ApplyMouseLocationInput(me, framesOfPlayer[frameNum].mouseX, framesOfPlayer[frameNum].mouseY);
                }
            }

            if (framesOfPlayer[frameNum].mouseChange)
            {
                deterministicGame.ApplyMouseButtonInput(me, framesOfPlayer[frameNum].mouseDown);
                framesOfPlayer[frameNum].mouseChange = false;
            }

        }

        public void removeFrames(int frameNumber)
        {
            for (int i = begFrame; i < frameNumber; i++)
                framesOfPlayer.Remove(i);

            begFrame = frameNumber;
        }

    }
}
