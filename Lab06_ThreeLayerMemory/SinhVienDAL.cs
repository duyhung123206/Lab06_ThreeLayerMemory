using System.Collections.Generic;

namespace Lab06_ThreeLayerMemory.DAL
{
    public class SinhVienDAL
    {
        private readonly List<SinhVien> _data = new List<SinhVien>();

        public List<SinhVien> GetAll()
        {
            return new List<SinhVien>(_data);
        }

        public void Add(SinhVien sv)
        {
            _data.Add(sv);
        }
    }
}