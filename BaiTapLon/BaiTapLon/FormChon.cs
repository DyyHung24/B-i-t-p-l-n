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
    public partial class FormChon : Form
    {
        public FormChon()
        {
            InitializeComponent();
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            this.Hide(); // 1. Ẩn FormChon

            using (FormDangNhap frmLogin = new FormDangNhap())
            {
                // 2. Mở FormDangNhap dưới dạng Dialog
                if (frmLogin.ShowDialog() == DialogResult.OK)
                {
                    // Nếu Đăng nhập thành công -> Mở thẳng MainForm
                    MainForm main = new MainForm();
                    main.ShowDialog();

                    this.Close(); // Đóng hẳn FormChon khi người dùng thoát MainForm
                }
                else
                {
                    this.Show(); // Hiện lại FormChon nếu người dùng hủy/thoát Đăng nhập
                }
            }
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            this.Hide(); // 1. Ẩn FormChon

            using (FormDangKy frmRegister = new FormDangKy())
            {
                // 2. Mở FormDangKy dưới dạng Dialog
                if (frmRegister.ShowDialog() == DialogResult.OK)
                {
                    // Nếu Đăng ký thành công -> Mở thẳng MainForm
                    MainForm main = new MainForm();
                    main.ShowDialog();

                    this.Close(); // Đóng hẳn FormChon khi thoát MainForm
                }
                else
                {
                    this.Show(); // Hiện lại FormChon nếu thoát Đăng ký
                }
            }
        }
    }
}
