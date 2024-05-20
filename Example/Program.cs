using hqm_ranked_backend.Helpers;
using hqm_replay_handler;

var filePath = "D:\\test.hrp";
var data = File.ReadAllBytes(filePath);
var result = Replay.ParseReplay(data);

var processedData = ReplayDataHelper.GetReplayCalcData(result);

Console.WriteLine();
