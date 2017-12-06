using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using FontAwesome.WPF;
using System.Windows;
using lab1.Annotations;
using System.Windows.Controls;
using System.Threading;
using FontAwesome.WPF;

namespace lab1
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private DbUsers _userCandidate;
        private RelayCommand _signInCommand;
        private RelayCommand _signUpCommand;
        private RelayCommand _closeCommand;

        private ImageAwesome _loader;


        public LoginViewModel(DbUsers userCandidate)
        {
            this._userCandidate = userCandidate; //creates candidate for a login\signup
        }
         
        public RelayCommand CloseCommand 
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

        public RelayCommand SignInCommand //lets to sign in if username and password are provided
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
                using (var context = new FileManagerEntities())
                {
                    if (context.DbUsers.Any(u => u.Login == Username))
                    {
                        MessageBox.Show("User with this username already exists");
                        Logger.Log("User with  " + Username + " username already exists", "Msg");
                        return;
                    }
                    context.DbUsers.AddObject(_userCandidate);
                    context.SaveChanges();

                    MessageBox.Show("User successfully created");
                    Logger.Log("New User - " + Username + " created", "Msg");
                }
            });
            OnRequestLoader(false);//"вимикаємо" спіннер

        }


      

        internal String Password // get set for password
        {
            get => _userCandidate.Password;
            set
            {
                _userCandidate.Password = value;
            }
        }
        public String Username //get set for username
        {
            get => _userCandidate.Login;
            set
            {
                _userCandidate.Login = value;
                OnPropertyChanged();
            }
        }

        private async void SignIn(Object obj) //шукає, чи ім'я та пароль відповідають існуючим в базі. якщо так - переходить до браузера
        {
            OnRequestLoader(true);

            await Task.Run(() =>
            {
                Thread.Sleep(1000);  //трохи часу для спіннера

                using (var context = new FileManagerEntities())
                {
                    var currentUser = context.DbUsers.FirstOrDefault(u => u.Login == Username);
                    if (currentUser == null)
                    {
                        MessageBox.Show("Wrong Username!");
                        Logger.Log("Wrong Username entered", "ERR");
                        return;
                    }

                    else
                    {
                        if (currentUser.Password == Password)
                        {
                            StationManager.CurrentUser = currentUser;
                            StaticResources.CurrUserId = currentUser.ID;
                            Logger.Log("_ New session on " + (DateTime.Now) + "_", "MSG");
                        }
                        else
                        {
                            MessageBox.Show("Wrong password!");
                            Logger.Log("Wrong password entered", "ERR");
                            return;
                        }
                    }


                    StationManager.CurrentUser = currentUser;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        new Explorer().Show();      //open second form           
                    }));
                }
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

            Logger.Log("App closed.", "MSG");
        }
    }
}
