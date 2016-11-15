using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfChat.DynamicData.Interfaces;

namespace WpfChat.DynamicData
{
    public class AnimalSorter : IComparer<IAnimalVm>
    {
        public int Compare(IAnimalVm x, IAnimalVm y)
        {
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}
