using System;
using System.Linq;

namespace lab21
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int multiplicand = 55; 
            int multiplier = 47; 
            
            BoothMultiply(multiplicand, multiplier);

            int decimalResult = multiplicand * multiplier;

            Console.WriteLine($"{multiplicand} * {multiplier} is {decimalResult}");
        }

        static int BoothMultiply(int multiplicand, int multiplier)
        {
            int[] m = ToBinary(multiplicand);
            int[] m2 = ToBinary(-multiplicand); 
            int[] q = ToBinary(multiplier); 

            int[] Addition = new int[17]; 
            int[] Subtraction = new int[17]; 
            int[] ProductReg = new int[17]; 

            for (int i = 0; i < 16; i++)
            {
                Addition[i] = m[i];
                Subtraction[i] = m2[i];
            }

            for (int i = 8; i <= 16; i++)
            {
                ProductReg[i] = q[i - 8];
            }

            ProductReg[16] = 0;

            ShowSteps(Addition, "Addition");
            ShowSteps(Subtraction, "Subtraction");
            ShowSteps(ProductReg, "ProductReg");
            Console.WriteLine(" ");
           
            for (int i = 0; i < 8; i++)
            {
                if (ProductReg[15] == 0 && ProductReg[16] == 0){}
                if (ProductReg[15] == 1 && ProductReg[16] == 1){} // знаходимся в середині області одиниць
                if (ProductReg[15] == 1 && ProductReg[16] == 0) // починається група одиниць
                {                 
                    ShowSteps(Subtraction, "Subtraction"); // в цей момент ми можемо здійснити віднімання
                    Add(ProductReg, Subtraction); // віднімаємо перший множник від результата
                }
                if (ProductReg[15] == 0 && ProductReg[16] == 1) // ми дійшли до кінця області одиниць
                {  
                    ShowSteps(Addition, "Addition"); 
                    Add(ProductReg, Addition); // потрібно додати перший множник до нашого результата
                }
                
                ShiftRight(ProductReg); // автоматично робимо зсув
                ShowSteps(ProductReg, "ProductReg"); // відображення проміжного результату
                Console.WriteLine();
            }

            string original = string.Join("", ProductReg);

            if (original.Length >= 17)
            {
                string binaryResult = original.Remove(original.Length - 1);
                Console.WriteLine("Binary Result: " + binaryResult);
                return ConvertDecimal(ProductReg);
            }
            else
            {
                Console.WriteLine("Binary Result: " + original);
                return ConvertDecimal(ProductReg);
            }
        }

        static void ShiftRight(int[] X)
        {
            for (int i = 16; i >= 1; i--)
            {
                X[i] = X[i - 1];
            }
        }

        static int ConvertDecimal(int[] Y)
        {
            int m = 0;
            int n = 1;

            if (Y[0] == 1){}

            for (int i = 15; i >= 0; i--, n *= 2)
            {
                m += (Y[i] * n);
            }

            if (m > 64)
            {
                m = -(256 - m);
                return m;
            }

            return m;
        }

        static void Add(int[] X, int[] Y)
        {
            int carryBit = 0;
            for (int i = 8; i >= 0; i--)
            {
                int temporary = X[i] + Y[i] + carryBit;
                X[i] = temporary % 2;
                carryBit = temporary / 2;
            }
        }

        static int[] ToBinary(int number)
        {
            int n = number;

            var bin = Convert.ToString(n, 2);

            if (n < 0)
            {
                if (bin.Length == 32)
                {
                    bin = bin.Remove(1, 24);
                    bin = bin.PadRight(16, '0');
                }
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }

            if (bin.Length == 32)
            {
                bin = bin.Remove(0, 8);
                bin = bin.PadRight(16, '0');
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }

            if (bin.Length < 8)
            {
                bin = bin.PadLeft(8, '0');
                bin = bin.PadRight(16, '0');
                return bin.Select(c => int.Parse(c.ToString())).ToArray();
            }
            return bin.Select(c => int.Parse(c.ToString())).ToArray();
        }

        static void ShowSteps(int[] X, string name)
        {
            Console.Write(name + ": ");

            for (int i = 0; i < X.Length; i++)
            {

                if (i == 8)
                {
                    Console.Write(" ");
                }

                if (i == 16)
                {
                    Console.Write(" ");
                }

                Console.Write(X[i]);
            }
        }
    }
}
