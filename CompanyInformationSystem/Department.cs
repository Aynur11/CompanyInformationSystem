using System;
using System.Collections.ObjectModel;
using System.Linq;
using CompanyInformationSystem.Workers;

namespace CompanyInformationSystem
{
    /// <summary>
    /// Описание департамента.
    /// </summary>
    public class Department
    {
        private static int id;
        public int Id { get; }
        public string Name { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        static Department()
        {
            id = 0;
        }

        /// <summary>
        /// Конструктор для департаментов.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="employees"></param>
        /// <param name="departments"></param>
        public Department(string name, ObservableCollection<Employee> employees, ObservableCollection<Department> departments = null)
        {
            Id = ++id;
            Name = name;
            if (departments != null)
            {
                Departments = departments;
                if (employees.Count != 1)
                {
                    throw new Exception("Количество директоров не может быть больше 1");
                }

                //ЗП для директора.
                Employees = new ObservableCollection<Employee>();
                Employees.Add(employees[0]);
                Employees[0].SalaryCoefficient = departments.Select(e => e.Employees.Select(r => r.Salary).Sum()).Sum();
                (Employees[0] as Director).CalcSalary();
            }
            else
            {
                Employees = employees;
                Employees[0].SalaryCoefficient = employees.Select(s => s.Salary).Sum();
                (Employees[0] as Director).CalcSalary();
            }
        }

        public static void ResetId()
        {
            id = 0;
        }
    }
}