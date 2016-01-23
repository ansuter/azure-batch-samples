//Copyright (c) Microsoft Corporation

namespace Microsoft.Azure.BatchExplorer.Messages
{
    public class ShowCreateComputeNodeUserWindow : ShowCreateComputeNodeUsersWindow
    {
        public string ComputeNodeId { get; private set; }

        public ShowCreateComputeNodeUserWindow(string poolId, string computeNodeId) : base(poolId)
        {
            this.ComputeNodeId = computeNodeId;
        }
    }

    public class ShowCreateComputeNodeUsersWindow
    {
        public string PoolId { get; protected set; }

        public ShowCreateComputeNodeUsersWindow(string poolId)
        {
            this.PoolId = poolId;
        }
    }
}
