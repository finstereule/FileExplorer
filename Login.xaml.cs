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
using System.Windows.Shapes;
using static System.String;

namespace lab1
{

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent(); 
            LoginViewModel = new LoginViewModel(new User()); 
            LoginViewModel.RequestClose += Close; //дає можливість відправити запит на закриття
            DataContext = LoginViewModel; //дає можливість обмінюватись данними між формою і виконувальним кодом з LoginViewModel
        }

        private LoginViewModel LoginViewModel { get; set; }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e) //зміна паролю
        {
            LoginViewModel.Password = Password.Password; 
        }


        private void Close(bool isQuitApp) //закриття додатку
        {
            if (!isQuitApp)
                this.Close();
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
