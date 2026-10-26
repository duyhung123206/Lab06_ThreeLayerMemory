using System;

namespace Lab06_ThreeLayerMemory
{
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public double DiemTB { get; set; }
        public string Khoa { get; set; } // Thêm thuộc tính Khoa cho phần TreeView

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, double diemTB, string khoa = "Công nghệ Thông tin")
        {
            MaSV = maSV;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            DiemTB = diemTB;
            Khoa = khoa;
        }
    }
}