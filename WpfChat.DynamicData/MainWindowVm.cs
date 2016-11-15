using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using PropertyChanged;
using WpfChat.DynamicData.Enums;
using WpfChat.DynamicData.Interfaces;
using WpfChat.DynamicData.ViewModels;

namespace WpfChat.DynamicData
{
    [ImplementPropertyChanged]
    public class MainWindowVm
    {
        #region GUI Vars

        public ReadOnlyObservableCollection<IAnimalVm> Animals => _animals;
        public ObservableCollection<EnumSelectorVm> AnimalClasses { get; } = new ObservableCollection<EnumSelectorVm>();

        #endregion

        #region ICommands

        private RelayCommand _cmdSort;
        public ICommand CmdSort => _cmdSort ?? (_cmdSort = new RelayCommand(Sort));

        #endregion

        #region Private Vars

        private readonly ReadOnlyObservableCollection<IAnimalVm> _animals;
        private ISourceList<IAnimalVm> SourceList { get; } = new SourceList<IAnimalVm>();
        private readonly AnimalFilterer _animalFilterer = new AnimalFilterer();
        private readonly AnimalSorter _animalSorter = new AnimalSorter();

        #endregion

        #region C'Tor

        private static MainWindowVm _instance;
        public static MainWindowVm Instance => _instance ?? (_instance = new MainWindowVm());

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

        #endregion

        #region GUI Methods

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

        #endregion

        #region Private Methods

        private void PopulateAnimalClasses()
        {
            foreach (AnimalClass animalClass in Enum.GetValues(typeof(AnimalClass)))
            {
                AnimalClasses.Add(new EnumSelectorVm(animalClass));
            }
        }

        #endregion
    }
}
