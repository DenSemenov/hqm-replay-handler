using Example.Classes;
using hqm_ranked_backend.Models.DTO;
using ReplayHandler.Classes;

namespace hqm_ranked_backend.Helpers
{
    public static class ReplayDataHelper
    {

        public static ReplayCalculatedData GetReplayCalcData(List<ReplayTick> ticks)
        {
            var result = new ReplayCalculatedData();

            result.Chats = ticks.SelectMany(x => x.Messages.Select(m => new { Packet = x.PacketNumber, Message = m })).Where(x => x.Message.ReplayMessageType == ReplayMessageType.Chat).Select(x => new ReplayCalculatedChat
            {
                Text = (!String.IsNullOrEmpty(x.Message.PlayerName) ? x.Message.PlayerName +": " : String.Empty) +x.Message.Message,
                Packet = x.Packet
            }).ToList();

            result.Goals = ticks.SelectMany(x => x.Messages.Select(m => new { Packet = x.PacketNumber, Message = m, Period = x.Period, Time = x.Time })).Where(x => x.Message.ReplayMessageType == ReplayMessageType.Goal).Select(x => new ReplayCalculatedGoal
            {
                GoalBy = x.Message.PlayerName,
                Packet = x.Packet,
                Period = x.Period,
                Time = x.Time
            }).ToList();

            var withVectors = new List<ReplayTickWithVectors>();

            ReplayTick? prevTick = null;

            foreach (var tick in ticks)
            {
                var packet = new ReplayTickWithVectors();
                packet.Packet = tick.PacketNumber;

                foreach (var player in tick.Players)
                {
                    ReplayPlayer? oldObject = null;

                    if (prevTick != null)
                    {
                        oldObject = prevTick.Players.FirstOrDefault(x => x.Index == player.Index);
                    }

                    var pos = new Vector(player.PosX, player.PosY, player.PosZ);
                    var rot = new Vector(player.RotX, player.RotY, player.RotZ);
                    var stickPos = new Vector(player.StickPosX, player.StickPosY, player.StickPosZ);
                    var stickRot = new Vector(player.StickRotX, player.StickRotY, player.StickRotZ);

                    packet.Objects.Add(new ObjectWithVectors
                    {
                        Type = ObjectType.Player,
                        Index = player.Index,
                        Pos = pos,
                        Rot = rot,
                        StickPos = stickPos,
                        StickRot = stickRot,
                        PosVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.PosX, oldObject.PosY, oldObject.PosZ) : null, pos),
                        RotVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.RotX, oldObject.RotY, oldObject.RotZ) : null, rot),
                        StickPosVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.StickPosX, oldObject.StickPosY, oldObject.StickPosZ) : null, stickPos),
                        StickRotVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.StickRotX, oldObject.StickRotY, oldObject.StickRotZ) : null, stickRot),
                    });
                }

                foreach (var puck in tick.Pucks)
                {
                    ReplayPuck? oldObject = null;

                    if (prevTick != null)
                    {
                        oldObject = prevTick.Pucks.FirstOrDefault(x => x.Index == puck.Index);
                    }
                    var pos = new Vector(puck.PosX, puck.PosY, puck.PosZ);
                    var rot = new Vector(puck.RotX, puck.RotY, puck.RotZ);

                    int? lastTouched = null;

                    foreach(var player in packet.Objects.Where(x=>x.Type == ObjectType.Player))
                    {
                        var p = player.StickPos.Subtract(pos);
                        var m = p.Magnitude();
                        if (m < 0.25)
                        {
                            lastTouched = player.Index;
                        }
                    }

                    packet.Objects.Add(new ObjectWithVectors
                    {
                        Type = ObjectType.Puck,
                        Index = puck.Index,
                        Pos = pos,
                        Rot = rot,
                        PosVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.PosX, oldObject.PosY, oldObject.PosZ) : null, pos),
                        RotVelocity = Vector.CalcVector(oldObject != null ? new Vector(oldObject.RotX, oldObject.RotY, oldObject.RotZ) : null, rot),
                        TouchedBy = lastTouched
                    });
                }

                withVectors.Add(packet);

                prevTick = tick;
            }

            int? prevTouched = null;

            foreach(var tick in withVectors)
            {
                if (tick.Objects.Where(x => x.Type == ObjectType.Puck).Count() == 1)
                {
                    var puck = tick.Objects.FirstOrDefault(x => x.Type == ObjectType.Puck);
                    var lastTouchedBy = puck.TouchedBy ?? prevTouched;

                    if (lastTouchedBy !=null)
                    {
                        var currentPacketData = ticks.FirstOrDefault(x => x.PacketNumber == tick.Packet);
                        if (currentPacketData != null)
                        {
                            var foundPlayer = currentPacketData.PlayersInList.FirstOrDefault(x => x.Index == lastTouchedBy);
                            if (foundPlayer != null)
                            {
                                var playerInPossession = result.Possession.FirstOrDefault(x => x.Name == foundPlayer.Name);
                                if (playerInPossession != null)
                                {
                                    playerInPossession.Touches += 1;
                                }
                                else
                                {
                                    result.Possession.Add(new ReplayCalculatedPossession
                                    {
                                        Name = foundPlayer.Name,
                                        Touches = 1
                                    });
                                }
                            }
                        }

                        prevTouched = lastTouchedBy;
                    }
                }
            }

            return result;
        }
    }
}
