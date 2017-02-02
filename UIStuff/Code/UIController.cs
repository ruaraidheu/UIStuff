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
    class UIController
    {
        private List<UIBase> list;
        private int current = 0;
        public UIController()
        {
            list = new List<UIBase>();
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
#endif
                    return false;
                }
            }
            list.Add(uib);
            return true;
        }
        public UIBase.Overlaytype Update()
        {
            list[current].Update();
            return list[current].overlay;
        }
        public UIBase.Overlaytype Draw(SpriteBatch sb, Viewport v)
        {
            list[current].Draw(sb, v);
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
        }
    }
    class UIBase
    {
        public string name { get; private set; }
        List<UIControl> ctrls;
        public Overlaytype overlay { get; private set; }
        public enum Type
        {
            full, partial
        }
        //Replace with string or int?
        public enum Overlaytype
        {
            Menu, Game, Paused, Running
        }
        private Type t;
        public UIBase(string _name, Type _t, Overlaytype _overlay, params UIControl[] controls)
        {
            name = _name;
            overlay = _overlay;
            t = _t;
            ctrls = new List<UIControl>();
            foreach (UIControl control in controls)
            {
                ctrls.Add(control);
            }
        }
        public void Update()
        {
            foreach (UIControl control in ctrls)
            {
                control.Update();
            }
        }
        public void Draw(SpriteBatch sb, Viewport v)
        {
            foreach (UIControl control in ctrls)
            {
                control.Draw(sb, v);
            }
        }
    }
    class UIControl
    {
        protected Point pos;
        protected Size size;
        protected Positioning p;
        protected Origin o;
        protected Alignment al;
        protected Point calcuedpos;
        protected Size calcuedsize;
        protected bool calcsize = true;
        Viewport lview;
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
            pos = _pos;
            size = _size;
            p = _p;
            o = _o;
            al = _al;
        }
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch sb, Viewport v)
        {
            //May be a bit expensive?
            if (!lview.Equals(v))
            {
                lview = v;
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
                return new Point(0, 0);
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
                return new Point(0, 0);
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
    }
    class UIImage : UIControl
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
