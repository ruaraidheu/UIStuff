using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIStuff;

namespace UIStuff
{
    class UIText : UIControl
    {
        protected string t;

        //TODO: add font scaling
        protected SpriteFont f;
        protected Color c;
        public UIText(Positioning p, Origin o, Point pos, string txt, SpriteFont font, Color col):base(p, o, pos, new Size(font.MeasureString(txt)))
        {
            t = txt;
            f = font;
            c = col;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
            sb.DrawString(f, t, calcuedpos.GetVector(), c);
        }
    }
}
