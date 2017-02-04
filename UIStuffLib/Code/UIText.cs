using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public void Changetext(string s)
        {
            Changetext(s, c, f);
        }
        public void Changetext(string s, Color col)
        {
            Changetext(s, col, f);
        }
        public void Changetext(string s, Color col, SpriteFont font)
        {
            t = s;
            c = col;
            f = font;
            size = new Size(f.MeasureString(t));
            CalcAll(lview);
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            Draw(sb, v, Point.Zero);
        }
        public void Draw(SpriteBatch sb, Viewport v, Point parent)
        {
            base.Draw(sb, v);
            sb.DrawString(f, t, calcuedpos.GetVector() + parent.GetVector(), c);
        }
    }
    public class UIBGText : UIImage
    {
        UIText text;
        public UIBGText(Positioning p, Origin o, Alignment al, Point pos, Size size, Texture2D img, string txt, SpriteFont font, Color col)
            : base(p, o, al, pos, size, img)
        {
            text = new UIText(Positioning.Relative, Origin.MiddleCenter, Alignment.MiddleCenter, new Point(0), txt, font, col);
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
            v.Height = (int)calcuedsize.height;
            v.Width = (int)calcuedsize.width;
            text.Draw(sb, v, calcuedpos);
        }
    }
    public class UIBGImg : UIText
    {
        UIImage i;
        /// <summary>
        /// Margin doesn't work yet. Leave it at 0.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <param name="al"></param>
        /// <param name="pos"></param>
        /// <param name="img"></param>
        /// <param name="txt"></param>
        /// <param name="font"></param>
        /// <param name="col"></param>
        /// <param name="margin"></param>
        public UIBGImg(Positioning p, Origin o, Alignment al, Point pos, Texture2D img, string txt, SpriteFont font, Color col, int margin = 0)
            : base(p, o, al, pos, txt, font, col)
        {
            margin = 0;

            i = new UIImage(p, o, al, pos, new Size(size.width + (2 * margin), size.height + (2 * margin)), img);
            i.calcsize = false;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            i.Draw(sb, v);
            base.Draw(sb, v);
        }
    }
    public struct ButtonData
    {
        public Texture2D t;
        public string s;
        public SpriteFont f;
        public Color c;
        public ButtonData(Texture2D _t, string _s, SpriteFont _f, Color _c)
        {
            t = _t;
            s = _s;
            f = _f;
            c = _c;
        }
        public static ButtonData Empty
        {
            get
            {
                return new ButtonData(null, null, null, Color.White);
            }
        }
    }
    public class UIButton : UIBGImg
    {
        ButtonState lastupdate = ButtonState.Released;
        ButtonData stand;
        ButtonData hover;
        ButtonData click;
        enum ButtonCurr
        {
            Standard, Hover, Click
        }
        ButtonCurr bc = ButtonCurr.Standard;
        bool aor;
        string targ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <param name="al"></param>
        /// <param name="pos"></param>
        /// <param name="standard"></param>
        /// <param name="hover"></param>
        /// <param name="activateonrelease"></param>
        /// <param name="click">Use ButtonData.Empty if not using activate on release.</param>
        public UIButton(Positioning p, Origin o, Alignment al, Point pos, ButtonData standard, ButtonData _hover, string target, bool activateonrelease, ButtonData _click)
            : base(p, o, al, pos, standard.t, standard.s, standard.f, standard.c)
        {
            targ = target;
            stand = standard;
            hover = _hover;
            click = _click;
            if (activateonrelease && (click.t == null || click.s == null || click.s == String.Empty || click.f == null))
            {
                Console.WriteLine("You need to assign a click state to use activateonrelease.");
#if DEBUG
                throw new Exception("You need to assign a click state to use activateonrelease.");
#else
                activateonrelease = false;
#endif
            }
            aor = activateonrelease;
        }
        public override string Update(MouseState m, GameTime gt)
        {
            if (m.X > calcuedpos.x && m.X < (calcuedpos.x + calcuedsize.width) && m.Y > calcuedpos.y && m.Y < (calcuedpos.y + calcuedsize.height))
            {
                if (m.LeftButton == ButtonState.Pressed)
                {
                    if (aor)
                    {
                        if (bc != ButtonCurr.Click)
                        {
                            bc = ButtonCurr.Click;
                            Changetext(click.s, click.c, click.f);
                        }
                    }
                    else
                    {
                        return targ;
                    }
                }
                else if (aor && lastupdate == ButtonState.Pressed)
                {
                    return targ;
                }
                else if (bc != ButtonCurr.Hover)
                {
                    bc = ButtonCurr.Hover;
                    Changetext(hover.s, hover.c, hover.f);
                }
            }
            else if (bc != ButtonCurr.Standard)
            {
                bc = ButtonCurr.Standard;
                Changetext(stand.s, stand.c, stand.f);
            }
            lastupdate = m.LeftButton;
            return null;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
        }
    }
}
