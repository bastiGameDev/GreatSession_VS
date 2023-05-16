using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SESSIONWPF
{
    public class ValidationChecker
    {

        /// <summary>
        /// Требования к паролю в сессии 1
        /// 1.4 РЕГИСТРАЦИЯ ЗАКАЗЧИКОВ
        /// </summary>
        /// <param name="loginUser"></param>
        /// <param name="passwordUser"></param>
        public bool PasswordStringValidation(string loginUser, string passwordUser)
        {
            if (passwordUser.Contains(loginUser))
            {
                MessageBox.Show("Пароль не должен состоять из логина!");
                return false;
            }
            else if (passwordUser.Length > 4 && passwordUser.Length < 21)
            {
                MessageBox.Show("Пароль должен содержать от 5 до 20 символов!");
                return false;
            }
            else if (passwordUser.Any(symbol => Char.IsUpper(symbol)))
            {
                MessageBox.Show("Пароль должен содержать буквы верхнего регистра!");
                return false;
            }
            else if (passwordUser.Any(symbol => Char.IsLower(symbol)))
            {
                MessageBox.Show("Пароль должен содержать буквы нижнего регистра!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// После трех неудачных попыток авторизации форма должна блокироваться на 5 секунд, не позволяя вводить данные или любые кнопки
        /// Должно выдаваться сообщение об ошибке в случае неправильного ввода связки логин/пароль.
        /// </summary>
        /// <param name="loginUser">Поле логин UI</param>
        /// <param name="passwordBox">Поле пароля UI</param>
        /// <param name="count">Статическая переменная</param>
        public bool CheckTRY(TextBox loginUser, PasswordBox passwordBox, Button sumbit, int count)
        {
            if (loginUser.Text.Trim() != passwordBox.Password.Trim())
            {
                count++;
                if (count == 5)
                {
                    loginUser.IsEnabled = false;
                    passwordBox.IsEnabled = false;
                    sumbit.IsEnabled = false;
                    count = 0;
                }
                MessageBox.Show("Неверный логин или пароль!");
                return false;
            }
            else if (string.IsNullOrEmpty(loginUser.Text.Trim()) || string.IsNullOrEmpty(passwordBox.Password.Trim()))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return false;
            }
            return true;
        }
    }
}
