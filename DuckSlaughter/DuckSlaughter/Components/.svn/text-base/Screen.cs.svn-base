using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

//general menu/screen class
namespace Project07Game.Components
{
    public partial class Screen : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //list of components of the screen (menus, etc.)
        private List<GameComponent> children;
        public SpriteBatch spriteBatch;

        //Constructor creates the list of child components and sets it so
        //component will not draw/update itself unless you want it to.
        public Screen(Game game)
            : base(game)
        {
            children = new List<GameComponent>();
            Visible = false;
            Enabled = false;
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); 
        }

        public List<GameComponent> Components
        {
            get { return children; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
        //Update method that has been added so it will scan through
        //the child components and if they are enabled, update them.
        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState(); 

            foreach (GameComponent child in children)
            {
                if (child.Enabled)
                {
                    child.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        //Draw method basic except same exception as above Update
        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent child in children)
            {
                if ((child is DrawableGameComponent) &&
                    ((DrawableGameComponent)child).Visible)
                {
                    ((DrawableGameComponent)child).Draw(gameTime);
                }
            }
            base.Draw(gameTime);
        }

        //Shows the screen 
        public virtual void Show()
        {
            Visible = true;
            Enabled = true;
        }

        //Hides the screen
        public virtual void Hide()
        {
            Visible = false;
            Enabled = false;
        }

    }
}
