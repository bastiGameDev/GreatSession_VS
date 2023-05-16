using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SESSIONWPF
{
    internal class Query
    {
        public static string selectAuth = "select [Login], [Password], [Role] from [dbo].[User] where [Login] = '{0}' and [Password] = '{1}' "; //Запрос для валидации
        public static string selectRandomManagerClient = "select top(1) [fullName], [login] from [dbo].[user] where [role] = 'Менеджер по работе с клиентами'";//Получения логина менеджера отвественного за заказ
        public static string selectOrderClient = "select number, format([dbo].[Order].[Data], 'dd.mm.yyyy'), [dbo].[Order].[Name], [dbo].[Product].[Name], [dbo].[Product].[Size], [dbo].[User].[FullName] from [dbo].[Order] inner join [dbo].[Product] on [dbo].[Product].[Name] = [dbo].[Order].[product_FK] inner join [dbo].[User] on [Login] = [dbo].[Order].[user_login_FK_manager] where [user_login_FK_customer] = '{0}'"; //Вывод заказа клиента
        public static string selectProduct = "select [name], concat([name], ' размер: ', [size]) from [dbo].[Product]"; //Вывод продуктов
        public static string insertOrder = "insert into [dbo].[Order] ([Number], [Data], [Name], [product_FK], [user_login_FK_customer], [user_login_FK_manager], [image]) values('{0}', getDate(), '{1}', '{2}', '{3}', '{4}', null) ";
        public static string countOrder = "select count(*) from [dbo].[Order] where convert(date, [Data]) = convert(date, getdate())"; //Получения количества заказов за сегодня
        public static string deleteOrder = "delete from [dbo].[Order] where [Number] = '{0}'";
        public static string insertOrderManager = "insert into [dbo].[Order] ([Number], [Data], [Name], [product_FK], [user_login_FK_customer], [user_login_FK_manager]) values('{0}', getdate(), '{1}', '{2}', '{3}', '{4}')";
        public static string selectClientsCB = "select [FullName], [Login] from [dbo].[User] where [role] = 'Заказчик'";
        public static string findUser = "select fullname from [dbo].[User] where [login] = '{0}'";
    }
}
