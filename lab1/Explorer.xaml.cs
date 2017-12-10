using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace lab1
{

    public partial class Explorer : Window
    {
        private ExplorerModel explorerModel { get; set; }

        public Explorer()
        {

            InitializeComponent();
            this.LoadDirectories();


            explorerModel = new ExplorerModel();
            explorerModel.RequestClose += Close2; //дає можливість відправити запит на закриття
        }

        

        private void node_MouseLeftButtonUp(object sender, EventArgs e)
        {
            if (treeView.SelectedItem != null)
            {
                TreeViewItem temp = ((TreeViewItem)treeView.SelectedItem);

                if (temp == null) return;

                string path;

                path = "";

                string temp1 = "";
                string temp2 = "";

                while (true)
                {
                    temp1 = temp.Header.ToString();

                    if (temp1.Contains(@"\"))
                    {
                        temp2 = "";
                    }

                    path = temp1 + temp2 + path;

                    if (temp.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                    {
                        break;
                    }
                    temp = ((TreeViewItem)temp.Parent);
                    temp2 = @"\";

                }
                selectedPath.Text = path;

                Logger.Log(path,"PATH");
            }
            else
            {
                System.Windows.MessageBox.Show("Sorry, can't get correct path!", "Bad news");

                Logger.Log("Can't get correct path!","ERR");
            }
        }

        public void LoadDirectories()
        {
            var drives = DriveInfo.GetDrives();  //get the list of the active system drives
            foreach (var drive in drives)
            {
                if(drive.DriveType == DriveType.Fixed ) // shows only hard drives
                this.treeView.Items.Add(this.GetItem(drive));
            }
        }

        private TreeViewItem GetItem(DriveInfo drive)
        {
            var item = new TreeViewItem
            {
                Header = drive.Name,
                DataContext = drive,
                Tag = drive
            };
            this.AddDummy(item);  //creates a temporary dummy item inside each TreeViewItem, so the TreeViewItem is collapsible without loading the child dirs and files
            item.Expanded += new RoutedEventHandler(item_Expanded); // event handler to the expand dir list event (arrow click)
            return item;
        }

        public class DummyTreeViewItem : TreeViewItem  //determine whether the item is dummy or not
        {
            public DummyTreeViewItem()
                : base()
            {
                base.Header = "Dummy";
                base.Tag = "Dummy";
            }
        }

        // overloaded methods of GetItem to get folders and files
        private TreeViewItem GetItem(DirectoryInfo directory)
        {

            var item = new TreeViewItem
            {
                Header = directory.Name,
                DataContext = directory,
                Tag = directory
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }

        private TreeViewItem GetItem(FileInfo file)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                DataContext = file,
                Tag = file
            };
            return item;
        }


        //methods to process Dummies
        private void AddDummy(TreeViewItem item)
        {
            item.Items.Add(new DummyTreeViewItem());
        }

        private bool HasDummy(TreeViewItem item)
        {
            return item.HasItems && (item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem).Count > 0);
        }

        private void RemoveDummy(TreeViewItem item)
        {
            var dummies = item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem);
            foreach (var dummy in dummies)
            {
                item.Items.Remove(dummy);
            }
        }


        private void ExploreDirectories(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;  //get the list of child directories and files
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    var isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                    if (!isHidden && !isSystem)
                    {
                        item.Items.Add(this.GetItem(directory));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("ERROR: "+ ex, "ERR");
            }
        }
    

               


        private void ExploreFiles(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            try
            {
                foreach (var file in directoryInfo.GetFiles())
                {
                    var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    var isSystem = (file.Attributes & FileAttributes.System) == FileAttributes.System;
                    if (!isHidden && !isSystem)
                    {
                        item.Items.Add(this.GetItem(file));
                    }
                }
            }

            catch (Exception ex)
            {
                Logger.Log("ERROR: " + ex, "ERR");

                System.Windows.Forms.MessageBox.Show("No access", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void item_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (this.HasDummy(item))    //check whether current item has a dummy and remove it
            {
                this.Cursor = System.Windows.Input.Cursors.Wait;
                this.RemoveDummy(item);
                this.ExploreDirectories(item);   //load child directories and files
                this.ExploreFiles(item);
                this.Cursor = System.Windows.Input.Cursors.Arrow;
            }
        }

 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                new UserHistory().ShowDialog();      //open second form           
            }));
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //explorerModel.SignOut();
        }


        private void Close2(bool isQuitApp) //закриття додатку
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
