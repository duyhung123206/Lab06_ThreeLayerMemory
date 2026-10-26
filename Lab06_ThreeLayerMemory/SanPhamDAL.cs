using System.Collections.Generic;

namespace Lab06_ThreeLayerMemory.DAL
{
    public class SanPhamDAL
    {
        private readonly List<SanPham> _data = new List<SanPham>();

        public List<SanPham> GetAll()
        {
            return new List<SanPham>(_data);
        }

        public void Add(SanPham sp)
        {
            _data.Add(sp);
        }

        public void Remove(SanPham sp)
        {
            _data.Remove(sp);
        }
    }
}