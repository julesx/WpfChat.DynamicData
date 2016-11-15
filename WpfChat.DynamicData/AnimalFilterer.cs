using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfChat.DynamicData.Enums;
using WpfChat.DynamicData.Interfaces;

namespace WpfChat.DynamicData
{
    public class AnimalFilterer
    {
        public bool Filter(IAnimalVm animalVm)
        {
            var selectedAnimalClasses = MainWindowVm.Instance.AnimalClasses.Where(x => x.IsSelected).Select(x => (AnimalClass)Enum.Parse(typeof(AnimalClass), x.Header)).ToList();

            if (selectedAnimalClasses.Any())
                return selectedAnimalClasses.Contains(animalVm.AnimalClass);

            return true;
        }
    }
}
