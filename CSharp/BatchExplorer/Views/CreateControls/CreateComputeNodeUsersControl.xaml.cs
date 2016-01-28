//Copyright (c) Microsoft Corporation

using Microsoft.Azure.BatchExplorer.ViewModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Microsoft.Azure.BatchExplorer.Views.CreateControls
{
    /// <summary>
    /// Interaction logic for CreateComputeNodeUserControl.xaml
    /// </summary>
    public partial class CreateComputeNodeUsersControl : UserControl
    {
        private readonly CreateComputeNodeUsersViewModel viewModel;


        public CreateComputeNodeUsersControl(CreateComputeNodeUsersViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            var context = this.DataContext as CreateComputeNodeUsersViewModel;
            if (context != null)
            {
                context.Password = (sender as PasswordBox).SecurePassword;
            }
        }
    }
}
