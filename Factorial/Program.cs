using System;
using System.Collections.Generic;
using System.Text;

namespace Factorial
{
    class Program
    {
        static ulong Exp(ulong a, ulong b, ulong modulo)
        {
            ulong result = 1;
            ulong moc = a;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    result = (result * moc) % modulo;
                }
                b /= 2;
                moc = (moc * moc) % modulo;
            }
            return result;
        }
        static List<ulong> DFT_SLOW(List<ulong> x)
        {
            return x;
        }
        static List<ulong> FFT(List<ulong> x, ulong W, ulong modulo)
        {
            int N = x.Count;
            if (N <= 1)
            {
                return DFT_SLOW(x);
            }
            if (N % 2 == 1)
            {
                Console.WriteLine("Try with size of x which is power of 2");
                System.Environment.Exit(1);
            }
            var x_even = new List<ulong>();
            var x_odd = new List<ulong>();
            for(int i = 0; i < x.Count; i += 2)
            {
                x_even.Add(x[i]);
                x_odd.Add(x[i + 1]);
            }
            var W_inv = Exp(W, modulo - 2, modulo);
            var X_even = FFT(x_even, (W*W)%modulo, modulo);
            var X_odd = FFT(x_odd, (W * W) % modulo, modulo);
            var factor = new List<ulong>();
            for(ulong k = 0; k < (ulong)x.Count; k++)
            {
                factor.Add(Exp(W_inv, k, modulo));
            }
            var result = new List<ulong>();
            for( int i = 0; i < factor.Count/2; i++)
            {
                result.Add((X_even[i] + (X_odd[i] * factor[i]) % modulo) % modulo);
            }
            for (int i = 0; i < factor.Count / 2; i++)
            {
                result.Add((X_even[i] + (X_odd[i] * factor[i+factor.Count/2]) % modulo) % modulo);
            }
            return result;
        }
        static List<ulong> Compactifi(List<ulong> A)
        {
            ulong more = (ulong)0;
            for(int i = 0; i < A.Count; i++)
            {
                A[i] += more/100000;
                more = (A[i]) - (A[i]) % 100000;
                A[i] %= 100000;
            }
            return A;
        }
        static int Nxt_pow_two(int x)
        {
            if(x == 0)
            {
                return 0;
            }
            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x++;
            return x;
        }
        static List<ulong> Rozsir(List<ulong> A)
        {
            int index = A.Count-1;
            while(index > 0 && A[index] == 0)
            {
                index -= 1;
            }
            index = Nxt_pow_two(index+1)*2;
            while(A.Count < index)
            {
                A.Add(0);
            }
            while(A.Count > index)
            {
                A.RemoveAt(A.Count - 1);
            }
            return A;
        }
        static List<ulong> MLT(List<ulong> A, List<ulong> B, ulong modulo)
        {
            A = Rozsir(A);
            B = Rozsir(B);
            while(A.Count < B.Count)
            {
                A.Add(0);
            }
            while(B.Count < A.Count)
            {
                B.Add(0);
            }
            var N = (ulong)A.Count;
            ulong W = Exp((ulong)199, ((ulong)1 << 25) / N, modulo);
            var F_A = FFT(A, W, modulo);
            var F_B = FFT(B, W, modulo);
            var C = new List<ulong>();
            for(int i = 0; i < F_A.Count; i++)
            {
                C.Add((F_A[i] * F_B[i]) % modulo);
            }
            var tmp = FFT(C, Exp(W, modulo - 2, modulo), modulo);
            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i] = (tmp[i] * Exp(N, modulo-2, modulo))%modulo;
            }
            return Compactifi(tmp);
        }
        static List<ulong> Num_to_List (ulong x)
        {
            var res = new List<ulong>();
            while(x > 0)
            {
                res.Add(x % 100000);
                x /= 100000;
            }
            return res;
        }
        static void Main(string[] args)
        {
            ulong modulo = 125 * ((ulong)1 << 25) + 1;
            char[] delims = null;
            string[] array = Console.ReadLine().Split(delims, StringSplitOptions.RemoveEmptyEntries);
            var numbers = new List<int>();
            foreach (string s in array)
            {
                numbers.Add(int.Parse(s));
            }
            int N = numbers[0];
            var res = Num_to_List(1);
            for (int i = 2; i < N+1; i++)
            {
                res = MLT(res, Num_to_List((ulong)i), modulo);
            }
            int index = res.Count - 1;
            while (index > 0 && res[index] == 0)
            {
                index -= 1;
            }
            for (int i = index; i >= 0; i--)
            {
                if (i < index && res[i] < 10000)
                {
                    Console.Write(0);
                }
                if (i < index && res[i] < 1000)
                {
                    Console.Write(0);
                }
                if (i < index && res[i] < 100)
                {
                    Console.Write(0);
                }
                if (i < index && res[i] < 10)
                {
                    Console.Write(0);
                }
                Console.Write(res[i]);
            }
            Console.Write("\n");
        }
    }
}
