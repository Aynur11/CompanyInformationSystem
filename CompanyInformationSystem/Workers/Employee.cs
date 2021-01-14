using System.ComponentModel;

namespace CompanyInformationSystem.Workers
{
    /// <summary>
    /// Описание сотрудников.
    /// </summary>
    public abstract class Employee : INotifyPropertyChanged
    {
        private static int id;
        private double salary;
        private double salaryCoefficient;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }

        public double Salary
        {
            get => salary;
            set
            {
                salary = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(salary)));
            }
        }

        public double SalaryCoefficient
        {
            get => salaryCoefficient;
            set
            {
                salaryCoefficient = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(salaryCoefficient)));
            }
        }

        static Employee()
        {
            id = 0;
        }

        public Employee(string name, string surname)
        {
            Id = ++id;
            Name = name;
            Surname = surname;
        }

        public Employee(string name, string surname, double salaryCoefficient)
        {
            Id = ++id;
            Name = name;
            Surname = surname;
            SalaryCoefficient = salaryCoefficient;
        }

        public static void ResetId()
        {
            id = 0;
        }
    }
}