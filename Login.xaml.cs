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

namespace QUANLYGIANGVIEN
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsPasswordBoxEmpty(PasswordBox passwordBox)
        {
            System.Security.SecureString securePassword = passwordBox.SecurePassword;

            return (securePassword == null || securePassword.Length == 0);
        }

        private void Succes() {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool Active = false;
            bool Checked = false;
            if (Active) {
                Checked = true;
                Succes();
            }

            if (String.IsNullOrEmpty(txtUsr.Text)) {
                Checked = false;
                if (!Active && !Checked) { 
                    lblUsr.Content = "*";
                }
            } else { lblUsr.Content = ""; }

            if (String.IsNullOrEmpty(txtUsr.Text)) { 
                Checked = false;
                if (!Active && !Checked) { 
                    lblPwd.Content = "*";
                }
            } else { lblPwd.Content = ""; }

            if (txtPasswd.Password == "admin" && txtUsr.Text == "admin") { 
                Active = true;
                Succes();
            } else {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }

        private void ChkCheck(object sender, RoutedEventArgs e)
        {

        }

        private void ChkunCheck(object sender, RoutedEventArgs e)
        {

        }
    }
}
