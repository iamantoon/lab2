using System;

namespace lab22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dividend = "10000"; 
            string divisor = "1000"; 

            string quotient = Divide(dividend, divisor);
            Console.WriteLine("Ділене: " + dividend);
            Console.WriteLine("Дільник: " + divisor); 
            Console.WriteLine("Частка: " + quotient);
        }

        static string Divide(string dividend, string divisor)
        {
            if (divisor == "0")
            {
                throw new DivideByZeroException();
            }

            string quotient = "";
            string remainder = "";

            bool isNegative = (dividend[0] == '1') ^ (divisor[0] == '1');

            string ldividend = dividend[0] == '1' ? dividend.Substring(1) : dividend;
            string ldivisor = divisor[0] == '1' ? divisor.Substring(1) : divisor;

            for (int i = 0; i < ldividend.Length; i++) // Спочатку ітеруємося по кожному біту діленого числа
            {
                remainder += ldividend[i]; // На кожній ітерації додаємо наступний біт діленого до решти
                if (Compare(remainder, ldivisor) >= 0) // Після додавання перевіряємо, чи результат ділення на дільник більший або рівний 0
                {
                    quotient += '1'; // Якщо так, це означає, що ділене число більше або дорівнює дільнику. У цьому випадку додається біт 1 до частки (quotient)
                    remainder = Subtract(remainder, ldivisor); // і з решти віднімається дільник
                }
                else // Якщо результат ділення на дільник менший за 0, це означає, що ділене число менше за дільник.
                {
                    quotient += '0'; //  У цьому випадку додається біт 0 до частки
                }
            }

            return isNegative ? "-" + quotient : quotient;
        }

        static int Compare(string a, string b)
        {
            if (a.Length > b.Length)
                return 1;
            if (a.Length < b.Length)
                return -1;
            return string.Compare(a, b);
        }

        static string Subtract(string a, string b)
        {
            string result = "";
            int i = a.Length - 1;
            int j = b.Length - 1;
            int borrow = 0;

            while (i >= 0)
            {
                int difference = (a[i--] - '0') - borrow;
                if (j >= 0) difference -= b[j--] - '0'; 

                if (difference < 0)
                {
                    difference += 2;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result = difference + result;
            }

            return result.TrimStart('0');
        }
    }
}
