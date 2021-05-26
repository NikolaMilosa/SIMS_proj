using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoHospital.GUI.ManagerUI.Commands;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class HelpViewModel : ViewModel
    {
        #region Fields

        private ObservableCollection<TreeViewItem> _tree;
        private TreeViewItem _selectedItem;

        #endregion

        #region Properties

        public ObservableCollection<TreeViewItem> Tree
        {
            get => _tree;
            set
            {
                _tree = value;
                OnPropertyChanged();
            }
        }

        public TreeViewItem SelectedItem
        {
            get => _selectedItem;
            set => _selectedItem = value;
        }

        #endregion

        #region Commands

        public MyICommand<object> ChangeSelectionCommand { get; set; }
        public MyICommand EnterCommand { get; set; }

        #endregion

        public HelpViewModel()
        {
            ChangeSelectionCommand = new MyICommand<object>(OnSelectionChanged);
            EnterCommand = new MyICommand(OnEnterClick);
            FillTree();
        }

        #region Button functions

        private void OnSelectionChanged(object selectedObject)
        {
            SelectedItem = selectedObject as TreeViewItem;
        }

        private void OnEnterClick()
        {
            SelectedItem.IsExpanded = (SelectedItem.IsExpanded == false) ? true : false;
        }

        #endregion

        #region Private functions

        private void FillTree()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            
            Tree.Add(new TreeViewItem()
            {
                Header = "Rooms"
            });
            Tree.Add(new TreeViewItem()
            {
                Header = "Staff"
            });
            Tree.Add(new TreeViewItem()
            {
                Header = "Inventory"
            });
            Tree.Add(new TreeViewItem()
            {
                Header = "Notifications"
            });
            Tree.Add(new TreeViewItem()
            {
                Header = "Controls and navigation"
            });

            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Show rooms"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Add rooms"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Manage inventory"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Plan renovation"
            });

            Tree[2].Items.Add(new TreeViewItem()
            {
                Header = "Show inventory"
            });
            Tree[2].Items.Add(new TreeViewItem()
            {
                Header = "Add inventory"
            });
            Tree[2].Items.Add(new TreeViewItem()
            {
                Header = "Show medicine"
            });
            Tree[2].Items.Add(new TreeViewItem()
            {
                Header = "Add medicine"
            });

            Tree[4].Items.Add(new TreeViewItem()
            {
                Header = "Room controls"
            });
            Tree[4].Items.Add(new TreeViewItem()
            {
                Header = "Inventory controls"
            });
            Tree[4].Items.Add(new TreeViewItem()
            {
                Header = "Medicine controls"
            });
        }

        #endregion
    }
}
