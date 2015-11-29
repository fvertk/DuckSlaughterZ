using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

/**
 * A frame object represents one frame for one player and includes and the input of that frame
 * Used for multiplayer support
 */
namespace CS_3505_Project_06
{

    public class frame
    {
        public int frameNumber;
        public List<Keys> pressedKeys;
        public List<Keys> releasedKeys;
        public int mouseX;
        public int mouseY;
        public bool mouseDown;
        public bool mouseChange;
        public byte events;

        public frame()
        {
            this.pressedKeys = new List<Keys>();
            this.releasedKeys = new List<Keys>();

        }
        public frame(int frameNumber, List<Keys> pressedKeys, List<Keys> releasedKeys, int mouseStateX, int mouseStateY, bool buttonPressed, bool mouseChange)
        {
            this.frameNumber = frameNumber;
            this.pressedKeys = pressedKeys;
            this.releasedKeys = releasedKeys;
            this.mouseX = mouseStateX;
            this.mouseY = mouseStateY;
            this.mouseDown = buttonPressed;
            this.mouseChange = mouseChange;

            countEvents();
        }

        public void countEvents()
        {
            events = (byte)(pressedKeys.Count + releasedKeys.Count + 1);

            if (mouseChange)
                events++;
        }

        public String toString()
        {
            String result = "Frame: " + frameNumber + "\n";


            foreach (Keys k in pressedKeys)
            {
                result += "Pressed Key: " + k.ToString() + "\n";
            }
            foreach (Keys k in releasedKeys)
            {
                result += ("Pressed Key: " + k.ToString() + "\n");
            }

            if (mouseChange)
                result += "Mouse Change Up: " + mouseDown;
            return result;
        }

    }
}
