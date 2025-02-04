using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace XStudio.Win {
    internal static class Program {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            // 在应用程序启动时设置全局异常处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 设置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // 设置日志最低级别
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}") // 控制台输出模板
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}") // 文件输出模板
                .CreateLogger();

            try {
                // 在应用程序启动时设置全局异常处理
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch(Exception ex) {
                Log.Fatal(ex, "应用程序意外崩溃");
            }
            finally {
                Log.CloseAndFlush(); // 确保在程序结束时关闭日志
            }
        }

        #region 异常捕获
        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            // 处理UI线程中抛出的未捕获异常
            HandleException(e.Exception);
            e.Handled = true; // 表示异常已处理
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            // 处理非UI线程中抛出的未捕获异常
            Exception? exception = e.ExceptionObject as Exception;
            HandleException(exception);
        }

        private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e) {
            // 处理使用Task生成的异常
            HandleException(e.Exception);
            e.SetObserved(); // 通知系统异常已处理
        }

        private static void HandleException(Exception? ex) {
            // 在这里实现异常处理逻辑，例如显示错误对话框、记录日志等
            // MessageBox.Show($"发生异常: {ex.Message}\n\n异常类型: {ex.GetType().Name}");
            //HandyControl.Controls.Growl.Clear(MessageToken.GrowlMainWindow);
            //if(ex == null) {
            //    HandyControl.Controls.Growl.Error("发生了未知错误", MessageToken.GrowlMainWindow);
            //    return;
            //}
            //HandyControl.Controls.Growl.Error($"{ex.Message}", MessageToken.GrowlMainWindow);
            Log.Error(ex, ex.Message);
        }
        #endregion
    }
}
