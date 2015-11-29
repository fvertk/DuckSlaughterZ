/**
 * Wesley Simon, Jason Butcher, Zach Adams, Michael Goleniewski
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections;
using Project7;
using Project7.CS_3505;

/**
 * Extends the IDeterministicGame class (to allow multiplayer support) and includes all of its implementation
 * 
 * Also contains the duck slaughter game and all of its implentation including drawing the game (except background) and 
 * updating
 */
namespace DuckSlaughter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DuckSlaughterGame : IDeterministicGame
    {
        public ContentManager contentManager;
        Texture2D crosshair;       

        //IDet info
        GameState state;
        NextInputs inputs;
        Object[] playerIdentifiers;
        int thisPlayerID;

        //an array of ducks
        List<duck> ducks;
        List<spaceDuck> sducks;
        Random random;
        List<HugeDuck> gducks;

        //values that are changed based on what level we are on
        String targetName;
        
        //points variables
        public int cumulPoints;
        public int playerOnePoints;
        public int playerTwoPoints;

        //mouse click variables
        public int presses;
        public int presses1;
        public int presses2;

        //number of duck hits
        public int duckHits;
        public int duckHits1;
        public int duckHits2;

        //cumulative accuracy
        public double cumulAccuracy;
        public double accuracy1;
        public double accuracy2; 

        //sharpshooter mode
        public int P1sharpshooterCount;
        public bool P1inSharpshooterMode;
        public bool hitDuck;

        //sound effects
        SoundEffect rifleSound;

        // GraphicsDeviceManager graphics;
        SpriteFont gameFont;

        //code for mouse
        Texture2D m_mouseTexture;

        //code for scoreboard
        Texture2D scoreboardTexture;

        Vector2 point_left = new Vector2(0, 0);

        public level currentLevel;

        int requiredPoints;
        double requiredAccuracy;


        public int duckCount;
        public double duckAccuracy;

        public DuckSlaughterGame()
        {
            playerIdentifiers = new Object[4];
        }

        // Helper methods

        private int idPlayer(Object playerIdentifier)
        {
            for (int i = 0; i < playerIdentifiers.Length; i++)
                if (playerIdentifiers[i] == playerIdentifier)
                    return i;
            throw new Exception("Illegal player identifier" + playerIdentifier);
        }



        public Vector2 PreferredScreenSize
        {
            get { return new Vector2(800, 600); }
        }

        public int MinimumSupportedPlayers
        {
            get { return 4; }
        }

        public int MaximumSupportedPlayers
        {
            get { return 4; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public void Initialize()
        {
            requiredPoints = 1000;
            requiredAccuracy = .5;

            // I just created a few ducks to pop on the screen at the start
            random = new Random(230948285);

            //ducks = new List<duck>();
            //sducks = new List<spaceDuck>();

            cumulPoints = 0;
            presses = 0;
            duckHits = 0;

            //this will need to be changed based off of the level. Default is 2 ducks every 100 frames with max of 45

            targetName = "Duck";
            //godDuckSet = false;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(ContentManager contentManager)
        {
            ducks = new List<duck>();
            sducks = new List<spaceDuck>();
            gducks = new List<HugeDuck>();
            this.contentManager = contentManager;

            currentLevel = new level(contentManager.Load<Texture2D>("basicBackground"), contentManager.Load<Texture2D>("endLevelScores"), contentManager.Load<SpriteFont>("title"));

            scoreboardTexture = contentManager.Load<Texture2D>("scoreBoard");

            gameFont = contentManager.Load<SpriteFont>("Georgia");
            crosshair = contentManager.Load<Texture2D>("CS 3505/Crosshair");

            rifleSound = contentManager.Load<SoundEffect>("Rifle");

            //load the content for each duck
            foreach (duck d in ducks)
            {
                d.LoadContent(contentManager);
            }
            foreach (spaceDuck sd in sducks)
            {
                sd.LoadContent(contentManager);
            }
            foreach (HugeDuck gd in gducks)
            {
                gd.LoadContent(contentManager);
            }
        }   

        public void ResetGame(Object[] playerIdentifiers, Object thisPlayer)
        {
            if (playerIdentifiers.Length != 4)
                throw new Exception("This game requires four players.");

            // Copy the player identifiers - do not rely on the array parameter not changing.

            for (int i = 0; i < 4; i++)
                this.playerIdentifiers[i] = playerIdentifiers[i];

            // Create new game state and inputs objects.

            state = new GameState();
            inputs = new NextInputs();

            // Record 'this' player.

            this.thisPlayerID = idPlayer(thisPlayer);
        }

        public long CurrentFrameNumber
        {
            get { return state.frameNumber; }
        }

        public long CurrentChecksum
        {
            get { return state.Checksum; }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public void restart()
        {
            currentLevel.backgroundRect = new Rectangle(0, 1795, 190, 253);
            currentLevel.winLose = false;
            currentLevel.levelTransition = true;
            currentLevel.count = 0;

            currentLevel.nextFrameToMakeDucks -= 10;

            requiredPoints += 1000 + currentLevel.levelNumber * 100;

            if(requiredAccuracy < .9)
                requiredAccuracy += .2;

        }

        /**
         * Any update we want to do based on a key press we do here!
         */
        public void ApplyKeyInput(Object playerIdentifier, Keys key, bool isKeyPressed)
        {
            if (key == Keys.R && currentLevel.levelNumber % 4 == 1 && currentLevel.winLose)
                restart();

            int player = idPlayer(playerIdentifier);

            if (isKeyPressed && !inputs.keysPressed[player].Contains(key))
                inputs.keysPressed[player].Add(key);

            if (!isKeyPressed && !inputs.keysReleased[player].Contains(key))
                inputs.keysReleased[player].Add(key);
        }

        public void ApplyMouseLocationInput(Object playerIdentifier, int x, int y)
        {
            int player = idPlayer(playerIdentifier);
            inputs.mouseLocationX[player] = x;
            inputs.mouseLocationY[player] = y;
            inputs.mouseLocationChanged[player] = true;
        }

        /**
         * Any update we want to do based on a mouse button press we do here!
         */
        public void ApplyMouseButtonInput(Object playerIdentifier, bool isButtonPressed)
        {
            if (isButtonPressed && playerIdentifier == "One")
            {
                presses1++;
                presses++;
            }
            else if (isButtonPressed && playerIdentifier == "Two")
            {
                presses2++;
                presses++;
            }
            
            int player = idPlayer(playerIdentifier);
            inputs.mousePressed[player] = isButtonPressed;
            inputs.mousePressedChanged[player] = true;

            if (P1sharpshooterCount > 10)
            {
                P1inSharpshooterMode = true;
            }

            if (isButtonPressed)
            {
                SoundEffectInstance rifleSoundInstance = rifleSound.CreateInstance();
                rifleSoundInstance.Volume = 0.1f;
                rifleSoundInstance.Play();

                foreach (duck d in ducks)
                {
                    if ((int)(inputs.mouseLocationX[player] - d.location.X) < 20 && (int)(inputs.mouseLocationY[player] - d.location.Y) < 20 &&
                        (int)(inputs.mouseLocationX[player] - d.location.X) > -20 && (int)(inputs.mouseLocationY[player] - d.location.Y) > -20 && !d.isShot && !d.wasShot)
                    {
                        duckHits++;
                        d.isShot = true;
                        d.pointsCounted = false;
                        if (playerIdentifier == "One")
                        {
                            P1sharpshooterCount ++;
                            hitDuck = true;
                            duckHits1++;
                        }
                        else if (playerIdentifier == "Two")
                        {
                            duckHits2++; 
                        }
                    }

                    if (d.isShot && !d.pointsCounted)
                    {
                        cumulPoints += d.points;
                        if (playerIdentifier == "One")
                        {
                            playerOnePoints += d.points;
                        }
                        else if (playerIdentifier == "Two")
                        {
                            playerTwoPoints += d.points;
                        }
                        d.pointsCounted = true;
                    }
                }
                foreach (spaceDuck sd in sducks)
                {

                    if ((int)(inputs.mouseLocationX[player] - sd.location.X) < 20 && (int)(inputs.mouseLocationY[player] - sd.location.Y) < 20 &&
                        (int)(inputs.mouseLocationX[player] - sd.location.X) > -20 && (int)(inputs.mouseLocationY[player] - sd.location.Y) > -20&& !sd.isShot && !sd.wasShot)
                    {
                        sd.howManyHits++;
                        if (sd.howManyHits >= 2)
                        {
                            sd.isShot = true;
                            sd.pointsCounted = false;
                            if (playerIdentifier == "One")
                            {
                                P1sharpshooterCount++;
                                hitDuck = true;
                                duckHits1++;
                            }
                            else if (playerIdentifier == "Two")
                            {
                                duckHits2++;
                            }
                            duckHits++;
                        }
                        else
                            sd.frozen = true;
                        //duckHits++;
                    }

                    if (sd.isShot && !sd.pointsCounted)
                    {
                        cumulPoints += sd.points;
                        if (playerIdentifier == "One")
                        {
                            playerOnePoints += sd.points;
                        }
                        else if (playerIdentifier == "Two")
                        {
                            playerTwoPoints += sd.points;
                        }
                        sd.pointsCounted = true;
                    }
                }

                if (hitDuck == false)
                {
                    P1sharpshooterCount = 0;
                    P1inSharpshooterMode = false;
                }
                foreach (HugeDuck gd in gducks)
                {
                    if ((int)(inputs.mouseLocationX[player] - gd.location.X) < 45 && (int)(inputs.mouseLocationY[player] - gd.location.Y -75) < 45 &&
                        (int)(inputs.mouseLocationX[player] - gd.location.X) > -45 && (int)(inputs.mouseLocationY[player] - gd.location.Y -75) > -45&& !gd.isShot && !gd.wasShot)
                    {
                        if (gd.mouthOpen)
                        {
                            gd.mouthOpenAndShot = true;
                            gd.howManyHits++;
                            if (gd.howManyHits >= 30)
                            {
                                gd.isShot = true;
                                gd.pointsCounted = false;

                                currentLevel.levelTransition = true;
                                if (currentLevel.levelNumber % 4 == 0)
                                {
                                    currentLevel.levelNumber++;
                                    currentLevel.count = 0;
                                }
                                duckHits++;
                            }
                            //duckHits++;
                            if (playerIdentifier == "One")
                            {
                                duckHits1++;
                            }
                            else if (playerIdentifier == "Two")
                            {
                                duckHits2++;
                            }
                        }
                    }
                    if (gd.isShot && !gd.pointsCounted)
                    {

                        playerOnePoints += gd.points;
                        playerTwoPoints += gd.points;
                        gd.pointsCounted = true;

                        cumulPoints += gd.points * 2;
                        if (playerIdentifier == "One")
                        {
                            duckHits1++;
                        }
                        else if (playerIdentifier == "Two")
                        {
                            duckHits2++;
                        }
                        gd.pointsCounted = true;

                    }
                }
            }
        }

        public bool IsGameOver(Object playerIdentifier)
        {
            int player = idPlayer(playerIdentifier);
            return state.isGameOver[player];
        }

        public bool IsTerminated(object playerIdentifier)
        {
            int player = idPlayer(playerIdentifier);
            return state.isTerminated[player];
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public long Update(TimeSpan elapsedTime)
        {
            currentLevel.update();

            state.advanceFrame(inputs, elapsedTime.Milliseconds);  // Apply the inputs, advance game state.

            inputs = new NextInputs();  // Start with inputs cleared on the next frame.

            if (presses != 0)
            {
                cumulAccuracy = ((double)duckHits / presses) * 100;
                accuracy1 = ((double)duckHits1 / presses1) * 100;
                accuracy2 = ((double)duckHits2 / presses2) * 100;
                duckAccuracy = ((double)duckHits / duckCount) * 100;
            }

            //Stuff that used to be here is now all done in APPLYMOUSEBUTTONINPUT
            if (!currentLevel.levelTransition)
            {
                //keeps the ducks coming for a set amount fo frames default is 70;
                if (state.frameNumber % currentLevel.nextFrameToMakeDucks == 0)
                {
                    System.Random ran = new System.Random();

                    //creates the number of ducks per cylce 
                    //targetName is by default Duck, expandable to be other names
                    for (int i = 0; i < currentLevel.ducksPerCylce; i++)
                    {

                        if (currentLevel.levelNumber % 4 == 3)
                        {
                            duckCount++;
                            getRandom(currentLevel.maxSpeed, "SpaceDuck");
                        }
                        if (currentLevel.levelNumber % 4 == 1 || currentLevel.levelNumber % 4 == 2 || currentLevel.levelNumber % 4 == 0)
                        {
                            duckCount++;
                            getRandom(currentLevel.maxSpeed, targetName);
                        }
                        
                        if (this.currentLevel.levelNumber % 4 == 0 && gducks.Count < 1)
                        {
                            gducks.Add(new HugeDuck(this, new Vector2(300, 400)));
                            duckCount++;
                        }
                    }
                }

                //increases the amount of ducks to be realeased each set amount
                //this is based off of the frame number
                if (CurrentFrameNumber % 1500 == 0)
                {
                    currentLevel.ducksPerCylce++;
                }
                //since we have a god duck we only want three coming from the sides
                if (currentLevel.levelNumber % 4 == 0)
                {
                    currentLevel.ducksPerCylce = 3 * currentLevel.levelNumber / 4;
                }
                //if a gducks mouth is open and your at a frame divisible by 50 have ducks come out of mouth
                for (int i = 0; i < gducks.Count; i++)
                {
                    if (gducks[i].mouthOpen && state.frameNumber % 50 == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            randomMouthDucks();
                        }
                    }
                }
            }
            

            //update each duck using the current game time
            for (int i = 0; i < ducks.Count; i++)
            {
                ducks[i].Update(elapsedTime, state.frameNumber);
                if (ducks[i].location.Y < 0 || ducks[i].location.Y > 800 || ducks[i].location.X < 0 || ducks[i].location.X > 800)
                {
                    ducks.Remove(ducks[i]);
                }
            }
            
            for (int i = 0; i < sducks.Count; i++)
            {
                sducks[i].Update(elapsedTime, state.frameNumber);
                if (sducks[i].location.Y < 0 || sducks[i].location.Y > 800 || sducks[i].location.X < 0 || sducks[i].location.X > 800)
                {
                    sducks.Remove(sducks[i]);
                }
            }
            for (int i = 0; i < gducks.Count; i++)
            {
                gducks[i].Update(elapsedTime, state.frameNumber);
                if (gducks[i].location.Y < 0 || gducks[i].location.Y > 800 || gducks[i].location.X < 0 || gducks[i].location.X > 800)
                {
                    gducks.Remove(gducks[i]);
                }
            }


            if (cumulPoints >= requiredPoints && cumulAccuracy >= requiredAccuracy) // && cumulAccuracy >= 0.60 && currentLevel.levelNumber == 4 && currentLevel.count > 600)
            {
                currentLevel.winLose = true;
            }
            else
                currentLevel.winLose = false;



            return state.frameNumber;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public long Draw(SpriteBatch spriteBatch)
        {

            currentLevel.draw(spriteBatch);

            spriteBatch.Draw(scoreboardTexture, new Vector2(0, 0), Color.White);

            foreach (HugeDuck gd in gducks)
            {
                gd.Draw(spriteBatch);
            }

            //draw each duck in array
            foreach (duck d in ducks)
            {
                d.Draw(spriteBatch);
            }
            foreach (spaceDuck sd in sducks)
            {
                sd.Draw(spriteBatch);
            }
            //mouse setup
            if (!state.isGameOver[0])
                spriteBatch.Draw(crosshair, new Vector2(state.mouseLocationX[0] - 5, state.mouseLocationY[0] - 5), Color.Turquoise);
            else
                Console.WriteLine("game over");

            if (!state.isGameOver[1])
                spriteBatch.Draw(crosshair, new Vector2(state.mouseLocationX[1] - 5, state.mouseLocationY[1] - 5), Color.Wheat);
            else
                Console.WriteLine("game over");
            

            //Frame information

            spriteBatch.DrawString(gameFont, cumulPoints.ToString(), new Vector2(180, 10), Color.Black);
            spriteBatch.DrawString(gameFont, System.Math.Round(duckAccuracy,1).ToString() + "%", new Vector2(460, 10), Color.Black);


            return state.frameNumber;
        }


        public void getRandom(int MaxSpeed, String type)
        {

            if (type.Equals("Duck"))
            {
                int Type = (int)random.Next(0, 4);
                int Color = (int)random.Next(0, 3);
                int location;

                if (Type == 0 || Type == 1)
                {
                    location = (int)random.Next(50, 750);
                }
                else
                    location = (int)random.Next(380, 755);

                int side = 0;

                if (Type == 1 || Type == 3)
                {
                    side = 600;
                }

                int Speed = (int)random.Next(30, MaxSpeed);

                int speed2 = 0;
                if (Type == 2 || Type == 3)
                    speed2 = -20;

                //increase the speed of a duck as the level goes on
                //due this based on the the frameNumber
                if (CurrentFrameNumber >= 100)
                {
                    Speed += (int)(CurrentFrameNumber / 45);
                    if (Type == 2 || Type == 3)
                        speed2 -= (int)(CurrentFrameNumber / 75);
                }

                Vector2 location_on_map = new Vector2(side, location);
                Vector2 actualSpeed;

                if (Type == 1 || Type == 3)
                    actualSpeed = new Vector2(-Speed, speed2);
                else
                    actualSpeed = new Vector2(Speed, speed2);
                ducks.Add(new duck(this, location_on_map, actualSpeed, Type, Color));
            }

            if (type.Equals("SpaceDuck"))
            {
                int Type = (int)random.Next(0, 4);
                int location;

                if (Type == 0 || Type == 1)
                {
                    location = (int)random.Next(50, 750);
                }
                else
                    location = (int)random.Next(380, 755);

                int side = 0;

                if (Type == 1 || Type == 3)
                {
                    side = 600;
                }

                int Speed = (int)random.Next(30, MaxSpeed);

                int speed2 = 0;
                if (Type == 2 || Type == 3)
                    speed2 = -20;

                //increase the speed of a duck as the level goes on
                //d0 this based on the the frameNumber
                if (CurrentFrameNumber >= 100)
                {
                    Speed += (int)(CurrentFrameNumber / 45);
                    if (Type == 2 || Type == 3)
                        speed2 -= (int)(CurrentFrameNumber / 75);
                }

                Vector2 location_on_map = new Vector2(side, location);
                Vector2 actualSpeed;

                if (Type == 1 || Type == 3)
                    actualSpeed = new Vector2(-Speed, speed2);
                else
                    actualSpeed = new Vector2(Speed, speed2);

                sducks.Add(new spaceDuck(this, location_on_map, actualSpeed, Type));
            }
        }

        public void randomMouthDucks()
        {

            int Type = (int)random.Next(0, 4);
            int Color = (int)random.Next(0, 3);
            Vector2 location = new Vector2(325, 465);

            int Speed = (int)random.Next(30, 50);

            int speed2 = 0;
            if (Type == 2 || Type == 3)
                speed2 = -20;

            Vector2 actualSpeed;

            if (Type == 1 || Type == 3)
                actualSpeed = new Vector2(-Speed, speed2);
            else
                actualSpeed = new Vector2(Speed, speed2);

            ducks.Add(new duck(this, location, actualSpeed, Type, Color));
            duckCount++;
        }


    }
}
