using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Abramov41
{
    /// <summary>
    /// Логика взаимодействия для Authage.xaml
    /// </summary>
    public partial class Authage : Page
    {
        int currentlogin = 0;
        public Authage()
        {
            InitializeComponent();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            CapchaPanel.Visibility = Visibility.Hidden;
            capchaOneWord.Text = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            capchaTwoWord.Text = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            capchaThreeWord.Text = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            capchaFourWord.Text = new string(Enumerable.Repeat(chars, 1).Select(s => s[random.Next(s.Length)]).ToArray());
            
        }
        private async void BlockLoginButton()
        {
            LogInBtn.IsEnabled = false;
            for (int i = 10; i > 0; i--)
            {
                LogInBtn.Content = $"Подождите {i}с";
                await Task.Delay(1000);
            }
            LogInBtn.Content = "Войти";
            LogInBtn.IsEnabled = true;
        }
        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;
            string cupchatext = CurrentCapcha.Text;
            string capcha = capchaOneWord.Text + capchaTwoWord.Text + capchaThreeWord.Text + capchaFourWord.Text;
            int sch=0;
            if (login =="" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }
            if (cupchatext.Length==4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (cupchatext[i] == capcha[i])
                        sch ++;
                }
            }
            
            User user = ABRAMOV41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            
            if ((user != null)&&(currentlogin == 0))
            {
                    manager.MainFrame.Navigate(new ProductPage(user));
                    LoginTB.Text = "";
                    PassTB.Text = "";
            }
            else if ((user != null)&&(sch == 4))
            {
                LoginTB.Text = "";
                PassTB.Text = "";
                currentlogin = 0;
                CapchaPanel.Visibility = Visibility.Hidden;
                CurrentCapcha.Text = "";
                manager.MainFrame.Navigate(new ProductPage(user));
               
            }
            else 
            {
                currentlogin++;
                MessageBox.Show("Введены неверные данные");
                if (currentlogin > 1)
                 BlockLoginButton();
               
                CapchaPanel.Visibility = Visibility.Visible;
            }
        }
        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            User user=null;
            
                manager.MainFrame.Navigate(new ProductPage(user));
                LoginTB.Text = "";
                PassTB.Text = "";
        }
    }
}
