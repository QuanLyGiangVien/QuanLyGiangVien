using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using QUANLYGIANGVIEN.Pages;
using System.Configuration;
// using System.Data.SqlClient;
// using Microsoft.Data.SqlClient;
// using System.Collections.Specialized;
using System.IO;
// using System.Data;
using System.Threading;
using System.Text.RegularExpressions;

namespace QUANLYGIANGVIEN
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["constr"]);

        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => DragMove(); // di chuyển cửa sổ
            PagesNavigation.Navigate(new System.Uri("Pages/Home.xaml", UriKind.RelativeOrAbsolute));
        }

        private void GotoPage(string Pages)
        {
            PagesNavigation.Navigate(new System.Uri(Pages, UriKind.RelativeOrAbsolute));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
            // GotoPage("Pages/CloseApp.xaml");
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rdHome_Click(object sender, RoutedEventArgs e)
        {
            // PagesNavigation.Navigate(new HomePage());
            GotoPage("Pages/Home.xaml");
        }

        private void rdGiangVien_Click(object sender, RoutedEventArgs e)
        {
            GotoPage("Pages/GiangVien.xaml");
        }

        private void rdMonHoc_Click(object sender, RoutedEventArgs e)
        {
            GotoPage("Pages/monhoc.xaml");
        }

        private void rdKhoa_Click(object sender, RoutedEventArgs e)
        {
            GotoPage("Pages/khoa.xaml");
        }

        private void rdLogin_Click(object sender, RoutedEventArgs e)
        {
            GotoPage("Pages/khoa.xaml");
        }

        private void github_StackPanel(object sender, RoutedEventArgs e)
        {
            //System.Uri("https://github.com/dopaemon");
        }

        private void LoginFrm()
        {
            Login login = new Login();
            login.Show();
        }

        private void rdLogout_Checked(object sender, RoutedEventArgs e)
        {
            string magv = null;

            con.Open();
            SqlCommand cmd = new SqlCommand("SP_FINDMAGV", con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int magvColumnIndex = reader.GetOrdinal("MAGV");
                    if (!reader.IsDBNull(magvColumnIndex))
                    {
                        magv = reader.GetString(magvColumnIndex);
                    }
                }
            }
            con.Close();

            MessageBoxResult yesno = MessageBox.Show("Bạn có muốn đăng xuất không ?", "Đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            bool thoat = yesno == MessageBoxResult.Yes;
            if (thoat)
            {
                con.Open();
                cmd = new SqlCommand("SP_STATUS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MAGV", magv);
                cmd.Parameters.AddWithValue("@IS_STATUS", false);
                cmd.ExecuteNonQuery();
                con.Close();

                LoginFrm();

                Close();
            }
        }
    }
}
