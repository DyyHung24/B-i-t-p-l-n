using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private Form activeForm = null;

        private void OpenChildForm(Form childForm)
        {
            // Đóng form cũ nếu đang mở
            if (activeForm != null)
            {
                activeForm.Close();
            }

            activeForm = childForm;

            // Cấu hình nhúng phẳng vào Panel
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.WindowState = FormWindowState.Normal; 

            panelBody.Controls.Clear();
            panelBody.Controls.Add(childForm);
            panelBody.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
        }

        private void btnQuanLySinhVien_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQLSV());
        }

        private void btnQLDSV_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQLD());
        }

        private void btnQLK_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormKhoa());
        }
        private void btnQLL_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormLop());
        }
        private void btnQLMH_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormMonHoc());
        }

        private void trợGiúpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void hệThốngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tspHeThong_Click(object sender, EventArgs e)
        {
            FormDoiMatKhau frm = new FormDoiMatKhau();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog(); 
        }
    }
}