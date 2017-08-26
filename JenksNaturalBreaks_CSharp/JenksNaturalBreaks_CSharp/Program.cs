using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;


namespace JenksNaturalBreaks_CSharp
{
    public class JenksNaturalBreaks_CSharp
    {
        const int MaxSize = 10000;
        const int MaxClasses = 255;

        static int[,] Mat1;
        static float[,] Mat2;
        
        static JenksNaturalBreaks_CSharp()
        {
            Mat1 = new int[MaxSize+1, MaxClasses+1];
            Mat2 = new float[MaxSize + 1, MaxClasses + 1];
        }
        static void Main(string[] args)
        {
            const int N = 10000;
            int[] a = new int[N];// {9, 99, 147, 148, 177, 186, 186, 197, 243, 250 };
            //int[] a = new int[] { 1, 2, 4, 4, 10 };
            Random rand = new Random();
            for (int i = 0; i < N; i++)
            {
                a[i] = rand.Next(0, 256);
            }
            
            int k = 12;
            
            DateTime start = DateTime.Now;
            int[] breaks1 = GetBreaks(a, k);
            Console.WriteLine(DateTime.Now - start);

            start = DateTime.Now;
            int[] breaks2 = GetBreaksFast(a, k);
            Console.WriteLine(DateTime.Now - start);

            for (int i = 0; i <= k; i++)
            {
                if (breaks1[i] != breaks2[i])
                {
                    Console.WriteLine("FAIL!!!");
                    for (int j = 0; j < N; j++)
                    {
                        Console.Write(a[j] + " ");
                    }
                    break;
                    
                }
            }
        }

        static int[] GetBreaks( int[] values, int num_classes)
        {
            int num_vals = values.Length;

            //1. sorting values
            Array.Sort(values);

            //2. initializing Matrixes
            for (int j = 0; j < (num_vals+1); j++)
            {
                for (int i = 0; i < (num_classes+1); i++)
                {
                    Mat1[j, i] = 0;
                    Mat2[j, i] = 0;
                    if ((j == 1) && (i > 0)) Mat1[j, i] = 1;
                    if ((j > 1) && (i > 0)) Mat2[j, i] = float.MaxValue;
                }
                
            }

            //3. main calculation
            float v = 0;
            for (int l = 2; l < num_vals + 1; l++)
            {
                float s1 = 0;
                float s2 = 0;
                int w = 0;
                for (int m = 1; m < l + 1; m++)
                {
                    int i3 = l - m + 1;
                    int val = values[i3 - 1];
                    s2 += val * val;
                    s1 += val;
                    w += 1;
                    v = s2 - (s1 * s1) / w;
                    int i4 = i3 - 1;
                    if (i4 != 0)
                        for (int j = 2; j < num_classes + 1; j++)
                        {
                            if (Mat2[l, j] >= (v + Mat2[i4, j - 1]))
                            {
                                Mat1[l, j] = i3;
                                Mat2[l, j] = v + Mat2[i4, j - 1];
                            }
                        }
                }
                Mat1[l, 1] = 1;
                Mat2[l, 1] = v;
            }

            /*for (int j = 0; j < (num_vals + 1); j++)
            {
                Console.WriteLine();
                for (int i = 0; i < (num_classes + 1); i++)
                {
                    Console.Write(Mat2[j, i] + " ");
                }
            }*/

            int[] result = new int[num_classes + 1];
            result[0] = values[0];
            result[num_classes] = values[num_vals-1];
            int countNum = num_classes;
            int k = num_vals;
            while (countNum >= 2)
            {
                int index = Mat1[k,countNum] - 2;
                result[countNum - 1] = values[index];
                k = Mat1[k,countNum] - 1;
                countNum -= 1;
            }

            return result;
        }
        static public int[] GetBreaksFast(int[] values, int num_classes)
        {
            //1. sorting values
            Array.Sort(values);
            
            int max_value = values[values.Length - 1];
            int min_value = values[0];
            int[] freq = new int[max_value + 1];
            for (int i = 0; i < values.Length; i++)
                freq[values[i]]++;

            int num_vals = freq.Length;
            for (int i = 0; i < freq.Length; i++)
                if (freq[i] == 0) num_vals--;
            
            values = new int[num_vals];
            num_vals = 0;
            int index = 0;
            while (num_vals < values.Length)
            {
                while (freq[index] == 0) index++;
                values[num_vals] = index;
                index++;
                num_vals++;
            }

            //2. initializing Matrixes
            for (int j = 0; j < (num_vals + 1); j++)
            {
                for (int i = 0; i < (num_classes + 1); i++)
                {
                    Mat1[j, i] = 0;
                    Mat2[j, i] = 0;
                    if ((j == 1) && (i > 0)) Mat1[j, i] = 1;
                    if ((j > 1) && (i > 0)) Mat2[j, i] = float.MaxValue;
                }
            }

            //3. main calculation
            float v = 0;
            for (int l = 2; l < num_vals + 1; l++)
            {
                float s1 = 0;
                float s2 = 0;
                int w = 0;
                for (int m = 1; m < l + 1; m++)
                {
                    int i3 = l - m + 1;
                    int val = values[i3 - 1];
                    s2 += val * val * freq[val];
                    s1 += val * freq[val];
                    w += freq[val];
                    v = s2 - (s1 * s1) / w;
                    int i4 = i3 - 1;
                    if (i4 != 0)
                        for (int j = 2; j < num_classes + 1; j++)
                        {
                            if (Mat2[l, j] >= (v + Mat2[i4, j - 1]))
                            {
                                Mat1[l, j] = i3;
                                Mat2[l, j] = v + Mat2[i4, j - 1];
                            }
                        }
                }
                Mat1[l, 1] = 1;
                Mat2[l, 1] = v;
            }

            /*for (int j = 0; j < (num_vals + 1); j++)
            {
                Console.WriteLine();
                for (int i = 0; i < (num_classes + 1); i++)
                {
                    Console.Write(Mat2[j, i] + " ");
                }
            }*/

            int[] result = new int[num_classes + 1];
            result[0] = min_value;
            result[num_classes] = max_value;
            int countNum = num_classes;
            int k = num_vals;
            while (countNum >= 2)
            {
                index = Mat1[k, countNum] - 2;
                result[countNum - 1] = values[index];
                k = Mat1[k, countNum] - 1;
                countNum -= 1;
            }

            return result;
        }
    }
}
