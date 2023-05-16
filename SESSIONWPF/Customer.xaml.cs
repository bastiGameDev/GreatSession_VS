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
    /// Логика взаимодействия для Customer.xaml
    /// </summary>
    public partial class Customer : Window
    {
        DataBaseClass dataBaseClass;
        bool IsImageLoaded = false;

        byte[] imageBytes;
        public string loginClientManager;

        public Customer()
        {
            InitializeComponent();
        }

        public string shortName()
        {
            List<string> names = lb_clientManager.Content.ToString().Split(' ').ToList<string>();
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

        private DataBaseClass Ini()
        {
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.selectRandomManagerClient, DataBaseClass.act.select);
            lb_clientManager.Content = dataBaseClass.resultTable.DefaultView.Table.Rows[0][0].ToString();
            loginClientManager = dataBaseClass.resultTable.DefaultView.Table.Rows[0][1].ToString();

            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format(Query.selectOrderClient, System.Windows.Application.Current.Properties["UserName"]), DataBaseClass.act.select);
            return dataBaseClass;
        }

        private void dg_OrderClient_Loaded(object sender, RoutedEventArgs e)
        {
            dg_OrderClient.ItemsSource = Ini().resultTable.DefaultView;
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

            DialogResult result = openFileDialog.ShowDialog();
            if (File.Exists(openFileDialog.FileName)) imageBytes = File.ReadAllBytes(openFileDialog.FileName);
            else System.Windows.MessageBox.Show("Файл не найден!");
            if(imageBytes != null)
            {
                StatusFile.Content = "Картинка загружена";
            }
        }

        private void InsertOrder_Click(object sender, RoutedEventArgs e)
        {
            dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(string.Format(Query.insertOrder, formatNumber(), tb_NameOrder.Text, cb_Products.SelectedValue, System.Windows.Application.Current.Properties["UserName"], loginClientManager), DataBaseClass.act.manipulation);
            System.Windows.MessageBox.Show("Объект добавлен!");
        }


        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            dataBaseClass = new DataBaseClass();
            DataRowView row = dg_OrderClient.SelectedItems[0] as DataRowView;

            if (row.DataView.Count != 0)
            {
                string number = row.Row[0].ToString();
                string sql = string.Format(Query.deleteOrder, number);
                dataBaseClass.sqlExecute(sql, DataBaseClass.act.manipulation);
                System.Windows.MessageBox.Show("Строка удалена!");
            }
        }

        private void cb_Products_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ComboBox combo = sender as System.Windows.Controls.ComboBox;
            DataBaseClass dataBaseClass = new DataBaseClass();
            dataBaseClass.sqlExecute(Query.selectProduct, DataBaseClass.act.select);
            combo.ItemsSource = dataBaseClass.resultTable.DefaultView;
            combo.SelectedValuePath = dataBaseClass.resultTable.Columns[1].ColumnName;
            combo.DisplayMemberPath = dataBaseClass.resultTable.Columns[0].ColumnName;
        }
    }
}
