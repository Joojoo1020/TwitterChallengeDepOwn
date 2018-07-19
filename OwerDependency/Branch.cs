using System;
using System.Collections.Generic;
namespace OwnerDependencyCheck
{
    // Brach is a Directory which has Owens and Dependency
    public class Branch : IEqualityComparer<Branch>
    {
        public List<String> Owers { set; get; }
        public string Name { get; set; }



        public bool Equals(Branch x, Branch y)
        {
            if (x == null || y == null) return false;
            return string.Compare(x.Name, y.Name) == 0;
        }

        public int GetHashCode(Branch obj)
        {
            return obj.Name.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return string.Compare(((Branch)obj).Name, Name) == 0;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
