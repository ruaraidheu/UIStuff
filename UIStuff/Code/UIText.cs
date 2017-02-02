﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIStuff.Code;

namespace UIStuff.Code
{
    class UIText : UIControl
    {
        protected string t;

        //TODO: add font scaling
        protected SpriteFont f;
        protected Color c;
        public UIText(Positioning p, Origin o, Point pos, string txt, SpriteFont font, Color col):base(p, o, pos, new Size(0, 0))
        {
            t = txt;
            f = font;
            c = col;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            //TODO: fix the overuse of CalcPos
            sb.DrawString(f, t, new Vector2(CalcAlign(CalcPos(pos, new Size(v.Width, v.Height)), new Size(v.Width, v.Height)).x, CalcAlign(CalcPos(pos, new Size(v.Width, v.Height)), new Size(v.Width, v.Height)).y), c);
        }
    }
}
