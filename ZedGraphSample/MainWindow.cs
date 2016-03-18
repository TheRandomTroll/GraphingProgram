using System.Collections.Generic;

namespace Graphing
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using ZedGraph;

    public partial class MainWindow : Form
    {
        private float a = 0;
        private float b = 0;
        private float c = 0;
        private int equationType = 0;

        private readonly List<Color> allAvailableColors = new List<Color>
            {
                Color.Aqua,
                Color.Red,
                Color.Violet,
                Color.Green,
                Color.Magenta,
                Color.Goldenrod,
                Color.BlueViolet,
                Color.Indigo,
                Color.Sienna,
                Color.Cyan,
                Color.DarkMagenta,
                Color.ForestGreen,
                Color.DarkBlue,
                Color.LawnGreen,
                Color.Coral,
                Color.SteelBlue,
                Color.Maroon
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetLabelText(a, b, c);
            CreateGraph(this.zg1, 0, 0, 0);
            this.SetSize();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.SetSize();
        }

        #region Main Logic

        /// <summary>
        /// Generates a grid for a graph.
        /// </summary>
        /// <param name="zgc">A <see cref="ZedGraphControl"/> used to get a graph pane.</param>
        public void CreateGrid(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;
            myPane.XAxis.Scale.Min = -20;
            myPane.XAxis.Scale.Max = 20;
            myPane.YAxis.Scale.Min = -20.0;
            myPane.YAxis.Scale.Max = 20.0;

            myPane.Title.IsVisible = false;
            myPane.Legend.IsVisible = false;
            myPane.XAxis.Title.IsVisible = false;
            myPane.YAxis.Title.IsVisible = false;

            for (int i = -19; i < 20; i++)
            {
                LineObj thresholdLineX = new LineObj(Color.Black,
                    -20,
                    i * 1.230769230769231,
                    20,
                    i * 1.230769230769231);

                LineObj thresholdLineY = new LineObj(Color.Black, i, -20, i, 20);

                if (Math.Abs(i) % 5 == 0.0)
                {
                    thresholdLineX.Line.Width = 2f;
                    thresholdLineY.Line.Width = 2f;
                }
                else
                {
                    thresholdLineX.Line.Width = 0.5f;
                    thresholdLineY.Line.Width = 0.5f;
                }

                if (Math.Abs(i) == 0.0)
                {
                    thresholdLineX.Line.Color = allAvailableColors[0];
                    thresholdLineY.Line.Color = allAvailableColors[0];
                    thresholdLineX.Line.Width = 2f;
                    thresholdLineY.Line.Width = 2f;
                }

                if (!(i * 1.230769230769231 < -20 || i * 1.230769230769231 > 20))
                {
                    myPane.GraphObjList.Add(thresholdLineX);
                }

                myPane.GraphObjList.Add(thresholdLineY);
            }
        }

        /// <summary>
        /// Clears the current graph.
        /// </summary>
        /// <param name="zgc">A <see cref="ZedGraphControl"/> from which it sets the size.</param>
        public void ClearGraph(ZedGraphControl zgc)
        {
            zgc.GraphPane.CurveList.Clear();
            this.CreateGrid(zgc);
            index = 1;
            zgc.Refresh();
        }

        private int tagIncrementator;
        private int index;

        /// <summary>
        /// Creates a new graph, based on given parameters.
        /// </summary>
        /// <param name="zgc">A <see cref="ZedGraphControl"/> which we use to generate our graph.</param>
        /// <param name="a">The constant parameter a.</param>
        /// <param name="b">The constant parameter b.</param>
        /// <param name="c">The constant parameter c.</param>
        public void CreateGraph(ZedGraphControl zgc, float a, float b, float c)
        {
            GraphPane myPane = zgc.GraphPane;

            CreateGrid(zgc);

            PointPairList list = new PointPairList();

            for (double x = -100; x <= 100; x += 0.05)
            {
                double y;

                switch (equationType)
                {
                    case 1:
                        y = a * Math.Pow(x, b) + c;
                        break;

                    case 2:
                        y = Math.Abs(a * x + b) + c;
                        break;

                    case 3:
                        y = -Math.Abs(a * x + b) + c;
                        break;

                    case 4:
                        y = 1 / x - 1;
                        break;

                    case 5:
                        y = Math.Sin(x);
                        break;

                    case 6:
                        y = Math.Cos(x);
                        break;

                    case 7:
                        y = Math.Tan(x);
                        break;

                    case 8:
                        y = Math.Sqrt(x);
                        break;

                    case 9:
                        y = a * Math.Pow(x, 2) + b * x + c;
                        break;

                    case 10:
                        y = Math.Abs(a * x + b) + c * x;
                        break;

                    default:
                        y = 0;
                        break;
                }

                list.Add(x, y * 1.230769230769231);
            }

            if (index >= allAvailableColors.Count)
            {
                index = index - allAvailableColors.Count + 1;
            }

            Color currentColor = allAvailableColors[index];
            LineItem myCurve = myPane.AddCurve(string.Empty, list, currentColor, SymbolType.None);

            myCurve.Tag = tagIncrementator;

            myPane.CurveList.Move(0, tagIncrementator);

            tagIncrementator++;
            index++;

            myCurve.Line.Width = 3f;

            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45F);

            myPane.AxisChange();
            zgc.Refresh();
        }

        /// <summary>
        /// Sets the graph size based on the current window dimensions.
        /// </summary>
        private void SetSize()
        {
            this.zg1.Location = new Point(50, 10);

            this.zg1.Size = new Size(this.ClientRectangle.Width - 100, this.ClientRectangle.Height - 130);
        }

        #endregion Main Logic

        #region Events and other methods

        private void zg1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                c = float.Parse(textBox1.Text);
            }
            catch (FormatException)
            {
                c = 0;
            }

            SetLabelText(a, b, c);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                a = float.Parse(textBox2.Text);
            }
            catch (FormatException)
            {
                a = 0;
            }

            SetLabelText(a, b, c);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                b = float.Parse(textBox3.Text);
            }
            catch (FormatException)
            {
                b = 0;
            }

            SetLabelText(a, b, c);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ClearGraph(this.zg1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.CreateGraph(this.zg1, this.a, this.b, this.c);
            this.zg1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var conditionalEquationWindow = new ConditionalEquationWindow();
            conditionalEquationWindow.Show();
            zg1.Refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void label9_Click(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            equationType = GetEquationType();
            SetLabelText(a, b, c);
        }

        public void SetLabelText(float a, float b, float c)
        {
            switch (equationType)
            {
                case 1:
                    char oper = '+';
                    if (c < 0)
                    {
                        oper = '-';
                    }

                    float absPOW = Math.Abs(c);
                    label9.Text = $"{a}x ^ {b} {oper} {absPOW}";
                    label2.Show();
                    textBox1.Show();
                    label3.Show();
                    textBox2.Show();
                    label4.Show();
                    textBox3.Show();
                    break;

                case 2:
                    char oper2 = '+';
                    if (b < 0)
                    {
                        oper2 = '-';
                    }

                    char oper3 = '+';
                    if (c < 0)
                    {
                        oper3 = '-';
                    }

                    float absB2 = Math.Abs(b);
                    float absPow = Math.Abs(c);
                    label9.Text = $"|{a}x {oper2} {absB2}| {oper3} {absPow}";
                    label2.Show();
                    textBox1.Show();
                    label3.Show();
                    textBox2.Show();
                    label4.Show();
                    textBox3.Show();
                    break;

                case 3:
                    char oper4 = '+';
                    if (b < 0)
                    {
                        oper4 = '-';
                    }

                    char oper5 = '+';

                    if (c < 0)
                    {
                        oper5 = '-';
                    }

                    float absB3 = Math.Abs(b);
                    float absPow2 = Math.Abs(c);
                    label9.Text = $"-|{a}x {oper4} {absB3}| {oper5} {absPow2}";
                    label2.Show();
                    textBox1.Show();
                    label3.Show();
                    textBox2.Show();
                    label4.Show();
                    textBox3.Show();
                    break;

                case 4:
                    label9.Text = "1/x";
                    label2.Hide();
                    textBox1.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    label4.Hide();
                    textBox3.Hide();
                    break;

                case 5:
                    label9.Text = "sin(x)";
                    label2.Hide();
                    textBox1.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    label4.Hide();
                    textBox3.Hide();
                    break;

                case 6:
                    label9.Text = "cos(x)";
                    label2.Hide();
                    textBox1.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    label4.Hide();
                    textBox3.Hide();
                    break;

                case 7:
                    label9.Text = "tan(x)";
                    label2.Hide();
                    textBox1.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    label4.Hide();
                    textBox3.Hide();
                    break;

                case 8:
                    label9.Text = "sqrt(x)";
                    label2.Hide();
                    textBox1.Hide();
                    label3.Hide();
                    textBox2.Hide();
                    label4.Hide();
                    textBox3.Hide();
                    break;

                case 9:
                    char oper6 = '+';
                    char oper7 = '+';
                    if (b < 0)
                    {
                        oper6 = '-';
                    }

                    if (c < 0)
                    {
                        oper7 = '-';
                    }

                    label9.Text = $"{a}x ^ 2 {oper6} {Math.Abs(b)}x {oper7} {Math.Abs(c)}";
                    label2.Show();
                    textBox1.Show();
                    label3.Show();
                    textBox2.Show();
                    label4.Show();
                    textBox3.Show();
                    break;

                case 10:
                    char oper8 = '+';
                    char oper9 = '+';
                    if (b < 0)
                    {
                        oper8 = '-';
                    }

                    if (c < 0)
                    {
                        oper9 = '-';
                    }
                    label9.Text = $"|{a}x {oper8} {Math.Abs(b)}| {oper9} {Math.Abs(c)}x";
                    label2.Show();
                    textBox1.Show();
                    label3.Show();
                    textBox2.Show();
                    label4.Show();
                    textBox3.Show();
                    break;

                default:
                    label9.Text = string.Empty;
                    break;
            }

            label1.Text = $"GRAPHING FUNCTION: {comboBox1.Text}";
        }

        private int GetEquationType()
        {
            switch (comboBox1.Text)
            {
                case "ax ^ b + c":
                    return 1;

                case "|ax + b| + c":
                    return 2;

                case "-|ax + b| + c":
                    return 3;

                case "1 / x":
                    return 4;

                case "sin(x)":
                    return 5;

                case "cos(x)":
                    return 6;

                case "tan(x)":
                    return 7;

                case "sqrt(x)":
                    return 8;

                case "ax ^ 2 + bx + c":
                    return 9;

                case "|ax + b| + cx":
                    return 10;

                default:
                    return -1;
            }
        }

        #endregion Events and other methods
    }
}