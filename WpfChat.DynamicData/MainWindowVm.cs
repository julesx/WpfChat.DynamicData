using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    public class MainWindowVm: IDisposable
    {
        #region GUI Vars

        public ReadOnlyObservableCollection<IAnimalVm> Animals => _animals;
        public ObservableCollection<EnumSelectorVm> AnimalClasses { get; } = new ObservableCollection<EnumSelectorVm>();

        #endregion

        #region ICommands

        private RelayCommand _cmdSort;
        public ICommand CmdSort => _cmdSort ?? (_cmdSort = new RelayCommand(obj =>
                                   {
                                       _sortSubject.OnNext((string)obj);
                                   }));

        #endregion

        #region Private Vars

        private readonly IDisposable _cleanUp;
        private readonly ReadOnlyObservableCollection<IAnimalVm> _animals;
        private ISourceList<IAnimalVm> SourceList { get; } = new SourceList<IAnimalVm>();
        //Use BehaviorSubject so we get an initial value
        private readonly ISubject<string> _sortSubject  = new BehaviorSubject<string>("Name");

        #endregion

        #region C'Tor

        private static MainWindowVm _instance;
        public static MainWindowVm Instance => _instance ?? (_instance = new MainWindowVm());

        private MainWindowVm()
        {
            PopulateAnimalClasses();

            //TODO: Create different class types, or just make AnimalVm non-abstract
            SourceList.Add(new CatVm("Dexter", AnimalClass.Mammal));
            SourceList.Add(new DogVm("Shiro", AnimalClass.Mammal));
            SourceList.Add(new DogVm("Rover", AnimalClass.Mammal));
            SourceList.Add(new DogVm("Rex", AnimalClass.Mammal));
            SourceList.Add(new CatVm("Whiskers", AnimalClass.Mammal));
            SourceList.Add(new CatVm("Nemo", AnimalClass.Fish));
            SourceList.Add(new CatVm("Moby Dick", AnimalClass.Fish));
            SourceList.Add(new CatVm("Moby Dick", AnimalClass.Amphibian));

            //create an observable comparer which changes each time the sort changes
            var sorter = _sortSubject.Select(CreateSort);

            //this bit of magic monitors the observable colletion and produces a collection of selected items
            //Using this list, we create an observable predicate which is used to filter the result set
            var filter = AnimalClasses.ToObservableChangeSet()
                .FilterOnProperty(animal => animal.IsSelected, animal => animal.IsSelected)
                .ToCollection()
                .StartWith(Enumerable.Empty<EnumSelectorVm>())
                .Select(selectedItems =>
                {
                    var toInclude = selectedItems.Select(enumVm => enumVm.AnimalClass).ToArray();
                    Func<IAnimalVm, bool> predicate = animal => true;

                    //include all if nothing is selected
                    if (toInclude.Length == 0)
                        return predicate;

                    //otherwise match only what is slected
                    predicate = animal => toInclude.Contains(animal.AnimalClass);
                    return predicate;
                });

            var loader = SourceList
                        .Connect()
                        .ObserveOnDispatcher()
                        .Filter(filter)
                        .Sort(sorter)
                        .Bind(out _animals)
                        .Subscribe();

            _cleanUp = new CompositeDisposable(loader);
        }

        #endregion

        #region Private Methods


        private IComparer<IAnimalVm> CreateSort(string sortOn)
        {
            return sortOn == "Name"
                ? SortExpressionComparer<IAnimalVm>.Ascending(animal => animal.Name).ThenByAscending(animal=>animal.AnimalClass)
                : SortExpressionComparer<IAnimalVm>.Ascending(animal => animal.AnimalClass).ThenByAscending(animal => animal.Name);
        }


        private void PopulateAnimalClasses()
        {
            foreach (AnimalClass animalClass in Enum.GetValues(typeof(AnimalClass)))
            {
                AnimalClasses.Add(new EnumSelectorVm(animalClass));
            }
        }

        #endregion

        public void Dispose()
        {
            _cleanUp.Dispose();
        }
    }
}
