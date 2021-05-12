using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRDemo.Services
{
    public interface IVoteManager
    {
        Task CastVote(string voteFor);
        Dictionary<string, int> GetCurrentVotes();
    }
}