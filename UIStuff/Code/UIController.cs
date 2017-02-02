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
        public void Add(UIBase uib)
        {
            list.Add(uib);
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
        private string name;
        public UIBase(string _name)
        {
            name = _name;
        }
        public void Update()
        {

        }
        public void Draw(SpriteBatch sb)
        {

        }
    }
}
