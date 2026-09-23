using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class FormQLD : Form
    {
        // Thay đổi chuỗi kết nối phù hợp với máy của bạn
        string strConn = @"Data Source=L\SQLEXPRESS;Initial Catalog=QL_DSV;Integrated Security=True";
        SqlConnection conn = null;

        public FormQLD()
        {
            InitializeComponent();
        }

        private void FormQLD_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(strConn);
            LoadComboboxMaMon();
            conn = new SqlConnection(strConn);
            dgvDanhSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhSach.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        // 1. TẢI DANH SÁCH MÃ MÔN HỌC VÀO COMBOBOX
        private void LoadComboboxMaMon()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = "SELECT MaMon FROM tblMON";
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                cboMaMon.Items.Clear();
                while (dr.Read())
                {
                    cboMaMon.Items.Add(dr["MaMon"].ToString());
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách môn học: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // 2. HÀM TẢI TẤT CẢ SINH VIÊN CÙNG ĐIỂM CỦA MÔN HỌC ĐƯỢC CHỌN
        private void LoadDataByMon(string maMon)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Lấy TẤT CẢ sinh viên từ tblSINH_VIEN, LEFT JOIN với tblKET_QUA theo MaMon
                string sql = @"SELECT 
                                  sv.MaSV, 
                                  sv.HoTen, 
                                  ISNULL(kq.MaMon, @MaMon) AS MaMon, 
                                  kq.DiemTB, 
                                  kq.DiemThiLan1, 
                                  kq.DiemThiLan2, 
                                  kq.HanhKiem, 
                                  kq.DiemTongKet
                               FROM tblSINH_VIEN sv
                               LEFT JOIN tblKET_QUA kq ON sv.MaSV = kq.MaSV AND kq.MaMon = @MaMon";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaMon", maMon);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvDanhSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // 3. SỰ KIỆN NÚT OK (Áp dụng Mã Môn)
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboMaMon.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Mã môn học trước khi nhấn OK!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maMonSelected = cboMaMon.SelectedItem.ToString();
            LoadDataByMon(maMonSelected);
        }

        // 4. CHỨC NĂNG SỬA / CẬP NHẬT ĐIỂM (Nếu chưa có điểm -> Tự động THÊM, nếu có rồi -> SỬA)
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) || cboMaMon.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên và môn học cần nhập/sửa điểm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                string sql = @"
                    IF EXISTS (SELECT 1 FROM tblKET_QUA WHERE MaSV = @MaSV AND MaMon = @MaMon)
                    BEGIN
                        UPDATE tblKET_QUA 
                        SET DiemTB = @DiemTB, 
                            DiemThiLan1 = @DiemThiLan1, 
                            DiemThiLan2 = @DiemThiLan2, 
                            HanhKiem = @HanhKiem, 
                            DiemTongKet = @DiemTongKet 
                        WHERE MaSV = @MaSV AND MaMon = @MaMon
                    END
                    ELSE
                    BEGIN
                        INSERT INTO tblKET_QUA (MaSV, MaMon, DiemTB, DiemThiLan1, DiemThiLan2, HanhKiem, DiemTongKet)
                        VALUES (@MaSV, @MaMon, @DiemTB, @DiemThiLan1, @DiemThiLan2, @HanhKiem, @DiemTongKet)
                    END";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                cmd.Parameters.AddWithValue("@MaMon", cboMaMon.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@DiemTB", string.IsNullOrEmpty(txtDiemTB.Text) ? (object)DBNull.Value : Convert.ToDouble(txtDiemTB.Text));
                cmd.Parameters.AddWithValue("@DiemThiLan1", string.IsNullOrEmpty(txtDiemLan1.Text) ? (object)DBNull.Value : Convert.ToDouble(txtDiemLan1.Text));
                cmd.Parameters.AddWithValue("@DiemThiLan2", string.IsNullOrEmpty(txtDiemLan2.Text) ? (object)DBNull.Value : Convert.ToDouble(txtDiemLan2.Text));
                cmd.Parameters.AddWithValue("@DiemTongKet", string.IsNullOrEmpty(txtTongKet.Text) ? (object)DBNull.Value : Convert.ToDouble(txtTongKet.Text));
                cmd.Parameters.AddWithValue("@HanhKiem", cboHanhKiem.Text.Trim());
                

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Lưu / Cập nhật điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataByMon(cboMaMon.SelectedItem.ToString()); // Tải lại bảng theo môn hiện tại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        // 5. CHỨC NĂNG XOÁ ĐIỂM MÔN HỌC
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text) || cboMaMon.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sinh viên và môn học cần xóa điểm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa điểm môn {cboMaMon.SelectedItem} của sinh viên {txtMaSV.Text}?",
                                              "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string sql = "DELETE FROM tblKET_QUA WHERE MaSV = @MaSV AND MaMon = @MaMon";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaMon", cboMaMon.SelectedItem.ToString());

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa điểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataByMon(cboMaMon.SelectedItem.ToString());
                        btnLamMoi_Click(sender, e);
                    }
                    else
                    {
                        MessageBox.Show("Sinh viên này chưa có bản ghi điểm môn học để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa điểm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        // 6. CHỨC NĂNG LÀM MỚI FORM
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtMaSV.Clear();
            txtDiemTB.Clear();
            txtDiemLan1.Clear();
            txtDiemLan2.Clear();
            txtTongKet.Clear();
            cboHanhKiem.SelectedIndex = -1;

            if (cboMaMon.SelectedItem != null)
            {
                LoadDataByMon(cboMaMon.SelectedItem.ToString());
            }
        }

        // 7. HIỂN THỊ THÔNG TIN LÊN CÁC Ô KHI CLICK VÀO BẢNG DATAGRIDVIEW
        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];

                txtMaSV.Text = row.Cells["MaSV"].Value?.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value?.ToString();

                if (row.Cells["MaMon"].Value != DBNull.Value && row.Cells["MaMon"].Value != null)
                {
                    cboMaMon.SelectedItem = row.Cells["MaMon"].Value.ToString();
                }

                txtDiemTB.Text = row.Cells["DiemTB"].Value?.ToString();
                txtDiemLan1.Text = row.Cells["DiemThiLan1"].Value?.ToString();
                txtDiemLan2.Text = row.Cells["DiemThiLan2"].Value?.ToString();
                txtTongKet.Text = row.Cells["DiemTongKet"].Value?.ToString();
                cboHanhKiem.Text = row.Cells["HanhKiem"].Value?.ToString();
                
            }
        }

        // 8. NÚT THOÁT
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