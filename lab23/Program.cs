using System;

namespace lab23
{
    internal class Program
    {
        static void Main(string[] args)
        {
            float a = 11.125f;
            float b = 18.75f;

            Console.WriteLine($"Бітове представлення числа {a}: {GetBinary(a)}");
            Console.WriteLine($"Бітове представлення числа {b}: {GetBinary(b)}");

            float result = Add(a, b);
            Console.WriteLine($"Результат додавання: {a} + {b} = {result}");

            Console.WriteLine("Бітове представлення результата: {0}", GetBinary(result));
        }

        static string GetBinary(float num)
        {
            byte[] bytes = BitConverter.GetBytes(num);
            int integerRepresentation = BitConverter.ToInt32(bytes, 0);
            string binaryString = Convert.ToString(integerRepresentation, 2).PadLeft(32, '0');

            string result = "";
            for (int i = 0; i < binaryString.Length; i++)
            {
                result += binaryString[i];
                if (i == 22 || i == 30)
                    result += " ";
            }
            return result;
        }

        static float Add(float a, float b)
        {
            uint bits_a = BitConverter.ToUInt32(BitConverter.GetBytes(a), 0);
            uint bits_b = BitConverter.ToUInt32(BitConverter.GetBytes(b), 0);

            int sign_a = (int)((bits_a >> 31) & 1);
            int sign_b = (int)((bits_b >> 31) & 1);
            int exponent_a = (int)((bits_a >> 23) & 0xFF);
            int exponent_b = (int)((bits_b >> 23) & 0xFF);
            int mantissa_a = (int)(bits_a & 0x7FFFFF);
            int mantissa_b = (int)(bits_b & 0x7FFFFF);

            mantissa_a |= 0x800000;
            mantissa_b |= 0x800000;

            if (exponent_a > exponent_b)
            {
                mantissa_b = mantissa_b >> (exponent_a - exponent_b);
                exponent_b = exponent_a;
            }
            else
            {
                mantissa_a = mantissa_a >> (exponent_b - exponent_a);
                exponent_a = exponent_b;
            }

            int sign_result = sign_a;
            int exponent_result = exponent_a;
            int mantissa_result = mantissa_a + mantissa_b;

            if ((mantissa_result & 0x800000) != 0)
            {
                while (((mantissa_result & 0x7FFFFF) >> 23) != 1 && (mantissa_result >> 24 > 0))
                {
                    mantissa_result = mantissa_result >> 1;
                    exponent_result++;
                }
            }

            uint result = (uint)((sign_result << 31) | (exponent_result << 23) | (mantissa_result & 0x7FFFFF));

            return BitConverter.ToSingle(BitConverter.GetBytes(result), 0);
        }
    }
}
