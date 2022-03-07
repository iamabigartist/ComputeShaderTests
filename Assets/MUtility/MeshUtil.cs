using System.Runtime.CompilerServices;
namespace MUtility
{
    public static class MeshUtil
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void Decompose_3_CPU(int source, int size, double reverse_size, out int result, out int remain_source)
        {
            remain_source = source % size;
            result = (int)((source - remain_source) * reverse_size);
        }

        /// <summary>
        ///     Arbitrary rank decompose all, inefficient because of array.
        /// </summary>
        /// <remarks>Note that size array should be the product</remarks>
        public static void Decompose_3_CPU_All(int source, int[] size, double[] reverse_size, out int[] result)
        {
            result = new int[size.Length];
            int cur_source = source;

            for (int i = 0; i < size.Length; i++)
            {
                Decompose_3_CPU( cur_source, size[i], reverse_size[i], out result[i], out cur_source );
            }
        }
    }
}
