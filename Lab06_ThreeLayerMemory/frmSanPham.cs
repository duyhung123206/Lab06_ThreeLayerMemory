using System;
using System.ComponentModel;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BLL;

namespace Lab06_ThreeLayerMemory
{
    public partial class frmSanPham : Form
    {
        private SanPhamBUS _bus = new SanPhamBUS();

        public frmSanPham()
        {
            InitializeComponent();
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            // Thêm dữ liệu mẫu
            try
            {
                _bus.Add(new SanPham("SP01", "Laptop Dell", 15000000, 5));
                _bus.Add(new SanPham("SP02", "Bàn phím cơ", 1200000, 20));
            }
            catch { }

            HienThi(_bus.GetAll());
        }

        private void HienThi(System.Collections.Generic.IEnumerable<SanPham> data)
        {
            dgvSanPham.DataSource = new BindingList<SanPham>(new System.Collections.Generic.List<SanPham>(data));

            // Cập nhật tổng giá trị hàng tồn kho lên giao diện
            lblTongTien.Text = "Tổng giá trị hàng tồn: " + _bus.TinhTongGiaTriTonKho().ToString("N0") + " VNĐ";
        }

        // Thêm sản phẩm
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SanPham sp = new SanPham(
                    txtMaSP.Text.Trim(),
                    txtTenSP.Text.Trim(),
                    double.Parse(txtDonGia.Text),
                    int.Parse(txtSoLuong.Text)
                );

                _bus.Add(sp);
                HienThi(_bus.GetAll());
                MessageBox.Show("Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Xóa sản phẩm
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow != null)
            {
                SanPham sp = dgvSanPham.CurrentRow.DataBoundItem as SanPham;
                if (sp != null)
                {
                    _bus.Remove(sp);
                    HienThi(_bus.GetAll());
                    MessageBox.Show("Xóa thành công!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!");
            }
        }

        // Tìm kiếm tự động khi gõ vào ô tìm kiếm hoặc bấm nút
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            var ketQua = _bus.TimKiem(txtTimKiem.Text.Trim());
            HienThi(ketQua);
        }

        // Sắp xếp theo đơn giá
        private void btnSapXep_Click(object sender, EventArgs e)
        {
            var sapXep = _bus.SapXepTheoDonGia();
            HienThi(sapXep);
            MessageBox.Show("Đã sắp xếp theo đơn giá tăng dần!");
        }
    }
}