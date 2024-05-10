using MergeSortSteps;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    public class MergeSortStepper
    {
        public struct StepItem
        {
            public int fixedIndex; // this is needed for animation
            public int value;
        }
        MergeSorterWithSteps sorter;
        public MergeSortStepper(MergeSorterWithSteps sorter)
        {
            this.sorter = sorter;
            items = new StepItem[sorter.originalItems.Length];
            for (int i = 0;i<items.Length;i++)
            {
                items[i] = new StepItem { fixedIndex = i, value = sorter.originalItems[i] };
            }
            splits = new bool[items.Length];
            internalSpace = new StepItem[items.Length];
        }
        public bool[] splits;
        public StepItem[] items;
        public StepItem[] internalSpace;
        public int currentStep = 0;
        public bool joining = false;
        public int joinIndex = 0;
        public int joinCount = 0;
        public int joinIndex1 = 0;
        public int joinIndex2 = 0;
        public int joinSplit = 0;
        public int joinIndexCurrent => joinIndex1 + joinIndex2;

        public bool DoStep()
        {
            if (currentStep >= sorter.steps.Count) return false;
            MergeSorterWithSteps.MergeSortStep step = sorter.steps[currentStep++];
            if (step is MergeSorterWithSteps.MergeSortSplitStep splitStep)
            {
                splits[splitStep.idx] = true;
            }
            else if (step is MergeSorterWithSteps.MergeSortBeginJoinStep beginJoinStep)
            {
                joinIndex = beginJoinStep.idx;
                joinIndex1 = joinIndex2 = 0;
                joinCount = beginJoinStep.count;
                joining = true;
                for (int i = 1; i < joinCount; i++)
                {
                    if (splits[joinIndex + i]) { joinSplit = i; break; }
                }
                Array.Copy(items, internalSpace, items.Length);
            }
            else if (step is MergeSorterWithSteps.MergeSortJoinTakeStep joinTakeStep)
            {
                items[joinIndex + joinIndexCurrent] = internalSpace[joinTakeStep.idx];
                if (joinTakeStep.idx < joinSplit + joinIndex) joinIndex1++;
                else joinIndex2++;
            }
            else if (step is MergeSorterWithSteps.MergeSortEndJoinStep)
            {
                for (int i = 1; i < joinCount; i++)
                {
                    splits[joinIndex + i] = false;
                }
                joining = false;
                DoStep();
            }
            return currentStep < sorter.steps.Count;
        }
    }
}
