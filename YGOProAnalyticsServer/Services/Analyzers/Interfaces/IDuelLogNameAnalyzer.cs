using System;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Analyzers.Interfaces
{
    public interface IDuelLogNameAnalyzer
    {
        Banlist GetBanlist(string roomName, DateTime endOfTheDuelDate);
        bool IsAnyBanlist(string roomName);
        bool IsDefaultBanlist(string roomName);
        bool IsDuelVersusAI(string roomName);
        bool IsNoDeckCheckEnabled(string roomName);
    }
}