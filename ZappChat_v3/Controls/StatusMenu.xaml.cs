using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZappChat_v3.Annotations;
using ZappChat_v3.Core;
using ZappChat_v3.Core.ChatElements;
using ZappChat_v3.Core.ChatElements.Enums;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Controls
{
    /// <summary>
    /// Interaction logic for StatusMenu.xaml
    /// </summary>
    public partial class StatusMenu : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Group> _groupCollectionInMainModel; 
        private Dictionary<Group, Status> _currentDictionary;
        private Dictionary<Group, Status> _changedStatusesDictionary; 

        public ICollection<Group> Groups => _currentDictionary.Keys;

        public StatusMenu()
        {
            InitializeComponent();
            _currentDictionary = new Dictionary<Group, Status>();
            _changedStatusesDictionary = new Dictionary<Group, Status>();
        }

        public void SetGroup(ObservableCollection<Group> groups)
        {
            _groupCollectionInMainModel = groups;
            GroupTuggleButton.IsChecked = false;
            GroupList.SelectedIndex = -1;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            GroupGrid.Visibility = Visibility.Visible;
            _currentDictionary = _groupCollectionInMainModel.ToDictionary(g => g, g => g.Status);
            OnPropertyChanged(nameof(Groups));
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            GroupGrid.Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var group = Support.FindAnchestor<ListBoxItem>((DependencyObject) e.OriginalSource).DataContext as Group;
            if(group == null) throw new NullReferenceException("Не могу найти группу");
            _changedStatusesDictionary[group] = (Status) (e.Source as ComboBox).SelectedIndex;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BeginStatusUpdate((o, args) =>
            {
                foreach (var statuse in _changedStatusesDictionary)
                {
                    _groupCollectionInMainModel.First(g => g.Equals(statuse.Key)).Status = statuse.Value;
                    DbContentManager.Instance.Groups.First(g => g.ChatMemberId.Equals(statuse.Key.ChatMemberId)).Status = statuse.Value;
                }
                DbContentManager.Instance.SaveChanges();
                _changedStatusesDictionary = new Dictionary<Group, Status>();
            });
        }

        private void BeginStatusUpdate(DoWorkEventHandler task, RunWorkerCompletedEventHandler callback = null)
        {
            var back = new BackgroundWorker();
            back.DoWork += task;
            back.RunWorkerCompleted += (sender, args) =>
            {
                Thread.Sleep(1000);
                UnblockUserInput();
                GroupTuggleButton.IsChecked = false;
            };
            if(callback != null)
                back.RunWorkerCompleted += callback;
            BlockUserInput();
            back.RunWorkerAsync();
        }

        private void BlockUserInput()
        {
            BlockedGrid.Visibility = Visibility.Visible;
            OpenGroupCreate.Visibility = Visibility.Collapsed;
            AddFriend.Visibility = Visibility.Collapsed;
        }

        private void UnblockUserInput()
        {
            BlockedGrid.Visibility = Visibility.Collapsed;
            OpenGroupCreate.Visibility = Visibility.Visible;
            AddFriend.Visibility = Visibility.Visible;
        }
    }
}
