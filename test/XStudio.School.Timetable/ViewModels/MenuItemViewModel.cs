using MahApps.Metro.Controls;
using MahApps.Metro.ValueBoxes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XStudio.School.Timetable.ViewModels
{
    /// <summary>
    /// Implement the IHamburgerMenuItemBase to allow set the Visibility of the item itself.
    /// </summary>
    public class MenuItemViewModel : BindableBase, IHamburgerMenuItemBase {
        private object _icon;
        private object _label;
        private object _toolTip;
        private bool _isVisible = true;
        private object _tag;

        public MenuItemViewModel(HamburgerMenuControlViewModel hmburgerMenuViewModel) {
            HmburgerMenuViewModel = hmburgerMenuViewModel;
        }

        public HamburgerMenuControlViewModel HmburgerMenuViewModel { get; }

        public object Icon {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }

        public object Label {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public object ToolTip {
            get => _toolTip;
            set => SetProperty(ref _toolTip, value);
        }

        public bool IsVisible {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public object Tag {
            get => _tag;
            set => SetProperty(ref _tag, value);
        }
    }
}
