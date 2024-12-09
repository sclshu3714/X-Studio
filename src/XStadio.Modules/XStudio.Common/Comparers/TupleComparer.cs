using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Comparers {
    // 自定义的 TupleComparer 类，用于 HashSet 中的 Tuple 比较
    public class TupleComparer<T1, T2> : IEqualityComparer<(T1, T2)>, ITransientDependency {
        public bool Equals((T1, T2) x, (T1, T2) y) {
            return x.Item1 != null && x.Item2 != null && x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2);
        }

        public int GetHashCode((T1, T2) obj) {
            if (obj.Item1 != null && obj.Item2 != null)
                return obj.Item1.GetHashCode() ^ obj.Item2.GetHashCode();
            else
                return 0;
        }
    }
}
