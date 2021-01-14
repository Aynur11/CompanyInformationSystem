namespace CompanyInformationSystem.Workers
{
    public class Analyst : Employee, ISalary
    {
        public Analyst(string name, string surname, double salaryAnHour)
            : base(name, surname, salaryAnHour)
        {
            Position = "Аналитик";
            CalcSalary();
        }

        /// <summary>
        /// Подсчет зарплаты.
        /// </summary>
        /// <returns>Зп в месяц.</returns>
        public void CalcSalary()
        {
            Salary = SalaryCoefficient * 8 * 30;
        }
    }
}