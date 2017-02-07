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
    public class UIController
    {
        private List<UIBase> list;
        private int current = 0;
        public bool Updating { get; set; }
        public bool Drawing { get; set; }
        bool mig;
        Game game;
        public UIController(Game _game, bool mouseingame)
        {
            game = _game;
            mig = mouseingame;
            list = new List<UIBase>();
            Updating = true;
            Drawing = true;
            Add(
                new UIBase(
                    "none",
                    UIBase.Type.over,
                    "game",
                    mig,
                    0,
                    null
                )
            );
            Add(
                new UIBase(
                    "exit",
                    UIBase.Type.over,
                    "game",
                    mig,
                    0,
                    null
                )
            );
        }
        /// <summary>
        /// Adds UI to Controller, Returns false if the name is the same as a previously added UI
        /// </summary>
        /// <param name="uib">The UI you want to add to the controller</param>
        /// <returns></returns>
        public bool Add(UIBase uib)
        {
            foreach (UIBase uibase in list)
            {
                if (uib.name == uibase.name)
                {
                    Console.WriteLine("Name of new UIBase is the same as one already in use: " + uib.name);
#if DEBUG
                    throw new Exception("Name of new UIBase is the same as one already in use: " + uib.name);
#else
                    return false;
#endif
                }
            }
            list.Add(uib);
            return true;
        }
        public void Visible(bool b)
        {
            Updating = b;
            Drawing = b;
        }
        public Texture2D GetColor(Color c)
        {
            Texture2D tex = new Texture2D(game.GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = c;
            tex.SetData(data);
            return tex;
        }
        public string Update(GameTime gt)
        {
            if (Updating)
            {
                string tmp = list[current].Update(Mouse.GetState(), gt);
                if (tmp != null)
                {
                    if (tmp == "exit")
                    {
                        game.Exit();
                    }
                    else
                    {
                        Switchto(tmp);
                    }
                }
            }
            return list[current].overlay;
        }
        public string Update(GameTime gt, bool mouseingame)
        {
            mig = mouseingame;
            return Update(gt);
        }
        public string Draw(SpriteBatch sb)
        {
            if (Drawing)
            {
                list[current].Draw(sb, game.GraphicsDevice.Viewport);
            }
            else
            {
                game.IsMouseVisible = mig;
            }
            return list[current].overlay;
        }
        public void Switchto(string s)
        {
            int i = 0;
            bool tmp = false;
            foreach (UIBase uibase in list)
            {
                if (s == uibase.name)
                {
                    Switchto(i);
                    tmp = true;
                    break;
                }
                i++;
            }
            if (!tmp)
            {
                Console.WriteLine("No menu by that name.");
#if DEBUG
                throw new Exception("No menu by that name exists");
#endif
            }
        }
        public void Switchto(int i)
        {
            current = i;
            game.IsMouseVisible = list[current].sm;
        }
    }
    public class UIBase
    {
        public string name { get; private set; }
        List<UIControl> ctrls;
        public string overlay { get; private set; }
        public bool sm { get; set; }
        float time;
        double currtime;
        string targ;
        public enum Type
        {
            over, partial, world
        }
        private Type t;
        public UIBase(string _name, Type _t, string _overlay, bool showmouse, float secondstochange, string timetarget, params UIControl[] controls)
        {
            name = _name;
            overlay = _overlay;
            time = secondstochange;
            targ = timetarget;
            t = _t;
            sm = showmouse;
            ctrls = new List<UIControl>();
            foreach (UIControl control in controls)
            {
                ctrls.Add(control);
            }
        }
        public string Update(MouseState m, GameTime gt)
        {
            if (time > 0)
            {
                currtime += gt.ElapsedGameTime.TotalSeconds;
                if (currtime > time)
                {
                    currtime = 0;
                    return targ;
                }
            }
            foreach (UIControl control in ctrls)
            {
                string tmp = control.Update(m, gt);
                if (tmp != null)
                {
                    return tmp;
                }
            }
            return null;
        }
        public void Draw(SpriteBatch sb, Viewport v)
        {
            foreach (UIControl control in ctrls)
            {
                control.Draw(sb, v);
            }
        }
    }
    public class UIControl
    {
        protected Point pos;
        protected Size size;
        protected Positioning p;
        protected Origin o;
        protected Alignment al;
        protected Point calcuedpos;
        protected Size calcuedsize;
        public bool calcsize { get; set; }
        protected Viewport lview;
        /// <summary>
        /// Absolute is in pixels 
        /// Relative is percentage of the viewport
        /// Square is the same as relative but the original aspect ratio is maintained
        /// </summary>
        public enum Positioning
        {
            Absolute, Relative, Square
        }
        /// <summary>
        /// Where 0,0 is on the page.
        /// </summary>
        public enum Origin
        {
            TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight
        }
        /// <summary>
        /// Where 0,0 is on the object
        /// </summary>
        public enum Alignment
        {
            TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight
        }
        public UIControl(Positioning _p, Origin _o, Alignment _al, Point _pos, Size _size)
        {
            calcsize = true;
            pos = _pos;
            size = _size;
            p = _p;
            o = _o;
            al = _al;
        }
        public virtual string Update(MouseState m, GameTime gt)
        {
            return null;
        }
        public virtual void Draw(SpriteBatch sb, Viewport v)
        {
            //May be a bit expensive?
            if (!lview.Equals(v))
            {
                lview = v;
                CalcAll(v);
            }
        }
        public void Resise(Size s)
        {
            size = s;
        }
        protected void CalcAll(Viewport v)
        {
            Size tmp0 = new Size(v);
            if (calcsize)
            {
                calcuedsize = CalcSize(size, tmp0);
            }
            else
            {
                calcuedsize = size;
            }
            calcuedpos = CalcOrigin(CalcPos(pos, tmp0), calcuedsize, tmp0);
        }
        public Point CalcOrigin(Point a, Size s, Size ssize)
        {
            Point tmp0 = CalcAlign(s);
            if (o == Origin.TopLeft)
            {
                return new Point(a.x - tmp0.x, a.y - tmp0.y);
            }
            else if (o == Origin.TopCenter)
            {
                return new Point(a.x + (ssize.width / 2) - tmp0.x, a.y - tmp0.y);
            }
            else if (o == Origin.TopRight)
            {
                return new Point(a.x + (ssize.width) - tmp0.x, a.y - tmp0.y);
            }
            else if (o == Origin.MiddleLeft)
            {
                return new Point(a.x - tmp0.x, a.y + (ssize.height / 2) - tmp0.y);
            }
            else if (o == Origin.MiddleCenter)
            {
                return new Point(a.x + (ssize.width / 2) - tmp0.x, a.y + (ssize.height / 2) - tmp0.y);
            }
            else if (o == Origin.MiddleRight)
            {
                return new Point(a.x + (ssize.width) - tmp0.x, a.y + (ssize.height / 2) - tmp0.y);
            }
            else if (o == Origin.BottomLeft)
            {
                return new Point(a.x - tmp0.x, a.y + (ssize.height) - tmp0.y);
            }
            else if (o == Origin.BottomCenter)
            {
                return new Point(a.x + (ssize.width / 2) - tmp0.x, a.y + (ssize.height) - tmp0.y);
            }
            else if (o == Origin.BottomRight)
            {
                return new Point(a.x + (ssize.width) - tmp0.x, a.y + (ssize.height) - tmp0.y);
            }
            else
            {
                return a;
            }
        }
        public Point CalcAlign(Size s)
        {
            if (al == Alignment.TopLeft)
            {
                return Point.Zero;
            }
            else if (al == Alignment.TopCenter)
            {
                return new Point(s.width / 2, 0);
            }
            else if (al == Alignment.TopRight)
            {
                return new Point(s.width, 0);
            }
            else if (al == Alignment.MiddleLeft)
            {
                return new Point(0, s.height / 2);
            }
            else if (al == Alignment.MiddleCenter)
            {
                return new Point(s.width / 2, s.height / 2);
            }
            else if (al == Alignment.MiddleRight)
            {
                return new Point(s.width, s.height / 2);
            }
            else if (al == Alignment.BottomLeft)
            {
                return new Point(0, s.height);
            }
            else if (al == Alignment.BottomCenter)
            {
                return new Point(s.width / 2, s.height);
            }
            else if (al == Alignment.BottomRight)
            {
                return new Point(s.width, s.height);
            }
            else
            {
                return Point.Zero;
            }
        }
        public Point CalcPos(Point a, Size ssize)
        {
            if (p == Positioning.Relative || p == Positioning.Square)
            {
                return new Point(ssize.width * (a.x / 100), ssize.height * (a.y / 100));
            }
            else
            {
                return a;
            }
        }
        public Size CalcSize(Size a, Size ssize)
        {
            if (p == Positioning.Relative)
            {
                return new Size(ssize.width * (a.width / 100), ssize.height * (a.height / 100));
            }
            else if (p == Positioning.Square)
            {
                return new Size(ssize.width * (a.width / 100), ssize.width * (a.height / 100));
            }
            else
            {
                return a;
            }
        }
    }
    public struct Point
    {
        public float x, y;
        public Point(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
        public Point(Vector2 v)
        {
            x = v.X;
            y = v.Y;
        }
        public Point(float s)
        {
            x = s;
            y = s;
        }
        public Vector2 GetVector()
        {
            return new Vector2(x, y);
        }
        public Rectangle GetRectangle(Size s)
        {
            return new Rectangle((int)x, (int)y, (int)s.width, (int)s.height);
        }
        public static Point Zero
        {
            get
            {
                return new Point(0);
            }
        }
    }
    public struct Size
    {
        public float width, height;
        public Size(float _width, float _height)
        {
            width = _width;
            height = _height;
        }
        public Size(Viewport v)
        {
            width = v.Width;
            height = v.Height;
        }
        public Size(Vector2 v)
        {
            width = v.X;
            height = v.Y;
        }
        public Size(float s)
        {
            width = s;
            height = s;
        }
        public Vector2 GetVector()
        {
            return new Vector2(width, height);
        }
        public static Size Zero
        {
            get
            {
                return new Size(0);
            }
        }
    }
    public class UIImage : UIControl
    {
        protected Texture2D t;
        public UIImage(Positioning p, Origin o, Alignment al, Point pos, Size size, Texture2D img)
            : base(p, o, al, pos, size)
        {
            t = img;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            base.Draw(sb, v);
            sb.Draw(t, calcuedpos.GetRectangle(calcuedsize), Color.White);
        }
    }
}
