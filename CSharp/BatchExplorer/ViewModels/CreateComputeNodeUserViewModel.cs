﻿//Copyright (c) Microsoft Corporation

using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Azure.BatchExplorer.Helpers;
using Microsoft.Azure.BatchExplorer.Messages;
using Microsoft.Azure.BatchExplorer.Models;
using System.Security;

namespace Microsoft.Azure.BatchExplorer.ViewModels
{
    public class CreateComputeNodeUserViewModel : EntityBase
    {
        public SecureString Password { get; set; }

        #region Services
        private readonly IDataProvider batchService;
        #endregion

        #region Public UI Properties
        private string poolId;
        public string PoolId
        {
            get
            {
                return this.poolId;
            }
            set
            {
                this.poolId = value;
                this.FirePropertyChangedEvent("PoolId");
            }
        }

        private string computeNodeId;
        public string ComputeNodeId
        {
            get
            {
                return this.computeNodeId;
            }
            private set
            {
                this.computeNodeId = value;
                this.FirePropertyChangedEvent("ComputeNodeId");
            }
        }

        private string userName;
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
                this.FirePropertyChangedEvent("UserName");
            }
        }
        
        private DateTime expiryTime;
        public DateTime ExpiryTime
        {
            get
            {
                return this.expiryTime;
            }
            set
            {
                this.expiryTime = value;
                this.FirePropertyChangedEvent("ExpiryTime");
            }
        }
        
        private bool isAdmin;
        public bool IsAdmin
        {
            get
            {
                return this.isAdmin;
            }
            set
            {
                this.isAdmin = value;
                this.FirePropertyChangedEvent("IsAdmin");
            }
        }
        #endregion

        public CreateComputeNodeUserViewModel(IDataProvider batchService, string poolId, string computeNodeId)
        {
            this.batchService = batchService;

            this.PoolId = poolId;
            this.ComputeNodeId = computeNodeId;
            this.IsAdmin = true;
            this.ExpiryTime = DateTime.UtcNow + TimeSpan.FromDays(1);

            this.IsBusy = false;
        }
        
        public CommandBase CreateUser
        {
            get
            {
                return new CommandBase(
                    async (o) =>
                    {
                        this.IsBusy = true;
                        
                        try
                        {
                            await this.CreateVMUserAsync(this.Password);
                        }
                        finally
                        {
                            this.IsBusy = false;
                        }
                    }
                );
            }
        }

        private async Task CreateVMUserAsync(SecureString password)
        {
            try
            {
                if (this.IsInputValid(password))
                {
                    password.MakeReadOnly();
                    Task asyncTask = this.batchService.CreateComputeNodeUserAsync(
                        this.PoolId, 
                        this.ComputeNodeId, 
                        this.UserName,
                        password, 
                        this.ExpiryTime, 
                        this.IsAdmin);

                    AsyncOperationTracker.Instance.AddTrackedOperation(new AsyncOperationModel(
                        asyncTask,
                        new ComputeNodeOperation(ComputeNodeOperation.CreateUser, this.PoolId, this.ComputeNodeId)));
                    await asyncTask;

                    Messenger.Default.Send(new CloseGenericPopup());
                }
            }
            catch (Exception e)
            {
                Messenger.Default.Send<GenericDialogMessage>(new GenericDialogMessage(e.ToString()));
            }
        }

        private bool IsInputValid(SecureString password)
        {
            if (string.IsNullOrEmpty(this.PoolId))
            {
                Messenger.Default.Send<GenericDialogMessage>(new GenericDialogMessage("Invalid values for Pool Id"));
                return false;
            }
            else if (string.IsNullOrEmpty(this.UserName))
            {
                Messenger.Default.Send<GenericDialogMessage>(new GenericDialogMessage("Invalid value for user name"));
                return false;
            }
            else if (password == null)
            {
                Messenger.Default.Send<GenericDialogMessage>(new GenericDialogMessage("Invalid value for password"));
                return false;
            }

            return true;
        }
    }
}
