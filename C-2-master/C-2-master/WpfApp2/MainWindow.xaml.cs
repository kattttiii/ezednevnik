using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        // Словарь для хранения ежедневных заметок
        private Dictionary<DateTime, List<todo>> todoNote;

        public MainWindow()
        {
            InitializeComponent();
            // Инициализация словаря при запуске приложения
            todoNote = new Dictionary<DateTime, List<todo>>();
            // Установка текущей даты в элемент DatePicker
            DatePicker.SelectedDate = DateTime.Today;
            // Загрузка списка заметок из файла
            LoadList();
            // Обновление списка заметок на основе выбранной даты
            UpdateList();

            // Установка свойства отображения списка заметок на "Name"
            TodoList.DisplayMemberPath = "Name";
            // Обработчик события изменения выбранной заметки в списке
            TodoList.SelectionChanged += TodoList_Select;
        }

        // Обработчик кнопки "Создать новую заметку"
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной даты из элемента DatePicker
            DateTime selectedDate = DatePicker.SelectedDate ?? DateTime.Today;

            // Создание новой заметки
            todo newNote = new(Name.Text, Desc.Text, selectedDate);
            // Добавление заметки в словарь
            if (todoNote.ContainsKey(selectedDate))
            {
                todoNote[selectedDate].Add(newNote);
            }
            else
            {
                todoNote[selectedDate] = new List<todo>() { newNote };
            }
            // Очистка полей ввода
            Name.Text = "";
            Desc.Text = "";
            // Обновление списка заметок и сохранение изменений
            UpdateList();
            SaveList();
        }

        // Обработчик кнопки "Сохранить изменения"
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной заметки из списка
            todo selectNote = (todo)TodoList.SelectedItem;

            if (selectNote != null)
            {
                // Обновление данных выбранной заметки
                selectNote.Name = Name.Text;
                selectNote.Description = Desc.Text;
                selectNote.Date = DatePicker.SelectedDate ?? DateTime.Today;

                // Обновление списка заметок и сохранение изменений
                UpdateList();
                SaveList();
            }
        }

        // Обработчик кнопки "Удалить заметку"
        private void Delete__Click(object sender, RoutedEventArgs e)
        {
            // Получение выбранной даты из элемента DatePicker
            DateTime selectedDate = DatePicker.SelectedDate ?? DateTime.Today;

            // Удаление выбранной заметки из словаря
            if (todoNote.ContainsKey(selectedDate))
            {
                todoNote[selectedDate].Remove((todo)TodoList.SelectedItem);

                // Обновление списка заметок и сохранение изменений
                UpdateList();
                SaveList();
            }
        }

        // Обработчик изменения выбранной заметки в списке
        private void TodoList_Select(object sender, SelectionChangedEventArgs e)
        {
            // Заполнение полей ввода данными выбранной заметки
            if (TodoList.SelectedItem != null)
            {
                todo selectNote = (todo)TodoList.SelectedItem;

                Name.Text = selectNote.Name;
                Desc.Text = selectNote.Description;
            }
            else
            {
                Name.Text = "";
                Desc.Text = "";
            }
        }

        // Обновление списка заметок на основе выбранной даты
        private void UpdateList()
        {
            // Получение выбранной даты из элемента DatePicker
            DateTime selectedDate = DatePicker.SelectedDate ?? DateTime.Today;

            // Обновление источника данных списка заметок
            if (todoNote.ContainsKey(selectedDate))
            {
                TodoList.ItemsSource = todoNote[selectedDate];
            }
            else
            {
                TodoList.ItemsSource = null;
            }
        }

        // Обработчик изменения выбранной даты в элементе DatePicker
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обновление списка заметок при изменении выбранной даты
            UpdateList();
        }

        // Сохранение списка заметок в файл
        private void SaveList()
        {
            Load.Serialize(todoNote, "TodoList.json");
        }

        // Загрузка списка заметок из файла
        private void LoadList()
        {
            todoNote = Load.Deserialize("TodoList.json");
        }
    }
}
