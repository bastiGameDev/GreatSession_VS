using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SESSIONWPF
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void RegisterData()
        {
            if(string.IsNullOrEmpty(FirstName.Text) && string.IsNullOrEmpty(SecondName.Text))
            {
                string fullName = FirstName.Text + " " + SecondName.Text + " " + LastName.Text;
                string role = "Клиент";

                DataBaseClass dataBaseClass = new DataBaseClass();
                dataBaseClass.sqlExecute(string.Format("insert into [dbo].[User] " +
                    "([Login], [Password], [Role], [FullName], [Photo_Link]) " +
                    "values ('{0}', '{1}', '{2}', '{3}', '{4}')",
                    LoginUser.Text, PasswordUser.Text, role, fullName, " "), DataBaseClass.act.manipulation);

                MessageBox.Show("Пользователь успешно добавлен");
            }
            else
            {
                MessageBox.Show("Персональная информация должна быть заполнена! (Фамилия и Имя)");
            }

        }


        private void btnRegister_RegistrationWindow_Click_1(object sender, RoutedEventArgs e)
        {
            string inputPassword = PasswordUser.Text.ToString();

            // Проверка длины строки
            if (inputPassword.Length < 5 || inputPassword.Length > 20)
            {
                MessageBox.Show("Пароль должен содержать от 5 до 20 символов");
                return;
            }


            // Проверка наличия заглавных букв
            if (!inputPassword.Any(char.IsUpper))
            {
                MessageBox.Show("Пароль должен содержать заглавные буквы");
                return;
            }

            // Проверка наличия маленьких букв
            if (!inputPassword.Any(char.IsLower))
            {
                MessageBox.Show("Пароль должен содержать маленькие буквы");
                return;
            }

            RegisterData();

        }
    }
}
