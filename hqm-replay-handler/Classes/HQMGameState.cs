
namespace hqm_replay_handler.Classes
{
    public class HQMServerPlayer
    {
        public string name { get; set; }
        public Tuple<int?, HQMTeam?>? team_and_skater { get; set; }
        public int? index { get; set; }
    }


    public enum HQMTeam
    {
        Red,
        Blue,
    }

    public class HQMSkater
    {
        public int index { get; set; }
        public float pos_x { get; set; }
        public float pos_y { get; set; }
        public float pos_z { get; set; }
        public float rot_x { get; set; }
        public float rot_y { get; set; }
        public float rot_z { get; set; }
        public float stick_pos_x { get; set; }
        public float stick_pos_y { get; set; }
        public float stick_pos_z { get; set; }
        public float stick_rot_x { get; set; }
        public float stick_rot_y { get; set; }
        public float stick_rot_z { get; set; }
        public float body_turn { get; set; }
        public float body_lean { get; set; }
    }

    public class HQMPuck
    {
        public int index { get; set; }
        public float pos_x { get; set; }
        public float pos_y { get; set; }
        public float pos_z { get; set; }
        public float rot_x { get; set; }
        public float rot_y { get; set; }
        public float rot_z { get; set; }
    }

    public enum HQMMessageType
    {
        PlayerUpdate,
        Goal,
        Chat
    }
    public class HQMMessage
    {
        public HQMMessageType type { get; set; }
        public string player_name { get; set; }
        public Tuple<int?, HQMTeam?>? objectItem { get; set; }
        public int? player_index { get; set; }
        public bool in_server { get; set; }
        public HQMTeam team { get; set; }
        public int? goal_player_index { get; set; }
        public int? assist_player_index { get; set; }
        public string message { get; set; }
    }

    public class HQMGameState
    {
        public uint packet_number { get; set; }
        public uint red_score { get; set; }
        public uint blue_score { get; set; }
        public uint period { get; set; }
        public bool game_over { get; set; }
        public uint time { get; set; }
        public uint goal_message_timer { get; set; }
        public List<dynamic> objects { get; set; }
        public List<HQMServerPlayer> player_list { get; set; }
        public List<HQMMessage> messages_in_this_packet { get; set; }
    }

    public class HQMSkaterPacket
    {
        public (uint, uint, uint) pos;
        public (uint, uint) rot;
        public (uint, uint, uint) stick_pos;
        public (uint, uint) stick_rot;
        public uint body_turn;
        public uint body_lean;
    }

    public class HQMPuckPacket
    {
        public (uint, uint, uint) pos;
        public (uint, uint) rot;
    }
}
