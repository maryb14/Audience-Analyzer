using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audience_Analyzer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void receiveInfo(int[] max_face_with_em, int[] min_faces_with_em, double[] overall_scores, int number_of_faces, double[] avg_faces_with_em)
        {
            string[] emotions = new string[10];
            emotions[1] = "anger"; emotions[2] = "contempt"; emotions[3] = "disgust"; emotions[4] = "fear";
            emotions[5] = "happiness"; emotions[6] = "neutral"; emotions[7] = "sadness"; emotions[8] = "surprise";
            for (int i = 1; i <= 8; ++i)
            {
                textBox1.Text += ("Statistics on " + emotions[i] + ":\r\n" + "Max and min number of faces with this emotion: ")
                    + max_face_with_em[i].ToString() + " " + min_faces_with_em[i].ToString() + "\r\n" + "Average number of people with this emotion: "+
                    avg_faces_with_em[i].ToString() + "\r\n" + "The average of the scores for this emotion: " + overall_scores[i].ToString() + "\r\n";
            }
        }
    }
}
