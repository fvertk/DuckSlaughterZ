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
using Project7;
using DuckSlaughter;
using Project07Game;
using Project07Game.Components;

/**
 * Contains implementation (update and draw) for the initial menu, lobby, waiting room, and all network 
 * communications including sending and recieving packets
 * 
 */
namespace CS_3505_Project_06
{
    /// <summary>
    /// A game outline for testing network communications
    /// </summary>
    public class Game06 : Microsoft.Xna.Framework.Game
    {
        MusicControl musicControl;
        SoundEffect youLoseSound;

        //Menu System
        Start startS;
        Help helpS;
        Screen activeS;
        Texture2D back;
        bool inMenu;
        SpriteFont normalF;
        SpriteFont titleF;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont lobbyFont;

        Texture2D background_lobby;

        Rectangle mainFrame;

        DuckSlaughterGame deterministicGame;
        TimeSpan targetTimeSpan;
        Object[] playerIdentifiers = { "One", "Two", "Three", "Four" };  // Any objects will do, strings are easy to debug.

        // For debugging

        List<Keys> lastPressedKeys;
        bool lastButtonPressed;
        Object activePlayer;
        bool paused;


        // Added
        NetworkSession networkSession;
        const int maxPlayers = 2;
        AvailableNetworkSessionCollection games;
        bool gameStarted;
        bool isHost;
        IAsyncResult KeyboardResult; //somewhere accessible 
        PacketReader reader;
        PacketWriter writer;
        String chatText;
        int chatlines;
        int lrFrame;
        int previousLRF;
        int secondLRF;

        bool isChatting;
        int[] pid;
        GamerCollection<NetworkGamer> players;
        int frameNumber;
        short owd;
        int latency;
        short stallCounter;
        bool mouseChange;
        bool buttonPressed;
        object myIdentifier;
        player myPlayer;
        player[] others;
        bool update;
        List<Keys> releasedKeys;

        public Dictionary<int, frame> frames;

        // Constructor
        public Game06()
        {
            musicControl = new MusicControl();


            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Make the game object.  The game is currently called 'DuckSlaughterGame'.

            deterministicGame = new DuckSlaughterGame();

            // Debugging setup

            lastPressedKeys = new List<Keys>();
            activePlayer = playerIdentifiers[0];
            paused = false;
            gameStarted = false;


            //Added
            Components.Add(new GamerServicesComponent(this));
            isHost = false;
            chatText = "";
            reader = new PacketReader();
            writer = new PacketWriter();
            chatlines = 0;
            isChatting = false;
            mouseChange = false;
            buttonPressed = false;
            others = new player[4];
            update = true;

            inMenu = true;
            releasedKeys = new List<Keys>();


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Start music
            musicControl.Initialize(Content);
            musicControl.Play();

            //Added

            networkSession = null;

            // Set a fixed time span of 1/60th of a second.

            targetTimeSpan = new TimeSpan(166666); // In 100 nanosecond units = 16 666 600 nanoseconds
            IsFixedTimeStep = true;
            TargetElapsedTime = targetTimeSpan;

            // Reset the game - indicate that player #1 (player 0) owns this instance of the game.

            deterministicGame.ResetGame(playerIdentifiers, playerIdentifiers[0]);

            // For debugging - reset the mouse position to the center of the window.

            Mouse.SetPosition(400, 300);

            // Allow the base class to initialize.

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Menu
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            normalF = Content.Load<SpriteFont>("normal");
            titleF = Content.Load<SpriteFont>("title");

            back = Content.Load<Texture2D>("d1");
            startS = new Start(this, titleF, back, normalF);
            Components.Add(startS);

            back = Content.Load<Texture2D>("d4");
            helpS = new Help(this, back, titleF, normalF);
            Components.Add(helpS);

            youLoseSound = Content.Load<SoundEffect>("ds_lost");


            startS.Show();
            helpS.Hide();
            activeS = startS;



            //Background
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


            // Let the game load its content.

            background_lobby = Content.Load<Texture2D>("d3");

            font = Content.Load<SpriteFont>("InstructionFont");
            lobbyFont = Content.Load<SpriteFont>("Georgia");

            deterministicGame.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            deterministicGame.UnloadContent();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //IN BEG. MENU
            if (inMenu)
            {
                // Get user's input state.

                KeyboardState keyState = Keyboard.GetState();
                MouseState mouseState = Mouse.GetState();

                // Make a list of the keys pressed or released this frame.

                List<Keys> pressedKeys = new List<Keys>();
                releasedKeys = new List<Keys>();

                mouseChange = false;

                Keys[] pressedKeysArray = keyState.GetPressedKeys();
                foreach (Keys k in pressedKeysArray)
                {
                    if (!lastPressedKeys.Contains(k))
                        pressedKeys.Add(k);
                    else
                        lastPressedKeys.Remove(k);
                }

                releasedKeys = lastPressedKeys;


                lastPressedKeys = new List<Keys>(pressedKeysArray);

                //Act on the input depending on which screen we are on
                if (activeS == startS)
                {
                    
                    SSInput();
                }
                else if (activeS == helpS)
                {
                    HSInput();
                }
            }
            else
            {
                //IN LOBBY
                if (networkSession == null)
                {
                    //if we are not in game - in lobby
                    UpdateLobby();
                }
                else //update game
                {
                    networkSession.Update();

                    //IN GAME
                    if (gameStarted)
                    {
                        if (musicControl.gameplayStarted == false)
                        {
                            musicControl.changeToGameplayMusic();
                            musicControl.gameplayStarted = true;
                        }

                        musicControl.Update();

                        if (deterministicGame.duckAccuracy < 60)
                        {
                            deterministicGame.currentLevel.playerLose = true;
                        }
                        else
                        {
                            deterministicGame.currentLevel.playerLose = false;
                        }

                        if (deterministicGame.currentLevel.gameRestart == true)
                        {
                            restart();
                            return;
                        }

                        // Get user's input state.

                        KeyboardState keyState = Keyboard.GetState();
                        MouseState mouseState = Mouse.GetState();

                        // Make a list of the keys pressed or released this frame.

                        List<Keys> pressedKeys = new List<Keys>();
                        releasedKeys = new List<Keys>();

                        mouseChange = false;

                        Keys[] pressedKeysArray = keyState.GetPressedKeys();
                        foreach (Keys k in pressedKeysArray)
                        {
                            if (!lastPressedKeys.Contains(k))
                                pressedKeys.Add(k);
                            else
                                lastPressedKeys.Remove(k);
                        }

                        releasedKeys = lastPressedKeys;
                        lastPressedKeys = new List<Keys>(pressedKeysArray);

                        // Get mouse button state.
                        if (buttonPressed != (mouseState.LeftButton == ButtonState.Pressed))
                        {
                            buttonPressed = mouseState.LeftButton == ButtonState.Pressed;
                            mouseChange = true;
                        }

                        /***** Begining of game logic. *****/

                        // Debug - allow user to exit.

                        if (pressedKeys.Contains(Keys.Escape))
                            this.Exit();

                        // Debug - allow user on this machine to direct input to any player's state in the game.

                        if (pressedKeys.Contains(Keys.F1)) activePlayer = playerIdentifiers[0];
                        if (pressedKeys.Contains(Keys.F2)) activePlayer = playerIdentifiers[1];
                        if (pressedKeys.Contains(Keys.F3)) activePlayer = playerIdentifiers[2];
                        if (pressedKeys.Contains(Keys.F4)) activePlayer = playerIdentifiers[3];

                        // Debug - allow user on this machine to pause/resume game state advances.


                        // Debug - automatically pause every 1000 frames.


                        // Game update

                        // Direct inputs to the game engine - only report changes.


                        lastButtonPressed = buttonPressed;
                        if (!frames.ContainsKey(frameNumber))
                        {
                            frame newFrame = new frame(frameNumber, pressedKeys, releasedKeys, mouseState.X, mouseState.Y, buttonPressed, mouseChange);
                            frames.Add(frameNumber, newFrame);
                        }

                        if (!myPlayer.framesOfPlayer.ContainsKey(frameNumber + latency))
                        {
                            myPlayer.framesOfPlayer.Add(frameNumber + latency, new frame());
                            myPlayer.lastFrameNumber = frameNumber + latency;
                        }

                        networkCommunications();


                        if (!paused && deterministicGame.CurrentFrameNumber <= frameNumber)
                        {
                            // Advance the game engine.

                            deterministicGame.Update(targetTimeSpan);
                        }

                        /***** End of game logic. *****/
                    }
                    //IN WAITING ROOM
                    else
                        UpdateWR();
                    // Allow the superclass to do any needed updates (unknown purpose).
                }
            }

            base.Update(gameTime);

        }

        /**
         * Start Screen input 
         */
        private void SSInput()
        {
            if (releasedKeys.Contains(Keys.Enter))
            {
                switch (startS.selectIndex)
                {
                    case 0:
                        inMenu = false;
                        CreateGame();
                        beginGame();
                        break;
                    case 1:
                        inMenu = false;
                        activeS.Hide();
                        //activeS.Show();
                        break;
                    case 2:
                        activeS.Hide();
                        activeS = helpS;
                        activeS.Show();
                        break;
                    case 3:
                        Exit();
                        break;
                }
            }
        }

        private void HSInput()
        {
            if (releasedKeys.Contains(Keys.Space) || releasedKeys.Contains(Keys.Enter) || releasedKeys.Contains(Keys.Escape))
            {
                activeS.Hide();
                activeS = startS;
                activeS.Show();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //IN MENU
            if (inMenu)
            {

                GraphicsDevice.Clear(Color.CornflowerBlue);

                // TODO: Add your drawing code here
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

                //spriteBatch.Draw(crosshair, new Vector2(Mouse.GetState().X - 5, Mouse.GetState().Y - 5), Color.White);
                base.Draw(gameTime);
                //activeS.Draw(gameTime);
                spriteBatch.End();
            }
            else
            {
                //IN LOBBY
                if (networkSession == null)
                {
                    GraphicsDevice.Clear(new Color(16, 16, 16, 255));
                    // If we are in the lobby
                    DrawLobby();
                }

                //IN GAME
                else //Draw the game
                {
                    if (gameStarted)
                    {
                        GraphicsDevice.Clear(Color.CornflowerBlue);

                        spriteBatch.Begin();

                        
                        // Draw a few instructions.

                        if (paused && gameTime.TotalRealTime.Milliseconds < 500)
                            spriteBatch.DrawString(font, "-=> Paused <=-", new Vector2(10, 130), Color.White);

                        // Let the game draw itself.

                        deterministicGame.Draw(spriteBatch);

                        
                        //Begin transition music. 
                        if (deterministicGame.currentLevel.levelTransition && deterministicGame.currentLevel.count > 600)
                        {
                            musicControl.changeToAwesomeMusic();
                        }
                        if (musicControl.gameplayStarted &&
                            !deterministicGame.currentLevel.levelTransition)
                        {
                            musicControl.changeToGameplayMusic();
                        }

                        //Draw level stats
                        if (deterministicGame.currentLevel.levelTransition && deterministicGame.currentLevel.count > 600)
                        {
                            spriteBatch.DrawString(lobbyFont, "CURRENT STATS", new Vector2(220, 415), Color.Black);

                            string players = this.players[0].ToString().ToUpper();
                            string points  = "POINTS:        " + deterministicGame.playerOnePoints;
                            

                            if (this.players.Count == 2)
                            {
                                
                                players += "                         " + this.players[1].ToString().ToUpper();
                                points += "                                      " + deterministicGame.playerTwoPoints;
                                
                            }

                            spriteBatch.DrawString(lobbyFont, players, new Vector2(110, 440), Color.Black);
                            spriteBatch.DrawString(lobbyFont, points, new Vector2(50, 465), Color.Black);
                        }


                        spriteBatch.End();
                    }
                    //IN Waiting Room
                    else
                        DrawWR();
                }

                //base.Draw(gameTime);
            }

        }

        void restart()
        {
            youLoseSound.Play();
            deterministicGame.currentLevel.gameRestart = false;

            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();


            musicControl.Restart();


            // Make the game object.  The game is currently called 'DuckSlaughterGame'
            deterministicGame = new DuckSlaughterGame();
            deterministicGame.ResetGame(playerIdentifiers, playerIdentifiers[0]);
            deterministicGame.LoadContent(Content);
            deterministicGame.Initialize();

            // Debugging setup
            lastPressedKeys = new List<Keys>();
            activePlayer = playerIdentifiers[0];
            paused = false;
            gameStarted = false;


            //Added
            isHost = false;
            chatText = "";

            reader.Close();
            writer.Close();

            networkSession.Dispose();

            networkSession = null;

            reader = new PacketReader();
            writer = new PacketWriter();
            chatlines = 0;
            isChatting = false;
            mouseChange = false;
            buttonPressed = false;
            others = new player[4];
            update = true;
            gameStarted = false;

            inMenu = true;
            startS.Show();
            helpS.Hide();
            activeS = startS;
            releasedKeys = new List<Keys>();
        }


       #region MultiplayerSupport
        /*
         * Draws the lobby screen
         */
        void DrawLobby()
        {
            string message = "A = Host Game\n" +
                       "\nGames( 'r' to refresh )";
            string players = "Players\n";
            if (Gamer.SignedInGamers.Count != 0 && games == null)
            {
                // Search for Games
                games = NetworkSession.Find(NetworkSessionType.SystemLink, 1, null);
            }
            if (games != null)
            {
                for (int i = 0; i < games.Count(); i++)
                {

                    message += "\n" + i + ") " + games[i].HostGamertag + "'s Game";
                    players += games[i].CurrentGamerCount.ToString() + "\n";
                }
            }

            spriteBatch.Begin();

            spriteBatch.Draw(background_lobby, mainFrame, Color.White);

            spriteBatch.DrawString(lobbyFont, players, new Vector2(500, 270), Color.Black);
            spriteBatch.DrawString(lobbyFont, "Lobby", new Vector2(100, 250), Color.Black);
            spriteBatch.DrawString(lobbyFont, message, new Vector2(100, 270), Color.Black);

            spriteBatch.End();
        }


        /*
         * Draws the waiting room
         */
        void DrawWR()
        {
            GraphicsDevice.Clear(new Color(16, 16, 16, 255));

            string message = "Waiting for players to join game...\n\nCurrent Players:\n";
            String status = "Status (Press 'r')\n";
            GamerCollection<NetworkGamer> players = networkSession.AllGamers;
            for (int i = 0; i < players.Count; i++)
            {
                message += players[i].ToString() + "\n";
                if (players[i].IsReady)
                    status += "Ready" + "\n";
                else
                    status += "Not Ready" + "\n";
            }
            if (isHost)
                message += "\nPress 's' to force start game";

            spriteBatch.Begin();

            spriteBatch.DrawString(lobbyFont, "Press 't' to talk", new Vector2(160, 430), Color.White);
            spriteBatch.DrawString(lobbyFont, status, new Vector2(350, 207), Color.White);
            spriteBatch.DrawString(lobbyFont, message, new Vector2(160, 160), Color.White);
            spriteBatch.DrawString(lobbyFont, chatText, new Vector2(160, 460), Color.White);

            spriteBatch.End();
        }

        /*
         * Updates the wating room
         */
        void UpdateWR()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (networkSession.SessionState == NetworkSessionState.Playing)
                beginGame();


            //startgame if host
            if (keyState.IsKeyDown(Keys.S) && isHost && !isChatting)
                beginGame();
            //chat
            if (keyState.IsKeyDown(Keys.T) && !isChatting)
                chat();
            if (keyState.IsKeyDown(Keys.R) && !isChatting)
                networkSession.LocalGamers[0].IsReady = true;

            if (isHost)
            {
                bool start = true;
                GamerCollection<NetworkGamer> players = networkSession.AllGamers;
                for (int i = 0; i < players.Count; i++)
                {
                    if (!players[i].IsReady)
                    {
                        start = false;
                    }
                }
                if (start)
                    beginGame();
            }

            recievePacket();

            String inputString;
            if (KeyboardResult != null && KeyboardResult.IsCompleted)
            {
                inputString = Guide.EndShowKeyboardInput(KeyboardResult);
                if (inputString == null)
                    inputString = "";
                KeyboardResult = null;

                sendChatPacket(inputString);
            }
        }

        /*
         * Called when game should begin
         */
        void beginGame()
        {

            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 600;
            graphics.ApplyChanges();

            deterministicGame.Initialize();
            latency = 1;
            lrFrame = 1;
            previousLRF = 0;
            secondLRF = 0;
            frames = new Dictionary<int, frame>();

            frameNumber = 0;
            stallCounter = 0;
            gameStarted = true;
            if (isHost)
                networkSession.StartGame();

            players = networkSession.AllGamers;
            pid = new int[4];
            myIdentifier = getIdentifier(networkSession.LocalGamers[0].Id);

            others = new player[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                pid[i] = (int)players[i].Id;

                others[i] = new player(players[i], getIdentifier(players[i].Id), deterministicGame);
            }

            Array.Sort(pid);

            myPlayer = null;
            for (int i = 0; i < others.Length; i++)
            {
                if (others[i].me == myIdentifier)
                    myPlayer = others[i];
            }

        }
        /**
         * Deals with the network communications
         */
        void networkCommunications()
        {
            recievePacket();

            if (!paused)
            {
                update = true;
                for (int i = 0; i < others.Length; i++)
                {
                    if (!others[i].framesOfPlayer.ContainsKey(frameNumber) && others[i] != myPlayer)
                        update = false;
                }
                if (update)
                {
                    sendGamePacket(false);
                    for (int i = 0; i < others.Length; i++)
                        others[i].update(frameNumber);

                    if (frameNumber == lrFrame)
                        readjustLatency();
                    frameNumber++;
                }
                else
                    stall();
            }
        }


        /**
         * Deals with the stall
         */
        void stall()
        {
            stallCounter++;


            if (stallCounter == 1)
            {
                //Send my events to quiet client reliably
                sendGamePacket(true);

            }
            if (stallCounter == 1 || stallCounter % 60 == 0)
            {
                //Send stall packets
                sendStallPacket();
            }
        }


        /**
         * Updates the average OWD
         */
        void updateOWD()
        {
            if (others.Length == 1)
            {
                owd = 0;
                return;
            }

            double temp = 0;
            for (int i = 0; i < players.Count; i++)
            {
                temp += players[i].RoundtripTime.TotalSeconds;
            }
            temp = (temp + 400) / ((players.Count) * 2);
            temp = temp / 16.67;
            owd = (short)temp;
        }


        /**
         * Called for every latency readjustment frame
         */
        void readjustLatency()
        {
            int maxStalls = stallCounter;
            for (int i = 0; i < others.Length; i++)
            {
                if (others[i].numberOfFS > maxStalls)
                    maxStalls = others[i].numberOfFS;
            }

            if (maxStalls != 0)
            {
                int temp = latency;
                latency += maxStalls;

                for (int i = frameNumber + temp + 1; i <= frameNumber + latency; i++)
                    myPlayer.framesOfPlayer.Add(i, new frame());

                if (latency > 120)
                    latency = 120;
            }
            else
            {
                int maxOWD = owd;
                for (int i = 0; i < others.Length; i++)
                {
                    if (others[i].avOWD > maxOWD)
                        maxOWD = others[i].avOWD;
                }

                latency -= (int)(.6 * (double)(latency - maxOWD + 1.0));
                if (latency < 1)
                    latency = 1;

            }

            secondLRF = previousLRF;
            previousLRF = lrFrame;
            lrFrame = frameNumber + latency;
            updateOWD();

            stallCounter = 0;

            for (int i = 0; i < others.Length; i++)
                others[i].removeFrames(secondLRF - 1);
        }


        /*
         *  returns index of an a gamer 
         */
        object getIdentifier(int id)
        {

            for (int i = 0; i < players.Count; i++)
            {
                if (id == players[i].Id)
                {
                    return playerIdentifiers[i];
                }
            }
            return -1;          //fail
        }


        /*
         * Updates the lobby, detects key presses, called if we are in lobby 
        */
        void UpdateLobby()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (IsActive)
            {
                if (keyState.IsKeyDown(Keys.A))
                {
                    // Create a new session?
                    CreateGame();
                }
                else
                {
                    if (keyState.IsKeyDown(Keys.R))
                        games = null;
                    if (keyState.IsKeyDown(Keys.NumPad0) || keyState.IsKeyDown(Keys.D0))
                        JoinGame(0);
                    if (keyState.IsKeyDown(Keys.NumPad1) || keyState.IsKeyDown(Keys.D1))
                        JoinGame(1);
                    if (keyState.IsKeyDown(Keys.NumPad2) || keyState.IsKeyDown(Keys.D2))
                        JoinGame(2);
                    if (keyState.IsKeyDown(Keys.NumPad3) || keyState.IsKeyDown(Keys.D3))
                        JoinGame(3);
                    if (keyState.IsKeyDown(Keys.NumPad4) || keyState.IsKeyDown(Keys.D4))
                        JoinGame(4);
                    if (keyState.IsKeyDown(Keys.NumPad5) || keyState.IsKeyDown(Keys.D5))
                        JoinGame(5);
                    if (keyState.IsKeyDown(Keys.NumPad6) || keyState.IsKeyDown(Keys.D6))
                        JoinGame(6);
                    if (keyState.IsKeyDown(Keys.NumPad7) || keyState.IsKeyDown(Keys.D7))
                        JoinGame(7);
                    if (keyState.IsKeyDown(Keys.NumPad8) || keyState.IsKeyDown(Keys.D8))
                        JoinGame(8);
                    if (keyState.IsKeyDown(Keys.NumPad9) || keyState.IsKeyDown(Keys.D9))
                        JoinGame(9);
                }
            }
        }


        /*
         * Starts the player as hosting a game/network session
         * 
         */
        void CreateGame()
        {
            try
            {
                networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, maxPlayers, maxPlayers);
                isHost = true;
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


        /*
         * Joins a player to a game
         *  
         */
        void JoinGame(int gameNumber)
        {

            try
            {
                networkSession = NetworkSession.Join(games[gameNumber]);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }


        }


        /**
         * Implementation of chat input
         */
        void chat()
        {
            isChatting = true;
            try
            {
                KeyboardResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, "Chat", "", "", null, null);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

        }


        /*
         * Sends chat information packet of (byte, short and char[])
         */
        void sendChatPacket(String chat)
        {

            byte one = 1;
            short chars = (short)chat.Length;
            writer.Write(one);
            writer.Write(chars);
            writer.Write(chat.ToCharArray());

            networkSession.LocalGamers[0].SendData(writer, SendDataOptions.ReliableInOrder);
            isChatting = false;
        }


        /**
         *  Sends a packet to signifiy a stall to all players that are not quiet
         */
        void sendStallPacket()
        {
            //Send to players who we are not waiting for
            for (int i = 0; i < others.Length; i++)
            {
                //For each player not me that I am not waiting for
                if (others[i].framesOfPlayer.ContainsKey(frameNumber) == true && others[i] != myPlayer)
                {
                    writer.Write((byte)3);

                    byte stallers = 0;
                    byte[] playersStalling = new byte[4];

                    for (int j = 0; j < others.Length; j++)
                    {
                        //add all players that were are waiting for to the array
                        if (others[j].framesOfPlayer.ContainsKey(frameNumber) == false && others[j].me != myIdentifier)
                        {
                            playersStalling[stallers] = others[j].networkPlayer.Id;

                            stallers++;
                        }
                    }

                    writer.Write(stallers);

                    for (int j = 0; j < 4; j++)
                    {
                        if (playersStalling[j] != 0)
                            writer.Write(playersStalling[j]);
                    }


                    networkSession.LocalGamers[0].SendData(writer, SendDataOptions.Reliable, others[i].networkPlayer);
                }

            }
        }


        /*
         * Sends a game packet
         */
        void sendGamePacket(bool stalled)
        {
            if (stalled)
                return;
            writer.Write((byte)2);
            writer.Write(stallCounter);
            writer.Write(owd);
            writer.Write(frameNumber + latency);
            byte events = 0;

            //Count how many events we will send
            for (int i = secondLRF; i <= frameNumber; i++)
                events += frames[i].events;

            writer.Write(events);

            //Write each of those frames to the packet
            for (int i = secondLRF; i <= frameNumber; i++)
                writeFrame(frames[i]);

            //If we are not stalled send the packet to everyone
            if (!stalled)
            {
                networkSession.SimulatedPacketLoss = .1f;
                networkSession.LocalGamers[0].SendData(writer, SendDataOptions.None);
            }
            else
            {
                PacketWriter w2 = writer;
                for (int i = 0; i < others.Length; i++)
                {
                    if (others[i].framesOfPlayer.ContainsKey(frameNumber) == false && others[i] != myPlayer)
                    {
                        writer = w2;
                        networkSession.LocalGamers[0].SendData(writer, SendDataOptions.None);
                    }
                }
            }
        }

        /**
         * Writes the events of a single frame with the writer to the packet 
         */
        void writeFrame(frame f)
        {
            foreach (Keys k in f.pressedKeys)
            {
                writer.Write((byte)1);
                writer.Write(f.frameNumber + latency);
                writer.Write((int)k);

            }

            foreach (Keys k in f.releasedKeys)
            {
                writer.Write((byte)2);
                writer.Write(f.frameNumber + latency);
                writer.Write((int)k);
            }
            if (f.mouseChange)
            {
                if (f.mouseDown)
                {
                    writer.Write((byte)3);
                    writer.Write(f.frameNumber + latency);
                    writer.Write((byte)1);

                }
                else
                {
                    writer.Write((byte)4);
                    writer.Write(f.frameNumber + latency);
                    writer.Write((byte)1);
                }
            }

            writer.Write((byte)5);
            writer.Write(f.frameNumber + latency);
            writer.Write((short)f.mouseX);
            writer.Write((short)f.mouseY);
        }


        /*
         * Recieves packets, based on its byte value, it decides what type of packet it is and applys the information.
         * 
         */
        void recievePacket()
        {
            LocalNetworkGamer gamer = networkSession.LocalGamers[0];
            while (gamer.IsDataAvailable)
            {
                NetworkGamer sender;
                networkSession.LocalGamers[0].ReceiveData(reader, out sender);

                //its a chat packet
                byte packetID = reader.ReadByte();
                if (reader.Length != 0 && packetID.Equals(1))
                {

                    if (chatlines > 5)
                    {
                        chatText = chatText.Substring(chatText.IndexOf("\n") + 1);
                    }

                    chatlines++;

                    chatText += sender + ": ";
                    short chars = reader.ReadInt16();
                    char[] output = reader.ReadChars(chars);
                    for (short i = 0; i < chars; i++)
                        chatText += output[i];
                    chatText += "\n";

                }
                else if (reader.Length != 0 && packetID.Equals(2))  //Game Packet
                {
                    readGamePacket(getIdentifier(sender.Id));
                }
                else if (reader.Length != 0 && packetID.Equals(3))  //Stall Packet
                {
                    readStallPacket(getIdentifier(sender.Id));
                }
            }
        }

        /**
         * Reads the stall packets
         */
        void readStallPacket(object sender)
        {
            int i = reader.ReadByte();

            for (int j = 0; j < i; j++)
                reader.ReadByte();

        }

        /**
         * Reads a game packet 
         */
        void readGamePacket(object sender)
        {

            //Find the senders identifier
            player currentPlayer = null;

            for (int i = 0; i < others.Length; i++)
            {
                if (sender == others[i].me)
                    currentPlayer = others[i];
            }

            currentPlayer.numberOfFS = reader.ReadInt16();
            currentPlayer.avOWD = reader.ReadInt16();

            int lastFrame = reader.ReadInt32();

            //Add new frames up to the last frame that we recieved
            if (sender != getIdentifier(networkSession.LocalGamers[0].Id))
            {
                for (int i = currentPlayer.lastFrameNumber + 1; i <= lastFrame; i++)
                {
                    if (!currentPlayer.framesOfPlayer.ContainsKey(i))
                        currentPlayer.framesOfPlayer.Add(i, new frame());
                }
            }



            byte events = reader.ReadByte();

            int eventFrameNumber;

            for (byte i = 0; i < events; i++)
            {
                byte eventID = reader.ReadByte();
                eventFrameNumber = reader.ReadInt32();


                //Pressed key event
                if (eventID == 1)
                {
                    int keyCode = reader.ReadInt32();
                    if (currentPlayer.lastFrameNumber < eventFrameNumber || (currentPlayer == myPlayer && currentPlayer.framesOfPlayer.ContainsKey(eventFrameNumber)))
                        currentPlayer.framesOfPlayer[eventFrameNumber].pressedKeys.Add((Keys)(keyCode));
                }
                //Released Key Event
                if (eventID == 2)
                {
                    int keyCode = reader.ReadInt32();
                    if (currentPlayer.lastFrameNumber < eventFrameNumber || (currentPlayer == myPlayer && currentPlayer.framesOfPlayer.ContainsKey(eventFrameNumber)))
                        currentPlayer.framesOfPlayer[eventFrameNumber].releasedKeys.Add((Keys)(keyCode));
                }
                //Mouse down event
                if (eventID == 3)
                {
                    if (currentPlayer.lastFrameNumber < eventFrameNumber || (currentPlayer == myPlayer && currentPlayer.framesOfPlayer.ContainsKey(eventFrameNumber)))
                    {
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseDown = true;
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseChange = true;
                    }

                    reader.ReadByte();
                }
                //Mouse Up event
                if (eventID == 4)
                {
                    if (currentPlayer.lastFrameNumber < eventFrameNumber || (currentPlayer == myPlayer && currentPlayer.framesOfPlayer.ContainsKey(eventFrameNumber) ))
                    {
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseDown = false;
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseChange = true;
                    }

                    reader.ReadByte();
                }
                //Mouse location
                if (eventID == 5)
                {
                    if (currentPlayer.lastFrameNumber < eventFrameNumber || (currentPlayer == myPlayer && currentPlayer.framesOfPlayer.ContainsKey(eventFrameNumber) ))
                    {
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseX = reader.ReadInt16();
                        currentPlayer.framesOfPlayer[eventFrameNumber].mouseY = reader.ReadInt16();
                    }
                    else
                    {
                        reader.ReadInt16();
                        reader.ReadInt16();
                    }
                }


            }
            //set new last frame number
            currentPlayer.lastFrameNumber = lastFrame;

        }

    }
        #endregion
}
