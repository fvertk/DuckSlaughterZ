using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project07Game.Components; //uses the components defined 
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//class for the start screen
namespace Project07Game
{
    public class Start : Screen
    {
        Menu menu;
        SpriteFont spriteFont;
        Vector2 position;


        public Start(Game game, SpriteFont spriteFont, Texture2D back, SpriteFont spriteFont2)
            : base(game)
        {
            Components.Add(new Background(game, back, 1, spriteFont2, 800, 600));
            this.spriteFont = spriteFont;
            position = new Vector2(300, 100);
            string[] items = { "One Player", "Two Players", "Help", "Quit Game" };
            menu = new Menu(game, spriteFont, spriteFont2, "");
            menu.SetMenu(items);
            Components.Add(menu);

        }

        public int selectIndex
        {
            get
            {
                return menu.selectedIndex;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Show()
        {
            menu.Pos = new Vector2((Game.Window.ClientBounds.Width - menu.Width) / 2, 330);
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
        }



    }
}
