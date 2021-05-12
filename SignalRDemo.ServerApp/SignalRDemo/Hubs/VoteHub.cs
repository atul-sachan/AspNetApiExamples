using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Services;

namespace SignalRDemo.Hubs
{
    public class VoteHub : Hub
    {
        private readonly IVoteManager voteManager;

        public VoteHub(IVoteManager voteManager)
        {
            this.voteManager = voteManager;
        }

        public Dictionary<string, int> GetCurrentVotes()
        {
            return voteManager.GetCurrentVotes();
        }
    }
}