using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace lab1
{
 
    public partial class Explorer : Window
    {
       
            public Explorer()
        {
            InitializeComponent();

            webBrowser.LoadCompleted += browser_LoadCompleted;
        }

        private void btnOpen_Click(object sender, EventArgs e) //дозволяє користувачу обрати шлях до папки (Browse)
        {
            using (System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog() { Description = "Please, select path" }) //відкриває стандартний браузер папок для вибору потрібної папки
            {
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) //якщо папку було обрано і натиснуто ОК
                {
                    webBrowser.Source = new Uri(fbd.SelectedPath); 
                    txtPath.Text = fbd.SelectedPath; //присвоює Text елементу txtPath шлях до обраної папки
                }
            }

        }
        //Кнопка (OK) відкриває папку в webBrowser по введеному користувасем в txtPath шляху
        private void btnOpen1_Click(object sender, EventArgs e) 
        {
            try
            {
                webBrowser.Navigate(new Uri(txtPath.Text));
            }
            catch (System.UriFormatException ex)
            {
            }
        }

        //крок назад\вперед (по історії відкриття папок)

        private void btnBack_Click(object sender, EventArgs e) 
        {
            if (webBrowser.CanGoBack)
           webBrowser.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e) 
        {
            if (webBrowser.CanGoForward)
            webBrowser.GoForward();
        }
        
        //коли документ завершив своє завантаження
        void browser_LoadCompleted(object sender, NavigationEventArgs e) 
        {
            Console.WriteLine("The document being navigated to has finished downloading.");
            btnBack.IsEnabled = webBrowser.CanGoBack; //активує\деактивує кнопки навігації в залежності від того, чи їх можна використати
            btnForward.IsEnabled = webBrowser.CanGoForward;
            txtPath.Text = webBrowser.Source.ToString();//виводить шлях до поточної папки в  txtPath
        }
    }
}
