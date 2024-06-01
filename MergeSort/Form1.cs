using MergeSortSteps;

using OpenTK.Graphics.ES20;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MergeSort
{
    public partial class Form1 : Form
    {
        SKPaint progressPaint = new SKPaint() { Color = SKColors.Black };
        SKPaint progressBgPaint = new SKPaint() { Color = SKColors.White };
        SKMatrix matrix = SKMatrix.CreateIdentity();
        SKPoint drawOffset = new SKPoint();
        float scale = 1f;
        Timer tick;
        Drawer drawer;
        Shaker shaker;
        int enableExplosions = 0;
        const string explode = "boom";

        int mode = -1;
        public Form1()
        {
            InitializeComponent();
            tick = new Timer();
            NextMode();
            tick.Tick += Update;
        }

        private void NextMode()
        {
            mode = (mode + 1) % 3;
            switch (mode)
            {
                case 0:
                    Init(50, 10, 10, 10, DrawNumberFunctions.DrawNumber);
                    break;
                case 1:
                    Init(100, 100, 10, 10, DrawNumberFunctions.DrawNumber);
                    break;
                case 2:
                    Init(100, 100, 1, 0, DrawNumberFunctions.DrawGrayscaleLine, true, true);
                    break;
            }
        }

        private void Init(int numberCount, int differentNumbers, float squareHalfWidth, float splitWidth, DrawNumberFunctions.NumberDrawingFunction drawingFunction, bool ignoreSplits = false, bool skipAnimations = false)
        {
            tick.Interval = 15;
            int[] items = new int[numberCount];
            Random rnd = new Random(0);
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = i % differentNumbers;
            }
            for (int i = 0; i < items.Length; i++)
            {
                int s = items[i];
                int sI = rnd.Next(items.Length);
                items[i] = items[sI];
                items[sI] = s;
            }
            drawer = new Drawer(items, squareHalfWidth, splitWidth, skiaView.Invalidate, drawingFunction, ignoreSplits, skipAnimations);
            shaker = new Shaker(this);
            skiaView.MouseWheel += SkiaView_MouseWheel;
            tick.Start();
        }

        private void SkiaView_MouseWheel(object sender, MouseEventArgs e)
        {
            float oldScale = scale;
            if (e.Delta > 1)
            {
                scale *= 1.025f;
            } else
            {
                scale /= 1.025f;
            }
            if (scale > 10f) scale = 10f;
            if (scale < 0.1f) scale = 0.1f;
            drawOffset = new SKPoint(e.Location.X + ((drawOffset.X - e.Location.X) * scale / oldScale), e.Location.Y + ((drawOffset.Y - e.Location.Y) * scale / oldScale));
            matrix = SKMatrix.CreateScaleTranslation(scale, scale, drawOffset.X, drawOffset.Y);
            skiaView.Invalidate();
        }

        private void Update(object sender, EventArgs e)
        {
            drawer?.Tick();
            shaker?.Tick();
            if (shouldReset) shouldReset = !drawer.AnimateTo(0, true);
            if (autoStep != 0) AnimateToForAutoStep(drawer.Current + autoStep);
        }

        private void skiaView_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.SetMatrix(matrix);
            // make sure the canvas is blank
            canvas.Clear(SKColors.White);

            // Draw in movable area here
            drawer.Paint(canvas);


            canvas.SetMatrix(SKMatrix.Identity);
            // Draw fixed to screen here
            canvas.DrawRect(0, 0, canvas.LocalClipBounds.Width, 3, progressBgPaint);
            canvas.DrawRect(0, 0, canvas.LocalClipBounds.Width * drawer.FullProgress, 3, progressPaint);
        }

        Point mouseLastPos;

        private void skiaView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None)
            {
                int diffX = e.X - mouseLastPos.X;
                int diffY = e.Y - mouseLastPos.Y;
                mouseLastPos = e.Location;
                drawOffset = new SKPoint(drawOffset.X + diffX, drawOffset.Y + diffY);
                matrix = SKMatrix.CreateScaleTranslation(scale, scale, drawOffset.X, drawOffset.Y);
                skiaView.Invalidate();
            }
        }

        private void skiaView_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLastPos = e.Location;
        }

        private void AnimateToForAutoStep(int idx)
        {
            if (drawer.Animating) return;
            if (!drawer.AnimateTo(idx)) {
                autoStep = 0;
            }
        }

        private void NextStep()
        {
            drawer.AnimateTo(drawer.Current + 1);
        }

        private void LastStep()
        {
            drawer.AnimateTo(drawer.Current - 1);
        }

        int autoStep = 0;
        bool shouldReset = false;

        private void skiaView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'a')
            {
                LastStep();
            }
            if (e.KeyChar == 'd')
            {
                NextStep();
            }
            if (e.KeyChar == 's')
            {
                autoStep = 0;
            }
            if (e.KeyChar == 'q')
            {
                autoStep = -1;
            }
            if (e.KeyChar == 'e')
            {
                autoStep = 1;
            }
            if (e.KeyChar == 'r')
            {
                autoStep = 0;
                shouldReset = true;
                if (enableExplosions == explode.Length) shaker.Init();
            }
            if (e.KeyChar == 'b')
            {
                matrix = SKMatrix.CreateIdentity();
                scale = 1f;
                drawOffset = SKPoint.Empty;
                skiaView.Invalidate();
            }
            if (e.KeyChar == 'n')
            {
                NextMode();
                skiaView.Invalidate();
            }

            // Explosions
            if (enableExplosions == explode.Length) return;
            if (e.KeyChar == explode[enableExplosions])
            {
                enableExplosions++;
            }
        }
    }
}
