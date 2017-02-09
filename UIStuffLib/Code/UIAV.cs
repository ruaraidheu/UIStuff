using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIStuff;

namespace UIStuff
{
    public class UIVideo : UIImage
    {
        private Video vid;
        private VideoPlayer player;
        bool prev;
        UIVar<bool> play;
        string targ;
        public UIVideo(Positioning p, Origin o, Alignment al, Point pos, Size size, Video _vid, UIVar<bool> playing, string target):base(p,o,al,pos,size,null)
        {
            vid = _vid;
            player = new VideoPlayer();
            play = playing;
            //this is needed for some reason.
            try
            {
                player.Play(vid);
            }
            catch
            {

            }
            if (!play.Value)
            {
                player.Pause();
            }
            targ = target;
            prev = play.Value;
        }
        public override string Update(MouseState m, GameTime gt)
        {
            if (play.Value != prev)
            {
                if (play.Value)
                {
                    player.Pause();
                }
                else
                {
                    player.Resume();
                }
            }
            if (player.State == MediaState.Stopped)
            {
                return targ;
            }
            return null;
        }
        public override void Draw(SpriteBatch sb, Viewport v)
        {
            if (player.State != MediaState.Stopped)
            {
                base.t = player.GetTexture();
                base.Draw(sb, v);
            }
        }
    }
}
