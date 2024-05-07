using MergeSortSteps;

using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    public class MergeSortStepPreRenderer
    {
        public struct MergeSortValuePoint
        {
            public float positionX;
            public bool isTempRow;
            public int value;
        }
        MergeSortStepper step;
        public List<MergeSortValuePoint[]> points;
        public float squareHalfWidth;
        public float splitWidth;
        public MergeSortStepPreRenderer(MergeSorterWithSteps msws, float squareHalfWidth, float splitWidth)
        {
            step = new MergeSortStepper(msws);
            points = new List<MergeSortValuePoint[]>();
            this.squareHalfWidth = squareHalfWidth;
            this.splitWidth = splitWidth;
        }

        void Render()
        {
            MergeSortValuePoint[] currentPoints = new MergeSortValuePoint[step.items.Length];
            for (int i = 0; i < currentPoints.Length; i++)
            {
                currentPoints[i] = new MergeSortValuePoint { positionX = -5000 };
            }
            points.Add(currentPoints);

            int splits = 0;
            for (int i = 0; i < step.items.Length; i++)
            {
                if (step.splits[i] && !((step.joinIndex + 1) <= i && (step.joinIndex + step.joinCount) > i)) splits++;
                if (step.joining && (step.joinIndex + step.joinIndexCurrent) <= i && (step.joinIndex + step.joinCount) > i) continue;
                //if (currentPoints[step.items[i].fixedIndex].positionX != -5000) Console.WriteLine("Overwriting value, iteration 1 - " + step.items[i].fixedIndex + " in " + step.currentStep);
                currentPoints[step.items[i].fixedIndex] = new MergeSortValuePoint { positionX = (2 * squareHalfWidth * i) + (splits * splitWidth), isTempRow = false, value = step.items[i].value };
            }
            splits = 0;
            if (step.joining)
            {
                for (int i = step.joinIndex; i < step.joinCount + step.joinIndex; i++)
                {
                    if (step.splits[i]) splits++;
                    if (i < (step.joinSplit + step.joinIndex))
                    {
                        if (i < (step.joinIndex + step.joinIndex1)) continue;
                    }
                    else
                    {
                        if (i < (step.joinIndex + step.joinIndex2 + step.joinSplit)) continue;
                    }
                    //if (currentPoints[step.internalSpace[i].fixedIndex].positionX != -5000) Console.WriteLine("Overwriting value, iteration 2 - " + step.internalSpace[i].fixedIndex + " in " + step.currentStep);
                    currentPoints[step.internalSpace[i].fixedIndex] = new MergeSortValuePoint { positionX = (2 * squareHalfWidth * i) + (splits * splitWidth), isTempRow = true, value = step.internalSpace[i].value };
                }
            }
        }

        public void CompleteRender()
        {
            if (step == null) return;
            do { Render(); } while (step.DoStep());
            step = null;
        }
    }
}
