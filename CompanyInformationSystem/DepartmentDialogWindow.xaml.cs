using System.Windows;

namespace CompanyInformationSystem
{
    /// <summary>
    /// Interaction logic for DepartmentDialogWindow.xaml
    /// </summary>
    public partial class DepartmentDialogWindow : Window
    {
        //public string Title => "Ввод данных департамента";

        public DepartmentDialogWindow()
        {
            Title = "Ввод данных департамента";
            InitializeComponent();
        }

        public string Name => nameTextBox.Text;

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
            this.DialogResult = false;
        }
    }
}
