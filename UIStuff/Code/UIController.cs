using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Console.WriteLine("Name of new UIBase is the same as one already in use: "+uib.name);
                    return false;
                }
            }
            list.Add(uib);
            return true;
        }
        public void Update()
        {
            list[current].Update();
        }
        public void Draw(SpriteBatch sb, Viewport v)
        {
            list[current].Draw(sb, v);
        }
    }
    class UIBase
    {
        public string name { get; private set; }
        List<UIControl> ctrls;
        public enum Type
        {
            full, partial
        }
        private Type t;
        public UIBase(string _name, Type _t, params UIControl[] controls)
        {
            name = _name;
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
        protected Point calcuedpos;
        protected Size calcuedsize;
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
        public enum Allignment
        {
            TopLeft, TopCenter, TopRight, MiddleLeft, MiddleCenter, MiddleRight, BottomLeft, BottomCenter, BottomRight
        }
        public UIControl(Positioning _p, Origin _o, Point _pos, Size _size)
        {
            pos = _pos;
            size = _size;
            p = _p;
            o = _o;
        }
        public virtual void Update()
        {

        }
        public virtual void Draw(SpriteBatch sb, Viewport v)
        {
            //May be a bit expensive?
            if (!lview.Equals(v))
            {
                Size tmp0 = new Size(v);
                calcuedpos = CalcAlign(CalcPos(pos, tmp0), tmp0);
                calcuedsize = CalcSize(size, tmp0);
            }
        }
        public Point CalcAlign(Point a, Size ssize)
        {
            if (o == Origin.TopLeft)
            {
                return a;
            }
            else if (o == Origin.TopCenter)
            {
                return new Point(a.x + (ssize.width/2), a.y);
            }
            else if (o == Origin.TopRight)
            {
                return new Point(a.x + (ssize.width), a.y);
            }
            else if (o == Origin.MiddleLeft)
            {
                return new Point(a.x, a.y + (ssize.height / 2));
            }
            else if (o == Origin.MiddleCenter)
            {
                return new Point(a.x + (ssize.width / 2), a.y + (ssize.height/2));
            }
            else if (o == Origin.MiddleRight)
            {
                return new Point(a.x + (ssize.width), a.y + (ssize.height / 2));
            }
            else if (o == Origin.BottomLeft)
            {
                return new Point(a.x, a.y + (ssize.height));
            }
            else if (o == Origin.BottomCenter)
            {
                return new Point(a.x + (ssize.width / 2), a.y + (ssize.height));
            }
            else if (o == Origin.BottomRight)
            {
                return new Point(a.x + (ssize.width), a.y + (ssize.height));
            }
            else
            {
                return a;
            }
        }
        public Point CalcPos(Point a, Size ssize)
        {
            if (p == Positioning.Relative || p == Positioning.Square)
            {
                return new Point(ssize.width*(a.x/100), ssize.height*(a.y/100));
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
        public Vector2 GetVector()
        {
            return new Vector2(width, height);
        }
    }
    class UIImage : UIControl
    {
        protected Texture2D t;
        public UIImage(Positioning p, Origin o, Point pos, Size size, Texture2D img):base(p, o, pos, size)
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
