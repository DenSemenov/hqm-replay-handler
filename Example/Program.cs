using hqm_ranked_backend.Helpers;
using hqm_replay_handler;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

Start:
Console.WriteLine("Enter or paste path to hrp file");
var path = Console.ReadLine();
if (File.Exists(path))
{
    Console.WriteLine("Processing file");
    var data = File.ReadAllBytes(path);
    var result = Replay.ParseReplay(data);

    Console.WriteLine("Calculating data");

    var processedData = ReplayDataHelper.GetReplayCalcData(result);

    var jsonResult = JsonConvert.SerializeObject(processedData);
    var outputPath = path.Replace(".hrp", ".json");
    File.WriteAllText(outputPath, jsonResult);

    Console.WriteLine("File saved to {0}", outputPath);

    goto Start;
}
else
{
    Console.WriteLine("File not exists");

    goto Start;
}
