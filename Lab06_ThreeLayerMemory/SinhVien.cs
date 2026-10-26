using System;

public class SinhVien
{
    public string MaSV { get; set; }
    public string HoTen { get; set; }
    public DateTime NgaySinh { get; set; }
    public double DiemTB { get; set; }

    public SinhVien(string maSV, string hoTen, DateTime ngaySinh, double diemTB)
    {
        MaSV = maSV;
        HoTen = hoTen;
        NgaySinh = ngaySinh;
        DiemTB = diemTB;
    }
}   