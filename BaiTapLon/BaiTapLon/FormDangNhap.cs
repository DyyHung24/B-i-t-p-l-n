using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormDangNhap : Form
    {
        private string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";

        // Biến lưu trạng thái hiển thị mật khẩu (mặc định false = đang ẩn)
        private bool isPasswordVisible = false;

        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtUsername.Text = "Tên đăng nhập";
            txtUsername.ForeColor = Color.Gray;

            txtPassword.Text = "Mật khẩu";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.UseSystemPasswordChar = false; // Hiện rõ chữ "Mật khẩu"
        }

        #region Placeholder Username & Password

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Tên đăng nhập")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Tên đăng nhập";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Mật khẩu")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                // Nếu chưa chọn xem mật khẩu thì tự động ẩn chữ khi người dùng bắt đầu gõ
                txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Mật khẩu";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false; // Hiện lại chữ gợi ý
            }
        }

        #endregion

        // Nút bấm / PictureBox / CheckBox cạnh ô Mật khẩu để Ẩn/Hiện
        private void btnTogglePass_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible; // Đảo trạng thái

            // Đổi nhãn của nút bấm (hoặc đổi Image nếu dùng PictureBox)
            if (sender is Button btn)
            {
                btn.Text = isPasswordVisible ? "Ẩn" : "Hiện";
            }

            // Chỉ đổi chế độ che chuỗi nếu đang có mật khẩu thực tế
            if (txtPassword.Text != "Mật khẩu")
            {
                txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            string taiKhoan = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text;
            Session.TenDangNhap = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau) ||
                taiKhoan == "Tên đăng nhập" || matKhau == "Mật khẩu")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE [User] = @user AND Pass = @pass";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@user", taiKhoan);
                    cmd.Parameters.AddWithValue("@pass", matKhau);

                    int ketQua = (int)cmd.ExecuteScalar();

                    if (ketQua > 0)
                    {
                        MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK; // Báo thành công về cho FormChon
                        this.Close(); // Đóng FormDangNhap
                    }
                    else
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}