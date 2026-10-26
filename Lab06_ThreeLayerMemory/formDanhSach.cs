using System;
using System.ComponentModel;
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
            // Thêm dữ liệu mẫu vào danh sách
            _ds.Add(new SinhVien("SV01", "Nguyễn An", new DateTime(2004, 5, 12), 8.2));
            _ds.Add(new SinhVien("SV02", "Trần Bình", new DateTime(2004, 8, 20), 7.0));

            // Tắt tự động sinh cột để kiểm soát hiển thị tiêu đề tiếng Việt theo ý muốn[cite: 6]
            dgvSinhVien.AutoGenerateColumns = false;

            // Thêm các cột thủ công nếu trên Designer chưa có
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
    }
}