using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Berlekamp_Massey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[] s;
        int delta;
        int[] C;
        int[] C1;
        int L;
        int[] Ls;
        int[] T;
        int delta1;
        int l;

        int mod(int a, int r)
        {
            int result = a % r;
            if (result < 0)
                result += r;
            return result;
        }

        string str(int[] a, int length = 0)
        {
            if (length == 0)
            {
                for (int i = a.Length - 1; i >= 0; i--)
                {
                    if (a[i] != 0)
                    {
                        length = i+1;
                        break;
                    }
                }
            }
            var res = "";
            for (int i = 0; i < length; i++)
                res += a[i].ToString();
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int t = textBox1.TextLength;
            textBox2.Clear();
            s = new int[t];
            for (int i=0; i<t; i++)
            {
                s[i] = Convert.ToInt32(textBox1.Text[i].ToString());
            }
            C = new int[t];
            C[0] = 1;
            C1 = new int[t];
            C1[0] = 1;
            L = 0;
            T = new int[t];
            Ls = new int[t+1];
            delta1 = 1;
            l = 1;
            listView1.Items.Clear();
            listView1.Items.Add(new ListViewItem(new string[]{"","","","1","","1","0","1","1"}));
            for (int n = 0; n<t; n++)
            {
                var li = new ListViewItem(n.ToString());
                li.SubItems.Add(str(s, n + 1));
                delta = 0;
                for (int i=n; i>=0; i--)
                {
                    delta += C[n - i] * s[i];
                }
                delta = mod(delta,2);
                li.SubItems.Add(delta.ToString());
                if (delta == 0)
                {
                    l++;
                    li.SubItems.AddRange(new string[]{ "", "","","","",l.ToString()});
                }
                else
                {

                    C.CopyTo(T,0);
                    for (int i=l; i<t; i++)
                    {
                        C[i] = mod(C[i] - (C1[i-l] * (delta/delta1)), 2);
                    }
                    li.SubItems.Add(str(C));
                    if (2*L > n)
                    {
                        l++;
                        li.SubItems.AddRange(new string[] { "-", "", "", "", l.ToString() });
                    }
                    else
                    {
                        L = n + 1 - L;
                        l = 1;
                        delta1 = delta;
                        T.CopyTo(C1, 0);
                        li.SubItems.AddRange(new string[] { "+", str(C1), L.ToString(), delta1.ToString(), l.ToString() });
                    }
                }
                Ls[n+1] = L;
                listView1.Items.Add(li);
            }
            int k = 0;
            for (int i=C.Length - 1; i>=0; i--)
            {
                if (C[i] != 0)
                {
                    k = i;
                    break;
                }
            }
            for (int i = 0; i <= k; i++)
                textBox2.Text += C[i].ToString();
            textBox3.Text = L.ToString();

            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            chart1.ChartAreas.Add(new ChartArea("1"));
            chart1.Series.Add("Профиль");
            chart1.Series[0].IsVisibleInLegend = false;

            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            chart1.Series[0].ChartType = SeriesChartType.FastPoint;

            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < Ls.Length; i++ )
            {
                chart1.Series[0].Points.AddY(Ls[i]);
                chart1.Series[0].Points.Last().AxisLabel = i.ToString();
            }
            /*
                foreach (var o in outlines)
                {
                    //       listBox1.Items.Add("[" + o.Key + "] - " + o.Value.output + ", " + o.Value.input);
                    chart1.Series[0].Points.AddY(o.Value.input);
                    chart1.Series[1].Points.AddY(o.Value.output);
                    chart1.Series[1].Points.Last().AxisLabel = o.Key;
                    chart1.Series[1].Points.Last().ToolTip = o.Value.output.ToString();
                    chart1.Series[0].Points.Last().ToolTip = o.Value.input.ToString();
                    chart1.Series[1].Points.Last().LabelToolTip = o.Value.input.ToString() + "/" + o.Value.output.ToString();
                }*/
        }
    }
}
