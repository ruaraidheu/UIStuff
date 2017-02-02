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
        public void Draw(SpriteBatch sb)
        {
            list[current].Draw(sb);
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
        public void Draw(SpriteBatch sb)
        {
            foreach (UIControl control in ctrls)
            {
                control.Draw(sb);
            }
        }
    }
    class UIControl
    {
        Point pos;
        Size size;
        Positioning p;
        Origin o;
        public enum Positioning
        {
            Absolute, Relative
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
        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
    public struct Point
    {
        public float x;
        public float y;
    }
    public struct Size
    {
        public float width;
        public float height;
    }
    class UIImage : UIControl
    {
        Texture2D t;
        public UIImage(Positioning p, Origin o, Point pos, Size size, Texture2D img):base(p, o, pos, size)
        {
            t = img;
        }

    }
}
