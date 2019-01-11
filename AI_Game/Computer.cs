using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AI_Game
{
    class Computer
    {
        PictureBox[,] pbMat = new PictureBox[8, 8];
        public readonly string[,] sMat = new string[8, 8];

        public Computer(PictureBox[,] pbMatrix, string[,] matrix)
        {
            copyPBMatrix(pbMatrix);
            copyStrMatrix(matrix);
        }

        public void copyPBMatrix(PictureBox[,] matrix)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    pbMat[i, j] = matrix[i, j];
                }
            }
        }

        public void copyStrMatrix(string[,] matrix)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sMat[i, j] = matrix[i, j];
                }
            }
        }

        public string[,] HamleYap()
        {
            try
            {
                List<string> siyahPullar = new List<string>();
                List<Tuple<string, string, string, int>> siyahPulOlasıHamleler = new List<Tuple<string, string, string, int>>();

                #region Siyah Pulların tespiti

                int beyazIndex = 0;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (sMat[i, j] == "B")
                        {
                            siyahPullar.Add(i.ToString() + "," + j.ToString());
                        }
                    }
                }
                #endregion

                #region siyahın yapabileceği hamleler
                List<Tuple<string, int, List<Tuple<string, string, string, int>>>> SiyahveBeyaz = new List<Tuple<string, int, List<Tuple<string, string, string, int>>>>();
                foreach (var item in siyahPullar)
                {
                    string[] data = item.Split(',');
                    int row = Convert.ToInt32(data[0]);
                    int column = Convert.ToInt32(data[1]);

                    for (int i = row - 1; i < row + 2; i++)
                    {
                        for (int j = column - 1; j < column + 2; j++)
                        {
                            if ((i >= 0 && j >= 0) && (i <= 7 && j <= 7))
                            {
                                if (sMat[i, j] == "W")
                                {
                                    int rowDist = i - row;
                                    int columnDist = j - column;
                                    int newRow = row + rowDist;
                                    int newColumn = column + columnDist;

                                    if ((newRow >= 0 && newRow <= 7) && (newColumn >= 0 && newColumn <= 7))
                                    {
                                        string val = sMat[newRow, newColumn];
                                        while (val == "W" || (newRow + rowDist) > 7 || (newRow + rowDist) < 0 || (newColumn + columnDist) > 7 || (newColumn + columnDist) < 0)
                                        {
                                            if ((newRow >= 0 && newRow <= 7) && (newColumn >= 0 && newColumn <= 7))
                                            {
                                                val = sMat[newRow, newColumn];
                                                if (val == "W")
                                                {
                                                    beyazIndex++;
                                                }
                                                else if (val == "N")
                                                {
                                                    siyahPulOlasıHamleler.Add(new Tuple<string, string, string, int>(row.ToString() + "," + column.ToString(), newRow.ToString() + "," + newColumn.ToString(), (row - i).ToString() + "," + (column - j).ToString(), beyazIndex));
                                                    beyazIndex = 0;
                                                    break;
                                                }
                                                else
                                                {
                                                    beyazIndex = 0;
                                                    break;
                                                }
                                                newRow = newRow + rowDist;
                                                newColumn = newColumn + columnDist;
                                            }
                                            else
                                            {
                                                beyazIndex = 0;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion


                #region Siyah Pul Hamlelerinin Düzenlenmesi

                for (int i = 0; i < siyahPulOlasıHamleler.Count; i++)
                {
                    while (siyahPulOlasıHamleler[i].Item1 == "0")
                    {
                        i++;
                        if (i >= siyahPulOlasıHamleler.Count)
                        {
                            break;
                        }
                    }
                    if (i >= siyahPulOlasıHamleler.Count)
                    {
                        break;
                    }
                    var itemX = siyahPulOlasıHamleler[i];

                    for (int j = 0; j < siyahPulOlasıHamleler.Count; j++)
                    {
                        while (siyahPulOlasıHamleler[j].Item1 == "0")
                        {
                            j++;
                            if (j >= siyahPulOlasıHamleler.Count)
                            {
                                break;
                            }
                        }

                        if (j >= siyahPulOlasıHamleler.Count)
                        {
                            break;
                        }

                        var itemY = siyahPulOlasıHamleler[j];

                        if (i == j)
                        {

                        }
                        else if (itemX.Item2 == itemY.Item2)
                        {
                            var tup = new List<Tuple<string, string, string, int>>();
                            tup.Add(new Tuple<string, string, string, int>(itemX.Item1 + "*" + itemY.Item1, itemX.Item2, itemX.Item3 + "*" + itemY.Item3, itemX.Item4 + itemY.Item4));
                            siyahPulOlasıHamleler[i] = tup[0];
                            siyahPulOlasıHamleler[j] = new Tuple<string, string, string, int>("0", "0", "0", 0);
                            tup.Clear();
                        }
                    }
                }

                for (int i = 0; i < siyahPulOlasıHamleler.Count; i++)
                {
                    if (siyahPulOlasıHamleler[i].Item1 == "0")
                    {
                        siyahPulOlasıHamleler.RemoveAt(i);
                        i--;
                    }
                }

                #endregion

                #region Siyahın yapacağı hamleden sonra beyazın yapabileceği hamlelerin tespiti

                string[,] geciciMatris = new string[8, 8];
                int geciciIndex = 1;


                for (int i = 0; i < siyahPulOlasıHamleler.Count; i++)  // 3,2  2,3  3,5  5,4  6,5  2,1      6,2******
                {
                    List<Tuple<string, string, string, int>> beyazPulHamlesi = new List<Tuple<string, string, string, int>>();
                    List<string> beyazPullar = new List<string>();
                    for (int u = 0; u < 8; u++)
                    {
                        for (int o = 0; o < 8; o++)
                        {
                            geciciMatris[u, o] = sMat[u, o];
                        }
                    }

                    int nodeRow, nodeColumn, rowDist, columnDist, parentRow, parentColumn, alabildigiTas;
                    string[] parentNode, node, dist;

                    if (siyahPulOlasıHamleler[i].Item1.Contains('*'))
                    {
                        parentNode = siyahPulOlasıHamleler[i].Item2.Split(',');
                        parentRow = Convert.ToInt32(parentNode[0]);
                        parentColumn = Convert.ToInt32(parentNode[1]);
                        var NodeSplitbyStar = siyahPulOlasıHamleler[i].Item1.Split('*');
                        var DistSplitbyStar = siyahPulOlasıHamleler[i].Item3.Split('*');
                        alabildigiTas = siyahPulOlasıHamleler[i].Item4;
                        for (int m = 0; m < NodeSplitbyStar.Length; m++)
                        {
                            node = NodeSplitbyStar[m].Split(',');
                            nodeRow = Convert.ToInt32(node[0]);
                            nodeColumn = Convert.ToInt32(node[1]);
                            dist = DistSplitbyStar[m].Split(',');
                            rowDist = Convert.ToInt32(dist[0]);
                            columnDist = Convert.ToInt32(dist[1]);
                            geciciMatris[nodeRow, nodeColumn] = "B";
                            int Row = nodeRow + rowDist;
                            int Column = nodeColumn + columnDist;
                            if ((Row >= 0 && Column >= 0) && (Row <= 7 && Column <= 7))
                            {
                                string val = geciciMatris[Row, Column];

                                while (val == "W")
                                {
                                    geciciMatris[Row, Column] = "B";
                                    Row = Row + rowDist;
                                    Column = Column + columnDist;
                                    if ((Row >= 0 && Column >= 0) && (Row <= 7 && Column <= 7))
                                    {
                                        val = geciciMatris[Row, Column];
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } // Siyah pulun ilk hamlesini yapıp alabileceğini pulları bulduk
                                  // alt tarafta beyaz pulun ne yapabileceğine bakacağız

                                for (int k = 0; k < 8; k++)
                                {
                                    for (int n = 0; n < 8; n++)
                                    {
                                        if (geciciMatris[k, n] == "W") // beyaz pulları tespit eddiyoruz
                                        {
                                            beyazPullar.Add(k.ToString() + "," + n.ToString());
                                        }
                                    }
                                }

                                foreach (var itemB in beyazPullar)
                                {
                                    int siyahIndex = 0;
                                    string[] location = itemB.Split(',');
                                    int rowBeyaz = Convert.ToInt32(location[0]);
                                    int columnBeyaz = Convert.ToInt32(location[1]);

                                    for (int k = rowBeyaz - 1; k < rowBeyaz + 2; k++)
                                    {
                                        for (int l = columnBeyaz - 1; l < columnBeyaz + 2; l++)
                                        {
                                            if ((k >= 0 && l >= 0) && (k <= 7 && l <= 7))
                                            {
                                                if (geciciMatris[k, l] == "B" && k != rowBeyaz && l != columnBeyaz)
                                                {
                                                    int rowDistBeyaz = k - rowBeyaz;
                                                    int columnDistBeyaz = l - columnBeyaz;
                                                    int newRow = rowBeyaz + rowDistBeyaz;
                                                    int newColumn = columnBeyaz + columnDistBeyaz;

                                                    string value = geciciMatris[newRow, newColumn];
                                                    while (value == "B" && newRow < 7 && newRow > 0 && newColumn < 7 && newColumn > 0)
                                                    {
                                                        if ((newRow >= 0 && newRow <= 7) && (newColumn >= 0 && newColumn <= 7))
                                                        {
                                                            value = geciciMatris[newRow, newColumn];
                                                            if (value == "B")
                                                            {
                                                                siyahIndex++;
                                                            }
                                                            else if (value == "N")
                                                            {
                                                                beyazPulHamlesi.Add(new Tuple<string, string, string, int>(rowBeyaz.ToString() + "," + columnBeyaz.ToString(), newRow.ToString() + "," + newColumn.ToString(), (rowBeyaz - k).ToString() + "," + (columnBeyaz - l).ToString(), siyahIndex));
                                                                siyahIndex = 0;
                                                            }
                                                            else
                                                            {
                                                                siyahIndex = 0;
                                                                break;
                                                            }
                                                            newRow = newRow + rowDistBeyaz;
                                                            newColumn = newColumn + columnDistBeyaz;
                                                        }
                                                        else
                                                        {
                                                            siyahIndex = 0;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        SiyahveBeyaz.Add(new Tuple<string, int, List<Tuple<string, string, string, int>>>(siyahPulOlasıHamleler[i].Item1, siyahPulOlasıHamleler[i].Item4, beyazPulHamlesi));
                    }
                    else
                    {
                        parentNode = siyahPulOlasıHamleler[i].Item1.Split(',');
                        parentRow = Convert.ToInt32(parentNode[0]);
                        parentColumn = Convert.ToInt32(parentNode[1]);
                        node = siyahPulOlasıHamleler[i].Item2.Split(',');
                        nodeRow = Convert.ToInt32(node[0]);
                        nodeColumn = Convert.ToInt32(node[1]);
                        dist = siyahPulOlasıHamleler[i].Item3.Split(',');
                        rowDist = Convert.ToInt32(dist[0]);
                        columnDist = Convert.ToInt32(dist[1]);
                        alabildigiTas = siyahPulOlasıHamleler[i].Item4;

                        geciciMatris[nodeRow, nodeColumn] = "B";
                        int Row = nodeRow + rowDist;
                        int Column = nodeColumn + columnDist;
                        if ((Row >= 0 && Column >= 0) && (Row <= 7 && Column <= 7))
                        {
                            string val = geciciMatris[Row, Column];

                            while (val == "W")
                            {
                                geciciMatris[Row, Column] = "B";
                                Row = Row + rowDist;
                                Column = Column + columnDist;
                                if ((Row >= 0 && Column >= 0) && (Row <= 7 && Column <= 7))
                                {
                                    val = geciciMatris[Row, Column];
                                }
                                else
                                {
                                    break;
                                }
                            } // Siyah pulun ilk hamlesini yapıp alabileceğini pulları bulduk
                              // alt tarafta beyaz pulun ne yapabileceğine bakacağız

                            for (int m = 0; m < 8; m++)
                            {
                                for (int n = 0; n < 8; n++)
                                {
                                    if (geciciMatris[m, n] == "W") // beyaz pulları tespit eddiyoruz
                                    {
                                        beyazPullar.Add(m.ToString() + "," + n.ToString());
                                    }
                                }
                            }

                            foreach (var itemB in beyazPullar)
                            {
                                int siyahIndex = 0;
                                string[] location = itemB.Split(',');
                                int rowBeyaz = Convert.ToInt32(location[0]);
                                int columnBeyaz = Convert.ToInt32(location[1]);

                                for (int k = rowBeyaz - 1; k < rowBeyaz + 2; k++)
                                {
                                    for (int l = columnBeyaz - 1; l < columnBeyaz + 2; l++)
                                    {
                                        if ((k >= 0 && l >= 0) && (k <= 7 && l <= 7))
                                        {
                                            if (geciciMatris[k, l] == "B" && k != rowBeyaz && l != columnBeyaz)
                                            {
                                                int rowDistBeyaz = k - rowBeyaz;
                                                int columnDistBeyaz = l - columnBeyaz;
                                                int newRow = rowBeyaz + rowDistBeyaz;
                                                int newColumn = columnBeyaz + columnDistBeyaz;

                                                string value = geciciMatris[newRow, newColumn];
                                                while (value == "B" && newRow < 7 && newRow > 0 && newColumn < 7 && newColumn > 0)
                                                {
                                                    if ((newRow >= 0 && newRow <= 7) && (newColumn >= 0 && newColumn <= 7))
                                                    {
                                                        value = geciciMatris[newRow, newColumn];
                                                        if (value == "B")
                                                        {
                                                            siyahIndex++;
                                                        }
                                                        else if (value == "N")
                                                        {
                                                            beyazPulHamlesi.Add(new Tuple<string, string, string, int>(rowBeyaz.ToString() + "," + columnBeyaz.ToString(), newRow.ToString() + "," + newColumn.ToString(), (rowBeyaz - k).ToString() + "," + (columnBeyaz - l).ToString(), siyahIndex));
                                                            siyahIndex = 0;
                                                        }
                                                        else
                                                        {
                                                            siyahIndex = 0;
                                                            break;
                                                        }
                                                        newRow = newRow + rowDistBeyaz;
                                                        newColumn = newColumn + columnDistBeyaz;
                                                    }
                                                    else
                                                    {

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        SiyahveBeyaz.Add(new Tuple<string, int, List<Tuple<string, string, string, int>>>(siyahPulOlasıHamleler[i].Item1, siyahPulOlasıHamleler[i].Item4, beyazPulHamlesi));
                    }
                }
                #endregion

                #region Beyaz Pul Hamlelerinin Düzenlenmesi

                foreach (var kontrol in SiyahveBeyaz) // Beyaz pul hamlelerinin düzenlenmesi
                {
                    for (int i = 0; i < kontrol.Item3.Count; i++)
                    {
                        while (kontrol.Item3[i].Item1 == "0")
                        {
                            i++;
                            if (i >= kontrol.Item3.Count)
                            {
                                break;
                            }
                        }
                        if (i >= kontrol.Item3.Count)
                        {
                            break;
                        }

                        for (int j = 0; j < kontrol.Item3.Count; j++)
                        {
                            while (kontrol.Item3[j].Item1 == "0")
                            {
                                j++;
                                if (j >= kontrol.Item3.Count)
                                {
                                    break;
                                }
                            }

                            if (j >= kontrol.Item3.Count)
                            {
                                break;
                            }

                            if (i == j)
                            {

                            }
                            else if (kontrol.Item3[i].Item2 == kontrol.Item3[j].Item2)
                            {
                                var tup = new List<Tuple<string, string, string, int>>();
                                tup.Add(new Tuple<string, string, string, int>(kontrol.Item3[i].Item1 + "*" + kontrol.Item3[j].Item1, kontrol.Item3[i].Item2, kontrol.Item3[i].Item3 + "*" + kontrol.Item3[j].Item3, kontrol.Item3[i].Item4 + kontrol.Item3[j].Item4));
                                kontrol.Item3[i] = tup[0];
                                kontrol.Item3[j] = new Tuple<string, string, string, int>("0", "0", "0", 0);
                                tup.Clear();
                            }
                        }
                    }

                    for (int i = 0; i < kontrol.Item3.Count ; i++)
                    {
                        if (kontrol.Item3[i].Item1 == "0")
                        {
                            kontrol.Item3.RemoveAt(i);
                            i--;
                        }
                    }

                }
                #endregion


                #region  Karar Mekanizması
                List<Tuple<int, int, int>> son = new List<Tuple<int, int, int>>();
                int alabilecegiMax = 0;

                for (int i = 0; i < SiyahveBeyaz.Count; i++)
                {
                    for (int j = 0; j < SiyahveBeyaz[i].Item3.Count; j++)
                    {
                        if (j == 0)
                        {
                            alabilecegiMax = SiyahveBeyaz[i].Item3[j].Item4;
                        }
                        else
                        {
                            if (alabilecegiMax < SiyahveBeyaz[i].Item3[j].Item4)
                            {
                                alabilecegiMax = SiyahveBeyaz[i].Item3[j].Item4;
                            }
                        }
                    }

                    son.Add(new Tuple<int, int, int>(SiyahveBeyaz[i].Item2, SiyahveBeyaz[i].Item3.Count, alabilecegiMax));
                }

                int index = 0;

                for (int i = 0; i < son.Count; i++)
                {

                    for (int j = 0; j < son.Count; j++)
                    {
                        if (i == j || index == j)
                        {

                        }
                        else
                        {
                            if (son[j].Item1 >= son[index].Item1 && son[j].Item2 <= son[index].Item2 && son[j].Item3 <= son[index].Item3)
                            {
                                index = j;
                            }
                        }
                    }
                }

                if (index <0)
                {

                }

                if (siyahPulOlasıHamleler[index].Item1.Contains('*'))
                {
                    var positionSplitbyStar = siyahPulOlasıHamleler[index].Item1.Split('*');
                    var distanceSplitbyStar = siyahPulOlasıHamleler[index].Item3.Split('*');

                    for (int i = 0; i < positionSplitbyStar.Length; i++)
                    {
                        var position = siyahPulOlasıHamleler[index].Item2.Split(',');
                        var distance = distanceSplitbyStar[i].Split(',');
                        var lastIndex = positionSplitbyStar[i].Split(',');

                        var rowLast = Convert.ToInt32(position[0]);
                        var columnLast = Convert.ToInt32(position[1]);
                        var rowDistance = Convert.ToInt32(distance[0]);
                        var columnDistance = Convert.ToInt32(distance[1]);

                        string valueLast = sMat[rowLast, columnLast] = "B";
                        while (rowLast.ToString() + "," + columnLast.ToString() != lastIndex[0] + "," + lastIndex[1])
                        {
                            rowLast = rowLast + rowDistance;
                            columnLast = columnLast + columnDistance;
                            sMat[rowLast, columnLast] = "B";
                        }
                    }
                }
                else
                {
                    var position = siyahPulOlasıHamleler[index].Item2.Split(',');
                    var distance = siyahPulOlasıHamleler[index].Item3.Split(',');
                    var rowLast = Convert.ToInt32(position[0]);
                    var columnLast = Convert.ToInt32(position[1]);
                    var rowDistance = Convert.ToInt32(distance[0]);
                    var columnDistance = Convert.ToInt32(distance[1]);
                    var lastIndex = siyahPulOlasıHamleler[index].Item1;
                    string valueLast = sMat[rowLast, columnLast] = "B";
                    while (rowLast.ToString() + "," + columnLast.ToString() != lastIndex)
                    {
                        rowLast = rowLast + rowDistance;
                        columnLast = columnLast + columnDistance;
                        sMat[rowLast, columnLast] = "B";
                    }

                }

                return sMat;
                #endregion
            }
            catch (Exception ex)
            {
                return sMat;
            }
        }
    }
}
