using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalRDemo.Services;

namespace SignalRDemo.Controllers
{
    public class VoteController : ControllerBase
    {
        private readonly IVoteManager voteManager;

        public VoteController(IVoteManager voteManager)
        {
            this.voteManager = voteManager;
        }
        [HttpGet("/vote/pie")]
        public async Task<IActionResult> VotePie()
        {
            // save vote
            await voteManager.CastVote("pie");

            return Ok();
        }

        [HttpGet("/vote/bacon")]
        public async Task<IActionResult> VoteBacon()
        {
            // save vote
            await voteManager.CastVote("bacon");

            return Ok();
        }
    }
}