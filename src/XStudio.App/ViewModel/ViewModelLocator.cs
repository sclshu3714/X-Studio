using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.App.Service;
using XStudio.App.ViewModel.Main;

namespace XStudio.App.ViewModel
{
    public class ViewModelLocator
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

            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel Main => _serviceProvider.GetService<MainViewModel>()!;

        public NonClientAreaViewModel NoUser => _serviceProvider.GetService<NonClientAreaViewModel>()!;

    }
}
