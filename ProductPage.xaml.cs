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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Abramov41
{
    /// <summary>
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>

    public partial class ProductPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;
        public ProductPage(User user)
        {
            InitializeComponent();
            if (user!=null)
            {
                FIOTB.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Клиент"; break;
                    case 2:
                        RoleTB.Text = "Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Администратор"; break;
                }
            }
            else
            {
                FIOTB.Text = "Гость";
                RoleTB.Text = "Гость";
            }

                var CurrentProduct = ABRAMOV41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = CurrentProduct;
            ComboType.SelectedIndex = 0;

            UpdateProduct();
        }
        private void UpdateProduct()
        {
            var CurrentProduct = ABRAMOV41Entities.GetContext().Product.ToList();
            CurrentPageList = ABRAMOV41Entities.GetContext().Product.ToList();
            if (ComboType.SelectedIndex == 0)
                CurrentProduct = CurrentProduct.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList();
            if (ComboType.SelectedIndex == 1)
                CurrentProduct = CurrentProduct.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) < 10)).ToList();
            if (ComboType.SelectedIndex == 2)
                CurrentProduct = CurrentProduct.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) < 15)).ToList();
            if (ComboType.SelectedIndex == 3)
                CurrentProduct = CurrentProduct.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15)).ToList();
            CurrentProduct = CurrentProduct.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            ProductListView.ItemsSource = CurrentProduct.ToList();
            if (RButtonDown.IsChecked.Value)
                CurrentProduct = CurrentProduct.OrderByDescending(p => p.ProductCost).ToList();
            if (RButtonUp.IsChecked.Value)
                CurrentProduct = CurrentProduct.OrderBy(p => p.ProductCost).ToList();
            ProductListView.ItemsSource = CurrentProduct;
            TableList = CurrentProduct;
            ChangePage(0);
        }
        private void ChangePage(int? selectedPage)
        {
            

            TBCount.Text = "0";
            CountRecords = TableList.Count;
            TBCount.Text =  CountRecords.ToString();
            TBAllRecords.Text = " из " + CurrentPageList.Count;

        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
        }

        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProduct();

        }

     
    }

}
