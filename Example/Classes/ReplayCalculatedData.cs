﻿using ReplayHandler.Classes;

namespace hqm_ranked_backend.Models.DTO
{
    public class ReplayCalculatedData
    {
        public List<ReplayCalculatedChat> Chats { get; set; } = new List<ReplayCalculatedChat>();
        public List<ReplayCalculatedGoal> Goals { get; set; } = new List<ReplayCalculatedGoal>();
        public List<ReplayCalculatedPossession> Possession { get; set; } = new List<ReplayCalculatedPossession>();
    }

    public class ReplayCalculatedChat
    {
        public uint Packet { get; set; }
        public string Text { get; set; }
    }

    public class ReplayCalculatedGoal
    {
        public uint Packet { get; set; }
        public string GoalBy { get; set; }
        public int Period { get; set; }
        public int Time { get; set; }
    }

    public class ReplayCalculatedPossession
    {
        public string Name { get; set; }
        public int Touches { get; set; }
    }
}