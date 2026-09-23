using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormDangKy : Form
    {
        private string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";

        private bool isPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        public FormDangKy()
        {
            InitializeComponent();
        }

        private void FormDangKy_Load(object sender, EventArgs e)
        {
            // 1. Ô Tên đăng nhập
            txtUsername.Text = "Tên đăng nhập";
            txtUsername.ForeColor = Color.Gray;

            // 2. Ô Mật khẩu
            txtPassword.Text = "Mật khẩu";
            txtPassword.ForeColor = Color.Gray;
            txtPassword.PasswordChar = '\0';

            // 3. Ô Xác nhận mật khẩu
            txtConfirmPassword.Text = "Xác nhận mật khẩu";
            txtConfirmPassword.ForeColor = Color.Gray;
            txtConfirmPassword.PasswordChar = '\0';
        }

        // --- Placeholder Tên đăng nhập ---
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Mật khẩu")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                // Bật ẩn mật khẩu nếu người dùng chưa chọn "Hiện"
                txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.Text = "Mật khẩu";
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false; // Hiện chữ "Mật khẩu"
            }
        }

        // --- Placeholder Xác nhận mật khẩu ---
        private void txtConfirmPassword_Enter(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text == "Xác nhận mật khẩu")
            {
                txtConfirmPassword.Text = "";
                txtConfirmPassword.ForeColor = Color.Black;
                txtConfirmPassword.UseSystemPasswordChar = !isConfirmPasswordVisible;
            }
        }

        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                txtConfirmPassword.Text = "Xác nhận mật khẩu";
                txtConfirmPassword.ForeColor = Color.Gray;
                txtConfirmPassword.UseSystemPasswordChar = false;
            }
        }

        // --- Nút Ẩn / Hiện Mật Khẩu ---
        private void btnTogglePass_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;

            if (sender is Button btn)
            {
                btn.Text = isPasswordVisible ? "Ẩn" : "Hiện";
            }

            // Chỉ đổi trạng thái ẩn/hiện nếu ô không chứa chuỗi gợi ý
            if (txtPassword.Text != "Mật khẩu")
            {
                txtPassword.UseSystemPasswordChar = !isPasswordVisible;
            }
        }

        // --- Nút Ẩn / Hiện Xác Nhận Mật Khẩu ---
        private void btnToggleConfirmPass_Click(object sender, EventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;

            if (sender is Button btn)
            {
                btn.Text = isConfirmPasswordVisible ? "Ẩn" : "Hiện";
            }

            if (txtConfirmPassword.Text != "Xác nhận mật khẩu")
            {
                txtConfirmPassword.UseSystemPasswordChar = !isConfirmPasswordVisible;
            }
        }

        private void btnDangKi_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text;
            string nhapLaiMatKhau = txtConfirmPassword.Text; // Đổi tên txtConfirmPassword thành tên TextBox của bạn

            // 1. Kiểm tra rỗng hoặc chữ gợi ý (Placeholder)
            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau) || string.IsNullOrEmpty(nhapLaiMatKhau) ||
                taiKhoan == "Tên đăng nhập" || matKhau == "Mật khẩu" || nhapLaiMatKhau == "Nhập lại mật khẩu")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng ký!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Kiểm tra mật khẩu nhập lại có trùng khớp không
            if (matKhau != nhapLaiMatKhau)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus(); // Con trỏ chuột tự nhảy về ô nhập lại mật khẩu
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM TaiKhoan WHERE [User] = @user";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@user", taiKhoan);
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại! Vui lòng chọn tên khác.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtUsername.Focus();
                            return;
                        }
                    }

                    string insertSql = "INSERT INTO TaiKhoan ([User], Pass) VALUES (@user, @pass)";
                    using (SqlCommand insertCmd = new SqlCommand(insertSql, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@user", taiKhoan);
                        insertCmd.Parameters.AddWithValue("@pass", matKhau);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtConfirmPassword.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Đăng ký thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}