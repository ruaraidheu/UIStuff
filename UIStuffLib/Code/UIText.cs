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
        public string t { get; protected set; }

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
        protected UIText text;
        public enum TAlign
        {
            Left, Center, Right
        }
        public UIBGText(Positioning p, Origin o, Alignment al, Point pos, Size size, Texture2D img, string txt, SpriteFont font, Color col, TAlign ta)
            : base(p, o, al, pos, size, img)
        {
            if (ta == TAlign.Left)
            {
                text = new UIText(Positioning.Relative, Origin.MiddleLeft, Alignment.MiddleLeft, new Point(0), txt, font, col);
            }
            else if (ta == TAlign.Center)
            {
                text = new UIText(Positioning.Relative, Origin.MiddleCenter, Alignment.MiddleCenter, new Point(0), txt, font, col);
            }
            else
            {
                text = new UIText(Positioning.Relative, Origin.MiddleRight, Alignment.MiddleRight, new Point(0), txt, font, col);
            }
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
        float margin;
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
        public UIBGImg(Positioning p, Origin o, Alignment al, Point pos, Texture2D img, string txt, SpriteFont font, Color col, float _margin = 0)
            : base(p, o, al, pos, txt, font, col)
        {
            margin = _margin;
            margin = 0;
            i = new UIImage(p, o, al, new Point(pos.x - margin, pos.y - margin), size, img);
            i.calcsize = false;
        }
        public void ChangeImage(Texture2D img)
        {
            i = new UIImage(p, o, al, new Point(pos.x - margin, pos.y - margin), size, img);
            i.calcsize = false;
        }
        /*public float Calcmargin(int dim, float _margin)
        {
            if (p == Positioning.Absolute)
            {
                return _margin;
            }
            else
            {
                return (dim * (_margin / 100));
            }
        }*/
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            //i.Resise(new Size(size.width + (2 * Calcmargin(v.Width, margin)), size.height + (2 * Calcmargin(v.Height, margin))));
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
        public UIButton(Positioning p, Origin o, Alignment al, Point pos, ButtonData standard, ButtonData _hover, string target, bool activateonrelease, ButtonData _click, float margin = 0)
            : base(p, o, al, pos, standard.t, standard.s, standard.f, standard.c, margin)
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
                            ChangeImage(click.t);
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
                    ChangeImage(hover.t);
                }
            }
            else if (bc != ButtonCurr.Standard)
            {
                bc = ButtonCurr.Standard;
                Changetext(stand.s, stand.c, stand.f);
                ChangeImage(stand.t);
            }
            lastupdate = m.LeftButton;
            return null;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
        }
    }
    public class UITextBox : UIBGText
    {
        KeyboardState oldstate;
        bool? sel;
        public UITextBox(Positioning p, Origin o, Alignment al, Point pos, Size size, Texture2D img, string txt, SpriteFont font, Color col, TAlign ta, bool select)
            : base(p, o, al, pos, size, img, txt, font, col, ta)
        {
            oldstate = Keyboard.GetState();
            if (select)
            {
                sel = false;
            }
            else
            {
                sel = null;
            }
        }
        public override string Update(MouseState m, GameTime gt)
        {
            KeyboardState currstate = Keyboard.GetState();
            if (m.LeftButton == ButtonState.Pressed)
            {
                if (m.X > calcuedpos.x && m.X < (calcuedpos.x + calcuedsize.width) && m.Y > calcuedpos.y && m.Y < (calcuedpos.y + calcuedsize.height))
                {
                    sel = true;
                }
                else
                {
                    sel = false;
                }
            }
            if (sel == null || sel == true)
            {
                if (currstate != oldstate)
                {
                    if (currstate.GetPressedKeys().Length > 0)
                    {
                        if (currstate.GetPressedKeys()[0].ToString() == Keys.Back.ToString())
                        {
                            if (text.t.Length > 0)
                                text.Changetext(text.t.Remove(text.t.Length - 1));
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.OemPeriod.ToString())
                        {
                            text.Changetext(text.t + ".");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.Enter.ToString()
                            || currstate.GetPressedKeys()[0].ToString() == Keys.LeftControl.ToString()
                            || currstate.GetPressedKeys()[0].ToString() == Keys.RightControl.ToString()
                            || currstate.GetPressedKeys()[0].ToString() == Keys.RightAlt.ToString()
                            || currstate.GetPressedKeys()[0].ToString() == Keys.LeftAlt.ToString())
                        {
                            //Nothing
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.Space.ToString())
                        {
                            text.Changetext(text.t + " ");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.OemComma.ToString())
                        {
                            text.Changetext(text.t + ",");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.OemMinus.ToString())
                        {
                            text.Changetext(text.t + "-");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.OemPlus.ToString())
                        {
                            text.Changetext(text.t + "+");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.OemQuestion.ToString())
                        {
                            text.Changetext(text.t + "/");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D0.ToString())
                        {
                            text.Changetext(text.t + "0");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D1.ToString())
                        {
                            text.Changetext(text.t + "1");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D2.ToString())
                        {
                            text.Changetext(text.t + "2");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D3.ToString())
                        {
                            text.Changetext(text.t + "3");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D4.ToString())
                        {
                            text.Changetext(text.t + "4");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D5.ToString())
                        {
                            text.Changetext(text.t + "5");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D6.ToString())
                        {
                            text.Changetext(text.t + "6");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D7.ToString())
                        {
                            text.Changetext(text.t + "7");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D8.ToString())
                        {
                            text.Changetext(text.t + "8");
                        }
                        else if (currstate.GetPressedKeys()[0].ToString() == Keys.D9.ToString())
                        {
                            text.Changetext(text.t + "9");
                        }
                        else
                        {
                            if (currstate.GetPressedKeys().Length > 1)
                            {
                                if (currstate.GetPressedKeys()[1].ToString() == Keys.LeftShift.ToString() || currstate.GetPressedKeys()[1].ToString() == Keys.RightShift.ToString())
                                {
                                    text.Changetext(text.t + currstate.GetPressedKeys()[0].ToString().ToUpper());
                                }
                                else
                                {
                                    if (currstate.GetPressedKeys()[0].ToString() != Keys.LeftShift.ToString() && currstate.GetPressedKeys()[0].ToString() != Keys.RightShift.ToString())
                                        text.Changetext(text.t + currstate.GetPressedKeys()[0].ToString().ToLower());
                                }
                            }
                            else
                            {
                                if (currstate.GetPressedKeys()[0].ToString() != Keys.LeftShift.ToString() && currstate.GetPressedKeys()[0].ToString() != Keys.RightShift.ToString())
                                    text.Changetext(text.t + currstate.GetPressedKeys()[0].ToString().ToLower());
                            }
                        }
                    }
                    if (text.t.ToLower() == "localhost")
                    {
                        text.Changetext("127.0.0.1");
                    }
                }
            }
            oldstate = currstate;
            return base.Update(m, gt);
        }
    }
}
