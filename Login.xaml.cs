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
using System.Data.SqlClient;
// using Microsoft.Data.SqlClient;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Data;
using System.Threading;
using System.Text.RegularExpressions;

namespace QUANLYGIANGVIEN
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["constr"]);

        //private bool StatusCheck() {
        //    con.Open();
        //    SqlCommand cmd = new SqlCommand("SP_CHECKSTATUS", con);
        //    int check = (int)cmd.ExecuteScalar();
        //    con.Close();
        //    if (check > 0) {
        //        return true;
        //    }
        //    return false;
        //}

        private bool StatusCheck()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["constr"]))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SP_CHECKSTATUS", con))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int check = Convert.ToInt32(result);
                        return check > 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private void Succes()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public Login()
        {
            InitializeComponent();

            try {
                con.Open();
                con.Close();
            } catch (SqlException) { MessageBox.Show("Lỗi kết nối Microsoft SQL", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error); Close(); return; }

                bool Active = StatusCheck();
            if (Active) { Succes(); }
            this.MouseLeftButtonDown += (sender, e) => DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult yesno = MessageBox.Show("Bạn có muốn thoát không", "Thoát", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            bool thoat = yesno == MessageBoxResult.Yes;
            if (thoat)
            {
                Close();   
            }
        }

        private void ChkCheck(object sender, RoutedEventArgs e)
        {
            txtPasswd.PasswordChar = '\0';
        }

        private void ChkunCheck(object sender, RoutedEventArgs e)
        {
            txtPasswd.PasswordChar = '●';
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsr.Text) || String.IsNullOrEmpty(txtPasswd.Password)) {
                MessageBox.Show("Tài khoản mật khẩu không được để trống", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return;
            }

            con.Open();
            SqlCommand cmd = new SqlCommand("SP_LOGIN", con);
            cmd.CommandType = CommandType.StoredProcedure;

            // Thêm tham số @MAGV
            SqlParameter paramMAGV = new SqlParameter("@MAGV", SqlDbType.NVarChar);
            paramMAGV.Value = txtUsr.Text;
            cmd.Parameters.Add(paramMAGV);

            // Thêm tham số @PASSWD
            SqlParameter paramPASSWD = new SqlParameter("@PASSWD", SqlDbType.NVarChar);
            paramPASSWD.Value = txtPasswd.Password;
            cmd.Parameters.Add(paramPASSWD);

            int login = (int)cmd.ExecuteScalar();
            con.Close();
            if (login == 0) {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else {
                con.Open();
                cmd = new SqlCommand("SP_STATUS", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MAGV", txtUsr.Text);
                cmd.Parameters.AddWithValue("@IS_STATUS", true);
                cmd.ExecuteNonQuery();
                con.Close();

                bool Active = StatusCheck();
                if (Active) { Succes(); }
            }
        }
    }
}
