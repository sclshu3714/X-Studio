using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace XStudio.App.Helper {

    public static class PasswordBoxHelper {
        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindPasswordChanged));

        public static string GetBindPassword(DependencyObject d) {
            return (string)d.GetValue(BindPasswordProperty);
        }

        public static void SetBindPassword(DependencyObject d, string value) {
            d.SetValue(BindPasswordProperty, value);
        }

        private static void OnBindPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            PasswordBox? passwordBox = d as PasswordBox;
            if (passwordBox == null) return; 
            // 初始化绑定源的值（如果需要的话）  
            if (e.NewValue != null) {
                passwordBox.Password = (string)e.NewValue; // 这通常不会触发 PasswordChanged 事件，因为它是由 SetBinding 触发的，而不是用户输入  
            }

            SetSelection(passwordBox, passwordBox.Password.Length, 0);
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            PasswordBox? passwordBox = sender as PasswordBox;
            if (passwordBox == null) return;
            SetBindPassword(passwordBox, passwordBox.Password);
        }

        /// <summary>
        /// 设置光标位置
        /// </summary>
        /// <param name="passwordBox"></param>
        /// <param name="start">光标开始位置</param>
        /// <param name="length">选中长度</param>
        private static void SetSelection(PasswordBox passwordBox, int start, int length) {
            passwordBox.GetType()?
                       .GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic)?
                       .Invoke(passwordBox, new object[] { start, length
            });
        }
    }
}
