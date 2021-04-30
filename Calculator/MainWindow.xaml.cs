using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        string inputText = "";
        int countBrackets = 0;
        private PostfixNotation newNotation = new PostfixNotation();
        private List<char> bad_standart_operators =
            new List<char>(new char[] { '(', '+', '-', '*', '/' });
        private List<string> operators =
            new List<string>(new string[] { "+", "-", "*", "/" });

        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement btn in LayoutRoot.Children)
            {
                if (btn is Button)
                {
                    ((Button)btn).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(inputText == "")
            {
                input.Text = "";
                result.Text = "";
            }

            string str = (string)((Button)e.OriginalSource).Content;

            if (str == "=")
            {
                if (inputText.Length != 0 && !bad_standart_operators.Contains(inputText[inputText.Length - 1]))
                {
                    if(countBrackets != 0)
                    {
                        while(countBrackets > 0)
                        {
                            input.Text += ")";
                            inputText += ")";
                            countBrackets--;
                        }
                    }
                    input.Text += str;
                    result.Text += newNotation.result(inputText);
                    inputText = "";
                }
            }
            else
            {
                if(operators.Contains(str))
                {
                    if(str != "-")
                    {
                        if (inputText.Length == 0 || inputText[inputText.Length - 1] == '(')
                        {
                            str = "";
                        }
                        else
                        {
                            if (operators.Contains(Convert.ToString(inputText[inputText.Length - 1])))
                            {
                                char tmp = inputText[inputText.Length - 1];
                                inputText = inputText.Remove(inputText.Length - 1, 1);
                                if (inputText.Length != 0 && inputText[inputText.Length - 1] != '(')
                                {
                                    inputText += str;
                                    input.Text = inputText;
                                } else
                                {
                                    inputText += Convert.ToString(tmp);
                                }   
                            }
                            else
                            {
                                inputText += str;
                                input.Text += str;
                            }
                        }
                    } else
                    {
                        if(inputText.Length == 0)
                        {
                            inputText += "0"+str;
                            input.Text += str;
                        } else if (operators.Contains(Convert.ToString(inputText[inputText.Length - 1])))
                        {
                            inputText = inputText.Remove(inputText.Length - 1, 1);
                            inputText += str;
                            input.Text = inputText;
                        }
                        else
                        {
                            if (inputText[inputText.Length - 1] == '(')
                            {
                                inputText += "0"+str;
                                input.Text += str;
                            } else
                            {
                                inputText += str;
                                input.Text += str;
                            }        
                        }
                    }
                } else
                {
                    if(str == "(")
                    {
                        if (inputText.Length == 0 || operators.Contains(Convert.ToString(inputText[inputText.Length - 1])))
                        {
                            countBrackets++;
                        } else
                        {
                            str = "";
                        }
                            
                    } else if(countBrackets > 0 && str == ")")
                    {
                        if(!operators.Contains(Convert.ToString(inputText[inputText.Length - 1])))
                        {
                            countBrackets--;
                        } else
                        {
                            str = "";
                        }   
                    } else if(countBrackets == 0 && str == ")")
                    {
                        str = "";
                    }
                    if(str == "Clear")
                    {
                        input.Text = "";
                        result.Text = "";
                        inputText = "";
                        countBrackets = 0;
                    } else
                    {
                        if(str == "Backspace")
                        {
                            if(inputText.Length != 0)
                            {
                                inputText = inputText.Remove(inputText.Length - 1, 1);
                                input.Text = inputText;
                            }
                        } else
                        {
                            if(str == ",")
                            {
                                if(inputText.Length == 0 || bad_standart_operators.Contains(inputText[inputText.Length - 1]) || inputText[inputText.Length - 1] == ')')
                                {
                                    inputText += "0,";
                                    input.Text += "0,";
                                } else
                                {
                                    inputText += str;
                                    input.Text += str;
                                } 
                            } else
                            {
                                inputText += str;
                                input.Text += str;
                            }       
                        }   
                    }  
                }
            }
        }
    }
}
