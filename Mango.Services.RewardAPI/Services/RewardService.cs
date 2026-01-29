using Mango.Services.RewardAPI.Data;
using Mango.Services.RewardAPI.Models;
using Mango.Services.RewardAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.RewardAPI.Services;

public class RewardService : IRewardService
{
    private DbContextOptions<AppDbContext> _dbOptions;

    public RewardService(DbContextOptions<AppDbContext> options)
    {
        _dbOptions = options;
    }


    public async Task UpdateRewards(RewardsMessage rewardsMessage)
    {
        try
        {
            Rewards reward = new()
            {
                OrderId = rewardsMessage.OrderId,
                RewardsActivity = rewardsMessage.RewardsActivity,
                RewardsDate = DateTime.Now,
                UserId = rewardsMessage.UserId,
            };
            await using var _db = new AppDbContext(_dbOptions);
            await _db.Reward.AddAsync(reward);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
        }
    }
}
