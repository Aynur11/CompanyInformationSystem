using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CompanyInformationSystem.Workers;

namespace CompanyInformationSystem
{
    /// <summary>
    /// Описание комнании.
    /// </summary>
    public class Company : INotifyPropertyChanged
    {
        private int employeesCount;
        public string Name { get; set; }
        public ObservableCollection<Department> Departments;
        public event PropertyChangedEventHandler PropertyChanged;

        public int EmployeesCount
        {
            get => employeesCount;
            set
            {
                employeesCount = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmployeesCount)));
            }
        }

        public Company(string name, ObservableCollection<Department> departments)
        {
            Name = name;
            Departments = departments;
        }

        /// <summary>
        /// Удаляет департамент
        /// </summary>
        /// <param name="departmentId">>Номер департамента.</param>
        /// <param name="departments">Департаменты.</param>
        /// <returns></returns>
        public bool RemoveDepartment(int departmentId, ObservableCollection<Department> departments)
        {
            if (departments == null)
            {
                return false;
            }
            Department department = departments.FirstOrDefault(e => e.Id == departmentId);
            if (departments.Remove(department))
            {
                return true;
            }

            foreach (Department dep in departments)
            {
                if (dep != null)
                {
                    if (RemoveDepartment(departmentId, dep.Departments))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Подсчитывает количество работников в отделе.
        /// </summary>
        /// <param name="departments">Отделы.</param>
        /// <returns>Количество работников</returns>
        public int GetEmployeesCount(ObservableCollection<Department> departments)
        {
            if (departments == null)
            {
                return EmployeesCount;
            }

            foreach (Department department in departments)
            {
                if (department.Employees != null)
                {
                    EmployeesCount += department.Employees.Count;
                }
                if (department.Departments != null)
                {
                    GetEmployeesCount(department.Departments);
                }
            }

            return EmployeesCount;
        }

        /// <summary>
        /// Удаляет сотрудника.
        /// </summary>
        /// <param name="employeeId">Номер сотрудника.</param>
        /// <param name="department">Департамент, где искать сотрудника.</param>
        /// <returns>Получилось удалить или нет.</returns>
        public bool RemoveEmployee(int employeeId, Department department)
        {
            if (department != null)
            {
                Employee employee = department.Employees.FirstOrDefault(e => e.Id == employeeId);
                if (department.Employees.Remove(employee))
                {
                    return true;
                }

                if (department.Departments != null)
                {
                    foreach (Department dep in department.Departments)
                    {
                        if (RemoveEmployee(employeeId, dep))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Считает коэффициент для ЗП директоров отделов.
        /// </summary>
        /// <param name="departments"></param>
        public void UpdateDirectorSalaryCoefficient(ObservableCollection<Department> departments)
        {
            if (departments == null)
            {
                return;
            }

            foreach (Department department in departments)
            {
                if (department.Employees != null)
                {
                    if (department.Employees.Count > 1)
                    {
                        department.Employees[0].SalaryCoefficient =
                            department.Employees.Select(s => s.Salary).Sum() - department.Employees[0].Salary;
                        (department.Employees[0] as Director)?.CalcSalary();
                    }
                    else
                    {
                        department.Employees[0].SalaryCoefficient = CalcHeadDirectorSalaryCoefficient(department.Departments);
                    }
                }

                if (department.Departments != null)
                {
                    UpdateDirectorSalaryCoefficient(department.Departments);
                }
            }
        }

        /// <summary>
        /// Считает  ЗП работников, кроме директоров.
        /// </summary>
        /// <param name="departments"></param>
        public void UpdateAllEmployeesSalary(ObservableCollection<Department> departments)
        {
            if (departments == null)
            {
                return;
            }

            foreach (Department department in departments)
            {
                if (department.Employees != null)
                {
                    foreach (Employee employee in department.Employees)
                    {
                        if (employee is Analyst analyst)
                        {
                            analyst.CalcSalary();
                        }
                        if (employee is Intern intern)
                        {
                            intern.CalcSalary();
                        }
                        if (employee is Manager manager)
                        {
                            manager.CalcSalary();
                        }
                        if (employee is QaEngineer qaEngineer)
                        {
                            qaEngineer.CalcSalary();
                        }
                    }
                }

                if (department.Departments != null)
                {
                    UpdateAllEmployeesSalary(department.Departments);
                }
            }
        }

        /// <summary>
        /// Считает коэффициент для ЗП ггде в отделе только подотделы.
        /// </summary>
        /// <param name="departments"></param>
        /// <returns></returns>
        public double CalcHeadDirectorSalaryCoefficient(ObservableCollection<Department> departments)
        {
            double sum = 0;
            if (departments == null)
            {
                return sum;
            }

            foreach (Department department in departments)
            {
                if (department.Employees != null)
                {
                    sum += department.Employees.Select(s => s.Salary).Sum();
                }

                if (department.Departments != null)
                {
                    CalcHeadDirectorSalaryCoefficient(department.Departments);
                }
            }

            return sum;
        }
    }
}
