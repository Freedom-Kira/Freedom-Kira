using ConsoleConvertJson.NET.Models.Response;
using Newtonsoft.Json;
using System;
using System.Text;
using static ConsoleConvertJson.NET.Models.Response.IssListCamera;

var client = new HttpClient();

var requestData = new
{
    telemetry_id = "1",
    telemetry_code = "666",
    alarm_rec = "agencia",
};



// Serializa o objeto usando Newtonsoft.Json
var json = JsonConvert.SerializeObject(requestData);
var content = new StringContent(json, Encoding.UTF8, "application/json");

// Envia a requisição POST
var response = await client.PostAsync("https://httpbin.org/post", content);

// Lê a resposta como string
var responseBody = response.Content.ReadAsStringAsync(CancellationToken.None).Result;


var result = JsonConvert.DeserializeObject<Root>(responseBody).Json!;


//var fields = new List<string> { "TelemetryId", "TelemetryCode22" };

//string json22 = result.FilterFieldsAsJson(fields, formatting: Formatting.Indented);


//var filter = new ObjectFieldFilter(result, ["TelemetryId", "TelemetryCode22"]);


var filtro = new ObjectFieldFilter(result, ["TelemetryId", "TelemetryCode22"]);
var jso22n = await filtro.ToJsonAsync();

//var filter = new ObjectFieldFilter(result, []);

//var filteredDict = filter.Filter();


Console.WriteLine(jso22n);



