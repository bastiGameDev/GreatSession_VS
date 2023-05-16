using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SESSIONWPF
{
    public partial class LayoutOfWorkshops : Window
    {
        private string[] factories = { "Заготовительный цех", "Пекарный цех", "Упаковочный цех", "Цех монтажа тортов", "Цех оформления" };
        private BitmapImage selectedIcon;
        private string imagePath = "";

        public LayoutOfWorkshops()
        {
            InitializeComponent();
            factoryComboBox.ItemsSource = factories;
            DataContext = this;

            // Загрузка сохраненных значков для выбранного цеха
            if (factoryComboBox.SelectedItem != null)
            {
                string selectedFactory = factoryComboBox.SelectedItem.ToString();
                ShowSavedIconsForFactory(selectedFactory);
            }
        }

        private void FactoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (factoryComboBox.SelectedItem != null)
            {
                string selectedFactory = factoryComboBox.SelectedItem.ToString();
                imagePath = $"Workspaces/{selectedFactory}.png";
                factoryPlanImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));

                // Очистка значков на плане при смене выбранного цеха
                factoryPlanCanvas.Children.Clear();

                // Отображение сохраненных значков для выбранного цеха
                ShowSavedIconsForFactory(selectedFactory);
            }
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.Source is BitmapImage bitmapImage)
            {
                selectedIcon = bitmapImage;
                DataObject dragData = new DataObject(typeof(BitmapImage), selectedIcon);
                DragDrop.DoDragDrop(image, dragData, DragDropEffects.Move);
            }
        }

        private void FactoryPlanCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedIcon != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(factoryPlanCanvas);
                Image draggedImage = new Image
                {
                    Source = selectedIcon,
                    Width = 48,
                    Height = 48
                };
                Canvas.SetLeft(draggedImage, currentPosition.X - 24);
                Canvas.SetTop(draggedImage, currentPosition.Y - 24);
                factoryPlanCanvas.Children.Add(draggedImage);
            }
        }

        private void FactoryPlanCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectedIcon = null;
        }

        private void FactoryPlanImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Обработчик события клика на изображении плана цеха
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (factoryPlanCanvas.Children.Count == 0)
            {
                MessageBox.Show("There are no icons to save.");
                return;
            }

            XDocument doc = new XDocument();

            XElement root = new XElement("Icons");

            foreach (var child in factoryPlanCanvas.Children)
            {
                if (child is Image icon)
                {
                    double left = Canvas.GetLeft(icon);
                    double top = Canvas.GetTop(icon);

                    XElement iconElement = new XElement("Icon",
                        new XAttribute("Source", icon.Source?.ToString() ?? string.Empty),
                        new XAttribute("Left", left),
                        new XAttribute("Top", top));

                    root.Add(iconElement);
                }
            }

            doc.Add(root);

            string selectedFactory = factoryComboBox.SelectedItem.ToString();
            string directoryPath = "SavedIcons";
            Directory.CreateDirectory(directoryPath); // Создание папки, если она не существует

            string savePath = Path.Combine(directoryPath, $"{selectedFactory}.xml");
            doc.Save(savePath);

            MessageBox.Show("Icons saved successfully.");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Обработчик события клика кнопки "Cancel"
        }

        private void ToolPanelListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem && listBoxItem.Content is Image image)
            {
                DataObject data = new DataObject("Image", image);
                DragDrop.DoDragDrop(listBoxItem, data, DragDropEffects.Copy);
            }
        }

        private void FactoryPlanCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Image") && sender is Canvas canvas)
            {
                Image image = e.Data.GetData("Image") as Image;
                Image droppedImage = new Image
                {
                    Source = image.Source,
                    Width = image.Width,
                    Height = image.Height
                }; Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(droppedImage, dropPosition.X - droppedImage.Width / 2);
                Canvas.SetTop(droppedImage, dropPosition.Y - droppedImage.Height / 2);

                canvas.Children.Add(droppedImage);
            }
        }

        private void ShowSavedIconsForFactory(string selectedFactory)
        {
            // Очистка значков на плане перед отображением сохраненных значков
            factoryPlanCanvas.Children.Clear();

            // Загрузка сохраненных значков для выбранного цеха
            // Вам нужно реализовать эту логику для загрузки значков из сохраненных данных или базы данных

            // Пример кода для отображения значков на холсте (замените его соответствующим кодом)
            Image icon1 = new Image
            {
                Source = new BitmapImage(new Uri("Source/Equipment.png", UriKind.Relative)),
                Width = 48,
                Height = 48
            };
            Canvas.SetLeft(icon1, 100);
            Canvas.SetTop(icon1, 100);
            factoryPlanCanvas.Children.Add(icon1);

            Image icon2 = new Image
            {
                Source = new BitmapImage(new Uri("Source/Exit.png", UriKind.Relative)),
                Width = 48,
                Height = 48
            };
            Canvas.SetLeft(icon2, 200);
            Canvas.SetTop(icon2, 200);
            factoryPlanCanvas.Children.Add(icon2);

            Image icon3 = new Image
            {
                Source = new BitmapImage(new Uri("Source/FireExtinguisher.png", UriKind.Relative)),
                Width = 48,
                Height = 48
            };
            Canvas.SetLeft(icon3, 200);
            Canvas.SetTop(icon3, 200);
            factoryPlanCanvas.Children.Add(icon3);

            Image icon4 = new Image
            {
                Source = new BitmapImage(new Uri("Source/FirstAid.png", UriKind.Relative)),
                Width = 48,
                Height = 48
            };
            Canvas.SetLeft(icon4, 200);
            Canvas.SetTop(icon4, 200);
            factoryPlanCanvas.Children.Add(icon4);

            // Добавьте здесь другие значки
        }





        private void ToolPanelListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toolPanelListBox.SelectedItem != null && factoryPlanCanvas.Children.Contains(toolPanelListBox.SelectedItem as UIElement))
            {
                factoryPlanCanvas.Children.Remove(toolPanelListBox.SelectedItem as UIElement);
            }
        }

        private void FactoryPlanCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Image"))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }

            e.Handled = true;
        }

        private void FactoryPlanCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Image"))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }


    }
}


