using System;
using System.Windows;

namespace CompanyInformationSystem
{
    /// <summary>
    /// Interaction logic for EmployeeInputDataWindow.xaml
    /// </summary>
    public partial class EmployeeInputDataWindow : Window
    {
        public EmployeeInputDataWindow()
        {
            Title = "Ввод данных сотрудника";
            InitializeComponent();
        }

        public string Name => nameTextBox.Text;
        public string Surname => surnameTextBox.Text;
        public double SalaryCoefficient => IsDouble(salaryCoefficientTextBox.Text) ?
            Convert.ToDouble(salaryCoefficientTextBox.Text) : 0;

        /// <summary>
        /// Предварительная проверка что значение является типом Double.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Булевыйй результат.</returns>
        public bool IsDouble(string text)
        {
            bool isDouble;

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            isDouble = Double.TryParse(text, out _);

            return isDouble;
        }

        /// <summary>
        /// Обработчик нажатия на ОК.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Обработчик нажатия на отмену.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Очистка текст-бокса.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Очистка текст-бокса.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SalaryCoefficientTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            salaryCoefficientTextBox.Text = string.Empty;
        }

        /// <summary>
        /// Очистка текст-бокса.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurnameTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            surnameTextBox.Text = string.Empty;
        }
    }
}
