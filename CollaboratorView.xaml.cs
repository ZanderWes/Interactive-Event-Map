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

namespace WPFEventMap
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CollaboratorView : Window
    {
        //unsafe SingletonObject *singleton_ = null;
        SingletonObject singleton_;

        public CollaboratorView()
        {
            InitializeComponent();
        }

        public CollaboratorView(ref SingletonObject singleton)
        {
            InitializeComponent();
            singleton_ = singleton;
            InitialSetup();
        }

        /*****************************************
         *  Initializes the list display of carers
         *****************************************/
        private void InitialSetup()
        {
            List<Collaborator> carer_list = singleton_.GetListOfCarers();
            Carer_List.ItemsSource = carer_list;
        }

        void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Exit_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void High_Support_chkbx_Checked(object sender, RoutedEventArgs e)
        {
            Intermediate_Support_chkbx.IsChecked = false;
            Casual_Support_chkbx.IsChecked = false;
        }

        private void Intermediate_Support_chkbx_Checked(object sender, RoutedEventArgs e)
        {
            Casual_Support_chkbx.IsChecked = false;
            High_Support_chkbx.IsChecked = false;
        }

        private void Casual_Support_chkbx_Checked(object sender, RoutedEventArgs e)
        {
            Intermediate_Support_chkbx.IsChecked = false;
            High_Support_chkbx.IsChecked = false;
        }

        private void Add_Collaborator_btn_Click(object sender, RoutedEventArgs e)
        {
            if(Name_txt.Text == "" || Organisation_txtbox.Text == "")
            {
                MessageBox.Show("Name and Organisation is required");
                return;
            }

            if (Casual_Support_chkbx.IsChecked is false && Intermediate_Support_chkbx.IsChecked is false && High_Support_chkbx.IsChecked is false)
            {
                MessageBox.Show("carer support level is required");
                return;
            }

            Collaborator carer = new Collaborator();
            carer.CarerOrganisation = Organisation_txtbox.Text;
            carer.Name = Name_txt.Text;

            if (Casual_Support_chkbx.IsChecked is true)
                carer.Support = Collaborator.SupportLevel.CASUAL_SUPPORT;
            else if (Intermediate_Support_chkbx.IsChecked is true)
                carer.Support = Collaborator.SupportLevel.INTERMEDIATE_SUPPORT;
            else if (High_Support_chkbx.IsChecked is true)
                carer.Support = Collaborator.SupportLevel.HIGH_SUPPORT;

            singleton_.Instanciate(SingletonObject.SingletonType.COLLABORATOR, carer);

            ResetWindow();
        }
        private void ResetWindow()
        {
            Name_txt.Text = "";
            Organisation_txtbox.Text = "";
            Casual_Support_chkbx.IsChecked = false;
            Intermediate_Support_chkbx.IsChecked = false;
            High_Support_chkbx.IsChecked = false;
            InitialSetup();
        }
    }
}
