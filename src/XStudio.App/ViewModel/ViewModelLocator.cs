using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Models.Users;
using XStudio.App.Service;
using XStudio.App.ViewModel.Main;

namespace XStudio.App.ViewModel
{
    public class ViewModelLocator : ViewModelBase
    {
        private static readonly Lazy<ViewModelLocator> InstanceInternal = new(() => new ViewModelLocator(), isThreadSafe: true);

        public static ViewModelLocator Instance => InstanceInternal.Value;

        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DataService>(); // 单例
            services.AddSingleton<MainViewModel>(); // 单例
            services.AddTransient<NonClientAreaViewModel>(); //瞬时
            services.AddSingleton<User>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel Main => _serviceProvider.GetService<MainViewModel>()!;

        /// <summary>
        /// 非控制区域，窗体标题菜单栏
        /// </summary>
        public NonClientAreaViewModel NoUser => _serviceProvider.GetService<NonClientAreaViewModel>()!;

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser => _serviceProvider.GetService<User>()!;

        /// <summary>
        /// 用户登录后，快捷获取Token
        /// </summary>
        public string? AccessToken => Instance?.CurrentUser?.TokenResponse?.AccessToken;

    }
}
