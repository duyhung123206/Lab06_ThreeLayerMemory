using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BLL;

namespace Lab06_ThreeLayerMemory
{
    public partial class frmDanhSach : Form
    {
        // Giao diện chỉ gọi qua BUS
        private SinhVienBUS _bus = new SinhVienBUS();

        public frmDanhSach()
        {
            InitializeComponent();
        }

        private void frmDanhSach_Load(object sender, EventArgs e)
        {
            // Thêm dữ liệu mẫu ban đầu qua BUS
            try
            {
                _bus.Add(new SinhVien("SV01", "Nguyễn An", new DateTime(2004, 5, 12), 8.2, "Công nghệ Thông tin"));
                _bus.Add(new SinhVien("SV02", "Trần Bình", new DateTime(2004, 8, 20), 7.0, "Hệ thống Thông tin"));
            }
            catch { }

            // --- 1. Cấu hình DataGridView ---
            dgvSinhVien.AutoGenerateColumns = false;
            if (dgvSinhVien.Columns.Count == 0)
            {
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaSV", HeaderText = "Mã Sinh Viên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HoTen", HeaderText = "Họ và Tên" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NgaySinh", HeaderText = "Ngày Sinh" });
                dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "DiemTB", HeaderText = "Điểm TB" });
            }
            RefreshDataGrid();
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
            LoadListView(_bus.GetAll());

            // --- 3. Cấu hình TreeView theo Khoa ---
            LoadTree(_bus.GetAll());
        }

        private void RefreshDataGrid()
        {
            BindingList<SinhVien> listShow = new BindingList<SinhVien>(_bus.GetAll());
            dgvSinhVien.DataSource = listShow;
        }

        private void LoadListView(IEnumerable<SinhVien> data)
        {
            lvSinhVien.Items.Clear();
            foreach (SinhVien sv in data)
            {
                ListViewItem item = new ListViewItem(sv.MaSV);
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(sv.DiemTB.ToString("N2"));
                item.Tag = sv;
                lvSinhVien.Items.Add(item);
            }
        }

        private void LoadTree(List<SinhVien> listSinhVien)
        {
            tvKhoa.Nodes.Clear();
            var dataGroup = listSinhVien.GroupBy(x => x.Khoa)
                                        .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var group in dataGroup)
            {
                TreeNode khoaNode = new TreeNode(group.Key);
                foreach (SinhVien sv in group.Value)
                {
                    TreeNode svNode = new TreeNode(sv.MaSV + " - " + sv.HoTen);
                    svNode.Tag = sv;
                    khoaNode.Nodes.Add(svNode);
                }
                tvKhoa.Nodes.Add(khoaNode);
            }
            tvKhoa.ExpandAll();
        }

        // 1. Chức năng Thêm sinh viên (Thông qua BUS kiểm tra nghiệp vụ)
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SinhVien sv = new SinhVien(
                    txtMaSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    dtpNgaySinh.Value,
                    double.Parse(txtDiemTB.Text),
                    "Công nghệ Thông tin"
                );

                // Gọi BUS xử lý
                _bus.Add(sv);

                // Cập nhật lại giao diện
                RefreshDataGrid();
                LoadListView(_bus.GetAll());
                LoadTree(_bus.GetAll());

                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. Chức năng Xóa sinh viên
        private void btnXoa_Click(object sender, EventArgs e)
        {
            SinhVien sv = null;

            if (dgvSinhVien.CurrentRow != null)
                sv = dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;
            else if (lvSinhVien.SelectedItems.Count > 0)
                sv = lvSinhVien.SelectedItems[0].Tag as SinhVien;
            else if (tvKhoa.SelectedNode != null && tvKhoa.SelectedNode.Tag is SinhVien)
                sv = tvKhoa.SelectedNode.Tag as SinhVien;

            if (sv != null)
            {
                DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {sv.HoTen} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    // Xóa trực tiếp trong danh sách của DAL/BUS
                    _bus.GetAll().Remove(sv);

                    RefreshDataGrid();
                    LoadListView(_bus.GetAll());
                    LoadTree(_bus.GetAll());
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 3. Chức năng Sửa sinh viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            SinhVien sv = null;

            if (dgvSinhVien.CurrentRow != null)
                sv = dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;
            else if (lvSinhVien.SelectedItems.Count > 0)
                sv = lvSinhVien.SelectedItems[0].Tag as SinhVien;
            else if (tvKhoa.SelectedNode != null && tvKhoa.SelectedNode.Tag is SinhVien)
                sv = tvKhoa.SelectedNode.Tag as SinhVien;

            if (sv != null)
            {
                try
                {
                    sv.MaSV = txtMaSV.Text.Trim();
                    sv.HoTen = txtHoTen.Text.Trim();
                    sv.NgaySinh = dtpNgaySinh.Value;
                    sv.DiemTB = double.Parse(txtDiemTB.Text);

                    RefreshDataGrid();
                    LoadListView(_bus.GetAll());
                    LoadTree(_bus.GetAll());

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi định dạng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

        private void tvKhoa_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is SinhVien sv)
            {
                txtMaSV.Text = sv.MaSV;
                txtHoTen.Text = sv.HoTen;
                dtpNgaySinh.Value = sv.NgaySinh;
                txtDiemTB.Text = sv.DiemTB.ToString();
            }
        }
    }
}