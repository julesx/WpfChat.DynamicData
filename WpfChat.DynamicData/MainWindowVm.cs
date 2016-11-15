using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using PropertyChanged;
using WpfChat.DynamicData.Enums;
using WpfChat.DynamicData.Interfaces;
using WpfChat.DynamicData.ViewModels;

namespace WpfChat.DynamicData
{
    [ImplementPropertyChanged]
    public class MainWindowVm
    {
        private static MainWindowVm _instance;
        public static MainWindowVm Instance => _instance ?? (_instance = new MainWindowVm());

        private ISourceList<IAnimalVm> SourceList { get; } = new SourceList<IAnimalVm>();
        readonly ReadOnlyObservableCollection<IAnimalVm> _animals;
        public ReadOnlyObservableCollection<IAnimalVm> Animals => _animals;

        public ObservableCollection<EnumSelectorVm> AnimalClasses { get; } = new ObservableCollection<EnumSelectorVm>();

        private readonly AnimalFilterer _animalFilterer = new AnimalFilterer();
        private readonly AnimalSorter _animalSorter = new AnimalSorter();

        public bool SortByName { get; set; } = true;
        public bool SortByClass { get; set; }

        private RelayCommand _cmdSort;
        public ICommand CmdSort => _cmdSort ?? (_cmdSort = new RelayCommand(Sort));

        private MainWindowVm()
        {
            PopulateAnimalClasses();

            SourceList.Add(new CatVm("Dexter", AnimalClass.Mammal));
            SourceList.Add(new DogVm("Shiro", AnimalClass.Mammal));

            var sourceList = SourceList
                                .Connect();
                                //.Publish();

            var animalsSubscription = sourceList
                                        .ObserveOnDispatcher()
                                        .Filter(_animalFilterer.Filter)
                                        .Sort(_animalSorter)
                                        .Bind(out _animals)
                                        .Subscribe();
        }

        private void PopulateAnimalClasses()
        {
            foreach (AnimalClass animalClass in Enum.GetValues(typeof(AnimalClass)))
            {
                AnimalClasses.Add(new EnumSelectorVm(animalClass));
            }
        }

        private void OnSortByNameChanged()
        {
            if (SortByName)
                SortByClass = false;
        }

        private void OnSortByClassChanged()
        {
            if (SortByClass)
                SortByName = false;
        }

        private void Sort(object o)
        {
            var sortColumn = (string) o;

            switch (sortColumn)
            {
                case "Name":
                    break;

                case "Class":
                    break;
            }
        }
    }

    public class AnimalSorter : IComparer<IAnimalVm>
    {
        public int Compare(IAnimalVm x, IAnimalVm y)
        {
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }

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
