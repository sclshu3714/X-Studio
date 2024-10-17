using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using XStudio.School.Timetable.Views.ContentViews;

namespace XStudio.School.Timetable.ViewModels {
    public class HamburgerMenuControlViewModel : BindableBase, IDataErrorInfo, IDisposable {
        private readonly IDialogCoordinator _dialogCoordinator;
        private bool isHamburgerMenuPaneOpen;
        private TimePeriodControlViewModel timePeriodControlViewModel;
        private BindableBase _selectedViewModel;
        private ObservableCollection<MenuItemViewModel> _menuItems;
        private ObservableCollection<MenuItemViewModel> _menuOptionItems;

        public HamburgerMenuControlViewModel(IDialogCoordinator dialogCoordinator) {
            this._dialogCoordinator = dialogCoordinator;
            CreateMenuItems();
            this._selectedViewModel = MenuItems.First();
        }

        public string this[string columnName] => null!;

        public string Error => string.Empty;

        public void Dispose() {
            
        }

        public bool IsHamburgerMenuPaneOpen {
            get => this.isHamburgerMenuPaneOpen;
            set => this.SetProperty(ref this.isHamburgerMenuPaneOpen, value);
        }

        public BindableBase SelectedViewModel {
            get => _selectedViewModel;
            set {
                _selectedViewModel = value;
                this.SetProperty(ref this._selectedViewModel, value);
            }
        }
        public ObservableCollection<MenuItemViewModel> MenuItems {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        public ObservableCollection<MenuItemViewModel> MenuOptionItems {
            get => _menuOptionItems;
            set => SetProperty(ref _menuOptionItems, value);
        }

        public void CreateMenuItems() {
            MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new TimePeriodControlViewModel(_dialogCoordinator,this)
                {
                    Icon = new PackIconMaterial() {
                        Kind = PackIconMaterialKind.Home,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center
                    },
                    Label = "时 段",
                    ToolTip = "管理维护节次的时段",
                    Tag = new TimePeriodControl()
                },
                new SectionControlViewModel(_dialogCoordinator,this)
                {
                    Icon = new PackIconMaterial() {
                        Kind = PackIconMaterialKind.Table,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center
                    },
                    Label = "节 次",
                    ToolTip = "管理维护节次模版",
                    Tag = new SectionControl()
                }
            };

            MenuOptionItems = new ObservableCollection<MenuItemViewModel>
            {
                new SettingsViewModel(_dialogCoordinator,this)
                {
                    Icon = new PackIconMaterial() { 
                        Kind = PackIconMaterialKind.Cog,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center 
                    },
                    Label = "设 置",
                    ToolTip = "设置系统配置"
                }
            };
        }
    }
}
