using System;
using System.Collections.Generic;
using System.Linq;
using Lab06_ThreeLayerMemory.DAL;

namespace Lab06_ThreeLayerMemory.BLL
{
    public class SinhVienBUS
    {
        private readonly SinhVienDAL _dal = new SinhVienDAL();

        public List<SinhVien> GetAll()
        {
            return _dal.GetAll();
        }

        public void Add(SinhVien sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSV))
                throw new ArgumentException("Thiếu mã sinh viên");

            if (_dal.GetAll().Any(x => x.MaSV == sv.MaSV))
                throw new InvalidOperationException("Trùng mã sinh viên");

            _dal.Add(sv);
        }
    }
}