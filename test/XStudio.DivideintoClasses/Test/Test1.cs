using MyConsoleApp.DivideintoClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.Test
{
    public static class Test1
    {
        public static void Test()
        { 
            List<Student> list = new List<Student>() { 
                new Student(){ Id=1, FirstName="A", LastName="1" },
                new Student(){ Id=2, FirstName="A", LastName="2" },
                new Student(){ Id=3, FirstName="A", LastName="3" },
                new Student(){ Id=4, FirstName="A", LastName="4" },
                new Student(){ Id=5, FirstName="A", LastName="5" },
                new Student(){ Id=1, FirstName="A", LastName="1" },
                new Student(){ Id=3, FirstName="A", LastName="3" },
                new Student(){ Id=4, FirstName="A", LastName="4" },
                new Student(){ Id=3, FirstName="A", LastName="3" },
                new Student(){ Id=2, FirstName="A", LastName="2" },
            };
            var a = list.DistinctBy(x => new { x.Id,x.Name }).ToList();
            var b = list.OrderByDescending(x=> new { x.Id, x.Name }).ToList();
        }
    }
}
