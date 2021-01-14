using System;

namespace CompanyInformationSystem.Workers
{
    public class Director : Employee, ISalary
    {
        public Director(string name, string surname)
            : base(name, surname)
        {
            Position = "Директор";
        }

        /// <summary>
        /// Подсчет зарплаты.
        /// </summary>
        /// <returns>Зп в месяц.</returns>
        public void CalcSalary()
        {
            double salary = Math.Round((SalaryCoefficient * 0.15), MidpointRounding.ToEven);
            Salary = salary < 1300 ? 1300 : salary;
        }
    }
}