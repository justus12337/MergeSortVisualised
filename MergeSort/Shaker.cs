using System;
using System.Drawing;
using System.Media;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace MergeSort
{
    public class Shaker
    {
        class ValueNoise1D
        {
            float[] r;
            public ValueNoise1D(int seed = 2011, int maxVertices = 500)
            {
                Random rnd = new Random(seed);
                r = new float[maxVertices];
                for (int i = 0; i< maxVertices;i++)
                {
                    r[i] = (float)rnd.NextDouble();
                }
            }
            public float eval(float x)
            {
                return evalUnscaled(x * (r.Length - 2f));
            }

            float evalUnscaled(float x)
            {
                int minX = (int)x;
                float t = x - minX;
                return r[minX] * (1f - t) + r[minX + 1] * t;
            }
        }

        int timer = 1000;
        int startX;
        int startY;
        float shakeAmountX;
        float shakeAmountY;
        Form parent;
        ValueNoise1D noiseX;
        ValueNoise1D noiseY;
        SoundPlayer player;
        Random rnd;
        public Shaker(Form form)
        {
            parent = form;
            player = new SoundPlayer(Properties.Resources.explosion);
            rnd = new Random();
            player.Load();
            //Init();
        }

        public void Init()
        {
            var screen = Screen.FromControl(parent);
            
            startX = screen.WorkingArea.Width / 2 + screen.WorkingArea.Left - parent.Width / 2;
            startY = screen.WorkingArea.Height / 2 + screen.WorkingArea.Top - parent.Height / 2;
            shakeAmountX = screen.WorkingArea.Width / 1.5f;
            shakeAmountY = screen.WorkingArea.Height / 1.5f;
            timer = 0;
            noiseX = new ValueNoise1D(rnd.Next());
            noiseY = new ValueNoise1D(rnd.Next());
            MakeClickThrough();
            player.Play();
        }

        int NextRnd(int min, int max, ValueNoise1D noise)
        {
            int v= (int)((noise.eval(timer / 1000f) * (max-min))+min);
            return v;
        }

        public void Tick()
        {
            if (timer >= 1000) return;
            timer += 30;
            if (timer >= 1000)
            {
                MakeClickable();
                timer = 1000;
            }
            parent.Left = startX + NextRnd(-StrayByTime(timer, shakeAmountX), StrayByTime(timer, shakeAmountX) +1, noiseX);
            parent.Top = startY + NextRnd(-StrayByTime(timer, shakeAmountY), StrayByTime(timer, shakeAmountY) +1, noiseY);
        }

        private float EaseInOutQuint(float x)
        {
            return (x < 0.5f ? 16 * x * x * x * x : 1 - (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) * (-2 * x + 2) / 2) * (x > 2f ? 10f : 1f );
        }

        public int StrayByTime(int time, float factor)
        {
            return (int)(EaseInOutQuint(1-time/1000f)*factor);
        }


        public void MakeClickThrough()
        {
            SetWindowLong(parent.Handle, -20, 0x20 | 0x80000);
            SetLayeredWindowAttributes(parent.Handle, 0, 255, 2);
            parent.TopMost = true;
        }

        public void MakeClickable()
        {
            SetWindowLong(parent.Handle, -20, 0);
            parent.TopMost = false;
        }

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll")]
        static extern long SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
    }
}