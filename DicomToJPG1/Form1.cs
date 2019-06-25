using Dicom.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DicomToJPG1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ////var image = new DicomImage(@"C:\dcm\input.dcm");
            //ImageManager.SetImplementation(WinFormsImageManager.Instance);
            //image.RenderImage().AsBitmap().Save(@"C;\dcm\inputtest.jpg");
            ////Bitmap renderedImage = image.RenderImage().As<Bitmap>();
            ////renderedImage.Save(@"C:\dcm\input123.jpg", ImageFormat.Jpeg);
            //saveJpeg(@"C:\dcm\input123.jpg",renderedImage,95);

            combostatic = comboBox1.Text;
            text1static = textBox1.Text;
            label3.Text = "Converting...";
            fileCount = 0;
           
            Thread outputThread = new Thread(new ThreadStart(DcmToImage));
            outputThread.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        public static string text1static = "";
        private void DcmToImage()
        {
            if (text1static != "")
            {
               
                List<string> lstr = DirSearch(text1static);

              

                if (!Directory.Exists(text1static + @"\Output")) {
                    Directory.CreateDirectory(text1static + @"\Output");
                }

                for (int i = 0; i < lstr.Count; i++)
                {
                    if (File.Exists(lstr[i]))
                    {
                        var image = new DicomImage(lstr[i]);
                        //ImageManager.SetImplementation(WinFormsImageManager.Instance);
                        //image.RenderImage().AsBitmap().Save(@"C;\dcm\inputtest.jpg");
                        Bitmap renderedImage = image.RenderImage().As<Bitmap>();
                        //String outputFileStr = lstr[i].Replace("dcm","jpg");
                        //MessageBox.Show(Path.GetFileNameWithoutExtension(lstr[i]) + ".jpg");
                        if (combostatic == "jpg")
                        {
                            renderedImage.Save(text1static + @"\Output\" + Path.GetFileNameWithoutExtension(lstr[i]) + "." + combostatic, ImageFormat.Jpeg);
                        }
                        else if (combostatic == "png")
                        {
                            renderedImage.Save(text1static + @"\Output\" + Path.GetFileNameWithoutExtension(lstr[i]) + "." + combostatic, ImageFormat.Png);
                        }
                        else if (combostatic == "bmp")
                        {
                            renderedImage.Save(text1static + @"\Output\" + Path.GetFileNameWithoutExtension(lstr[i]) + "." + combostatic, ImageFormat.Bmp);
                        }
                        else if (combostatic == "tiff")
                        {
                            renderedImage.Save(text1static + @"\Output\" + Path.GetFileNameWithoutExtension(lstr[i]) + "." + combostatic, ImageFormat.Tiff);
                        }
                        else if (combostatic == "gif")
                        {
                            renderedImage.Save(text1static + @"\Output\" + Path.GetFileNameWithoutExtension(lstr[i]) + "." + combostatic, ImageFormat.Gif);
                        }
                    }

                    
                }
                //MessageBox.Show("轉檔完成");
                ChangeLb(label3, "Convert Completed");
                System.Diagnostics.Process.Start("explorer.exe", "\""+ text1static + @"\Output"+"\""); 
            }
            else
            {
                MessageBox.Show("Import Folder can not be empty.");
            }
        }

        private delegate void lbCallBack(Label label, String value);
        private void ChangeLb(Label label, String value)
        {
            if (label.InvokeRequired)
            {
                lbCallBack d = new lbCallBack(ChangeLb);
                this.Invoke(d, new object[]
                {
            label,
            value
                });
            }
            else
            {
                label.Text = value;
            }
        }


        private static string combostatic = "";

        private static int fileCount = 0;
        private static List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*." + "dcm")) //ext name .dcm
                {
                    files.Add(f);
                    fileCount++;
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }
    
    }
}
