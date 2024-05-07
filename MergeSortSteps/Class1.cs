using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSortSteps
{
    public class MergeSorterWithSteps
    {
        public abstract class MergeSortStep {}
        public class MergeSortSplitStep : MergeSortStep
        {
            public int idx;
            public MergeSortSplitStep(int idx)
            {
                this.idx = idx;
            }
        }
        public class MergeSortBeginJoinStep : MergeSortStep
        {
            public int idx;
            public int count;
            public MergeSortBeginJoinStep(int idx, int count)
            {
                this.idx = idx;
                this.count = count;
            }
        }
        public class MergeSortJoinTakeStep : MergeSortStep
        {
            public int idx;
            public MergeSortJoinTakeStep(int idx)
            {
                this.idx = idx;
            }
        }
        public class MergeSortEndJoinStep : MergeSortStep
        {
        }
        public int[] originalItems;
        public int[] items;
        int[] internalSpace;
        public List<MergeSortStep> steps;
        public MergeSorterWithSteps(int[] items)
        {
            originalItems = items.ToArray(); // Clone
            this.items = items.ToArray();
            steps = new List<MergeSortStep>();
        }
        bool sorted = false;
        private void DoMergeSort(int idxStart, int count)
        {
            if (count < 2) return;
            int middle = count / 2;
            steps.Add(new MergeSortSplitStep(idxStart+middle));
            DoMergeSort(idxStart, middle);
            DoMergeSort(idxStart + middle, count - middle);
            Array.ConstrainedCopy(items, idxStart, internalSpace, 0, count);
            int i1 = 0;
            int i2 = 0;
            steps.Add(new MergeSortBeginJoinStep(idxStart, count));
            while (i1 < middle || i2 < (count - middle))
            {
                if (i2 >= (count - middle) || (i1 < middle && internalSpace[i1] <= internalSpace[middle + i2]))
                {
                    items[idxStart + i1 + i2] = internalSpace[i1];
                    steps.Add(new MergeSortJoinTakeStep(idxStart + i1));
                    i1++;
                } else
                {
                    items[idxStart + i1 + i2] = internalSpace[middle + i2];
                    steps.Add(new MergeSortJoinTakeStep(idxStart + middle + i2));
                    i2++;
                }
            }
            steps.Add(new MergeSortEndJoinStep());
        }
        public bool Sort()
        {
            if (sorted) return false;
            internalSpace = new int[items.Length];
            DoMergeSort(0, items.Length);
            internalSpace = null;
            sorted = true;
            return true;
        }
    }
}
