using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.View.HelpUserControls;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class HelpViewModel : ViewModel
    {
        #region Fields

        private ObservableCollection<TreeViewItem> _tree;
        private TreeViewItem _selectedItem;
        private UserControl _currentControl;

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

        public UserControl CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged();
            }
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
            ResolveCurrentControl();
        }

        #endregion

        #region Private functions

        private void FillTree()
        {
            Tree = new ObservableCollection<TreeViewItem>();
            
            //General
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


            //Rooms
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Show rooms"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Add room"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Manage inventory"
            });
            Tree[0].Items.Add(new TreeViewItem()
            {
                Header = "Plan renovation"
            });


            //Staff
            Tree[1].Items.Add(new TreeViewItem()
            {
                Header = "Generate report"
            });


            //Inventory
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


            //Controls
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

            //Controls -> room controls
            var roomControls = (TreeViewItem) Tree[4].Items[0];
            roomControls.Items.Add(new TreeViewItem()
            {
                Header = "Edit room"
            });
            roomControls.Items.Add(new TreeViewItem()
            {
                Header = "Delete room"
            });

            //Controls -> inventory controls
            var inventoryControls = (TreeViewItem) Tree[4].Items[1];
            inventoryControls.Items.Add(new TreeViewItem()
            {
                Header = "Edit inventory"
            });
            inventoryControls.Items.Add(new TreeViewItem()
            {
                Header = "Delete inventory"
            });
            inventoryControls.Items.Add(new TreeViewItem()
            {
                Header = "Filter"
            });
            inventoryControls.Items.Add(new TreeViewItem()
            {
                Header = "Change quantity"
            });

            //Controls -> medicine controls
            var medicineControls = (TreeViewItem) Tree[4].Items[2];
            medicineControls.Items.Add(new TreeViewItem()
            {
                Header = "Edit medicine"
            });
            medicineControls.Items.Add(new TreeViewItem()
            {
                Header = "Delete medicine"
            });
            medicineControls.Items.Add(new TreeViewItem()
            {
                Header = "Send on validation"
            });
            medicineControls.Items.Add(new TreeViewItem()
            {
                Header = "Rejection note"
            });
        }

        private void ResolveCurrentControl()
        {
            if (SelectedItem.Header.Equals("Show rooms"))
            {
                CurrentControl = new ShowRoomHelp();
            }
            else if (SelectedItem.Header.Equals("Add room"))
            {
                CurrentControl = new AddRoomHelp();
            }
            else if (SelectedItem.Header.Equals("Manage inventory"))
            {
                CurrentControl = new ManageInventoryHelp();
            }
            else if (SelectedItem.Header.Equals("Plan renovation"))
            {
                CurrentControl = new PlanRenovationHelp();
            }
            else if (SelectedItem.Header.Equals("Show inventory"))
            {
                CurrentControl = new ShowInventoryHelp();
            }
            else if (SelectedItem.Header.Equals("Add inventory"))
            {
                CurrentControl = new AddInventoryHelp();
            }
            else if (SelectedItem.Header.Equals("Show medicine"))
            {
                CurrentControl = new ShowMedicineHelp();
            }
            else if (SelectedItem.Header.Equals("Add medicine"))
            {
                CurrentControl = new AddMedicineHelp();
            }
            else if (SelectedItem.Header.Equals("Edit room"))
            {
                CurrentControl = new EditRoomHelp();
            }
            else if (SelectedItem.Header.Equals("Delete room"))
            {
                CurrentControl = new DeleteRoomHelp();
            }
            else if (SelectedItem.Header.Equals("Edit inventory"))
            {
                CurrentControl = new EditInventoryHelp();
            }
            else if (SelectedItem.Header.Equals("Delete inventory"))
            {
                CurrentControl = new DeleteInventoryHelp();
            }
            else if (SelectedItem.Header.Equals("Filter"))
            {
                CurrentControl = new FilterHelp();
            }

            if (SelectedItem.Items.Count == 0)
            {
                ClearColors(Tree.ToList());
                SelectedItem.Background = new SolidColorBrush(Colors.White);
            }
        }

        private void ClearColors(List<TreeViewItem> nodes)
        {
            foreach (var tvi in nodes)
            {
                if (tvi.Items.Count == 0)
                {
                    tvi.Background = new SolidColorBrush(Colors.Transparent);
                }
                else
                {
                    List<TreeViewItem> temp = new List<TreeViewItem>();
                    temp.AddRange(tvi.Items.OfType<TreeViewItem>().ToList());
                    ClearColors(temp);
                }
            }
        }

        #endregion
    }
}
