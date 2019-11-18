using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Birthday
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

    /// <summary>
    /// Метод доступа к информации о пользователе.
    /// </summary>
    /// <param name="name1">фамилия</param>
    /// <param name="name2">имя</param>
    /// <param name="name3">отчество</param>
    /// <param name="team">идентификатор подразделения</param>
    /// <param name="job">идентификатор должности</param>
    /// <param name="phone1">внутренний телефон</param>
    /// <param name="phone2">внешний телефон</param>
    /// <param name="dob">дата рождения</param>
    /// <param name="computer">адрес компьютера</param>
    /// <param name="login">логин в Active Directory</param>
    /// <param name="email">электронная почта</param>
    /// <param name="path">путь к фотографии сотрудника</param>
    public void showUserInfo(string name1, string name2, string name3, string team,
                                 string job, string phone1, string phone2, string dob,
                                 string computer, string login, string email, string path)
        {
            tB_name1.Text = name1;
            tB_name2.Text = name2;
            tB_name3.Text = name3;
            tB_team.Text = team;
            tB_job.Text = job;
            tB_phone1.Text = phone1;
            tB_phone2.Text = phone2;
            tB_dob.Text = dob;
            tB_computer.Text = computer;
            tB_login.Text = login;
            tB_email.Text = email;

            if (path != null)            // если файл фото существует
            {
                pictureBox1.Load(path);  // загрузить фото

                // подогнать размеры изображения:
                pictureBox1.Image = ScaleImage(pictureBox1.Image, pictureBox1.Size.Width, pictureBox1.Size.Height);

            }
            else                         // если файл фото не существует
            {
                pictureBox1.Image = ScaleImage(pictureBox1.ErrorImage, pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
        }

        private void b_OK_Click(object sender, EventArgs e)
        {
            Close();                     // закрыть форму
        }

        /// <summary>
        /// Метод изменения размера изображения с сохранением пропорций.
        /// </summary>
        /// <param name="source">Источник изображения.</param>
        /// <param name="width">Ширина изображения.</param>
        /// <param name="height">Высота изображения.</param>
        /// <returns></returns>
        static Image ScaleImage(Image source, int width, int height)
        {

            Image dest = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(dest))
            {
                gr.FillRectangle(Brushes.White, 0, 0, width, height);  // Очищаем экран
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                float srcwidth = source.Width;
                float srcheight = source.Height;
                float dstwidth = width;
                float dstheight = height;

                if (srcwidth <= dstwidth && srcheight <= dstheight)    // Исходное изображение меньше целевого
                {
                    int left = (width - source.Width) / 2;
                    int top = (height - source.Height) / 2;
                    gr.DrawImage(source, left, top, source.Width, source.Height);
                }
                else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
                {
                    float cy = srcheight / srcwidth * dstwidth;
                    float top = ((float)dstheight - cy) / 2.0f;
                    if (top < 1.0f) top = 0;
                    gr.DrawImage(source, 0, top, dstwidth, cy);
                }
                else                                                   // Пропорции исходного изображения более узкие
                {
                    float cx = srcwidth / srcheight * dstheight;
                    float left = ((float)dstwidth - cx) / 2.0f;
                    if (left < 1.0f) left = 0;
                    gr.DrawImage(source, left, 0, cx, dstheight);
                }

                return dest;
            }
        }
    }
}
