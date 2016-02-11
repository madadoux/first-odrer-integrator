using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace first_odrer_integrator
{
    public partial class Form1 : Form
    {

         PointF[] pEuler;
         PointF[] pHeun;
         PointF[] pImporvedEuler;
         PointF[] pRnugKutta4;
         PointF[] pTrue;

         PointF p1, p2,p3,p4;
         int size;
         float H;
         float Y_0;
         float X_0;
         float X_F , Y_F; 
        public Form1()
        {
            InitializeComponent();

        }


        float mainF(float x, float y)
        {

            return (2 * x * y); 
        }

        void calcTrue()
        {
            if (pTrue != null)
            {
                pTrue[0].X = X_0; pTrue[0].Y = Y_0;

                for (int i = 1; i < size; i++)
                {
                    pTrue[i].X = pTrue[i - 1].X + H;
                    pTrue[i].Y = (float)Math.Exp(Math.Pow(pTrue[i].X, 2) - 1);
                }
            }
        }
        void calcEuler()
        {
            if (pEuler != null)
            {
                pEuler[0].X = X_0; pEuler[0].Y = Y_0;

                for (int i = 1; i < size; i++)
                {
                    pEuler[i].X = pEuler[i - 1].X + H;
                    pEuler[i].Y = pEuler[i - 1].Y + H * mainF(pEuler[i - 1].X, pEuler[i - 1].Y);

                }
            }
        }
        void calcImEU()
        {
            if (pImporvedEuler != null)
            {
                pImporvedEuler[0].X = X_0; pImporvedEuler[0].Y = Y_0;

                for (int i = 1; i < size; i++)
                {
                    pImporvedEuler[i].X =  pImporvedEuler[i - 1].X + H;

                    float tmp0= mainF(pImporvedEuler[i - 1].X, pImporvedEuler  [i - 1].Y);//f(Xi,Yi)

                    float tmp1  = pEuler[i - 1].Y + H * mainF(pImporvedEuler[i - 1].X, pImporvedEuler[i - 1].Y);//Y*i+1

                    float tmp2 = mainF(pImporvedEuler[i].X, tmp1);//f(Xi+1,Y*i+1)

                    pImporvedEuler[i].Y = pImporvedEuler[i - 1].Y + (H / 2) * (tmp0 + tmp2);//Yi+1 = ( Yi + h/2 ( f(Xi,Yi) + f(Xi+1,Y*i+1))
                }
            }


        }
        void calcHEUN()
        {
            if (pHeun != null)
            {
                pHeun[0].X = X_0; pHeun[0].Y = Y_0;

                for (int i = 1; i < size; i++)
                {

                     pHeun[i].X = pHeun[i - 1].X + H;


                    float k1 = mainF(pHeun[i - 1].X, pHeun[i - 1].Y);
                    float k2 = mainF(pHeun[i - 1].X + H, pHeun[i - 1].Y + k1 * H);

                    pHeun[i].Y = pHeun[i - 1].Y + H*(k1 / 2+ k2 / 2); 


                }
            }

        }
        void calcRungKutta()
        {

            if (pRnugKutta4 != null)
            {

                pRnugKutta4[0].X = X_0; pRnugKutta4[0].Y = Y_0;

                for (int i = 1; i <size  ; i++)
                {
                    pRnugKutta4[i].X = pRnugKutta4[i - 1].X + H;

                    float k1 = mainF(pRnugKutta4[i - 1].X, pRnugKutta4[i - 1].Y);
                    float k2 = mainF(pRnugKutta4[i - 1].X +( H / 2), pRnugKutta4[i - 1].Y + (k1 * H) / 2);
                    float k3 = mainF(pRnugKutta4[i - 1].X +( H / 2), pRnugKutta4[i - 1].Y + (k2 * H) / 2);
                    float k4 = mainF(pRnugKutta4[i - 1].X + H, pRnugKutta4[i - 1].Y + (k3 * H));

                    pRnugKutta4[i].Y = pRnugKutta4[i - 1].Y + (H/6)* (k1 + 2 * k2 + 2 * k3 + k4);  


                }

            }

        }

        void VarInit()
        {

            float.TryParse(Y0.Text, out Y_0);
            float.TryParse(X0.Text, out X_0);
            float.TryParse(hVal.Text, out H);

            float.TryParse(XF.Text, out X_F);

            if (H != 0)
            {
                size = (int)((X_F - X_0) / H) + 1;
                if (size <= 0)
                { MessageBox.Show("X_F can't be less than X_0 or h ! enter  the X_F again "); 
                }
            }
            else
            {
                MessageBox.Show("H can't be 0 ");
            }
      
        }
        void DisTable()
        {
            tbl.Text = "X\r\nYtrue\r\n[1]nYEuler|E|RE\r\n[2]YImprovedEuler|E|RE\r\n[3]YHeun|E|RE\r\n[4]YRungKutta|E|RE\r\n"+"___________________\r\n";
            for (int i = 0; i < size; i++)
            {
                tbl.Text += pTrue[i].X.ToString() + "\r\n" + Math.Round(pTrue[i].Y, 2).ToString()+ "\r\n"
                +"[1]"+
                ((pEuler != null) ? (Math.Round(pEuler[i].Y, 2).ToString()) + '|' + Err(pTrue[i].Y, pEuler[i].Y).ToString() + '|' + RE(pTrue[i].Y, pEuler[i].Y).ToString() : "--") + "\r\n"
                +"[2]"+
               ((pImporvedEuler != null) ? (Math.Round(pImporvedEuler[i].Y, 2).ToString()) + '|' + Err(pTrue[i].Y, pImporvedEuler[i].Y).ToString() + '|' + RE(pImporvedEuler[i].Y, pImporvedEuler[i].Y).ToString() : "--") + "\r\n"
                +"[3]"+
               ((pHeun != null) ? (Math.Round(pHeun[i].Y, 2).ToString()) + '|' + Err(pTrue[i].Y, pHeun[i].Y).ToString() + '|' + RE(pTrue[i].Y, pHeun[i].Y).ToString() : "--") + "\r\n"
                +"[4]"+
               ((pRnugKutta4 != null) ? (Math.Round(pRnugKutta4[i].Y, 2).ToString()) + '|' + Err(pRnugKutta4[i].Y, pRnugKutta4[i].Y).ToString() + '|' + RE(pRnugKutta4[i].Y, pRnugKutta4[i].Y).ToString() : "--") + "\r\n"+
              
              "___________________\r\n";

            }
        }

        double Err( double v1 , double  v2 ){
            return  Math.Abs(v2 - v1); 
        
        }

        double RE ( double v1, double v2 ){

            return Math.Abs((v1 - v2) / v1) * 100;
        }

        private void Calc()
        {
            VarInit();
            if (size > 0)
            {
                pTrue = new PointF[size];
                calcTrue();


                if (checkBox1.Checked)
                {
                    pEuler = new PointF[size];
                    calcEuler();
                }
                if (checkBox2.Checked)
                {
                    pImporvedEuler = new PointF[size];
                    calcImEU(); 
                }
                if (checkBox3.Checked)
                {
                    pHeun = new PointF[size];
                    calcHEUN(); 
                }
                if (checkBox4.Checked)
                {
                    pRnugKutta4 = new PointF[size];
                    calcRungKutta();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void panel1_Paint(object sender, PaintEventArgs e)
        {

            
            p1 = new PointF(150, 0);
            p2 = new PointF(150, panel1.Size.Height);
            p3 = new PointF(0, panel1.Size.Height - 30);
            p4 = new PointF(panel1.Size.Width, panel1.Size.Height - 30);

            e.Graphics.DrawString("TRUE", DefaultFont, Brushes.Black, 0f, 0f);
            e.Graphics.DrawString("EULER", DefaultFont, Brushes.Blue, 0f, 15f);
            e.Graphics.DrawString("IMPROVED_EULER", DefaultFont, Brushes.Red, 0f,  30f);
            e.Graphics.DrawString("HEUN", DefaultFont, Brushes.Aqua, 0f, 45f);
            e.Graphics.DrawString("RUNG_KUTTA", DefaultFont, Brushes.Purple, 0f, 60f);

            e.Graphics.DrawLine(Pens.Gold, p1, p2);
            e.Graphics.DrawLine(Pens.Gold, p3, p4); 
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.TranslateTransform(150 ,panel1.Size.Height - 30);
            e.Graphics.ScaleTransform(40,-.025f);


            if (pTrue != null)
            {
                try
                {
                    e.Graphics.DrawLines(Pens.Black, pTrue.ToList().Where(p => p.Y <= int.MaxValue).ToArray());
                }
                catch (Exception)
                {
                    MessageBox.Show("CAN'T Draw (true) such these BIG numbers TRY to lower the |h| or |X| that u want ");
                    //throw;
                }
            }


            if (pEuler != null)
            {
                try
                {
                    e.Graphics.DrawLines(Pens.Blue, pEuler.ToList().Where(p => p.Y <= int.MaxValue).ToArray());
                }
                catch (Exception)
                {
                    MessageBox.Show("CAN'T Draw (Euler) such these BIG numbers TRY to lower the |h| or |X| that u want ");                   
                    //throw;
                }
            }


            if (pImporvedEuler != null)
            {
                try
                {
                    e.Graphics.DrawLines(Pens.Red,  pImporvedEuler.ToList().Where(p => p.Y <= int.MaxValue).ToArray());
                }
                catch (Exception)
                {
                    MessageBox.Show("CAN'T Draw (ImprovedEuler) such these BIG numbers TRY to lower the |h| or |X| that u want ");
                    //throw;
                }
            }

            if (pHeun != null)
            {
                try
                {
                    e.Graphics.DrawLines(Pens.Aqua, pHeun.ToList().Where(p => p.Y <= int.MaxValue).ToArray());
                }
                catch (Exception)
                {
                   MessageBox.Show("CAN'T Draw  (HEUN) such these BIG numbers TRY to lower the |h| or |X| that u want ");
                    //throw;
                }
            }

            if (pRnugKutta4 != null)
            {
                try
                {
                    e.Graphics.DrawLines(Pens.Purple, pRnugKutta4.ToList().Where(p => p.Y <= int.MaxValue).ToArray());
                }
                catch (Exception)
                {
                    MessageBox.Show("CAN'T Draw  (RungKutta) such these BIG numbers TRY to lower the |h| or |X| that u want ");
                    //throw;
                }
            }


        }
            
        private void button1_Click(object sender, EventArgs e)
        {

            Calc(); 
            panel1.Refresh();
            DisTable(); 
           
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
             pEuler=null;
             pHeun=null;
             pImporvedEuler=null;
             pRnugKutta4=null;
             pTrue=null;

            tbl.Text = "Cleared ";
            panel1.Refresh();
        }

    }
}
