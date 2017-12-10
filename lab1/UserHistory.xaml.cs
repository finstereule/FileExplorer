using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab1
{
    /// <summary>
    /// Логика взаимодействия для History.xaml
    /// </summary>
    public partial class UserHistory : Window
    {
        public UserHistory()
        {
            InitializeComponent();
            loadgrid();
        }

        private void loadgrid()
        {
            FileManagerEntities context = new FileManagerEntities();
            var query = from c in context.DbLoggs
                             where c.UserId == StaticResources.CurrUserId && c.Param!="ERR"
                        select c;
            
            HistoryTable.ItemsSource = query.ToList();
        }

        public UserHistory(DataGrid historyTable, bool contentLoaded)
        {
            HistoryTable = historyTable;
            _contentLoaded = contentLoaded;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loadgrid(); //update grid
        }

    }
}
