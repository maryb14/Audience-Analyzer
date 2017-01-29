using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;



namespace Audience_Analyzer
{
   
    public partial class Form1 : Form
    {

        string[] images_paths;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var emotion = new EmotionServiceClient("9bc1b83197874e59b1a6230dbf466353");
            //openFileDialog1.Filter = "JPEG Files (*.jpeg) | *.jpeg | PNG Files (*.png) | *.png | JPG Files (*.jpg) | *.jpg | GIF Files (*.gif) | *.gif | All files (*.*) | *.*";
            openFileDialog1.Multiselect = true;
            DialogResult find_image = openFileDialog1.ShowDialog();

            if (find_image == DialogResult.OK)
            {
                images_paths = openFileDialog1.FileNames;
                foreach (String path in images_paths) {
                    textBox1.Text += Path.GetFileName(path) + "; ";
                    button2.Enabled = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MakeRequest(images_paths);
        }
        public async void MakeRequest(String[] paths)
        {
            /* Core.Recognizer geani = new Core.Recognizer();
            string path = "C:\\Users\\Alexandru\\Downloads\\sample.jpg";
            Console.WriteLine(geani.getJSON(path));*/
            int[] faces_with_em = new int[10];
            int[] max_faces_with_em = new int[10];
            for (int i = 0; i < 10; ++i) max_faces_with_em[i] = 0;
            int[] min_faces_with_em = new int[10];
            for (int i = 0; i < 10; ++i) min_faces_with_em[i] = 1000;
            double[] avg_faces_with_em = new double[10];
            for (int i = 0; i < 10; ++i) avg_faces_with_em[i] = 0;
            double[] overall_scores = new double[10];
            for (int i = 0; i < 10; ++i) overall_scores[i] = 0;
            int number_of_faces = 0;
            foreach (String path in paths)
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0a88b606aa3d4ac69c54a6890ad09984");

                var uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?" + queryString;

                HttpResponseMessage response;

                // Request body
                //byte[] byteData = Encoding.UTF8.GetBytes(@"C:\Users\Alexandru\Downloads\sample.jpg");
                //string path = paths[i];
                byte[] byteData = System.IO.File.ReadAllBytes(path);

                Console.WriteLine(byteData);

                for (int i = 1; i <= 8; ++i)
                {
                    faces_with_em[i] = 0;
                }

                using (var content = new ByteArrayContent(byteData))
                {
                    int emotion = 0; double max_emotion = 0; double[] emotion_scores = new double[10] ;
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                    string json = JsonConvert.SerializeObject(response.Content.ReadAsStringAsync().Result);
                    //Console.WriteLine(json);
                    //System.IO.File.WriteAllText(@"D:\Programare\Hack Cambridge\Api Microsoft cognitive\json.txt", response.Content.ReadAsStringAsync().Result);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Face[] faces = js.Deserialize<Face[]>(response.Content.ReadAsStringAsync().Result);
                    number_of_faces += faces.Length;
                    foreach (Face f in faces)
                    {
                        emotion_scores[1] = f.scores.anger;
                        emotion_scores[2] = f.scores.contempt;
                        emotion_scores[3] = f.scores.disgust;
                        emotion_scores[4] = f.scores.fear;
                        emotion_scores[5] = f.scores.happiness;
                        emotion_scores[6] = f.scores.neutral;
                        emotion_scores[7] = f.scores.sadness;
                        emotion_scores[8] = f.scores.surprise;
                        for(int k = 1; k <= 8; ++k)
                        {
                            if(emotion_scores[k] > max_emotion)
                            {
                                max_emotion = emotion_scores[k];
                                emotion = k;
                            }
                            overall_scores[k] += emotion_scores[k];
                        }
                        faces_with_em[emotion]++;
                    }
                }
                for (int i = 1; i <= 8; ++i)
                {
                    if (faces_with_em[i] > max_faces_with_em[i]) max_faces_with_em[i] = faces_with_em[i];
                    if (faces_with_em[i] < min_faces_with_em[i]) min_faces_with_em[i] = faces_with_em[i];
                    avg_faces_with_em[i] += faces_with_em[i];
                }
            }
            for (int i = 1; i <= 8; ++i)
            {
                avg_faces_with_em[i] = (avg_faces_with_em[i] / paths.Length);
                overall_scores[i] /= (double)number_of_faces;
            }
            Form2 showing_result = new Form2();
            showing_result.Show();
            showing_result.receiveInfo(max_faces_with_em, min_faces_with_em, overall_scores, number_of_faces, avg_faces_with_em);
            
      }
}
    public class Face
    {
        public Rectangle faceRectangle { get; set; }
        public Scores scores { get; set; }
    }

    public class Rectangle
    {
        public int width { get; set; }
        public int top { get; set; }
        public int left { get; set; }
        public int height { get; set; }
    }

    public class Scores
    {
        public double anger { get; set; }
        public double disgust { get; set; }
        public double contempt { get; set; }
        public double fear { get; set; }
        public double happiness { get; set; }
        public double neutral { get; set; }
        public double sadness { get; set; }
        public double surprise { get; set; }
    }
}
