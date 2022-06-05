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
using System.Data;

namespace Задание__7
{
    public class CalculateEventArgs
    {
        public string? Message { get; }
        public string? IncorrectValue { get; }

        public CalculateEventArgs(string? msg, string? value)
        {
            Message = msg;
            IncorrectValue = value;
        }
    }

    public static class Calculate
    {
        public delegate void Message(CalculateEventArgs e);
        public static event Message? ErrorMessage; 

        private static double? firstOperand = null;
        private static string? op = null;
        private static double? lastOperand = null;
        private static double? result = null;
        private static bool radians = true;

        public static double? FirstOperand
        {
            get => firstOperand;
            set => firstOperand = value;
        }

        public static double? LastOperand
        {
            get => lastOperand;
            set => lastOperand = value;
        }
    
        public static string? Operator
        {
            get => op;
            set => op = value;
        }

        public static double? Result
        {
            get         => result;
            private set => result = value;
        }

        public static bool Radians
        {
            get => radians;
            set => radians = value;
        }

        public static double? GetResult()
        {
            try
            {
                result = Convert.ToDouble(new DataTable().Compute(firstOperand.ToString().Replace(',', '.') + op + lastOperand.ToString().Replace(',', '.'), null));
                firstOperand = result;
                lastOperand = null;
                op = "";
                return result;
            }
            catch 
            {
                ErrorMessage?.Invoke(new CalculateEventArgs("Некорректное выражение", firstOperand + op + lastOperand));
                return null;
            }
        }

        public static double? Percent(string? value)
        {
            try { return Convert.ToDouble(value) / 100; }
            catch
            {
                ErrorMessage?.Invoke(new CalculateEventArgs("Некорректное выражение", value));
                return null;
            }
        }

        public static double? CalculateE(string? value)
        {
            try { return Convert.ToDouble(value) * Math.E; }
            catch
            {
                ErrorMessage?.Invoke(new CalculateEventArgs("Некорректное выражение", value));
                return null;
            }
        }

        public static double? Root(string? value)
        {
            try { return Math.Sqrt(Convert.ToDouble(value)); }
            catch
            {
                ErrorMessage?.Invoke(new CalculateEventArgs("Некорректное выражение", value));
                return null;
            }
        }

        public static void Sign(TextBox text, string? op)
        {
            try
            {
                double? content = Calculate.CalculateExp(text.Text.Trim());
                if (firstOperand is null) firstOperand = content;
                else lastOperand = content;
                text.Clear();
                if (firstOperand is not null && lastOperand is not null) GetResult();
                Calculate.op = op;
            }
            catch { ErrorMessage?.Invoke(new CalculateEventArgs("Некорректный формат данных", null)); }
        }

        public static double? Factorial(string? value)
        {
            try
            {
                if (Convert.ToInt32(value) > 12) throw new Exception("Максимальное число для получения факториала - 12");
                return Enumerable.Range(1, Convert.ToInt32(value)).Aggregate((x, y) => x * y);
            }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? Resign(string? value)
        {
            try
            {
                if (value.Contains("-")) return Convert.ToDouble(value.Substring(1));
                else return Convert.ToDouble(value.Insert(0, "-"));
            }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }
    
        public static double? Tangent(string? value)
        {
            try { return radians ? Math.Tan(Convert.ToDouble(value)) : Math.Tan((Convert.ToDouble(value) * Math.PI) / 180); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? Sine(string? value)
        {
            try { return radians ? Math.Sin(Convert.ToDouble(value)) : Math.Sin((Convert.ToDouble(value) * Math.PI) / 180); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? Cosine(string? value)
        {
            try { return radians ? Math.Cos(Convert.ToDouble(value)) : Math.Cos((Convert.ToDouble(value) * Math.PI) / 180); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? CalculatePi(string? value)
        {
            try { return Convert.ToDouble(value) * Math.PI; }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? Ln(string? value)
        {
            try { return Math.Log(Convert.ToDouble(value)); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? Lg(string? value)
        {
            try { return Math.Log10(Convert.ToDouble(value)); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? CalculateDivine(string? value)
        {
            try { return 1 / Convert.ToDouble(value); }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }

        public static double? CalculateExp(string? value)
        {
            try
            {
                return value.Contains("^") ? value.Split("^").Select(x => Convert.ToDouble(x)).Aggregate((x, y) => Math.Pow(x, y)) : Convert.ToDouble(value);
            }
            catch (Exception ex)
            {
                ErrorMessage?.Invoke(new CalculateEventArgs(ex.Message, value));
                return null;
            }
        }
    }

    public partial class MainWindow : Window
    {
        public static string Operators = "+-*/";

        public void ShowError(CalculateEventArgs e) => MessageBox.Show($"{e.Message}! Выражение: {e.IncorrectValue}");

        public MainWindow()
        {
            InitializeComponent();
            MainTextBox.Focus();
            Calculate.ErrorMessage += ShowError;
        }
        
        public bool CheckData(TextBox text, string substring)
        {
            int value;
            bool number         = Int32.TryParse(substring, out value);
            bool negativeNumber = ("-" == substring && string.IsNullOrWhiteSpace(text.Text));
            bool floatNumber    = ("," == substring && !string.IsNullOrWhiteSpace(text.Text) && text.Text.Where(x => x.ToString() == substring).Count() < 1);
            bool staples        = ("()".Contains(substring));
            bool expanent       = ("^" == substring && !string.IsNullOrWhiteSpace(text.Text));
            return !(number || negativeNumber || floatNumber || staples || expanent);
        }

        private void TextBoxChanged(object sender, TextCompositionEventArgs e)
        {
            var txt = ((TextBox)sender);
            e.Handled = this.CheckData(txt, e.Text);
            UnicalOperator(txt, e.Text);
        }
        
        private void NumberClick(object sender, RoutedEventArgs e)
        {
            MainTextBox.Text += ((Button)sender).Content;
            if (!MainTextBox.IsFocused) MainTextBox.Focus();
            MainTextBox.SelectionStart = MainTextBox.Text.Length;
        }

        private void SignClick(object sender, RoutedEventArgs e)
        {
            try
            {
                string? content = ((Button)sender).Content.ToString().Replace("✕", "*").Replace("÷", "/");
                MainTextBox.Text += !this.CheckData(MainTextBox, content) ? content : "";
                UnicalOperator(MainTextBox, content);
                if (!MainTextBox.IsFocused) MainTextBox.Focus();
            } catch {}
        }

        private void UnicalOperator(TextBox txt, string content)
        {
            if (!string.IsNullOrWhiteSpace(txt.Text))
            {
                if (Operators.Contains(content)) Calculate.Sign(txt, content);
                switch (content)
                {
                    case "%": txt.Text = Calculate.Percent(txt.Text).ToString(); break;
                    case "e": txt.Text = Calculate.CalculateE(txt.Text).ToString(); break;
                    case "x!": case "!": txt.Text = Calculate.Factorial(txt.Text).ToString(); break;
                    case "mod": case ":": Calculate.Sign(txt, "%"); break;
                    case "xⁿ": txt.Text += "^"; break;
                }
            }
        }

        private void RadDegClick(object sender, RoutedEventArgs e)
        {
            Calculate.Radians = !Calculate.Radians;
            ((Button)sender).Content = Calculate.Radians ? "rad" : "deg";
        }

        private void SqrtClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Root(MainTextBox.Text).ToString();
        private void ResignClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Resign(MainTextBox.Text).ToString();
        private void SinClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Sine(MainTextBox.Text).ToString();
        private void CosClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Cosine(MainTextBox.Text).ToString();
        private void TanClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Tangent(MainTextBox.Text).ToString();
        private void PiClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.CalculatePi(MainTextBox.Text).ToString();
        private void LgClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Lg(MainTextBox.Text).ToString();
        private void LnClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.Ln(MainTextBox.Text).ToString();
        private void DivClick(object sender, RoutedEventArgs e) => MainTextBox.Text = Calculate.CalculateDivine(MainTextBox.Text).ToString();

        private void KeySetResult(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SetResult();
        }

        private void SetResult()
        {
            try 
            {
                if (MainTextBox.Text == "") MainTextBox.Text = Calculate.Result.ToString();
                else if (MainTextBox.Text.Contains("^")) MainTextBox.Text = Calculate.CalculateExp(MainTextBox.Text).ToString();
                else
                {
                    Calculate.LastOperand = Convert.ToDouble(MainTextBox.Text);
                    MainTextBox.Text = Calculate.GetResult().ToString();
                    Calculate.FirstOperand = null;
                }
                MainTextBox.SelectionStart = MainTextBox.Text.Length;
            } catch {}
        }

        private void Result(object sender, RoutedEventArgs e) => SetResult();
        private void Backspace(object sender, RoutedEventArgs e)
        {
            MainTextBox.Text = MainTextBox.Text.Length != 0 ? MainTextBox.Text.Remove(MainTextBox.Text.Length - 1) : MainTextBox.Text;
            MainTextBox.Focus();
        }

        private void FullBackspace(object sender, RoutedEventArgs e)
        { 
            MainTextBox.Clear();
            Calculate.FirstOperand = null;
            Calculate.LastOperand = null;
            MainTextBox.Focus();
        }
        private void ExitClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void DeactivateClick(object sender, RoutedEventArgs e) => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        private void Drag(object sender, RoutedEventArgs e) { if (Mouse.LeftButton == MouseButtonState.Pressed) { Application.Current.MainWindow.DragMove(); } }
    }
}
