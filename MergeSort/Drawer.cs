using MergeSortSteps;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MergeSort
{
    internal class Drawer
    {
        Action RequestUpdate;
        SKPaint[] grayscalePaints;
        MergeSortStepPreRenderer step;
        int idx = 0;
        const float squareHalfHeight = 10;
        public Drawer(int[] items, float squareHalfWidth, float splitWidth, Action Invalidate)
        {
            RequestUpdate = Invalidate;
            MergeSorterWithSteps msws = new MergeSorterWithSteps(items);
            msws.Sort();
            step = new MergeSortStepPreRenderer(msws, squareHalfWidth, splitWidth);
            step.CompleteRender();
            grayscalePaints = new SKPaint[10];
            for (int i = 0; i < 10; i++)
            {
                grayscalePaints[i] = new SKPaint { Color = SKColor.FromHsv(0, 0, 100f - (i*10f)), IsAntialias = true, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Center, TextSize = 17 };
            }
        }

        SKPaint black = new SKPaint { Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Center, TextSize = 17 };
        SKPaint blackOutline = new SKPaint { Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Stroke, TextAlign = SKTextAlign.Center};
        private void DrawNumber(int number, SKPoint center, SKCanvas canvas)
        {
            canvas.DrawRect(new SKRect(center.X - step.squareHalfWidth, center.Y - squareHalfHeight, center.X + step.squareHalfWidth, center.Y + squareHalfHeight), blackOutline);
            canvas.DrawText(number.ToString(), center + new SKPoint(0, 6), black);
        }

        private float InterpolateFloat(float a, float b, float x)
        {
            return (a * (1f - x)) + (b * x);
        }

        private void DrawInterpolatedNumber(int number, SKPoint center1, SKPoint center2, float progress, SKCanvas canvas)
        {
            DrawNumber(number, new SKPoint(InterpolateFloat(center1.X, center2.X, progress), InterpolateFloat(center1.Y, center2.Y, progress)), canvas);
        }

        private SKPoint GetPointFromMergePoint(MergeSortStepPreRenderer.MergeSortValuePoint pnt)
        {
            return new SKPoint(pnt.positionX + 100, pnt.isTempRow ? 70 : 100);
        } 

        public void Paint(SKCanvas canvas)
        {
            if (Animating)
            {
                for (int i = 0;i < step.points[idx].Length;i++)
                {
                    DrawInterpolatedNumber(step.points[idx][i].value, GetPointFromMergePoint(step.points[idx][i]), GetPointFromMergePoint(step.points[animatingTo][i]), animationProgress, canvas);
                }
            }
            else
            {
                foreach (MergeSortStepPreRenderer.MergeSortValuePoint point in step.points[idx])
                {
                    DrawNumber(point.value, GetPointFromMergePoint(point), canvas);
                }
            }
        }

        float animationProgress = 0f;
        public bool Animating { get; private set; } = false;
        int animatingTo = 0;

        public bool AnimateTo(int target, bool restartAnimation = false)
        {
            if (target < 0 || target >= step.points.Count) return false;
            if (Animating) return false;

            Animating = true;
            if (restartAnimation) animationProgress = 0f;
            animatingTo = target;
            RequestUpdate();
            return true;
        }

        public int Current => idx;
        private float GetProgressForIdx(int idx)
        {
            return idx / (step.points.Count - 1f);
        }
        public float FullProgress => Animating ? InterpolateFloat(GetProgressForIdx(Current), GetProgressForIdx(animatingTo), animationProgress) : GetProgressForIdx(Current);
        public void Tick()
        {
            if (Animating)
            {
                animationProgress += 0.3f;
                if (animationProgress >= 1f)
                {
                    animationProgress -= 1f;
                    idx = animatingTo;
                    Animating = false;
                }
                RequestUpdate();
            }
        }
    }
}
