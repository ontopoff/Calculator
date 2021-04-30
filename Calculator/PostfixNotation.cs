using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    public class PostfixNotation
    {
        public PostfixNotation()
        {
            operators = new List<string>(standart_operators);

        }
        private List<string> operators;
        private List<string> standart_operators =
            new List<string>(new string[] { "(", ")", "+", "-", "*", "/" });

        private IEnumerable<string> Separate(string input)
        {
            int pos = 0;
            while (pos < input.Length)
            {
                string s = string.Empty + input[pos];
                if (!standart_operators.Contains(input[pos].ToString()))
                {
                    if (Char.IsDigit(input[pos]) || Char.IsLetter(input[pos]))
                        for (int i = pos + 1; i < input.Length &&
                            (Char.IsDigit(input[i]) || Char.IsLetter(input[i]) || input[i] == ',' || input[i] == '.'); i++)
                            s += input[i];
                }
                yield return s;
                pos += s.Length;
            }
        }
        private byte GetPriority(string s)
        {
            switch (s)
            {
                case "(":
                case ")":
                    return 0;
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return 4;
            }
        }

        public string[] ConvertToPostfixNotation(string input)
        {
            List<string> outputSeparated = new List<string>();
            Stack<string> stack = new Stack<string>();
            foreach (string c in Separate(input))
            {
                if (operators.Contains(c))
                {
                    if (stack.Count > 0 && !c.Equals("("))
                    {
                        if (c.Equals(")"))
                        {
                            string s = stack.Pop();
                            while (s != "(")
                            {
                                outputSeparated.Add(s);
                                s = stack.Pop();
                            }
                        }
                        else if (GetPriority(c) > GetPriority(stack.Peek()))
                            stack.Push(c);
                        else
                        {
                            while (stack.Count > 0 && GetPriority(c) <= GetPriority(stack.Peek()))
                                outputSeparated.Add(stack.Pop());
                            stack.Push(c);
                        }
                    }
                    else
                        stack.Push(c);
                }
                else
                    outputSeparated.Add(c);
            }
            if (stack.Count > 0)
                foreach (string c in stack)
                    outputSeparated.Add(c);

            return outputSeparated.ToArray();
        }

        static Dictionary<char, int> digits = new Dictionary<char, int>()
        {
            { 'A', 10 },
            { 'B', 11 },
            { 'C', 12 },
            { 'D', 13 }
        };

        public string IntToDigit(int aVal)
        {
            string res;
            switch (aVal)
            {
                case 10:
                    res = "A";
                    break;
                case 11:
                    res = "B";
                    break;
                case 12:
                    res = "C";
                    break;
                case 13:
                    res = "D";
                    break;
                default:
                    res = Convert.ToString(aVal);
                    break;
            }
            return res;
        }

        public int DigitToInt(char aDigit)
        {
            int res;
            switch (aDigit)
            {
                case 'A':
                    res = 10;
                    break;
                case 'B':
                    res = 11;
                    break;
                case 'C':
                    res = 12;
                    break;
                case 'D':
                    res = 13;
                    break;
                default:
                    res = Convert.ToInt32(aDigit);
                    break;
            }
            return res;
        }

        public string converterBack(int to, double num)
        {
            int sign = (num < 0) ? -1 : 1;
            num = Math.Abs(num);
            int zel = (int)Math.Truncate(num);
            double FracVal = num - zel;
            string StrInt = "";
            do
            {
            StrInt = IntToDigit(zel % to) + StrInt;
            zel = zel / to;
            } while (zel != 0);
                       
            if (FracVal != 0) {
                string FracPart = "";
                int tmp;
                while(FracVal > 0 && FracPart.Length <= 5)
                {
                    FracVal = FracVal * to;
                    tmp = (int)Math.Truncate(FracVal);
                    FracPart = FracPart + IntToDigit(tmp);
                    FracVal = FracVal - tmp;
                }
                StrInt = StrInt + "," + Convert.ToString(FracPart);
            }
            return (sign == -1) ? "-" + StrInt : StrInt;
        }
        public double converterToDec(int from, string str)
        {
            string[] number = new string[2];
            if (str.Contains(','))
            {
                number = str.Split(',');
            }
            else
            {
                number[0] = str;
                number[1] = "0";
            }
            int sign;
            if (number[0].Contains('-'))
            {
                sign = -1;
                number[0] = number[0].Replace("-", "");
            } else
            {
                sign = 1;
            }
            double decimalNum = 0;
            int c = number[0].Length - 1;

            int tmp = 0;
            for (int i = 0; i < number[0].Length; ++i)
            {
                if (!(int.TryParse(number[0][i].ToString(), out tmp)))
                {
                    tmp = digits[number[0][i]];
                }
                decimalNum += tmp * Math.Pow(from, c);
                c--;
            }
            if (number[1].Length != 0 && number[1] != "0")
            {
                for (int i = 1; i <= number[1].Length; i++)
                {
                    if (!(int.TryParse(number[1][i - 1].ToString(), out tmp)))
                    {
                        tmp = digits[number[1][i - 1]];
                    }
                    decimalNum += tmp * Math.Pow(from, -i);
                }
            }
            return (sign == -1) ? -decimalNum : decimalNum;
        }

        public string result(string input)
        {
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>(ConvertToPostfixNotation(input));
            string str = queue.Dequeue();
            while (queue.Count >= 0)
            {
                if (!operators.Contains(str))
                {
                    stack.Push(str);
                    str = queue.Dequeue();
                }
                else
                {
                    string summ = "0";
                    try
                    {

                        switch (str)
                        {

                            case "+":
                                {
                                    double a = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    double b = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    summ = converterBack(14, a + b);
                                    break;
                                }
                            case "-":
                                {
                                    double a = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    if(stack.Count == 0)
                                    {
                                        summ = converterBack(14, -a);
                                    } else
                                    {
                                        double b = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                        summ = converterBack(14, b - a);
                                    }
                                    break;
                                }
                            case "*":
                                {
                                    double a = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    double b = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    summ = converterBack(14, b * a);
                                    break;
                                }
                            case "/":
                                {
                                    double a = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    double b = Convert.ToDouble(Math.Round(converterToDec(14, stack.Pop()), 5));
                                    summ = converterBack(14, b / a);
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    stack.Push(summ);
                    if (queue.Count > 0)
                        str = queue.Dequeue();
                    else
                        break;
                }

            }
            return stack.Pop();
        }
    }

}
