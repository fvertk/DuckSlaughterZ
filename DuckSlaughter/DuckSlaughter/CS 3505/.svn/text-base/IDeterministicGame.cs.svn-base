using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Project7
{
    /// <summary>
    /// A DeterministicGame object is a full XNA game, except that it does not
    /// extend the Microsoft.Xna.Framework.Game class.  It supports content loading
    /// and unloading, as well as modified Update and Draw functionality.
    /// 
    /// DeterministicGame objects are intented to be incorporated inside of an 
    /// existing game.  By simply calling update and draw at the appropriate times,
    /// and by supplying user inputs, the game will play just like any other game.
    /// 
    /// It is intended that a DeterministicGame be a multiplayer game, and support for
    /// this is listed in the interface below.  Each player is identified by a unique object
    /// reference (of the caller's choice, not a struct).  The game supports the notion of a
    /// current 'frame', or state.  The enclosing code supplies the user inputs for the
    /// next frame by calling methods.  The enclosing code then should call the update
    /// method to advance the game to the next frame.  Finally, the enclosing code
    /// calls the draw method to render the game state.  Note that the game state can
    /// be drawn multiple times without updating the game, thus allowing the game
    /// to be paused or stalled.
    /// </summary>
    public interface IDeterministicGame
    {
        /// <summary>
        /// Call this method to give the game a chance to load its content.
        /// </summary>
        /// <param name="contentManager">A valid content manager pointing to the root of the content tree</param>
        void LoadContent (ContentManager contentManager);

        /// <summary>
        /// Call this method to give the game a chance to unload its content.
        /// </summary>
        void UnloadContent();

        /// <summary>
        /// Returns the preferred screen size for this game.
        /// </summary>
        /// <returns></returns>
        Vector2 PreferredScreenSize { get; }

        /// <summary>
        /// Returns the minimum number of players this game can support.
        /// </summary>
        /// <returns>the minimum player count</returns>
        int MinimumSupportedPlayers { get; }

        /// <summary>
        /// Returns the maximum number of players this game can support.
        /// </summary>
        /// <returns>the maximum player count</returns>
        int MaximumSupportedPlayers { get; }

        /// <summary>
        /// Call this method to reset the game state, to set the current frame at 0, and
        /// to supply identifiers for each player in the game.  Player identifiers should
        /// be unique object references (not structs) that the caller will use later
        /// to identify each player.  (It is important that these not be 'boxed' object
        /// references or the reference will not be preserved.)
        /// 
        /// Since, in theory, there will be four copies of the game running, a second
        /// parameter identifies the player that is running this copy of the game.
        /// </summary>
        /// <param name="playerIdentifiers">An array of objects (references) that will identify each player</param>
        /// <param name="playerIdentifiers">An object identifier for the player whose machine is displaying this game</param>
        void ResetGame(Object[] playerIdentifiers, Object thisPlayer);

        /// <summary>
        /// Returns the current frame number.  This corresponds to the current state
        /// of the game world.
        /// </summary>
        /// <returns>the current frame number</returns>
        long CurrentFrameNumber { get; }

        /// <summary>
        /// Returns a checksum of all of the game world state.  This checksum can be used
        /// to ensure that multiple players at some frame all share the same state.  It is
        /// guaranteed that identical states will produce identical checksums.
        /// </summary>
        /// <returns>the current game state checksum</returns>
        long CurrentChecksum { get; }

        /// <summary>
        /// Call this method to report changes in keypresses to the game.  You should call this method
        /// to report any changes in keyboard state for a player.  The keyboard state will be
        /// applied to the next game state (not the current state).
        /// </summary>
        /// <param name="playerIdentifier">An object (reference) that was registered for a player in the game</param>
        /// <param name="key">A key identifier</param>
        /// <param name="isKeyPressed">The key state - true means pressed, false means released</param>
        void ApplyKeyInput (Object playerIdentifier, Keys key, bool isKeyPressed);

        /// <summary>
        /// Call this method to report changes in mouse locations to the game.  You should call this method
        /// any time the mouse coordinates for a player changes.  The mouse information will
        /// be applied to the next game state (not the current state).
        /// </summary>
        /// <param name="playerIdentifier">an object (reference) that was registered for a player in the game</param>
        /// <param name="x">the mouse x location</param>
        /// <param name="y">the mouse y location</param>
        void ApplyMouseLocationInput (Object playerIdentifier, int x, int y);

        /// <summary>
        /// Call this method to report changes in mouse button state to the game.  Note that only one
        /// mouse button is supported in game.  You should call this method to report any
        /// changes in mouse button state for a player.  The mouse button state will be
        /// applied to the next game state (not the current state).
        /// </summary>
        /// <param name="playerIdentifier">an object (reference) that was registered for a player in the game</param>
        /// <param name="isButtonPressed">the mouse button state</param>
        void ApplyMouseButtonInput (Object playerIdentifier, bool isButtonPressed);

        /// <summary>
        /// Returns true if the specified player's game is over.  They can be safely disconnected from the game
        /// when this flag is true, their inputs do not affect game state.  (You can continue to report inputs,
        /// to allow the player to view a game over screen, but no game state action is taken.)
        /// </summary>
        /// <param name="playerIdentifier">an object (reference) that was registered for a player in the game</param>
        /// <returns>true if the game is over</returns>
        bool IsGameOver(Object playerIdentifier);

        /// <summary>
        /// Returns true if the specified player's game is over, and the player has clicked on something indicating
        /// they wish to leave the game over screen.  (This only becomes true if inputs are reported
        /// even after the game is over.)
        /// </summary>
        /// <param name="playerIdentifier">an object (reference) that was registered for a player in the game</param>
        /// <returns>true if the player has terminated the game</returns>
        bool IsTerminated(Object playerIdentifier);

        /// <summary>
        /// Call this method to advance the game state.  Previously sent inputs are applied
        /// to the game state and the frame number is advanced and returned.  Caution should be used when
        /// supplying the seconds parameter - it can affect game state.  All players in a game
        /// should advance their game time by the same amount.
        /// </summary>
        /// <param name="timespan">The elapsed game time</param>
        /// <returns>the frame number of the new game state (now the current state)</returns>
        long Update(TimeSpan timespan);

        /// <summary>
        /// Draws the current game state.  This does not affect the game state - it may be called
        /// repeatedly to redraw the current game state if needed.
        /// </summary>
        /// <param name="spriteBatch">a SpriteBatch object that has begun a batch</param>
        /// <returns>the current game state frame number</returns>
        long Draw(SpriteBatch spriteBatch);
    }
}
