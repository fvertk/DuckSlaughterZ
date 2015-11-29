using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project7.CS_3505
{
    public class Event
    {
        public byte eventID;

        // variable for keyboard event
        public int keyCode;

        // variable for mouse event
        public byte button;

        // variable for mouse movement event
        public short xC;
        public short yC;

        // Constructor for keyboard event
        public Event(byte readEventID, int readKeyCode)
        {
            this.eventID = readEventID;
            this.keyCode = readKeyCode;
        }
        
        // Constructor for mouse event
        public Event(byte readEventID, byte readButton)
        {
            this.eventID = readEventID;
            this.button = readButton;
        }
        
        // Constructor for mouse movement event
        public Event(byte readEventID, short readXC, short readYC)
        {
            this.eventID = readEventID;
            this.xC = readXC;
            this.yC = readYC;
        }
    }
}
