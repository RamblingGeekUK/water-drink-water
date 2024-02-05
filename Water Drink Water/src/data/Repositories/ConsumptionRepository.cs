﻿using Microsoft.EntityFrameworkCore;
using TbdFriends.WaterDrinkWater.Data.Contexts;
using TbdFriends.WaterDrinkWater.Data.Contracts;
using TbdFriends.WaterDrinkWater.Data.Models;
using viewmodels;

namespace TbdFriends.WaterDrinkWater.Data.Repositories;

public class ConsumptionRepository(IDbContextFactory<ApplicationDbContext> factory) : IConsumptionRepository
{
    public void LogConsumption(int userId, int amount)
    {
        using var context = factory.CreateDbContext();

        context.Logs.Add(new Consumption
        {
            UserId = userId,
            FluidOunces = amount,
            ConsumedOn = DateTime.UtcNow
        });

        context.SaveChanges();
    }

    public PreferencesViewModel GetPreferences(int userId)
    {
        using var context = factory.CreateDbContext();

        var preferences = context.Preferences.FirstOrDefault(p => p.UserId == userId);

        return new PreferencesViewModel
        {
            TargetFluidOunces = preferences?.TargetFluidOunces ?? 0
        };
    }

    public void SetPreferences(int userId, int targetFluidOunces)
    {
        using var context = factory.CreateDbContext();

        var preferences = context.Preferences.FirstOrDefault(p => p.UserId == userId);

        if (preferences == null)
        {
            context.Preferences.Add(new Preference
            {
                UserId = userId,
                TargetFluidOunces = targetFluidOunces,
                CreatedOn = DateTime.UtcNow
            });
        }
        else
        {
            preferences.TargetFluidOunces = targetFluidOunces;
            preferences.UpdatedOn = DateTime.UtcNow;
        }

        context.SaveChanges();
    }
}