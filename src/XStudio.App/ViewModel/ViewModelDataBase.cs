using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq.Expressions;

namespace XStudio.App.ViewModel
{
    public class ViewModelDataBase<T> : ObservableObject
    {
        private IList<T> _dataList = [];

        public IList<T> DataList
        {
            get => _dataList;
            set => SetProperty(ref _dataList, value);
        }
    }

    public class ViewModelBase : ObservableObject
    {
        private IMessenger? _messengerInstance;

        //
        // 摘要:
        //     Gets a value indicating whether the control is in design mode (running under
        //     Blend or Visual Studio).
        //public bool IsInDesignMode => IsInDesignModeStatic;

        //
        // 摘要:
        //     Gets a value indicating whether the control is in design mode (running in Blend
        //     or Visual Studio).
        //public static bool IsInDesignModeStatic => DesignerLibrary.IsInDesignMode;

        //
        // 摘要:
        //     Gets or sets an instance of a GalaSoft.MvvmLight.Messaging.IMessenger used to
        //     broadcast messages to other objects. If null, this class will attempt to broadcast
        //     using the Messenger's default instance.
        protected IMessenger? MessengerInstance
        {
            get
            {
                return _messengerInstance;
            }
            set
            {
                _messengerInstance = value;
            }
        }

        //
        // 摘要:
        //     Initializes a new instance of the ViewModelBase class.
        public ViewModelBase()
            : this(null)
        {
        }

        //
        // 摘要:
        //     Initializes a new instance of the ViewModelBase class.
        //
        // 参数:
        //   messenger:
        //     An instance of a GalaSoft.MvvmLight.Messaging.Messenger used to broadcast messages
        //     to other objects. If null, this class will attempt to broadcast using the Messenger's
        //     default instance.
        public ViewModelBase(IMessenger? messenger)
        {
            MessengerInstance = messenger;
        }

        //
        // 摘要:
        //     Unregisters this instance from the Messenger class.
        //
        //     To cleanup additional resources, override this method, clean up and then call
        //     base.Cleanup().
        public virtual void Cleanup()
        {
            MessengerInstance?.Cleanup();
        }

        //
        // 摘要:
        //     Broadcasts a PropertyChangedMessage using either the instance of the Messenger
        //     that was passed to this class (if available) or the Messenger's default instance.
        //
        //
        // 参数:
        //   oldValue:
        //     The value of the property before it changed.
        //
        //   newValue:
        //     The value of the property after it changed.
        //
        //   propertyName:
        //     The name of the property that changed.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        protected virtual void Broadcast<T>(T oldValue, T newValue, string? propertyName)
        {
            PropertyChangedMessage<T> message = new PropertyChangedMessage<T>(this, propertyName, oldValue, newValue);
            MessengerInstance?.Send(message);
        }

        //
        // 摘要:
        //     Raises the PropertyChanged event if needed, and broadcasts a PropertyChangedMessage
        //     using the Messenger instance (or the static default instance if no Messenger
        //     instance is available).
        //
        // 参数:
        //   propertyName:
        //     The name of the property that changed.
        //
        //   oldValue:
        //     The property's value before the change occurred.
        //
        //   newValue:
        //     The property's value after the change occurred.
        //
        //   broadcast:
        //     If true, a PropertyChangedMessage will be broadcasted. If false, only the event
        //     will be raised.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        //
        // 言论：
        //     If the propertyName parameter does not correspond to an existing property on
        //     the current class, an exception is thrown in DEBUG configuration only.
        public virtual void RaisePropertyChanged<T>([CallerMemberName] string propertyName = null, T oldValue = default(T), T newValue = default(T), bool broadcast = false)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("This method cannot be called with an empty string", "propertyName");
            }

            //RaisePropertyChanged(propertyName);
            //if (broadcast)
            //{
            //    Broadcast(oldValue, newValue, propertyName);
            //}
        }

        //
        // 摘要:
        //     Raises the PropertyChanged event if needed, and broadcasts a PropertyChangedMessage
        //     using the Messenger instance (or the static default instance if no Messenger
        //     instance is available).
        //
        // 参数:
        //   propertyExpression:
        //     An expression identifying the property that changed.
        //
        //   oldValue:
        //     The property's value before the change occurred.
        //
        //   newValue:
        //     The property's value after the change occurred.
        //
        //   broadcast:
        //     If true, a PropertyChangedMessage will be broadcasted. If false, only the event
        //     will be raised.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue, bool broadcast)
        {
            //RaisePropertyChanged(propertyExpression);
            //if (broadcast)
            //{
            //    string propertyName = ObservableObject.GetPropertyName(propertyExpression);
            //    Broadcast(oldValue, newValue, propertyName);
            //}
        }

        //
        // 摘要:
        //     Assigns a new value to the property. Then, raises the PropertyChanged event if
        //     needed, and broadcasts a PropertyChangedMessage using the Messenger instance
        //     (or the static default instance if no Messenger instance is available).
        //
        // 参数:
        //   propertyExpression:
        //     An expression identifying the property that changed.
        //
        //   field:
        //     The field storing the property's value.
        //
        //   newValue:
        //     The property's value after the change occurred.
        //
        //   broadcast:
        //     If true, a PropertyChangedMessage will be broadcasted. If false, only the event
        //     will be raised.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        //
        // 返回结果:
        //     True if the PropertyChanged event was raised, false otherwise.
        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T field, T newValue, bool broadcast)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            T oldValue = field;
            field = newValue;
            RaisePropertyChanged(propertyExpression, oldValue, field, broadcast);
            return true;
        }

        //
        // 摘要:
        //     Assigns a new value to the property. Then, raises the PropertyChanged event if
        //     needed, and broadcasts a PropertyChangedMessage using the Messenger instance
        //     (or the static default instance if no Messenger instance is available).
        //
        // 参数:
        //   propertyName:
        //     The name of the property that changed.
        //
        //   field:
        //     The field storing the property's value.
        //
        //   newValue:
        //     The property's value after the change occurred.
        //
        //   broadcast:
        //     If true, a PropertyChangedMessage will be broadcasted. If false, only the event
        //     will be raised.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        //
        // 返回结果:
        //     True if the PropertyChanged event was raised, false otherwise.
        protected bool Set<T>(string propertyName, ref T field, T newValue = default(T), bool broadcast = false)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            T oldValue = field;
            field = newValue;
            RaisePropertyChanged(propertyName, oldValue, field, broadcast);
            return true;
        }

        //
        // 摘要:
        //     Assigns a new value to the property. Then, raises the PropertyChanged event if
        //     needed, and broadcasts a PropertyChangedMessage using the Messenger instance
        //     (or the static default instance if no Messenger instance is available).
        //
        // 参数:
        //   field:
        //     The field storing the property's value.
        //
        //   newValue:
        //     The property's value after the change occurred.
        //
        //   broadcast:
        //     If true, a PropertyChangedMessage will be broadcasted. If false, only the event
        //     will be raised.
        //
        //   propertyName:
        //     (optional) The name of the property that changed.
        //
        // 类型参数:
        //   T:
        //     The type of the property that changed.
        //
        // 返回结果:
        //     True if the PropertyChanged event was raised, false otherwise.
        protected bool Set<T>(ref T field, T newValue = default(T), bool broadcast = false, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            T oldValue = field;
            field = newValue;
            RaisePropertyChanged(propertyName, oldValue, field, broadcast);
            return true;
        }
    }
}
