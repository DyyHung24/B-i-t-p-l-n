using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormLop : Form
    {
        // Chuỗi kết nối CSDL
        private const string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";

        public FormLop()
        {
            InitializeComponent();
        }

        private void FormLop_Load(object sender, EventArgs e)
        {
            LoadComboBoxKhoa(); // Tải danh sách Mã khoa vào ComboBox
            LoadDataLop();      // Tải danh sách các lớp lên bảng
        }

        // ==========================================
        // 1. TẢI DỮ LIỆU LÊN DATAGRIDVIEW VÀ COMBOBOX
        // ==========================================

        private void LoadDataLop()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Truy vấn các thuộc tính: MaKhoa, MaLop, TenLop từ bảng Lớp
                string query = "SELECT MaKhoa, MaLop, TenLop FROM tblLOP";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvLop.DataSource = dt;
            }
        }

        private void LoadComboBoxKhoa()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaKhoa, TenKhoa FROM tblKHOA";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboMaKhoa.DataSource = dt;
                cboMaKhoa.DisplayMember = "MaKhoa"; // Hiển thị Mã Khoa
                cboMaKhoa.ValueMember = "MaKhoa";
                cboMaKhoa.SelectedIndex = -1;
            }
        }

        // ==========================================
        // 2. CHỨC NĂNG NHẬP (THÊM) - SỬA - XÓA (CRUD)
        // ==========================================

        // Nút NHẬP (Thêm Lớp mới)
        private void btnNhap_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO tblLOP (MaLop, TenLop, MaKhoa) VALUES (@MaLop, @TenLop, @MaKhoa)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaKhoa", cboMaKhoa.SelectedValue.ToString());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataLop();
                            ClearInput();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm dữ liệu (Có thể trùng Mã lớp): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút SỬA Thông tin Lớp
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE tblLOP SET TenLop = @TenLop, MaKhoa = @MaKhoa WHERE MaLop = @MaLop";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaKhoa", cboMaKhoa.SelectedValue.ToString());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật thông tin lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataLop();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút XÓA Lớp
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa lớp này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM tblLOP WHERE MaLop = @MaLop";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());

                        try
                        {
                            conn.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Xóa lớp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataLop();
                                ClearInput();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa (Lớp này đang chứa Sinh viên): " + ex.Message, "Lỗi khóa ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // ==========================================
        // 3. SỰ KIỆN VÀ TIỆN ÍCH
        // ==========================================

        // Đổ thông tin hàng được chọn lên ô nhập liệu
        private void dgvLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLop.Rows[e.RowIndex];

                txtMaLop.Text = row.Cells["MaLop"].Value?.ToString();
                txtTenLop.Text = row.Cells["TenLop"].Value?.ToString();

                if (row.Cells["MaKhoa"].Value != null)
                {
                    cboMaKhoa.SelectedValue = row.Cells["MaKhoa"].Value.ToString();
                }

                txtMaLop.ReadOnly = true; // Khóa Mã lớp khi xem/sửa
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Mã lớp không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLop.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                MessageBox.Show("Tên lớp không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLop.Focus();
                return false;
            }
            if (cboMaKhoa.SelectedValue == null || cboMaKhoa.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Mã khoa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaKhoa.Focus();
                return false;
            }
            return true;
        }

        private void ClearInput()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            cboMaKhoa.SelectedIndex = -1;
            txtMaLop.ReadOnly = false; // Mở lại quyền nhập Mã lớp
            txtMaLop.Focus();
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
    }
}