using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class ExplorerModel
    {
        
        private RelayCommand _signOutCommand;
        private RelayCommand _closeCommand;
        
        public ExplorerModel()
        {
        }

        public RelayCommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(obj => OnRequestClose(true))); }
        }

        

        public RelayCommand SignOutCommand 
        {
            get
            {
                return _signOutCommand;
            }
        }




        public void SignOut()
        {

            OnRequestClose(false);
            StationManager.CurrentUser = null;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        new Login().Show();      //open second form           
                    }));
                
        }



        internal event CloseHandler RequestClose;
        public delegate void CloseHandler(bool isQuitApp);
        protected virtual void OnRequestClose(bool isquitapp)
        {
            RequestClose?.Invoke(isquitapp);

            Logger.Log("App closed.", "MSG");
        }
    }
}
