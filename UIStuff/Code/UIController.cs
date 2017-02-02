using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIStuff.Code
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
        public enum Positioning
        {
            Absolute, Relative, Square
        }
        public enum Origin
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
            if (p == Positioning.Relative)
            {
                return new Point(ssize.width*(a.x/100), ssize.height*(a.y/100));
            }
            else if (p == Positioning.Square)
            {
                return new Point(ssize.width * (a.x / 100), ssize.width * (a.y / 100));
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
    }
    public struct Size
    {
        public float width, height;
        public Size(float _width, float _height)
        {
            width = _width;
            height = _height;
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
            //TODO: Optimise, get rid of repitition repitition repitition repitition of calcpos and calcalign
            sb.Draw(t, new Rectangle(
                (int)CalcAlign(CalcPos(pos, new Size(v.Width, v.Height)), new Size(v.Width, v.Height)).x, 
                (int)CalcAlign(CalcPos(pos, new Size(v.Width, v.Height)), new Size(v.Width, v.Height)).y, 
                (int)CalcSize(size, new Size(v.Width, v.Height)).width, 
                (int)CalcSize(size, new Size(v.Width, v.Height)).height), Color.White
            );
        }
    }
}
