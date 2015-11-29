using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project07Game.Components;

//class for the Help Screen
namespace Project07Game
{
    public class Help : Screen
    {
        Vector2 position;

        public Help(Game game, Texture2D back, SpriteFont spritefont, SpriteFont spritefont2)
            : base(game)
        {
            Components.Add(new Background(game, back, 1, spritefont2, 800, 600));

            position = new Vector2(250, 250);
            base.Hide();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Show()
        {
            base.Show();
        }

    }
}
