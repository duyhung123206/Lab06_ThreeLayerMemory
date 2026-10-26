using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Lab06_ThreeLayerMemory
{
    public partial class frmDanhSach : Form
    {
        // Khai báo BindingList quản lý danh sách sinh viên trong bộ nhớ
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

            // --- 1. Cấu hình DataGridView ---
            dgvSinhVien.AutoGenerateColumns = false;
            if (dgvSinhVien.Columns.Count == 0)
            {
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSV", HeaderText = "Mã Sinh Viên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ và Tên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiemTB", HeaderText = "Điểm TB" });
            }
            dgvSinhVien.DataSource = _ds;
            dgvSinhVien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // --- 2. Cấu hình ListView dạng Details ---
            lvSinhVien.View = View.Details;
            lvSinhVien.FullRowSelect = true;
            lvSinhVien.GridLines = true;

            if (lvSinhVien.Columns.Count == 0)
            {
                lvSinhVien.Columns.Add("Mã Sinh Viên", 100, HorizontalAlignment.Left);
                lvSinhVien.Columns.Add("Họ và Tên", 150, HorizontalAlignment.Left);
                lvSinhVien.Columns.Add("Điểm TB", 80, HorizontalAlignment.Left);
            }

            // Nạp dữ liệu lên ListView ban đầu
            LoadListView(_ds);
        }

        // Hàm nạp dữ liệu lên ListView theo code mẫu đề bài yêu cầu
        private void LoadListView(IEnumerable<SinhVien> data)
        {
            lvSinhVien.Items.Clear();
            lvSinhVien.View = View.Details;
            foreach (SinhVien sv in data)
            {
                ListViewItem item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(sv.DiemTB.ToString("N2"));
                item.Tag = sv; // Lưu đối tượng vào Tag để dễ truy xuất
                lvSinhVien.Items.Add(item);
            }
        }

        // 1. Chức năng Thêm sinh viên (Có kiểm tra trùng mã)
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

                // Cập nhật lại ListView sau khi thêm
                LoadListView(_ds);

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
            SinhVien sv = null;

            // Ưu tiên xóa theo dòng đang chọn trên DataGridView hoặc ListView
            if (dgvSinhVien.CurrentRow != null)
            {
                sv = dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;
            }
            else if (lvSinhVien.SelectedItems.Count > 0)
            {
                sv = lvSinhVien.SelectedItems[0].Tag as SinhVien;
            }

            if (sv != null)
            {
                DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {sv.HoTen} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _ds.Remove(sv);
                    LoadListView(_ds); // Cập nhật lại ListView
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa trên bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 3. Chức năng Sửa sinh viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = null;

            if (dgvSinhVien.CurrentRow != null)
            {
                sv = dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;
            }
            else if (lvSinhVien.SelectedItems.Count > 0)
            {
                sv = lvSinhVien.SelectedItems[0].Tag as SinhVien;
            }

            if (sv != null)
            {
                try
                {
                    sv.MaSV = txtMaSV.Text.Trim();
                    sv.HoTen = txtHoTen.Text.Trim();
                    sv.NgaySinh = dtpNgaySinh.Value;
                    sv.DiemTB = double.Parse(txtDiemTB.Text);

                    _ds.ResetBindings();
                    LoadListView(_ds); // Cập nhật lại ListView

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

        // 4. Sự kiện click vào DataGridView để đổ dữ liệu lên ô nhập
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

        // 5. Sự kiện chọn dòng trên ListView để đổ dữ liệu lên ô nhập
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSinhVien.SelectedItems[0];
                SinhVien sv = item.Tag as SinhVien;
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