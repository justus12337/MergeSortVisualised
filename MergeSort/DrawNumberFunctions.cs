using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    public static class DrawNumberFunctions
    {
        const float squareHalfHeight = 10;

        static SKPaint black = new SKPaint { Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Center, TextSize = 17 };
        static SKPaint blackOutline = new SKPaint { Color = SKColors.Black, IsAntialias = true, Style = SKPaintStyle.Stroke, TextAlign = SKTextAlign.Center };
        static SKPaint[] grayscalePaints;
        static SKPaint[] GrayscalePaints
        {
            get
            {
                if (grayscalePaints == null)
                {
                    grayscalePaints = new SKPaint[100];
                    for (int i = 0;i<grayscalePaints.Length;i++)
                    {
                        grayscalePaints[i] = new SKPaint { Color = SKColor.FromHsv(0, 0, 1f*i/grayscalePaints.Length * 90f), IsAntialias = false, Style = SKPaintStyle.Fill };
                    }
                }
                return grayscalePaints;
            }
        }

        public static void DrawNumber(int number, SKPoint center, SKCanvas canvas, MergeSortStepPreRenderer step)
        {
            canvas.DrawRect(new SKRect(center.X - step.squareHalfWidth, center.Y - squareHalfHeight, center.X + step.squareHalfWidth, center.Y + squareHalfHeight), blackOutline);
            canvas.DrawText(number.ToString(), center + new SKPoint(0, 6), black);
        }
        public static void DrawGrayscaleLine(int number, SKPoint center, SKCanvas canvas, MergeSortStepPreRenderer step)
        {
            canvas.DrawRect(new SKRect(center.X - step.squareHalfWidth, center.Y - squareHalfHeight, center.X + step.squareHalfWidth, center.Y + squareHalfHeight), GrayscalePaints[number]);
        }

        public delegate void NumberDrawingFunction(int number, SKPoint center, SKCanvas canvas, MergeSortStepPreRenderer step);
    }
}
