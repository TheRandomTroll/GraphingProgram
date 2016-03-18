using System.Text.RegularExpressions;

namespace Graphing
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using ZedGraph;

    public partial class ConditionalEquationWindow : Form
    {
        private string[] statements = new string[3];
        private string[] conditions = new string[3];

        public ConditionalEquationWindow()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointPairList list = new PointPairList();

            for (int i = 0; i < conditions.Length; i++)
            {
                for (double x = -100; x < 101; x += 0.05)
                {
                    var y = ConvertToCondition(conditions[i], x) ? ConvertToStatement(statements[i], x) : 0;

                    list.Add(x, y);
                }
            }

            var mainWindow = new MainWindow();

            LineItem myCurve = mainWindow.zg1.GraphPane.AddCurve(string.Empty, list, Color.Tomato);
            myCurve.Line.Width = 3f;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            statements[0] = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            statements[1] = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            statements[2] = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            conditions[0] = textBox4.Text;
        }

        private void textBox5_TextChanged_1(object cender, EventArgs e)
        {
            conditions[1] = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            conditions[2] = textBox6.Text;
        }

        private bool ConvertToCondition(string input, double x)
        {
            string[] inputArgs = input.Split(' ');
            float numberToCompareWith = float.Parse(inputArgs[2]);

            switch (inputArgs[1])
            {
                case "==":
                    return x == numberToCompareWith;
                case "!=":
                    return x != numberToCompareWith;
                case "<=":
                    return x <= numberToCompareWith;
                case "<":
                    return x < numberToCompareWith;
                case ">=":
                    return x >= numberToCompareWith;
                case ">":
                    return x > numberToCompareWith;
                default:
                    return false;
            }
        }

        private double ConvertToStatement(string input, double x)
        {
            string pattern = @"((\d*)(x) ([\+\-\*\/]) (\d+))|(\d+|-\d+)|(x|-x)";
            string[] inputArgs = input.Split(' ');

            Regex rgx = new Regex(pattern);

            Match match = rgx.Match(input);

            if (match.Success)
            {
                if (match.Groups.Count > 5)
                {
                    if (input.Contains("sqrt"))
                    {
                        if (match.Groups.Count == 6)
                        {
                            return Math.Sqrt(int.Parse(match.Groups[6].Value));
                        }

                        else if (match.Groups.Count == 7)
                        {
                            return Math.Sqrt(x);
                        }
                    }

                    else if (input.Contains("|"))
                    {
                        if (match.Groups.Count == 6)
                        {
                            return Math.Abs(int.Parse(match.Groups[6].Value));
                        }

                        else if (match.Groups.Count == 7)
                        {
                            return Math.Abs(x);
                        }
                    }

                    else
                    {
                        if (match.Groups.Count == 6)
                        {
                            return int.Parse(match.Groups[6].Value);
                        }

                        else if (match.Groups.Count == 7)
                        {
                            return x;
                        }
                    }
                }

                else
                {
                    var a = match.Groups[1].Value == string.Empty ? 1 : int.Parse(match.Groups[1].Value);
                    int b = int.Parse(match.Groups[5].Value);

                    if (input.Contains("sqrt"))
                    {
                        return Math.Sqrt(Operation(match.Groups[4].Value, a, b, x));
                    }

                    if (input.Contains("|"))
                    {
                        return Math.Abs(Operation(match.Groups[4].Value, a, b, x));
                    }

                    return Operation(match.Groups[4].Value, a, b, x);
                }
            }

            return 0;
        }

        private double Operation(String oper, double a, double b, double x)
        {
            double output;
            switch (oper)
            {
                case "+":
                    output = a*x + b;
                    break;
                case "-":
                    output = a * x + b;
                    break;
                case "*":
                    output = a * x + b;
                    break;
                case "/":
                    output = a * x + b;
                    break;
                default:
                    throw new ArgumentException("Invalid parameters.");
            }

            return output;
        }
    }
}

