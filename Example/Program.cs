using hqm_replay_handler;

var filePath = "D:\\replay.hrp";
var data = File.ReadAllBytes(filePath);
var result = ReplayHandler.ParseReplay(data);

Console.WriteLine();
