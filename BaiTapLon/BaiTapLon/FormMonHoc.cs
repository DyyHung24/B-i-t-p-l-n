using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormMonHoc : Form
    {
        // Connection string
        private const string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";

        public FormMonHoc()
        {
            InitializeComponent();
        }

        private void FormMonHoc_Load(object sender, EventArgs e)
        {
            LoadComboBoxKhoa(); // Load MaKhoa list
            LoadDataMonHoc();   // Load subject list to DataGridView
        }

        // ==========================================
        // 1. DATA LOAD FUNCTIONS
        // ==========================================

        private void LoadDataMonHoc()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Querying tblMON attributes based on schema
                string query = "SELECT MaMon, TenMon, SoDVHT, MaGV, HocKi, MaKhoa FROM tblMON";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvMonHoc.DataSource = dt;
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
                cboMaKhoa.DisplayMember = "MaKhoa";
                cboMaKhoa.ValueMember = "MaKhoa";
                cboMaKhoa.SelectedIndex = -1;
            }
        }

        // ==========================================
        // 2. CRUD FUNCTIONS (NHAP - SUA - XOA)
        // ==========================================

        // Button NHAP (Add Subject)
        private void btnNhap_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO tblMON (MaMon, TenMon, SoDVHT, MaGV, HocKi, MaKhoa) 
                                VALUES (@MaMon, @TenMon, @SoDVHT, @MaGV, @HocKi, @MaKhoa)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenMon", txtTenMon.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDVHT", Convert.ToInt32(txtSoTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@MaGV", txtGiangVien.Text.Trim());
                    cmd.Parameters.AddWithValue("@HocKi", txtHocKy.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaKhoa", cboMaKhoa.SelectedValue.ToString());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm mới môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataMonHoc();
                            ClearInput();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Duplicate MaMon error handle
                        MessageBox.Show("Lỗi: Mã môn học đã tồn tại hoặc dữ liệu không hợp lệ!\n" + ex.Message,
                                        "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Button SUA (Update Subject)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaMon.Text))
            {
                MessageBox.Show("Vui lòng chọn môn học cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE tblMON 
                                SET TenMon = @TenMon, SoDVHT = @SoDVHT, MaGV = @MaGV, HocKi = @HocKi, MaKhoa = @MaKhoa 
                                WHERE MaMon = @MaMon";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenMon", txtTenMon.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDVHT", Convert.ToInt32(txtSoTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@MaGV", txtGiangVien.Text.Trim());
                    cmd.Parameters.AddWithValue("@HocKi", txtHocKy.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaKhoa", cboMaKhoa.SelectedValue.ToString());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Sửa thông tin môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataMonHoc();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Button XOA (Delete Subject)
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaMon.Text))
            {
                MessageBox.Show("Vui lòng chọn môn học cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa môn học này không?",
                                                   "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM tblMON WHERE MaMon = @MaMon";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text.Trim());

                        try
                        {
                            conn.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Xóa môn học thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataMonHoc();
                                ClearInput();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa môn học này (Đang có điểm liên kết): " + ex.Message,
                                            "Lỗi khóa ngoại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // ==========================================
        // 3. EVENTS AND UTILITIES
        // ==========================================

        private void dgvMonHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMonHoc.Rows[e.RowIndex];

                txtMaMon.Text = row.Cells["MaMon"].Value?.ToString();
                txtTenMon.Text = row.Cells["TenMon"].Value?.ToString();
                txtSoTin.Text = row.Cells["SoDVHT"].Value?.ToString();
                txtGiangVien.Text = row.Cells["MaGV"].Value?.ToString();
                txtHocKy.Text = row.Cells["HocKi"].Value?.ToString();

                if (row.Cells["MaKhoa"].Value != null)
                {
                    cboMaKhoa.SelectedValue = row.Cells["MaKhoa"].Value.ToString();
                }

                txtMaMon.ReadOnly = true; 
            }
        }

        // Check missing input
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaMon.Text) ||
                string.IsNullOrWhiteSpace(txtTenMon.Text) ||
                string.IsNullOrWhiteSpace(txtSoTin.Text) ||
                string.IsNullOrWhiteSpace(txtGiangVien.Text) ||
                string.IsNullOrWhiteSpace(txtHocKy.Text) ||
                cboMaKhoa.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin môn học!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtSoTin.Text.Trim(), out _))
            {
                MessageBox.Show("Số ĐVHT phải là số nguyên hợp lệ!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTin.Focus();
                return false;
            }

            return true;
        }

        private void ClearInput()
        {
            txtMaMon.Clear();
            txtTenMon.Clear();
            txtSoTin.Clear();
            txtGiangVien.Clear();
            txtHocKy.Clear();
            cboMaKhoa.SelectedIndex = -1;
            txtMaMon.ReadOnly = false;
            txtMaMon.Focus();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearInput();
            LoadDataMonHoc();
        }
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