using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Game
{
    public partial class Form1 : Form
    {
        public static List<Tuple<PictureBox, double, int>> pbTuple = new List<Tuple<PictureBox, double, int>>();
        public static PictureBox[,] pbMatrix = new PictureBox[8, 8];
        public static string[,] table = new string[8, 8];
        public static string lastPlayedColor = "../img/black.png";
        public static string lastBoardColor = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            int x = 0, y = 0;
            foreach (Control item in this.Controls)
            {

                if (item.GetType() == typeof(System.Windows.Forms.PictureBox))
                {
                    string[] splitted = item.Name.Split('x');
                    double loc = Convert.ToDouble(splitted[1]);
                    double calculated = loc / 8;
                    pbTuple.Add(new Tuple<PictureBox, double, int>((PictureBox)item, calculated, Convert.ToInt32(splitted[1])));
                    pbMatrix[x, y] = (PictureBox)item;
                    if (y == 7)
                    {
                        x++;
                        y = 0;
                    }
                    else
                    {
                        y++;
                    }

                }
            }
            pbMatrix = ReverseMatrix(pbMatrix);

            pbTuple.Reverse();

            int row = 1;

            for (int i = 0; i < pbTuple.Count; i++)
            {
                var item = pbTuple[i].Item1;
                item.SizeMode = PictureBoxSizeMode.StretchImage;
                item.Click += İtem_Click;
                item.MouseHover += İtem_MouseHover;
                item.MouseLeave += İtem_MouseLeave;

                if (row == 1 || row == 3 || row == 5 || row == 7)
                {
                    if ((i + 1) % 2 == 0)
                    {
                        item.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        item.BackColor = Color.Green;
                    }
                }
                else
                {
                    if ((i + 1) % 2 == 0)
                    {
                        item.BackColor = Color.Green;
                    }
                    else
                    {
                        item.BackColor = Color.LightGreen;
                    }
                }

                if ((i + 1) % 8 == 0)
                {
                    row++;
                }
            }

            setStart();
        }

        private void İtem_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            if (pb.ImageLocation != "../img/black.png" && pb.ImageLocation != "../img/white.png")
            {
                pb.Image = null;
            }
        }

        private void İtem_MouseHover(object sender, EventArgs e)
        {
            try
            {
                PictureBox pb = (PictureBox)sender;
                string[] split = pb.Name.Split('x');
                int splitted = Convert.ToInt32(split[1]);
                if (lastPlayedColor.Contains("black"))
                {
                    if (splitted % 8 == 1 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) <= 1 && pb.Image == null)
                    {
                        if (pbTuple[1].Item1.Image != null || pbTuple[8].Item1.Image != null || pbTuple[9].Item1.Image != null)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (i == 0 && j == 0)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i;
                                            int colDist = j;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((splitted % 8 != 0 && splitted % 8 != 1) && Convert.ToDouble(split[1]) / Convert.ToDouble(8) <= 1 && pb.Image == null)
                    {
                        if (pbTuple[splitted - 2].Item1.Image != null || pbTuple[splitted].Item1.Image != null || pbTuple[splitted + 6].Item1.Image != null || pbTuple[splitted + 7].Item1.Image != null || pbTuple[splitted + 8].Item1.Image != null)
                        {
                            string[] splittedName = pb.Name.Split('x');
                            double picNum = Convert.ToDouble(splittedName[1]);
                            double calc = picNum / Convert.ToDouble(8);
                            string[] calculated = calc.ToString().Split(',');
                            int m = Convert.ToInt32(calculated[0]);
                            int n = 0;
                            if (calculated.Length == 1)
                            {
                                n = 7;
                            }
                            else
                            {
                                if (Convert.ToInt32(calculated[1].Substring(0, 1)) >= 5)
                                {
                                    n = Convert.ToInt32(calculated[1].Substring(0, 1)) - 2;
                                }
                                else if (Convert.ToInt32(calculated[1].Substring(0, 1)) < 5)
                                {
                                    n = Convert.ToInt32(calculated[1].Substring(0, 1)) - 1;
                                }
                            }


                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = n - 1; j < n + 2; j++)
                                {
                                    if (i == m && j == n)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i;
                                            int colDist = j - n;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splitted % 8 == 0 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) <= 1 && pb.Image == null)
                    {
                        if (pbTuple[splitted - 2].Item1.Image != null || pbTuple[splitted + 6].Item1.Image != null || pbTuple[splitted + 7].Item1.Image != null)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 6; j < 8; j++)
                                {
                                    if (i == 0 && j == 7)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int newRow = i;
                                            int newCol = j;
                                            int rowDist = i;
                                            int colDist = j - 7;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splitted % 8 == 1 && (Convert.ToDouble(split[1]) / Convert.ToDouble(8) > 1 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) < 7) && pb.Image == null)
                    {
                        if (pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 8].Item1.Image != null || pbTuple[splitted].Item1.Image != null || pbTuple[splitted + 7].Item1.Image != null || pbTuple[splitted - 8].Item1.Image != null)
                        {
                            string[] splittedName = pb.Name.Split('x');
                            double picNum = Convert.ToDouble(splittedName[1]);
                            double calc = picNum / Convert.ToDouble(8);
                            string[] calculated = calc.ToString().Split(',');
                            int m = Convert.ToInt32(calculated[0]);
                            int n = 0;

                            for (int i = m - 1; i < m + 2; i++)
                            {
                                for (int j = n; j < n + 2; j++)
                                {
                                    if (i == m && j == n)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i - m;
                                            int colDist = j;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splitted % 8 == 1 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) >= 7 && pb.Image == null)
                    {
                        if (pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 8].Item1.Image != null || pbTuple[splitted].Item1.Image != null)
                        {
                            for (int i = 6; i < 8; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    if (i == 7 && j == 0)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i - 7;
                                            int colDist = j;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((splitted % 8 != 0 && splitted % 8 != 1) && Convert.ToDouble(split[1]) / Convert.ToDouble(8) > 7 && pb.Image == null)
                    {
                        if (pbTuple[splitted - 10].Item1.Image != null || pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 8].Item1.Image != null || pbTuple[splitted - 2].Item1.Image != null || pbTuple[splitted].Item1.Image != null)
                        {
                            string[] splittedName = pb.Name.Split('x');
                            double picNum = Convert.ToDouble(splittedName[1]);
                            double calc = picNum / Convert.ToDouble(8);
                            string[] calculated = calc.ToString().Split(',');
                            int m = Convert.ToInt32(calculated[0]);
                            int n = 0;
                            if (calculated.Length == 1)
                            {
                                n = 7;
                            }
                            else
                            {
                                if (Convert.ToInt32(calculated[1].Substring(0, 1)) >= 5)
                                {
                                    n = Convert.ToInt32(calculated[1].Substring(0, 1)) - 2;
                                }
                                else if (Convert.ToInt32(calculated[1].Substring(0, 1)) < 5)
                                {
                                    n = Convert.ToInt32(calculated[1].Substring(0, 1)) - 1;
                                }
                            }


                            for (int i = 6; i < 8; i++)
                            {
                                for (int j = n - 1; j < n + 2; j++)
                                {
                                    if (i == m && j == n)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i - 7;
                                            int colDist = j - n;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splitted % 8 == 0 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) == 8 && pb.Image == null)
                    {
                        if (pbTuple[splitted - 10].Item1.Image != null || pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 2].Item1.Image != null)
                        {
                            for (int i = 6; i < 8; i++)
                            {
                                for (int j = 6; j < 8; j++)
                                {
                                    if (i == 7 && j == 7)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i - 7;
                                            int colDist = j - 7;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (splitted % 8 == 0 && (Convert.ToDouble(split[1]) / Convert.ToDouble(8) <= 7 && Convert.ToDouble(split[1]) / Convert.ToDouble(8) > 1) && pb.Image == null)
                    {
                        if (pbTuple[splitted - 10].Item1.Image != null || pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 2].Item1.Image != null || pbTuple[splitted + 6].Item1.Image != null || pbTuple[splitted + 7].Item1.Image != null)
                        {
                            string[] splittedName = pb.Name.Split('x');
                            double picNum = Convert.ToDouble(splittedName[1]);
                            double calc = picNum / Convert.ToDouble(8);
                            string[] calculated = calc.ToString().Split(',');
                            int m = Convert.ToInt32(calculated[0]) - 1;
                            int n = 7;
                            
                            for (int i = m - 1; i < m + 2; i++)
                            {
                                for (int j = 6; j < 8; j++)
                                {
                                    if (i == m && j == n)
                                    {

                                    }
                                    else
                                    {
                                        if (table[i, j] == "B")
                                        {
                                            int rowDist = i - m;
                                            int colDist = j - 7;
                                            int newRow = i;
                                            int newCol = j;
                                            string val = table[newRow, newCol];
                                            while (val == "B")
                                            {
                                                if ((newRow >= 0 && newRow <= 7) && (newCol >= 0 && newCol <= 7))
                                                {
                                                    val = table[newRow, newCol];
                                                    if (val == "W")
                                                    {
                                                        pb.ImageLocation = "../img/shadow.png";
                                                        break;
                                                    }
                                                    else if (val == "N")
                                                    {
                                                        break;
                                                    }
                                                    newRow = newRow + rowDist;
                                                    newCol = newCol + colDist;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (pb.Image != null)
                    {

                    }
                    else
                    {
                        if (pbTuple[splitted - 10].Item1.Image != null || pbTuple[splitted - 9].Item1.Image != null || pbTuple[splitted - 8].Item1.Image != null || pbTuple[splitted - 2].Item1.Image != null || pbTuple[splitted].Item1.Image != null || pbTuple[splitted + 6].Item1.Image != null || pbTuple[splitted + 7].Item1.Image != null || pbTuple[splitted + 8].Item1.Image != null)
                        {
                            int i = 0, j = 0;
                            List<string> lst = new List<string>();
                            double picNum = Convert.ToDouble(split[1]);
                            double calc = picNum / Convert.ToDouble(8);
                            string[] calculated = calc.ToString().Split(',');
                            if (Convert.ToInt32(calculated[1].Substring(0, 1)) >= 5)
                            {
                                i = Convert.ToInt32(calculated[0]);
                                j = Convert.ToInt32(calculated[1].Substring(0, 1)) - 2;
                            }
                            else if (Convert.ToInt32(calculated[1].Substring(0, 1)) == 0)
                            {
                                i = Convert.ToInt32(calculated[0]);
                                j = 7;
                            }
                            else if (Convert.ToInt32(calculated[1].Substring(0, 1)) < 5)
                            {
                                i = Convert.ToInt32(calculated[0]);
                                j = Convert.ToInt32(calculated[1].Substring(0, 1)) - 1;
                            }

                            for (int m = i - 1; m < i + 2; m++)
                            {
                                for (int n = j - 1; n < j + 2; n++)
                                {
                                    if ((m >= 0 && n >= 0) && (m <= 7 && n <= 7))
                                    {
                                        if (table[m, n] != "N" && table[m, n] != "W")
                                        {
                                            lst.Add(m.ToString() + "," + n.ToString());
                                        }
                                    }
                                }
                            }

                            foreach (var item in lst)
                            {
                                bool hareketOk = false;
                                string[] splt = item.Split(',');
                                int row = Convert.ToInt32(splt[0]);
                                int column = Convert.ToInt32(splt[1]);
                                int rowDist = row - i;
                                int columnDist = column - j;
                                if ((row >= 0 && column >= 0) && (column <= 7 && row <= 7))
                                {
                                    string val = table[row, column];
                                    while (val == "B" || (row + rowDist) > 7 || (row + rowDist) < 0 || (column + columnDist) > 7 || (column + columnDist) < 0)
                                    {
                                        if ((row >= 0 && row <= 7) && (column <= 7 && column >= 0))
                                        {
                                            val = table[row, column];
                                            if (val == "W")
                                            {
                                                pb.ImageLocation = "../img/shadow.png";
                                                hareketOk = true;
                                                break;
                                            }
                                            else if (val == "N")
                                            {
                                                hareketOk = false;
                                                break;
                                            }

                                            row = row + rowDist;
                                            column = column + columnDist;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    if (hareketOk)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void İtem_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox pb = (PictureBox)sender;

                if (pb.ImageLocation != null && pb.ImageLocation.Contains("shadow"))
                {
                    if (lastPlayedColor != "../img/white.png")
                    {
                        int i = 0, j = 0;
                        string[] splittedName = pb.Name.Split('x');
                        double picNum = Convert.ToDouble(splittedName[1]);
                        double calc = picNum / Convert.ToDouble(8);
                        string[] calculated = calc.ToString().Split(',');
                        List<string> lst = new List<string>();

                        if (calculated.Length == 1)
                        {

                            i = Convert.ToInt32(calculated[0]) - 1;
                            j = 7;
                            table[i, j] = "W";

                        }
                        else
                        {
                            if (Convert.ToInt32(calculated[1].Substring(0, 1)) >= 5)
                            {
                                i = Convert.ToInt32(calculated[0]);
                                j = Convert.ToInt32(calculated[1].Substring(0, 1)) - 2;
                                table[i, j] = "W";
                            }
                            else if (Convert.ToInt32(calculated[1].Substring(0, 1)) < 5)
                            {
                                i = Convert.ToInt32(calculated[0]);
                                j = Convert.ToInt32(calculated[1].Substring(0, 1)) - 1;
                                table[i, j] = "W";
                            }
                        }

                        pb.ImageLocation = "../img/white.png";

                        for (int m = i - 1; m < i + 2; m++)
                        {
                            for (int n = j - 1; n < j + 2; n++)
                            {
                                if ((m >= 0 && n >= 0) && (m <= 7 && n <= 7))
                                {
                                    if (table[m, n] != "N" && table[m, n] != "W")
                                    {
                                        lst.Add(m.ToString() + "," + n.ToString());
                                    }
                                }
                            }
                        }

                        foreach (var item in lst)
                        {
                            bool hareketOk = false;
                            string[] splt = item.Split(',');
                            int row = Convert.ToInt32(splt[0]);
                            int column = Convert.ToInt32(splt[1]);
                            int rowDist = row - i;
                            int colDist = column - j;
                            int newRow = row + rowDist;
                            int newColumn = column + colDist;

                            if ((row >= 0 && column >= 0) && (row <= 7 && column <= 7))
                            {
                                string val = table[row, column];
                                while (val == "B" || (newRow + rowDist) > 7 || (newRow + rowDist) < 0 || (newColumn + colDist) > 7 || (newColumn + colDist) < 0)
                                {
                                    if ((newRow >= 0 && newRow <= 7) && (newColumn >= 0 && newColumn <= 7))
                                    {
                                        val = table[newRow, newColumn];
                                        if (val == "W")
                                        {
                                            hareketOk = true;
                                            break;
                                        }
                                        else if (val == "N")
                                        {
                                            hareketOk = false;
                                            break;
                                        }

                                        newRow = newRow + rowDist;
                                        newColumn = newColumn + colDist;
                                    }
                                    else
                                    {
                                        hareketOk = false;
                                        break;
                                    }
                                }

                                val = table[row, column];

                                if (hareketOk)
                                {
                                    while (val == "B" || (row + rowDist) > 7 || (row + rowDist) < 0 || (column + colDist) > 7 || (column + colDist) < 0)
                                    {
                                        if ((row >= 0 && row <= 7) && (column >= 0 && column <= 7))
                                        {
                                            val = table[row, column];
                                            if (val == "B")
                                            {
                                                pbMatrix[row, column].ImageLocation = "../img/white.png";
                                                table[row, column] = "W";
                                            }
                                            else if (val == "W")
                                            {
                                                break;
                                            }

                                            row = row + rowDist;
                                            column = column + colDist;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        lastPlayedColor = "../img/white.png";
                        label8.Text = "Bilgisayarın Sırası";
                        int w = 0;
                        int b = 0;
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (table[k, m] == "W")
                                {
                                    w++;
                                }
                                else if (table[k, m] == "B")
                                {
                                    b++;
                                }
                            }
                        }
                        label2.Text = w.ToString();
                        label4.Text = b.ToString();

                        var gelen = callComputer();
                        table = gelen;
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (table[k, m] == "B")
                                {
                                    pbMatrix[k, m].ImageLocation = "../img/black.png";
                                }
                            }
                        }
                        lastPlayedColor = "../img/black.png";
                        label8.Text = "İnsanın Sırası";
                        w = 0;
                        b = 0;
                        for (int k = 0; k < 8; k++)
                        {
                            for (int m = 0; m < 8; m++)
                            {
                                if (table[k, m] == "W")
                                {
                                    w++;
                                }
                                else if (table[k, m] == "B")
                                {
                                    b++;
                                }
                            }
                        }
                        label2.Text = w.ToString();
                        label4.Text = b.ToString();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void setStart()
        {
            pbTuple[27].Item1.ImageLocation = "../img/black.png";
            pbTuple[35].Item1.ImageLocation = "../img/white.png";
            pbTuple[28].Item1.ImageLocation = "../img/white.png";
            pbTuple[36].Item1.ImageLocation = "../img/black.png";
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i == 3 && j == 3) || (i == 4 && j == 4))
                    {
                        table[i, j] = "B";
                    }
                    else if ((i == 3 && j == 4) || (i == 4 && j == 3))
                    {
                        table[i, j] = "W";
                    }
                    else
                    {
                        table[i, j] = "N";
                    }
                }
            }

            label2.Text = 2.ToString();
            label4.Text = 2.ToString();
            label8.Text = "Oyuncunun Sırası";
        }

        public static PictureBox[,] ReverseMatrix(PictureBox[,] inputMatrix)
        {
            var outputMatrix = new PictureBox[inputMatrix.GetLength(0), inputMatrix.GetLength(1)];
            var x = 0;
            for (int i = inputMatrix.GetLength(0) - 1; i >= 0; i--)
            {
                var y = 0;
                for (int j = inputMatrix.GetLength(1) - 1; j >= 0; j--)
                {
                    outputMatrix[x, y] = inputMatrix[i, j];
                    y++;
                }
                x++;
            }
            return outputMatrix;

            //https://codereview.stackexchange.com/questions/66791/reversing-a-matrix-in-c adresinden alıntı yapılmıştır.
        }

        public string[,] callComputer()
        {
            Computer computer = new Computer(pbMatrix, table);
            return computer.HamleYap();
        }
    }
}
