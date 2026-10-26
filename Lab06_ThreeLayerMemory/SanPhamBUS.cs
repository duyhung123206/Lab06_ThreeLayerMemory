using System;
using System.Collections.Generic;
using System.Linq;
using Lab06_ThreeLayerMemory.DAL;

namespace Lab06_ThreeLayerMemory.BLL
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL _dal = new SanPhamDAL();

        public List<SanPham> GetAll()
        {
            return _dal.GetAll();
        }

        public void Add(SanPham sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new ArgumentException("Mã sản phẩm không được để trống!");

            if (_dal.GetAll().Any(x => x.MaSP == sp.MaSP))
                throw new InvalidOperationException("Mã sản phẩm đã tồn tại!");

            _dal.Add(sp);
        }

        public void Remove(SanPham sp)
        {
            _dal.Remove(sp);
        }

        // 1. Tính tổng giá trị hàng tồn kho
        public double TinhTongGiaTriTonKho()
        {
            return _dal.GetAll().Sum(x => x.ThanhTien);
        }

        // 2. Tìm kiếm sản phẩm theo tên hoặc mã
        public List<SanPham> TimKiem(string keyword)
        {
            return _dal.GetAll()
                .Where(x => x.MaSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            x.TenSP.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        // 3. Sắp xếp sản phẩm theo đơn giá tăng dần
        public List<SanPham> SapXepTheoDonGia()
        {
            return _dal.GetAll().OrderBy(x => x.DonGia).ToList();
        }
    }
}