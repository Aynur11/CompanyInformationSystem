namespace CompanyInformationSystem.Workers
{
    public class QaEngineer : Employee, ISalary
    {
        public QaEngineer(string name, string surname, double salaryAnHour)
            : base(name, surname, salaryAnHour)
        {
            Position = "Тестировщик";
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
