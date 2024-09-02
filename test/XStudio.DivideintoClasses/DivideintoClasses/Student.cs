using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp.DivideintoClasses
{
    public class Student
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get { return $"{FirstName}{LastName}"; } }

        /// <summary>
        /// 学生姓
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// 学生名
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; } = 15;

        /// <summary>
        /// 性别 0 ♂ 1 ♀
        /// </summary>
        public int Sex { get; set; } = 0;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 座机号
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// 省
        /// </summary>
        public string Province {  get; set; } = string.Empty;

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 区域
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// 学生属于哪个行政班
        /// </summary>
        public string AdministrativeClassId {  get; set; } = string.Empty;

        /// <summary>
        /// 选考组合
        /// </summary>
        public ExaminationType Exams { get; set; } = ExaminationType.PCB;

        /// <summary>
        /// 判断是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is Student student)
            {
                return Id == student.Id; // 根据 Id 去重
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + Name.GetHashCode(); // 使用 Id 的哈希码
        }
    }
}
