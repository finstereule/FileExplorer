using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FontAwesome.WPF;
using System.Windows;
using lab1.Annotations;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;

namespace lab1
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private User _userCandidate;
        private RelayCommand _signInCommand;
        private RelayCommand _signUpCommand;
        private RelayCommand _closeCommand;

        private ImageAwesome _loader;

        public LoginViewModel(User userCandidate)
        {
            this._userCandidate = userCandidate; //отримує данні, введені в форму
        }
         
        public RelayCommand CloseCommand //відправляє? команду закриття
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); } 
        }

        private void OnRequestLoader(bool isShow)        // Створюємо лоадер
        {
            if (isShow == true && _loader == null)
            {
                _loader = new ImageAwesome();
                ((Login)System.Windows.Application.Current.MainWindow).LoginGrid.Children.Add(_loader);
                _loader.Icon = FontAwesomeIcon.CircleOutlineNotch;
                _loader.Spin = true;
                Grid.SetRow(_loader, 3);
                Grid.SetColumnSpan(_loader, 2);
                _loader.IsEnabled = false;
            }
            else  // if (_loader != null)
            {
                ((Login)System.Windows.Application.Current.MainWindow).LoginGrid.Children.Remove(_loader);
                _loader.IsEnabled = true;
                _loader = null;
            }
        }

        public RelayCommand SignInCommand //вхід,я к вже існуючий користувач
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand(SignIn,
                           o => !String.IsNullOrEmpty(Username) &&
                                !String.IsNullOrEmpty(Password)));
            }
        }


        public RelayCommand SignUpCommand //створення нового користувача 
        {
            get
            {
                if (_signUpCommand == null)
                {
                    _signUpCommand = new RelayCommand(SignUp, o => !String.IsNullOrEmpty(Username) &&
                                                                   !String.IsNullOrEmpty(Password));
                }
                return _signUpCommand;
            }
        }


        private async void SignUp(Object obj) //реєстрація нового користувача
        {
            OnRequestLoader(true); //запускаємо лоадер

            await Task.Run(() =>   //в іншому потоці
            {
                if (DBAdapter.Users.Any(user => user.Username == Username))   //якщо корисутвач з таким ім'ям вже існує
                {
                    MessageBox.Show("User with this username already exists");
                    Logger.Log("Trying to create user with existing name"); 
                    return;
                }
                DBAdapter.Users.Add(new User(Username, Password));
                MessageBox.Show("User successfully created");
                Logger.Log("New User created");
            });
                  
            OnRequestLoader(false);  //"вимикаємо" спіннер
        }


        internal String Password //отримує пароль, введений в форму
        {
            get => _userCandidate.Password;
            set
            {
                _userCandidate.Password = value;
            }
        }
        public String Username //отримує ім'я, введене в форму
        {
            get => _userCandidate.Username;
            set
            {
                _userCandidate.Username = value;
                OnPropertyChanged();
            }
        }
        
        private async void SignIn(Object obj) //шукає, чи ім'я та пароль відповідають існуючим в базі. якщо так - переходить до браузера
        {
            OnRequestLoader(true);

            await Task.Run(() =>
            {
                Thread.Sleep(1000);  //трохи часу для спіннера
                var currentUser = DBAdapter.Users.FirstOrDefault(user => user.Username == Username &&
                                                                     user.Password == Password);
                if (currentUser == null)
                {
                    MessageBox.Show("Wrong Username or Password");
                    Logger.Log("Wrong Username or Password entered");
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>   //для основного потоку
                    {
                        System.Windows.Forms.Application.Restart();    //перезапускаємо в разі, ящко пароль чи логін невірний
                    }));
                    return;
                }

                StationManager.CurrentUser = currentUser;
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    new Explorer().Show();      //open second form           
 }));
                //   MessageBox.Show("You have entered just now");
                Logger.Log("___________New session___________");
            });
         
            OnRequestLoader(false);
            OnRequestClose(false);
        }

        internal event CloseHandler RequestClose; 
        public delegate void CloseHandler(bool isQuitApp); 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnRequestClose(bool isquitapp) 
        {
            RequestClose?.Invoke(isquitapp);
        }
    }
}
