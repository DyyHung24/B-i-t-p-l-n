using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormKhoa : Form
    {
        // Chuỗi kết nối CSDL
        private const string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";

        public FormKhoa()
        {
            InitializeComponent();
        }

        private void FormKhoa_Load(object sender, EventArgs e)
        {
            LoadDataKhoa(); // Tải danh sách Khoa lên bảng
        }

        // ==========================================
        // 1. TẢI DỮ LIỆU LÊN DATAGRIDVIEW
        // ==========================================
        private void LoadDataKhoa()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaKhoa, TenKhoa FROM tblKHOA";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvKhoa.DataSource = dt;
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG THÊM - SỬA - XÓA (CRUD)
        // ==========================================

        // Nút THÊM Khoa
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblKHOA (MaKhoa, TenKhoa) VALUES (@MaKhoa, @TenKhoa)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", txtMaKhoa.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text.Trim());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm mới khoa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhoa();
                            ClearInput();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm dữ liệu (Có thể do trùng Mã khoa): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút SỬA Khoa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Vui lòng chọn khoa cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text))
            {
                MessageBox.Show("Tên khoa không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhoa.Focus();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE tblKHOA SET TenKhoa = @TenKhoa WHERE MaKhoa = @MaKhoa";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaKhoa", txtMaKhoa.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenKhoa", txtTenKhoa.Text.Trim());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin khoa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataKhoa();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút XÓA Khoa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Vui lòng chọn khoa cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa khoa này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM tblKHOA WHERE MaKhoa = @MaKhoa";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhoa", txtMaKhoa.Text.Trim());

                        try
                        {
                            conn.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Xóa khoa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataKhoa();
                                ClearInput();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa khoa này (Đang có Lớp thuộc Khoa này): " + ex.Message, "Lỗi khóa ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // ==========================================
        // 3. SỰ KIỆN CELLCLICK VÀ TIỆN ÍCH
        // ==========================================

        // Đổ thông tin dòng được chọn lên TextBox
        private void dgvKhoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhoa.Rows[e.RowIndex];

                txtMaKhoa.Text = row.Cells["MaKhoa"].Value?.ToString();
                txtTenKhoa.Text = row.Cells["TenKhoa"].Value?.ToString();

                // Khóa ô Mã khoa khi xem/sửa (để tránh sửa nhầm Khóa chính)
                txtMaKhoa.ReadOnly = true;
            }
        }

        // Nút LÀM MỚI
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadDataKhoa();
        }

        // Nút THOÁT
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Bạn có muốn đóng giao diện này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaKhoa.Text))
            {
                MessageBox.Show("Mã khoa không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKhoa.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenKhoa.Text))
            {
                MessageBox.Show("Tên khoa không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenKhoa.Focus();
                return false;
            }
            return true;
        }

        private void ClearInput()
        {
            txtMaKhoa.Clear();
            txtTenKhoa.Clear();
            txtMaKhoa.ReadOnly = false; 
            txtMaKhoa.Focus();
        }
    }
}