using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using lab1.Annotations;

namespace lab1
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private User _userCandidate;
        private RelayCommand _signInCommand;
        private RelayCommand _signUpCommand;
        private RelayCommand _closeCommand;

        public LoginViewModel(User userCandidate)
        {
            this._userCandidate = userCandidate;
        }

        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        public RelayCommand SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand(SignIn,
                           o => !String.IsNullOrEmpty(Username) &&
                                !String.IsNullOrEmpty(Password)));
            }
        }

        public RelayCommand SignUpCommand
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


        private void SignUp(Object obj)
        {

            if (DBAdapter.Users.Any(user => user.Username == Username))
            {
                MessageBox.Show("User with this username already exists");
                return;
            }
            DBAdapter.Users.Add(new User(Username, Password));
            MessageBox.Show("User successfully created");
        }


        internal String Password
        {
            get => _userCandidate.Password;
            set
            {
                _userCandidate.Password = value;
            }
        }
        public String Username
        {
            get => _userCandidate.Username;
            set
            {
                _userCandidate.Username = value;
                OnPropertyChanged();
            }
        }

        private void SignIn(Object obj)
        {

            var currentUser = DBAdapter.Users.FirstOrDefault(user => user.Username == Username &&
                                                                     user.Password == Password);
            if (currentUser == null)
            {
                MessageBox.Show("Wrong Username or Password");
                return;
            }

            StationManager.CurrentUser = currentUser;
         //   MessageBox.Show("You have entered just now");
          
            new Explorer().Show();      //open second form
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
