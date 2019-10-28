using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filtersView
{
    public static class Extension
    {
        public static string ListToString(this IList list)
        {
            StringBuilder result = new StringBuilder("");
            if (list.Count > 0)
            {
                result.Append(list[0].ToString());
                for (int i = 1; i < list.Count; i++)
                    result.AppendFormat(", {0}", list[i].ToString());
            }
            return result.ToString();
        }
        public static ObservableCollection<T> Convert<T>(this IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original.Cast<T>());
        }
    }
}
