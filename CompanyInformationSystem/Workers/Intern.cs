namespace CompanyInformationSystem.Workers
{
    public class Intern : Employee, ISalary
    {
        public Intern(string name, string surname, double salary)
            : base(name, surname, salary)
        {
            Position = "Интерн";
            CalcSalary();
        }

        /// <summary>
        /// Подсчет зарплаты.
        /// </summary>
        /// <returns>Зп в месяц.</returns>
        public void CalcSalary()
        {
            Salary = SalaryCoefficient;
        }
    }
}
