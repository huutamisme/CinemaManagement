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

namespace TestEntity
{
    public partial class MainWindow : Window
    {
        // nên khai báo db ở đây để đảm báo các quan hệ duy trì đúng
        // sử dụng using sẽ làm ngắt kết nối dẫn đến sai các quan hệ giữa các table
        TestEntityEntities db = new TestEntityEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        #region methods

        void LoadData()
        {
            // lấy hết
            /*viewData.ItemsSource = db.SinhViens.Select(p => p).ToList();*/

            // lấy 3 cột bỏ cột quan hệ
            /*var result = from c in db.SinhViens select new {c.ID, c.Name, c.IDLop};
            viewData.ItemsSource = result.ToList();*/

            // dùng quan hệ lấy tên lớp trong bảng lớp từ bảng sinh viên
            /*var result1 = from b in db.SinhViens select new { b.ID, b.Name, TenLop = b.Lop.Name };
            viewData.ItemsSource = result1.ToList();*/

            // thêm mệnh đề where
            var result2 = from a in db.SinhViens
                              //where a.ID < 16
                          select a;
            viewData.ItemsSource = result2.ToList();

            // tìm theo khóa chính
            //var result3 = db.SinhViens.Find(3);
        }

        void AddSinhVien()
        {
            SinhVien sv = new SinhVien();
            sv.Name = txtName.Text;
            sv.IDLop = int.Parse(txtIDLop.Text);
            db.SinhViens.Add(sv);
            db.SaveChanges();   
        }

        void DeleteSinhVien()
        {
            int idlop = int.Parse(txtIDLop.Text);
            SinhVien sv = db.SinhViens.Where(p => p.IDLop == idlop && p.Name == txtName.Text).SingleOrDefault();
            db.SinhViens.Remove(sv);
            db.SaveChanges();
        }

        void EditSinhVien() 
        {
            SinhVien sv = viewData.SelectedItem as SinhVien;
            if (sv != null)
            {
                SinhVien svte = db.SinhViens.Find(sv.ID);
                svte.IDLop = int.Parse(txtIDLop.Text);
                svte.Name = txtName.Text;
                db.SaveChanges();
            }
        }
        #endregion

        #region events

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewData.SelectedItem != null)
            {
                SinhVien selectedSinhVien = (SinhVien)viewData.SelectedItem;
                txtIDLop.Text = selectedSinhVien.IDLop.ToString();
                txtName.Text = selectedSinhVien.Name;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddSinhVien();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DeleteSinhVien();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            EditSinhVien();
        }

        #endregion
    }
}
