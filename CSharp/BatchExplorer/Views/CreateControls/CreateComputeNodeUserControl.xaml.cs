﻿//Copyright (c) Microsoft Corporation

using System.Windows.Controls;
using Microsoft.Azure.BatchExplorer.ViewModels;
using System.Security;

namespace Microsoft.Azure.BatchExplorer.Views.CreateControls
{
    /// <summary>
    /// Interaction logic for CreateComputeNodeUserControl.xaml
    /// </summary>
    public partial class CreateComputeNodeUserControl : UserControl
    {
        private readonly CreateComputeNodeUserViewModel viewModel;
        

        public CreateComputeNodeUserControl(CreateComputeNodeUserViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                (this.DataContext as CreateComputeNodeUserViewModel).Password = (sender as PasswordBox).SecurePassword;
            }
        }
    }
}
