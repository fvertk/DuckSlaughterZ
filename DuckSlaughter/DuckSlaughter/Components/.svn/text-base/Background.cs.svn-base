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

//class for the background of the menus 
namespace Project07Game.Components
{
    public partial class Background : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D backPic;
        SpriteBatch spriteBatch = null;
        Rectangle rectangle;
        int drawS; 
        SpriteFont sf;

        public Background(Game game, Texture2D texture, int test, SpriteFont spritefont, int width, int height)
            :base(game)
        {
            this.backPic = texture;
            this.drawS = test; 
            this.sf = spritefont; 
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            rectangle = new Rectangle(0, 0, width, height);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 pos = new Vector2(250, 250); 
            spriteBatch.Draw(backPic, rectangle, Color.White);
            if (drawS == 0)
            {
                spriteBatch.DrawString(sf, "TEST", pos, Color.Red); 
                
            }
            base.Draw(gameTime);
        }
    }
}
