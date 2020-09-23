using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scales
{
    public partial class Form1 : Form
    {
        bool mouseStatus;
        int lx = 0;
        private ArithmeticResult currentArithmeticResult;
        private List<AnagramResult> anagramsList = new List<AnagramResult>();
        int arithmeticRight = 0;
        int arithmeticWrong = 0;
        int anagramRight = 0;
        int anagramWrong = 0;
        int basketRight = 0;
        int basketWrong = 0;
        int circleLocationX;
        private int circleLocationY;
        int anagramIteration = 0;
        int timer = 5 * 60 * 1000;
        double M = 0.3;



        public Form1()
        {
            InitializeComponent();

        }

        private void circleLocation()
        {
            Random rnd = new Random();
            circleLocationX = rnd.Next(0, panel1.Width - pictureBox2.Width);
            circleLocationY = panel1.Location.Y;
            pictureBox2.Location = new Point(circleLocationX, 0);
            M = 1;
        }

        private void loadAnagrams()
        {
            string anagrams = Properties.Resources.anagrams_txt;
            string[] anagramsArray = anagrams.Split('\n');
            foreach (string anagram in anagramsArray)
            {
                AnagramResult result = new AnagramResult() { rightWord = anagram, cipherdWord = makeAnagram(anagram) };
                anagramsList.Add(result);
            }
        }

        private string makeAnagram(string v)
        {
            int anagramLength = v.Length - 1;
            List<int> array = new List<int>();
            string cipher = "";
            Random rnd = new Random();
            while (array.Count() < anagramLength)
            {
                int number = rnd.Next(0, anagramLength);
                if (!array.Contains(number))
                {
                    array.Add(number);
                }
            }

            for (var j = 0; j < array.Count; j++)
            {
                cipher += v[array[j]];
                cipher += " ";
            }
            return cipher;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseStatus = true;
            lx = e.X;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseStatus = false;


        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseStatus == true)
            {
                if (pictureBox1.Left + e.X - lx <= 0)
                {
                    pictureBox1.Left = 0;
                }
                else if (pictureBox1.Right + e.X - lx >= panel1.Width)
                {
                    pictureBox1.Left = panel1.Width - pictureBox1.Width;
                }
                else
                {
                    pictureBox1.Left = pictureBox1.Left + e.X - lx;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + Convert.ToInt32(M));
            M += 0.001;

            Point point1 = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + pictureBox2.Height);
            Point point2 = new Point(pictureBox2.Location.X + pictureBox2.Width, pictureBox2.Location.Y + pictureBox2.Height);
            Point point3 = new Point(pictureBox1.Location.X, pictureBox1.Location.Y);
            Point point4 = new Point(pictureBox1.Location.X + pictureBox1.Width, pictureBox2.Location.Y);

            if (point1.Y >= point3.Y - 2 && point1.Y <= point3.Y + 2)
            {
                if (point1.X >= point3.X && point2.X <= point4.X)
                {
                    basketRight++;
                }
                else
                {
                    basketWrong++;
                }
                circleLocation();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private ArithmeticResult generateArithmetics()
        {

            Random rnd = new Random();

            List<int> numbers = new List<int>();

            for (var i = 1; i <= 3; i++)
            {
                numbers.Add(rnd.Next(1, 10));
            }
            List<int> operators = new List<int>();

            while (operators.Count != 2)
            {
                if (!operators.Contains(2))
                {
                    operators.Add(rnd.Next(0, 3) % 3);
                }
                else
                {
                    operators.Add(rnd.Next(0, 2) % 2);
                }
            }
            Dictionary<int, string> operatorStrings = new Dictionary<int, string>
            {
                { 0, "+" },
                { 1, "-" },
                { 2, "*" }
            };
            string resultString = numbers[0].ToString() + " " + operatorStrings[operators[0]] + " " + numbers[1].ToString() + " " + operatorStrings[operators[1]] + " " + numbers[2].ToString();
            if (operators.Contains(2))
            {
                int mulIndex = operators.IndexOf(2);
                int newnumber = numbers[mulIndex] * numbers[mulIndex + 1];
                operators.RemoveAt(mulIndex);
                int oldnumber;
                switch (mulIndex)
                {
                    case 0:
                        oldnumber = numbers[2];
                        numbers.Clear();
                        numbers.Add(newnumber);
                        numbers.Add(oldnumber);
                        break;
                    case 1:
                        oldnumber = numbers[0];
                        numbers.Clear();
                        numbers.Add(oldnumber);
                        numbers.Add(newnumber);
                        break;
                }
            }
            int currentResult = numbers[0];

            for (int i = 0; i < operators.Count(); i++)
            {
                switch (operators[i])
                {
                    case 0:
                        currentResult += numbers[i + 1];
                        break;
                    case 1:
                        currentResult -= numbers[i + 1];
                        break;
                }
            }
            ArithmeticResult result = new ArithmeticResult() { Result = currentResult, Equation = resultString };
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkArithmetics();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (timer == 0)
            {
                timer3.Enabled = false;
                timer2.Enabled = false;
                arithTextBox.Enabled = false;
                anagramTextBox.Enabled = false;
                MessageBox.Show(
                    "Тестирование завершено",
                    "Тест завершен",
                    MessageBoxButtons.OK
                    );
                showResults();
            }

            int timerDivide = timer / 1000;
            label4.Text = (timerDivide / 60).ToString() + ":" + (timerDivide % 60);
            timer -= 100;
            label1.Text = basketRight.ToString();
            label3.Text = basketWrong.ToString();
            label29.Text = arithmeticRight.ToString();
            label28.Text = arithmeticWrong.ToString();
            label31.Text = anagramRight.ToString();
            label30.Text = anagramWrong.ToString();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkAnagram();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                checkAnagram();
                arithTextBox.Clear();
            }
            else
            {
                base.OnKeyPress(e);
            }
        }

        private void checkAnagram()
        {
            string rightAnswer = anagramsList[anagramIteration].rightWord;
            if (anagramTextBox.Text != "" && anagramTextBox.Text.ToUpper() == rightAnswer.Substring(0, rightAnswer.Length - 1))
            {
                anagramRight++;
                label22.Visible = true;
                label22.Text = "ВЕРНО";
                label22.ForeColor = System.Drawing.Color.ForestGreen;
                anagramIteration++;
                anagramTextBox.Clear();
                currentArithmeticResult = generateArithmetics();
                while (currentArithmeticResult.Result < 0)
                {
                    currentArithmeticResult = generateArithmetics();
                }
                arithText.Text = currentArithmeticResult.Equation;
                anagramTextBox.Enabled = false;
                anagramText.Enabled = false;
                arithText.Enabled = true;
                arithTextBox.Enabled = true;
                arithTextBox.Focus();

            }
            else
            {
                anagramTextBox.Clear();
                anagramWrong++;
                label22.Visible = true;
                label22.Text = "НЕВЕРНО";
                label22.ForeColor = System.Drawing.Color.Red;
            }

        }

        private void checkArithmetics()
        {
            Regex regex = new Regex(@"\D");
            arithTextBox.Text = regex.Replace(arithTextBox.Text, "");
            if (arithTextBox.Text != "" && Convert.ToInt32(arithTextBox.Text) == currentArithmeticResult.Result)
            {
                arithmeticRight++;
                label21.Visible = true;
                label21.Text = "ВЕРНО";
                label21.ForeColor = System.Drawing.Color.ForestGreen;
                anagramText.Text = anagramsList[anagramIteration].cipherdWord;
                arithTextBox.Clear();
                arithTextBox.Enabled = false;
                anagramTextBox.Enabled = true;
                anagramTextBox.Focus();
                arithText.Enabled = false;
                anagramText.Enabled = true;
            }
            else
            {
                arithTextBox.Clear();
                arithmeticWrong++;
                label21.Visible = true;
                label21.Text = "НЕВЕРНО";
                label21.ForeColor = System.Drawing.Color.Red;
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                e.Handled = true;
                checkArithmetics();
                arithTextBox.Clear();
            }
            else
            {
                base.OnKeyPress(e);
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            arithTextBox.Enabled = false;
            anagramTextBox.Enabled = false;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            arithmeticRight = 0;
            arithmeticWrong = 0;
            anagramRight = 0;
            anagramWrong = 0;
            basketRight = 0;
            basketWrong = 0;
            loadAnagrams();
            arithTextBox.Enabled = true;
            anagramTextBox.Enabled = false;
            if ((currentArithmeticResult = generateArithmetics()).Result < 0)
            {
                while (currentArithmeticResult.Result < 0)
                {
                    currentArithmeticResult = generateArithmetics();
                }
            }
            
            arithText.Text = currentArithmeticResult.Equation;
            arithTextBox.Focus();
            //anagramText.Text = anagramsList[anagramIteration].cipherdWord;
            circleLocation();
            timer2.Enabled = true;
            timer3.Enabled = true;
            startBtn.Text = "Начать заново";

        }

        public void showResults()
        {
            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            label14.Visible = true;
            label15.Visible = true;
            label16.Visible = true;
            label15.Visible = true;
            label16.Visible = true;
            label17.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            label20.Visible = true;
            label23.Visible = true;
            label24.Visible = true;
            label25.Visible = true;
            label26.Visible = true;
            label27.Visible = true;
            label9.Text = basketRight.ToString();
            label10.Text = basketWrong.ToString();
            label11.Text = arithmeticRight.ToString();
            label12.Text = arithmeticWrong.ToString();
            label16.Text = anagramRight.ToString();
            label17.Text = anagramWrong.ToString();
            label23.Text = Convert.ToString(anagramRight + arithmeticRight);
            label24.Text = Convert.ToString(anagramWrong + arithmeticWrong);
        }


        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }

}
