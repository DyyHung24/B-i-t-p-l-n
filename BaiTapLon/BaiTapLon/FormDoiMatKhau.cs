using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BaiTapLon.Properties;

namespace BaiTapLon
{
    public partial class FormDoiMatKhau : Form
    {
        private string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=Ql_DSV;Integrated Security=True";

        public FormDoiMatKhau()
        {
            InitializeComponent();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string matKhauCu = txtMatKhauCu.Text.Trim();
            string matKhauMoi = txtMatKhauMoi.Text.Trim();
            string xacNhanMK = txtXacNhanMK.Text.Trim();

            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi) || string.IsNullOrEmpty(xacNhanMK))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi != xacNhanMK)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không trùng khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtXacNhanMK.Focus();
                return;
            }

            if (matKhauMoi == matKhauCu)
            {
                MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu cũ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE [user] = @User AND [pass] = @Pass";
                        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                        checkCmd.Parameters.AddWithValue("@User", Session.TenDangNhap);
                        checkCmd.Parameters.AddWithValue("@Pass", matKhauCu);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            MessageBox.Show("Mật khẩu hiện tại không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtMatKhauCu.Focus();
                            return;
                        }

                    string updateQuery = "UPDATE TaiKhoan SET [pass] = @MatKhauMoi WHERE [user] = @User";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                        updateCmd.Parameters.AddWithValue("@User", Session.TenDangNhap); // Sửa tên tham số trùng với @User ở câu lệnh SQL trên

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            this.Close(); // Đóng form sau khi đổi thành công
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật mật khẩu thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearFields()
        {
            txtMatKhauCu.Clear();
            txtMatKhauMoi.Clear();
            txtXacNhanMK.Clear();
        }
    }
}