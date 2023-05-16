using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SESSIONWPF
{
    /// <summary>
    /// Логика взаимодействия для ClientManager.xaml
    /// </summary>
    public partial class ClientManager : Window
    {
        DataBaseClass dataBaseClass;
        byte[] imageBytes;

        public string shortName()
        {
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format(Query.findUser, System.Windows.Application.Current.Properties["UserName"]), DataBaseClass.act.select);
            string name = dataBaseClass.resultTable.DefaultView.Table.Rows[0][0].ToString();
            List<string> names = name.ToString().Split(' ').ToList<string>();
            names.RemoveAt(1);
            return names[0].Substring(0, 1) + names[1].Substring(0, 1);
        }

        public string shortNumber()
        {
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.countOrder, DataBaseClass.act.select);
            string numberSTR = dataBaseClass.resultTable.DefaultView.Table.Rows[0][0].ToString();
            if (numberSTR.Length == 1)
            {
                numberSTR = 0 + (int.Parse(numberSTR) + 1).ToString();
            }
            return numberSTR;
        }

        private string formatNumber()
        {
            string Name = shortName();
            string Number = shortNumber();
            string number = DateTime.Now.ToString("ddMMyyyy") + Name + Number;
            return number;
        }

        public ClientManager()
        {
            InitializeComponent();
        }

        private void dg_OrderManager_Loaded(object sender, RoutedEventArgs e)
        {
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.selectProduct, DataBaseClass.act.select);
            dg_OrderManager.ItemsSource = dataBaseClass.resultTable.DefaultView;
        }

        private void cb_client_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox combo = sender as System.Windows.Controls.ComboBox;
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.selectClientsCB, DataBaseClass.act.select);
            combo.ItemsSource = dataBaseClass.resultTable.DefaultView;
            combo.SelectedValuePath = dataBaseClass.resultTable.Columns[1].ColumnName;
            combo.DisplayMemberPath = dataBaseClass.resultTable.Columns[0].ColumnName;
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

            DialogResult result = openFileDialog.ShowDialog();
            if (File.Exists(openFileDialog.FileName)) imageBytes = File.ReadAllBytes(openFileDialog.FileName);
            else System.Windows.MessageBox.Show("Файл не найден!");
            if (imageBytes != null)
            {
                StatusFile.Content = "Картинка загружена";
            }
        }

        private void cb_Products_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox combo = sender as System.Windows.Controls.ComboBox;
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.selectProduct, DataBaseClass.act.select);
            combo.ItemsSource = dataBaseClass.resultTable.DefaultView;
            combo.SelectedValuePath = dataBaseClass.resultTable.Columns[0].ColumnName;
            combo.DisplayMemberPath = dataBaseClass.resultTable.Columns[1].ColumnName;
        }

        private void InsertOrder_Click(object sender, RoutedEventArgs e)
        {
            dataBaseClass = new DataBaseClass();
            string sql = string.Format(Query.insertOrderManager, formatNumber(), tb_NameOrder.Text, cb_Products.SelectedValue, cb_client.SelectedValue, System.Windows.Application.Current.Properties["UserName"].ToString());
            dataBaseClass.sqlExecute(sql, DataBaseClass.act.select);
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            dataBaseClass = new DataBaseClass();
            DataRowView row = dg_OrderManager.SelectedItems[0] as DataRowView;

            if (row.DataView.Count != 0)
            {
                string number = row.Row[0].ToString();
                string sql = string.Format(Query.deleteOrder, number);
                dataBaseClass.sqlExecute(sql, DataBaseClass.act.manipulation);
                System.Windows.MessageBox.Show("Строка удалена!");
            }
        }
    }
}
