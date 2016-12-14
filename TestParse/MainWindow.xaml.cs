using System;
using System.Windows;
using ResumeParser.Domain.Models;
using TestParse.Helpers.DbHelper;
using TestParse.Helpers.Log;
using TestParse.Helpers.Parser;

namespace TestParse
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string Host = "https://belgorod.hh.ru/";

        private const string Url =
            "/search/resume?exp_period=all_time&order_by=relevance&specialization=1&text=.net&pos=full_text&experience=moreThan6&logic=normal&clusters=true&skill=230&skill=15098&schedule=remote&from=cluster_schedule";

        private const string PageParam = "&page=";

        private readonly ILogger _logger;

        private readonly IParser _parser;

        private readonly IDbHelper _dbHelper;

        private Resume _selected;

        public MainWindow()
        {
            //TODO: Внедрить полноценное внедрение зависимостей, используя DI-контейнер и паттерн "внедрение конструктора" (Constructor Injection)

            InitializeComponent();
            _logger = new RichTextBoxLogger(richTextBox);
            _parser = new Parser(_logger, Host, Url, PageParam);
            _dbHelper = new DbHelper();

            button2.IsEnabled = false;
        }

        private void Update() => dataGrid.ItemsSource = _dbHelper.GetAll();

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            _logger.Message("Начинаем...");

            var result = await _parser.GetResumes();
            _dbHelper.SaveToDb(result);

            _logger.Message("Готово!");

            Update();

            richTextBox.ScrollToEnd();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void dataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                var resume = (Resume) dataGrid.SelectedItem;
                _selected = resume;
                button2.IsEnabled = true;
            }
            catch (Exception)
            {
                button2.IsEnabled = false;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start($"{_selected.Url}");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            _dbHelper.ClearAll();
            Update();
        }
    }
}
