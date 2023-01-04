using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Grpc.Net.Client;
using gRPCClient;

// The port number must match the port of the gRPC server.

// Set the rate of the exponential distribution
double rate = 5;
Random random = new();
// Generate the first event
double time = -Math.Log(1.0 - random.NextDouble()) / rate;
int count = 0;

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

// Generate subsequent events
while (true)
{
    // Sleep until the time of the next event has arrived
    Thread.Sleep((int)(time * 100));

    // Print the time of the event
    Console.WriteLine(time);

    using var channel = GrpcChannel.ForAddress("https://localhost:7232");
    var client = new Greeter.GreeterClient(channel);
    var filePath = "../../../test.json";
    var jsonString = File.ReadAllText(filePath);

    var reply = client.GetHealthDataJsonStringInput(
                      new HealthDataRequestJsonString { Message = jsonString });
    var reply2 = client.GetHealthData(
                      new HealthDataRequest
                      {
                          Medical = false,
                          Childdx = false,
                          Selfharm = false,
                          Sofas = 1,
                          Circadian = false,
                          Clinicalstage = 1,
                          Tripartite = 1,
                          Psychosis = false,
                          Neet = false
                      });
    Console.WriteLine("Greeting: " + reply.Message);

    // Generate the next event
    time += -Math.Log(1.0 - random.NextDouble()) / rate;
    Console.WriteLine("Requests: " + count++ + ", Time Elapsed: "+ stopwatch.Elapsed);
}

//Console.WriteLine("Greeting: " + time.Second);
//Console.WriteLine("Press any key to exit...");
//Console.ReadKey();