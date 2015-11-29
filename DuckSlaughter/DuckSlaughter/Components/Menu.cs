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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Collections.Specialized;

//class for the menus/text themselves 
namespace Project07Game.Components
{
    public partial class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string menuTitle; 
        SpriteBatch spriteBatch = null;
        SpriteFont spriteFont;
        SpriteFont spriteFont2;

        Color nCol = Color.Black;
        Color hCol = Color.Yellow;

        KeyboardState oldState;

        Vector2 position = new Vector2();
        Vector2 titleP; 

        int index = 0;
        private StringCollection menuOptions = new StringCollection();

        int w, h;

        public Menu(Game game, SpriteFont spriteFont, SpriteFont spriteFont2, string top)
            : base(game)
        {
            this.spriteFont = spriteFont;
            this.spriteFont2 = spriteFont2;
            this.menuTitle = top;
            titleP = new Vector2(265, 75); 
 
            //gets the sprite batch to be used to draw the menu instead of creating
            //a new spriteBatch object or passing one to the class.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
        }

        public int Width
        {
            get { return w; }
        }

        public int Height
        {
            get { return h; }
        }

        public int selectedIndex
        {
            get
            {
                return index;
            }
            set
            {
                //makes sure the value sent is valid
                index = (int)MathHelper.Clamp(value, 0, menuOptions.Count - 1);
            }
        }

        public Color Normal
        {
            get
            {
                return nCol;
            }
            set
            {
                nCol = value;
            }
        }

        public Color Highlight
        {
            get
            {
                return hCol;
            }
            set
            {
                hCol = value;
            }
        }

        public Vector2 Pos
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Vector2 titPos
        {
            get
            {
                return titleP;
            }
            set
            {
                titleP = value;
            }
        }

        public void SetMenu(string[] options)
        {
            menuOptions.Clear();
            menuOptions.AddRange(options);
            CalculateB();
        }

        private void CalculateB()
        {
            w = 0;
            h = 0;
            foreach (string option in menuOptions)
            {
                Vector2 size = spriteFont.MeasureString(option);
                if (size.X > w)
                    w = (int)size.X;
                h += spriteFont.LineSpacing;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        //checks to see if a key has been pressed and released
        public bool keyCheck(Keys k)
        {
            KeyboardState newState = Keyboard.GetState();

            return oldState.IsKeyDown(k) && newState.IsKeyUp(k);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (keyCheck(Keys.Down))
            {
                index++;
                if (index == menuOptions.Count)
                    index = 0;
            }

            if (keyCheck(Keys.Up))
            {
                index--;
                if (index == -1)
                {
                    index = menuOptions.Count - 1;
                }
            }

            oldState = newState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 menuPos = Pos;
            Color color;

            spriteBatch.DrawString(spriteFont, menuTitle, titleP, Color.Yellow); 
 
            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == index)
                    color = hCol;
                else
                    color = nCol;

                spriteBatch.DrawString(spriteFont2, menuOptions[i], menuPos + Vector2.One + new Vector2(50, 50), Color.Black);
                spriteBatch.DrawString(spriteFont2, menuOptions[i], menuPos + new Vector2(50,50), color);

                menuPos.Y += spriteFont.LineSpacing;
            }

            base.Draw(gameTime);
        }





    }
}
