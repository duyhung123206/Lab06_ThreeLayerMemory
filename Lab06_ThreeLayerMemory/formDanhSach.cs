using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Lab06_ThreeLayerMemory
{
    public partial class frmDanhSach : Form
    {
        // Khai báo BindingList quản lý danh sách sinh viên
        private BindingList<SinhVien> _ds = new BindingList<SinhVien>();

        public frmDanhSach()
        {
            InitializeComponent();
        }

        private void frmDanhSach_Load(object sender, EventArgs e)
        {
            // Thêm dữ liệu mẫu ban đầu
            _ds.Add(new SinhVien("SV01", "Nguyễn An", new DateTime(2004, 5, 12), 8.2));
            _ds.Add(new SinhVien("SV02", "Trần Bình", new DateTime(2004, 8, 20), 7.0));

            // Tắt tự động sinh cột để kiểm soát hiển thị tiêu đề tiếng Việt
            dgvSinhVien.AutoGenerateColumns = false;

            // Cấu hình các cột cho DataGridView nếu chưa có
            if (dgvSinhVien.Columns.Count == 0)
            {
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSV", HeaderText = "Mã Sinh Viên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ và Tên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiemTB", HeaderText = "Điểm TB" });
            }

            // Gán DataSource và cấu hình chế độ chọn dòng
            dgvSinhVien.DataSource = _ds;
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // 1. Chức năng Thêm sinh viên (Có kiểm tra trùng mã)
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trùng mã sinh viên trước khi thêm
            if (_ds.Any(x => x.MaSV == txtMaSV.Text.Trim()))
            {
                MessageBox.Show("Mã sinh viên đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _ds.Add(new SinhVien(
                    txtMaSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    dtpNgaySinh.Value,
                    double.Parse(txtDiemTB.Text)
                ));
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Chức năng Xóa sinh viên
        private void btnXoa_Click(object sender, EventArgs e)
        {
            SinhVien sv = dgvSinhVien.CurrentRow == null ? null : dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;

            if (sv != null)
            {
                DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {sv.HoTen} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _ds.Remove(sv); // Xóa khỏi BindingList, giao diện tự động cập nhật
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng sinh viên cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 3. Chức năng Sửa sinh viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = dgvSinhVien.CurrentRow == null ? null : dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;

            if (sv != null)
            {
                try
                {
                    sv.MaSV = txtMaSV.Text.Trim();
                    sv.HoTen = txtHoTen.Text.Trim();
                    sv.NgaySinh = dtpNgaySinh.Value;
                    sv.DiemTB = double.Parse(txtDiemTB.Text);

                    // Làm mới BindingList để DataGridView cập nhật lại dữ liệu sửa
                    _ds.ResetBindings();

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 4. Sự kiện khi click vào dòng trên bảng để đổ dữ liệu ngược lại các ô nhập liệu
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                SinhVien sv = row.DataBoundItem as SinhVien;
                if (sv != null)
                {
                    txtMaSV.Text = sv.MaSV;
                    txtHoTen.Text = sv.HoTen;
                    dtpNgaySinh.Value = sv.NgaySinh;
                    txtDiemTB.Text = sv.DiemTB.ToString();
                }
            }
        }
    }
}