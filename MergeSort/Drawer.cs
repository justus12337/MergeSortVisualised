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
        private bool noAnimations;
        public Drawer(int[] items, float squareHalfWidth, float splitWidth, Action Invalidate, DrawNumberFunctions.NumberDrawingFunction drawNumber, bool ignoreSplits = false, bool skipAnimations = false)
        {
            noAnimations = skipAnimations;
            DrawNumber = drawNumber;
            RequestUpdate = Invalidate;
            MergeSorterWithSteps msws = new MergeSorterWithSteps(items);
            msws.Sort();
            step = new MergeSortStepPreRenderer(msws, squareHalfWidth, splitWidth, ignoreSplits);
            step.CompleteRender();
            grayscalePaints = new SKPaint[10];
            for (int i = 0; i < 10; i++)
            {
                grayscalePaints[i] = new SKPaint { Color = SKColor.FromHsv(0, 0, 100f - (i*10f)), IsAntialias = true, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Center, TextSize = 17 };
            }
        }

        private DrawNumberFunctions.NumberDrawingFunction DrawNumber;

        private float LerpFloat(float a, float b, float x)
        {
            return (a * (1f - x)) + (b * x);
        }

        private void DrawInterpolatedNumber(int number, SKPoint center1, SKPoint center2, float progress, SKCanvas canvas)
        {
            DrawNumber(number, new SKPoint(LerpFloat(center1.X, center2.X, progress), LerpFloat(center1.Y, center2.Y, progress)), canvas, step);
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
                    DrawNumber(point.value, GetPointFromMergePoint(point), canvas, step);
                }
            }
        }

        float animationProgress = 0f;
        public bool Animating { get => animating && !noAnimations; private set => animating = value; }
        int animatingTo = 0;
        private bool animating = false;

        public bool AnimateTo(int target, bool restartAnimation = false)
        {
            if (target < 0 || target >= step.points.Count) return false;
            if (Animating) return false;

            if (noAnimations)
            {
                idx = target;
                RequestUpdate();
            }
            else
            {
                Animating = true;
                if (restartAnimation) animationProgress = 0f;
                animatingTo = target;
                RequestUpdate();
            }
            return true;
        }

        public int Current => idx;
        private float GetProgressForIdx(int idx)
        {
            return idx / (step.points.Count - 1f);
        }
        public float FullProgress => Animating ? LerpFloat(GetProgressForIdx(Current), GetProgressForIdx(animatingTo), animationProgress) : GetProgressForIdx(Current);
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
