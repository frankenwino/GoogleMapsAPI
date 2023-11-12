using ConsoleInputUtility;

ApiKey a = new();

string origin = ConsoleInput.GetString("Enter origin city");
string destination = ConsoleInput.GetString("Enter destination city");
ModeOfTransport transport = ConsoleInput.GetFromEnum<ModeOfTransport>();

MapsAPI m = new MapsAPI(a.Key, origin, destination, transport);

System.Console.WriteLine($"API key: {a.Key}");
System.Console.WriteLine($"Origin: {origin}");
System.Console.WriteLine($"Destination: {destination}");
System.Console.WriteLine($"Transport: {transport}");
System.Console.WriteLine($"Base URL: {m.DistanceMatrixBaseUrl}");
System.Console.WriteLine($"Params: {m.DistanceMatrixParameters["origins"]}");
System.Console.WriteLine($"URL: {m.DistanceMatrixURI}");
// await m.MakeRequestAsync(m.DistanceMatrixURI);
// await m.CallDistanceMatrixAPI();

