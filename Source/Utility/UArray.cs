namespace VE.Pawns.Utility
{
    public class UArray<T1>
    {
        public T1[] Array;

        public int binarySearch(T1 value)
        {
            int first = 0;

            int last = Array.Length - 1;

            int position = -1;

            bool found = false;

            int middle;

            while (found == false)
            {
                middle = (first + last) / 2;

                if (Array[middle] == value)
                {
                    found = true;
                    position = middle;
                }
                else if (Array[middle] > value)
                {
                    last = middle - 1;
                }
                else
                {
                    first = middle + 1;
                }
            }

            return position;
        }
    }
}