using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PiotApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class Department {
        public string Dept;
        public int SemesterCount;
        public List<string> Courses;
    }

    public class Teacher : Department
    {
        
        public string Name;
        public string Designation;
        //public string Dept;
    }

    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        ObservableCollection<Department> deptList = new ObservableCollection<Department>();

        ObservableCollection<Teacher> teacherList = new ObservableCollection<Teacher>();
        public MainPage()
        {
            this.InitializeComponent();

            //if (App.setting.Depertments.Count == 0 || App.setting.Teachers.Count == 0)
            //{
                //Department depertment = new Department();
                //depertment.Courses = new List<string> { "Remove this" };
                //depertment.Dept = "Sample Dept";
                //depertment.SemesterCount = 1;
                //var deList = new List<Department> { depertment };
                //var teaList = new List<Teacher> { new Teacher
                //        { Name = "Name", Designation = "Designation", Dept="Sample Dept" } };
            //}
            //foreach (var item in teaList)
            //{
            //    teacherList.Add(item);
            //}
            //foreach (var item in deList)
            //{
            //    deptList.Add(item);
            //}
            teacherList.Add(new Teacher { Name = "Name", Designation = "Designation", Dept = "Sample Dept" });
        }


        private void deptNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(deptNameInput.Text))
            {
                addDept.IsEnabled = false;
            }
            else
            {
                addDept.IsEnabled = true;
            }
        }
        private void deptNameInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                addDept_Click(sender, e);
            }
        }
        private void addDept_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(deptNameInput.Text)
                && !string.IsNullOrWhiteSpace(courseInput.Text))
            {
                deptAddWorning.Text = "";
                List<string> tempLS = courseInput.Text.Replace(" ", "").Split(',').ToList();
                Department depertment = new Department()
                {
                    Dept = string.Format(deptNameInput.Text.ToUpperInvariant().Trim()),
                    SemesterCount = (int)semesterInput.Value,
                    Courses = tempLS
                };
                deptList.Add(depertment);
                saveSettingButton.IsEnabled = true;
            }
            else
            {
                deptAddWorning.Text = "Something Wrong";
            }
        }
        private void removeDept_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in depertmentList.SelectedItems)
            {
                deptList.Remove((Department)item);
                Department temp = (Department)item;
                var a = from teacher in teacherList
                        where teacher.Dept == temp.Dept
                        select teacher;
                foreach (var teacherItem in a.ToList())
                {
                    teacherList.Remove((Teacher)teacherItem);
                }
            }
            saveSettingButton.IsEnabled = true;

        }
        private void depertmentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (depertmentList.SelectedItem == null)
            {
                removeDept.IsEnabled = false;
                teacherListView.ItemsSource = teacherList;
            }
            else
            {
                removeDept.IsEnabled = true;
                ObservableCollection<Department> t = new ObservableCollection<Department>();
                Department temp = (Department)depertmentList.SelectedItem;
                teacherListView.ItemsSource = from teacher in teacherList
                                              where teacher.Dept == temp.Dept
                                              select teacher;
            }
        }
        private void addTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(teachersName.Text)
                && !string.IsNullOrWhiteSpace(teacherDesignation.Text)
                && deptNameForTeacherCombo.SelectedItem != null)
            {
                teacherAddWarning.Text = "";
                Department d = (Department)deptNameForTeacherCombo.SelectedItem;
                Teacher teacher = new Teacher();
                teacher.Dept = d.Dept;
                teacher.Name = teachersName.Text;
                teacher.Designation = teacherDesignation.Text;
                teacherList.Add(teacher);
                saveSettingButton.IsEnabled = true;
            }
            else
            {
                teacherAddWarning.Text = "Something Wrong";
            }
        }
        private void teacherListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (teacherListView.SelectedItem == null)
            {
                removeTeacher.IsEnabled = false;
            }
            else
            {
                removeTeacher.IsEnabled = true;
            }
        }
        private void removeTeacher_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in teacherListView.SelectedItems)
            {
                teacherList.Remove((Teacher)item);
            }
            saveSettingButton.IsEnabled = true;
        }
        private async void saveSettingButton_Click(object sender, RoutedEventArgs e)
        {
            //App.setting.Depertments = deptList.ToList();
            //App.setting.Teachers = teacherList.ToList();
            //await FileIO.WriteTextAsync(App.settingFile, JsonConvert.SerializeObject(App.setting));
            //saveSettingButton.IsEnabled = false;
        }
        private void teacherSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (depertmentList.SelectedItems != null)
            {
                teacherListView.ItemsSource = from teacher in teacherList
                                              where teacher.Name.Contains(teacherSearch.Text, StringComparison.OrdinalIgnoreCase)
                                              select teacher;
            }
        }
    }
}
