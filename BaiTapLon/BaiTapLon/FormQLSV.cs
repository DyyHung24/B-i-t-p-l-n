using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormQLSV : Form
    {
        // Chuỗi kết nối CSDL
        private const string connectionString = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True;TrustServerCertificate=True;";
        private bool isLoading = true;

        public FormQLSV()
        {
            InitializeComponent();
        }

        private void FormQLSV_Load(object sender, EventArgs e)
        {
            isLoading = true;

            LoadComboBoxKhoa();
            LoadComboBoxLop();
            LoadComboBoxGioiTinh();

            isLoading = false;

            LoadDataSinhVien(); 
        }

        // ==========================================
        // 1. TẢI DỮ LIỆU LÊN BẢNG VÀ COMBOBOX
        // ==========================================

        private void LoadDataSinhVien()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT sv.MaSV, sv.HoTen, 
                                        sv.NgaySinh, sv.GioiTinh, 
                                        sv.DiaChi, sv.Malop, 
                                        k.MaKhoa, l.Tenlop
                        FROM tblSINH_VIEN sv 
                        INNER JOIN tblLOP l ON sv.Malop = l.Malop
                        INNER JOIN tblKHOA k ON l.MaKhoa = k.MaKhoa";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSinhVien.DataSource = dt;
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
                cboMaKhoa.DisplayMember = "MaKhoa"; // Hoặc "MaKhoa" tùy nhu cầu hiển thị
                cboMaKhoa.ValueMember = "MaKhoa";
                cboMaKhoa.SelectedIndex = -1;
                cboMaKhoa.Text = "";
            }
        }

        private void LoadComboBoxLop(string maKhoa = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Malop, Tenlop FROM tblLOP";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrEmpty(maKhoa))
                {
                    query += " WHERE MaKhoa = @MaKhoa";
                    cmd.Parameters.AddWithValue("@MaKhoa", maKhoa);
                }

                cmd.CommandText = query;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Hiển thị Mã lớp cho cboMaLop
                cboMaLop.DataSource = dt.Copy();
                cboMaLop.DisplayMember = "Malop"; // Sửa từ "Tenlop" thành "Malop"
                cboMaLop.ValueMember = "Malop";
                cboMaLop.SelectedIndex = -1;

                // Hiển thị Tên lớp cho cboTenLop (nếu cboTenLop là ComboBox)
                if (cboTenLop != null)
                {
                    cboTenLop.DataSource = dt.Copy();
                    cboTenLop.DisplayMember = "Tenlop";
                    cboTenLop.ValueMember = "Malop";
                    cboTenLop.SelectedIndex = -1;
                }
            }
        }

        private void LoadComboBoxGioiTinh()
        {
            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");
            cboGioiTinh.SelectedIndex = 0;
        }

        // ==========================================
        // 2. CHỨC NĂNG THÊM - SỬA - XÓA (CRUD)
        // ==========================================

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO tblSINH_VIEN (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, Malop) 
                                VALUES (@MaSV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @Malop)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Truyền tham số để tránh SQL Injection
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Malop", cboMaLop.SelectedValue.ToString());

                    try
                    {
                        conn.Open(); // Mở kết nối
                        int result = cmd.ExecuteNonQuery(); // Thực thi câu lệnh
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm mới sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataSinhVien(); // Tải lại bảng
                            ClearInput();       // Xóa trắng form
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm dữ liệu (Có thể trùng Mã SV): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút SỬA Sinh Viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE tblSINH_VIEN 
                                SET HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi, Malop = @Malop 
                                WHERE MaSV = @MaSV";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value.Date);
                    cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@Malop", cboMaLop.SelectedValue.ToString());

                    try
                    {
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataSinhVien();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Nút XÓA Sinh Viên
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa thông tin sinh viên này không?",
                                                   "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM tblSINH_VIEN WHERE MaSV = @MaSV";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());

                        try
                        {
                            conn.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result > 0)
                            {
                                MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadDataSinhVien();
                                ClearInput();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể xóa (Sinh viên đã có điểm trong hệ thống): " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        // ==========================================
        // 3. TÌM KIẾM VÀ LỌC DỮ LIỆU
        // ==========================================

        private void cboTimKiemLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoading) FilterData();
        }

        // Thực hiện truy vấn lọc theo Khoa và Lớp
        private void FilterData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Thêm INNER JOIN với tblKHOA và lấy k.TenKhoa, l.Tenlop
                string query = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, sv.DiaChi, sv.Malop, k.MaKhoa, l.Tenlop 
                FROM tblSINH_VIEN sv 
                INNER JOIN tblLOP l ON sv.Malop = l.Malop
                INNER JOIN tblKHOA k ON l.MaKhoa = k.MaKhoa 
                WHERE 1=1 ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSinhVien.DataSource = dt;
            }
        }

        // ==========================================
        // 4. TIỆN ÍCH VÀ SỰ KIỆN KHÁC
        // ==========================================

        // Bấm vào 1 hàng trong bảng -> Đổ thông tin lên các ô nhập liệu
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtHoTen.Text = row.Cells[1].Value?.ToString();
                if (row.Cells[2].Value != null && DateTime.TryParse(row.Cells[2].Value.ToString(), out DateTime ngaySinh))
                {
                    dtpNgaySinh.Value = ngaySinh;
                }
                cboGioiTinh.Text = row.Cells[3].Value?.ToString();
                txtDiaChi.Text = row.Cells[4].Value?.ToString();
                if (cboMaLop.DataSource != null)
                    cboMaLop.SelectedValue = row.Cells[5].Value?.ToString();
                else
                    cboMaLop.Text = row.Cells[5].Value?.ToString();

                cboMaKhoa.Text = row.Cells[6].Value?.ToString();
                cboTenLop.Text = row.Cells[7].Value?.ToString();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Mã sinh viên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSV.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên sinh viên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }
            if (cboMaLop.SelectedValue == null || cboMaLop.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Lớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMaLop.Focus();
                return false;
            }
            return true;
        }

        private void ClearInput()
        {
            txtMaSV.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtDiaChi.Clear();

            cboGioiTinh.SelectedIndex = 0;

            cboMaLop.SelectedIndex = -1;
            cboMaLop.Text = "";

            cboMaKhoa.SelectedIndex = -1;
            cboMaKhoa.Text = "";

            cboTenLop.SelectedIndex = -1;
            cboTenLop.Text = "";

            txtMaSV.Focus();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            isLoading = true;
            ClearInput();
            LoadComboBoxLop();
            isLoading = false;
            cboMaKhoa.SelectedIndex = -1;
            cboTenLop.SelectedIndex = -1;
            LoadDataSinhVien();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Bạn có muốn đóng giao diện này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Câu truy vấn gốc với WHERE 1=1 để dễ dàng nối thêm các điều kiện lọc
                string query = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, sv.DiaChi, sv.Malop, k.MaKhoa, l.Tenlop
                FROM tblSINH_VIEN sv 
                INNER JOIN tblLOP l ON sv.Malop = l.Malop
                INNER JOIN tblKHOA k ON l.MaKhoa = k.MaKhoa
                WHERE 1=1 ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                // 1. Kiểm tra ô Mã sinh viên
                if (!string.IsNullOrWhiteSpace(txtMaSV.Text))
                {
                    query += " AND sv.MaSV LIKE @MaSV";
                    cmd.Parameters.AddWithValue("@MaSV", "%" + txtMaSV.Text.Trim() + "%");
                }

                // 2. Kiểm tra ô Họ tên
                if (!string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    query += " AND sv.HoTen LIKE @HoTen";
                    cmd.Parameters.AddWithValue("@HoTen", "%" + txtHoTen.Text.Trim() + "%");
                }

                // 3. Kiểm tra ô Giới tính (ĐÃ SỬA: Xóa đoạn if thừa phía trên)
                if (!cbbTatCa.Checked && !string.IsNullOrWhiteSpace(cboGioiTinh.Text))
                {
                    query += " AND sv.GioiTinh = @GioiTinh";
                    cmd.Parameters.AddWithValue("@GioiTinh", cboGioiTinh.Text.Trim());
                }

                // 4. Kiểm tra ô Địa chỉ
                if (!string.IsNullOrWhiteSpace(txtDiaChi.Text))
                {
                    query += " AND sv.DiaChi LIKE @DiaChi";
                    cmd.Parameters.AddWithValue("@DiaChi", "%" + txtDiaChi.Text.Trim() + "%");
                }

                // 5. Kiểm tra ô Mã lớp
                if (!string.IsNullOrWhiteSpace(cboMaLop.Text))
                {
                    query += " AND sv.Malop LIKE @MaLop";
                    cmd.Parameters.AddWithValue("@MaLop", "%" + cboMaLop.Text.Trim() + "%");
                }

                // 6. Kiểm tra ô Mã Khoa / Tên Khoa (cboMaKhoa)
                if (!string.IsNullOrWhiteSpace(cboMaKhoa.Text))
                {
                    query += " AND (k.MaKhoa LIKE @Khoa OR k.TenKhoa LIKE @Khoa)";
                    cmd.Parameters.AddWithValue("@Khoa", "%" + cboMaKhoa.Text.Trim() + "%");
                }

                // 7. Kiểm tra ô Lớp học / Tên lớp (cboTenLop)
                if (!string.IsNullOrWhiteSpace(cboTenLop.Text))
                {
                    query += " AND l.Tenlop LIKE @TenLop";
                    cmd.Parameters.AddWithValue("@TenLop", "%" + cboTenLop.Text.Trim() + "%");
                }

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvSinhVien.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào phù hợp với các thông tin đã nhập!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}