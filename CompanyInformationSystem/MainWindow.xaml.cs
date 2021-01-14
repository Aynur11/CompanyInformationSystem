using CompanyInformationSystem.Workers;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace CompanyInformationSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Company Company;
        public MainWindow()
        {
            InitializeComponent();

           
            string companyName = "ОАО Прогрессивные разработки";
            ObservableCollection<Department> headDepartments = new ObservableCollection<Department>()
            {
                new Department
                ("Программные разработки",
                    new ObservableCollection<Employee>
                    {
                        new Director("Павел", "Сафронов")
                    },
                    new ObservableCollection<Department>
                    {
                        new Department("Отдел контроля качества",
                            new ObservableCollection<Employee>
                            {
                                new Director("Николай", "Павлов"),
                                new QaEngineer("Вася", "Васильев",170),
                                new Intern("Иван", "Петров", 20000),
                                new Intern("Михаил", "Михайлов", 15000),
                            }),
                        new Department("Отдел аналитики",
                            new ObservableCollection<Employee>
                            {
                                new Director("Виталий", "Литов"),
                                new Analyst("Дмитрий", "Дмитириев", 200),
                                new Analyst("Александр", "Павлов", 220),
                            })

                    }),
                new Department
                ("Ведомство продаж",
                    new ObservableCollection<Employee>
                    {
                        new Director("Алексей", "Крюков")
                    },
                    new ObservableCollection<Department>
                    {
                        new Department("Отдел продаж",
                            new ObservableCollection<Employee>
                            {
                                new Director("Константин", "Ткачюк"),
                                new Manager("Михаил", "Петров", 250),
                                new Manager("Иван", "Витальев", 180),
                            }),
                        new Department("Отдел продаж_2",
                            new ObservableCollection<Employee>
                            {
                                new Director("Шамиль", "Кудрявцев")
                            },
                            new ObservableCollection<Department>
                            {
                                new Department("Отдел продаж_3",
                                    new ObservableCollection<Employee>
                                    {
                                        new Director("Савелий", "Чупряк"),
                                        new Manager("Евгений", "Мальцов", 211),
                                        new Manager("Лев", "Борисов", 199),
                                    })
                            })
                    })
            };

            Company = new Company(companyName, headDepartments);

            #region MyRegion
            //string json = JsonConvert.SerializeObject(Company);
            //Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            //serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            //serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            //using (StreamWriter sw = new StreamWriter("Company.json"))
            //using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            //{
            //    serializer.Serialize(writer, Company, typeof(Company));
            //}

            //File.WriteAllText("Company.json", json);

            //string json = File.ReadAllText("Company.json");
            //Company = JsonConvert.DeserializeObject<Company>(json, new Newtonsoft.Json.JsonSerializerSettings
            //{
            //    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            //    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            //});

            //// Пересчитываем зарплаты после загрузки структуры из файла, т.к. в компании могли поменяться часы, дни работы.
            //Company.UpdateAllEmployeesSalary(Company.Departments);
            //Company.UpdateDirectorSalaryCoefficient(Company.Departments);
            #endregion

            DataContext = Company;
            treeView.ItemsSource = Company.Departments;
            EmpCountTextBlock.DataContext = Company;
            List<string> positions = new List<string>()
            {
                typeof(Analyst).Name,
                typeof(Director).Name,
                typeof(Intern).Name,
                typeof(Manager).Name,
                typeof(QaEngineer).Name
            };
            PositionsComboBox.ItemsSource = positions;
        }

        /// <summary>
        /// Событие при выделении узла.
        /// </summary>
        /// <param name="sender">Объект, где произошло событие.</param>
        /// <param name="e">Событие.</param>
        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem is Department department)
            {
                dataGrid.ItemsSource = department.Employees;
            }
            dataGrid.Columns[3].IsReadOnly = true;
            dataGrid.Columns[4].IsReadOnly = true;
        }

        /// <summary>
        /// Удаляет сотрудника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = dataGrid.SelectedItem as Employee;
            foreach (Department dep in Company.Departments)
            {
                if (employee != null && Company.RemoveEmployee(employee.Id, dep))
                {
                    MessageBox.Show("Сотрудник удален");
                    Company.EmployeesCount = Company.EmployeesCount - 1;
                    return;
                }
            }
        }

        /// <summary>
        /// Удаляет департамент.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is Department department && Company.RemoveDepartment(department.Id, Company.Departments))
            {
                Company.EmployeesCount = 0;
                Company.GetEmployeesCount(Company.Departments);
            }

            if (Company.EmployeesCount == 0)
            {
                dataGrid.Columns.Clear();
            }
        }

        /// <summary>
        /// Пересчет ЗП при обновлении ячеек.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_OnCurrentCellChanged(object sender, EventArgs e)
        {
            ISalary salary = null;
            if (dataGrid.SelectedItem is Analyst)
            {
                salary = dataGrid.SelectedItem as Analyst;
            }
            else if (dataGrid.SelectedItem is Director)
            {
                salary = dataGrid.SelectedItem as Director;
            }
            else if (dataGrid.SelectedItem is Intern)
            {
                salary = dataGrid.SelectedItem as Intern;
            }
            else if (dataGrid.SelectedItem is Manager)
            {
                salary = dataGrid.SelectedItem as Manager;
            }
            else if (dataGrid.SelectedItem is QaEngineer)
            {
                salary = dataGrid.SelectedItem as QaEngineer;
            }

            salary?.CalcSalary();
            Company.UpdateDirectorSalaryCoefficient(Company.Departments);
            salary = dataGrid.SelectedItem as Director;
            salary?.CalcSalary();
        }

        /// <summary>
        /// Добавляет департамент.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDepartment_OnClick(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is Department department)
            {
                // Если в ветке департаментов нет, то создать такую коллекцию.
                if (department.Departments == null)
                {
                    DepartmentDialogWindow departmentDialogWindow = new DepartmentDialogWindow();
                    if (departmentDialogWindow.ShowDialog() == true)
                    {
                        EmployeeInputDataWindow employeeWindow = new EmployeeInputDataWindow();
                        employeeWindow.salaryCoefficientTextBox.Text = "Не трожь ЗП директора!";
                        employeeWindow.salaryCoefficientTextBox.IsEnabled = false;
                        if (employeeWindow.ShowDialog() == true)
                        {
                            department.Departments = new ObservableCollection<Department>
                            {
                                new Department(departmentDialogWindow.nameTextBox.Text,
                                    new ObservableCollection<Employee>
                                    {
                                        new Director(employeeWindow.Name, employeeWindow.Surname)
                                    })
                            };
                            Company.EmployeesCount++;
                        }
                    }
                    treeView.Items.Refresh();
                }
                // Если в ветку департаменты уже есть, то просто добавить.
                else
                {
                    DepartmentDialogWindow departmentDialogWindow = new DepartmentDialogWindow();
                    if (departmentDialogWindow.ShowDialog() == true)
                    {
                        EmployeeInputDataWindow employeeWindow = new EmployeeInputDataWindow();
                        employeeWindow.salaryCoefficientTextBox.Text = "Не трожь ЗП директора!";
                        employeeWindow.salaryCoefficientTextBox.IsEnabled = false;
                        if (employeeWindow.ShowDialog() == true)
                        {
                            department.Departments.Add(new Department(departmentDialogWindow.nameTextBox.Text,
                                new ObservableCollection<Employee>
                                {
                                    new Director(employeeWindow.Name, employeeWindow.Surname)
                                }));
                            Company.EmployeesCount++;
                        }
                    }
                }
            }
            else
            {
                DepartmentDialogWindow departmentDialogWindow = new DepartmentDialogWindow();
                if (departmentDialogWindow.ShowDialog() == true)
                {
                    EmployeeInputDataWindow employeeWindow = new EmployeeInputDataWindow();
                    employeeWindow.salaryCoefficientTextBox.Text = "Не трожь ЗП директора!";
                    employeeWindow.salaryCoefficientTextBox.IsEnabled = false;
                    if (employeeWindow.ShowDialog() == true)
                    {
                        Company.Departments.Add(new Department(departmentDialogWindow.nameTextBox.Text,
                            new ObservableCollection<Employee>
                            {
                                new Director(employeeWindow.Name, employeeWindow.Surname)
                            }));
                        Company.EmployeesCount++;
                    }
                }
            }
        }

        private void AddEmployee_OnClick(object sender, RoutedEventArgs e)
        {
            Type selectedEmployee;
            switch (PositionsComboBox.Text)
            {
                case "Analyst":
                    selectedEmployee = typeof(Analyst);
                    break;
                case "Director":
                    selectedEmployee = typeof(Director);
                    break;
                case "Intern":
                    selectedEmployee = typeof(Intern);
                    break;
                case "Manager":
                    selectedEmployee = typeof(Manager);
                    break;
                case "QaEngineer":
                    selectedEmployee = typeof(QaEngineer);
                    break;
                default:
                    throw new Exception("Тип не распознан.");
            }

            if (treeView.SelectedItem is Department department && department.Employees != null)
            {
                EmployeeInputDataWindow employeeWindow = new EmployeeInputDataWindow();

                // Если добавляем директора, то блокируем поле ввода зп.
                if (selectedEmployee == typeof(Director))
                {
                    employeeWindow.salaryCoefficientTextBox.Text = "Не трожь ЗП директора!";
                    employeeWindow.salaryCoefficientTextBox.IsEnabled = false;
                }

                if (employeeWindow.ShowDialog() == true)
                {
                    // Если добавляем директора, то проверяем, что директора там нет.
                    if (selectedEmployee == typeof(Director) && department.Employees[0] is Director)
                    {
                        MessageBox.Show("В департаменте может быть только один директор!");
                        return;
                    }

                    // Проверяем что зп введен, за исключениям для директора.
                    while (selectedEmployee != typeof(Director) && !employeeWindow.IsDouble(employeeWindow.salaryCoefficientTextBox.Text))
                    {
                        MessageBox.Show("Введите коэффициент для расчета зарплаты!");
                        return;
                    }

                    // Добавляем сотрудника используя механихм позднего связывания.
                    Employee emp = employeeWindow.SalaryCoefficient == 0
                        ? (Employee)Activator.CreateInstance(selectedEmployee, employeeWindow.Name, employeeWindow.Surname)
                        : (Employee)Activator.CreateInstance(selectedEmployee, employeeWindow.Name, employeeWindow.Surname,
                            employeeWindow.SalaryCoefficient);
                    department.Employees.Add(
                        emp);
                    Company.EmployeesCount++;
                }
            }
        }

        /// <summary>
        /// Сохраняет структуру компании в JSON.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCompanyStruct_OnClick(object sender, RoutedEventArgs e)
        {
            string outputFileName = "SavedCompanyStruct.json";
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(outputFileName))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                serializer.Serialize(writer, Company, typeof(Company));
            }

            MessageBox.Show($"Структура компании успешно сохранена в файл {outputFileName}");
        }

        /// <summary>
        /// Загружает имеющуюся структуру компании из JSON файла.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadCompanyStruct_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(openFileDialog.FileName);
                Department.ResetId();
                Employee.ResetId();
                Company = JsonConvert.DeserializeObject<Company>(json, new Newtonsoft.Json.JsonSerializerSettings
                {
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                });
                // Пересчитываем зарплаты, т.к. в компании могли поменяться часы, дни работы.
                Company.UpdateAllEmployeesSalary(Company.Departments);
                Company.UpdateDirectorSalaryCoefficient(Company.Departments);

                treeView.ItemsSource = Company.Departments;
                EmpCountTextBlock.DataContext = Company;
                Company.EmployeesCount = 0;
                Company.GetEmployeesCount(Company.Departments);
            }
        }
    }
}