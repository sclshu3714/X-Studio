// See https://aka.ms/new-console-template for more information
using MyConsoleApp.DivideintoClasses;
using System.Security.Authentication;
using XStudio.DivideintoClasses.Excels;



Console.WriteLine("Hello, World!");
reDivide:
//await ExportToExcelHelper.ExportToExcelWithHeaderStyle();
//await ExportToExcelHelper.ImportExcelToModel("");
DivideClasses.StartDivide(950, 21, 40, 50);
Console.WriteLine("输入ESC退出、其他案件重新计算");
ConsoleKeyInfo keyInfo = Console.ReadKey();
if (keyInfo.Key != ConsoleKey.Escape)
{
    Console.Clear();
    goto reDivide;
}
