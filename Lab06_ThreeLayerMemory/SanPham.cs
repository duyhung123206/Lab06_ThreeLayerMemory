using System;

namespace Lab06_ThreeLayerMemory
{
    public class SanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }

        // Thuộc tính tính tổng giá trị tồn kho của sản phẩm đó
        public double ThanhTien => DonGia * SoLuong;

        public SanPham(string maSP, string tenSP, double donGia, int soLuong)
        {
            MaSP = maSP;
            TenSP = tenSP;
            DonGia = donGia;
            SoLuong = soLuong;
        }
    }
}