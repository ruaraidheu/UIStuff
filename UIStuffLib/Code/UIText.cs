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
    public class UIText : UIControl
    {
        protected string t;

        //TODO: add font scaling
        protected SpriteFont f;
        protected Color c;
        public UIText(Positioning p, Origin o, Alignment al, Point pos, string txt, SpriteFont font, Color col)
            : base(p, o, al, pos, new Size(font.MeasureString(txt)))
        {
            calcsize = false;
            t = txt;
            f = font;
            c = col;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            Draw(sb, v, Point.Zero);
        }
        public void Draw(SpriteBatch sb, Viewport v, Point parent)
        {
            base.Draw(sb, v);
            sb.DrawString(f, t, calcuedpos.GetVector()+parent.GetVector(), c);
        }
    }
    public class UIBGText : UIImage
    {
        UIText t;
        public UIBGText(Positioning p, Origin o, Alignment al, Point pos, Size size, Texture2D img, string txt, SpriteFont font, Color col)
            : base(p, o, al, pos, size, img)
        {
            t = new UIText(Positioning.Relative, Origin.MiddleCenter, Alignment.MiddleCenter, new Point(0), txt, font, col);
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
            v.Height = (int)calcuedsize.height;
            v.Width = (int)calcuedsize.width;
            t.Draw(sb, v, calcuedpos);
        }
    }
    public class UIBGImg : UIText
    {
        UIImage i;
        public UIBGImg(Positioning p, Origin o, Alignment al, Point pos, Texture2D img, string txt, SpriteFont font, Color col, int margin = 0)
            : base(p, o, al, pos, txt, font, col)
        {
            i = new UIImage(p, o, al, pos, new Size(size.width +(2*margin), size.height+(2*margin)), img);
            i.calcsize = false;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            i.Draw(sb, v);
            base.Draw(sb, v);
        }
    }
}
