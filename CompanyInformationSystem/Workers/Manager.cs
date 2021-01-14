namespace CompanyInformationSystem.Workers
{
    public class Manager : Employee, ISalary
    {
        public Manager(string name, string surname, double salaryAnHour)
            : base(name, surname, salaryAnHour)
        {
            Position = "Менеджер";
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
