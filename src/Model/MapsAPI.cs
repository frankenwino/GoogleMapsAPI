/* Referring to the following page for help deserialising the JSON reponse:

https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient */

using System.Text.Json;

class MapsAPI
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
    public long ArrivalTime
    {
        /*
        Google's Maps API requires that the arrival time is an integer
        representing the total seconds between 1970-01-01 00:00 and the
        datetime of desired arrival.

        An employee would arrive at the office at 0745 on a typical working
        Monday. We will set the arrival time to the next occurring Monday, 0745.
        */

        get
        {
            DateTime now = DateTime.Now;
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

            // Calculate the number of days until the next ocurring Monday
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;

            // Add the days to the current date to get the next Monday
            DateTime nextMonday = now.AddDays(daysUntilMonday);

            // Arrival at office will be the next occuring Monday at 0745
            DateTime arriveAtOffice = new(nextMonday.Year, nextMonday.Month, nextMonday.Day, 7, 45, 0);

            // Calculate seconds since epoch
            long secondsSinceEpoch = (long)(arriveAtOffice - epochStart).TotalSeconds;

            return secondsSinceEpoch;
        }
    }
    public string DistanceMatrixBaseUrl
    {
        get { return "https://maps.googleapis.com/maps/api/distancematrix/json"; }
    }
    public Dictionary<string, string> DistanceMatrixParameters
    {
        get
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "origins", Origin },
                { "destinations", Destination },
                { "units", Units} ,
                { "key", ApiKey },
                { "mode", Transport.ToString() },
                { "arrive", ArrivalTime.ToString() },
    };

            return parameters;
        }
    }
    public string DistanceMatrixURI
    {
        get
        {
            return BuildURL(DistanceMatrixBaseUrl, DistanceMatrixParameters);
        }
    }

    /* public Dictionary<string, string> DirectionsParameters
    {
        get
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "origin", Origin },
                { "destination", Destination },
                { "units", Units} ,
                { "key", ApiKey },
                { "mode", Transport.ToString() }
            };

            return parameters;
        }
    }

    public string DirectionsURI
    {
        get
        {
            string baseUrl = "https://maps.googleapis.com/maps/api/directions/json";
            return BuildURL(baseUrl, DirectionsParameters);
        }
    } */

    public string Origin { get; set; }
    public string Destination { get; set; }
    public ModeOfTransport Transport { get; set; }
    public string Units { get; set; }

    public MapsAPI(string apiKey, string origin, string destination, ModeOfTransport transport, string units = "metric")
    {
        ApiKey = apiKey;
        Origin = origin;
        Destination = destination;
        Transport = transport;
        Units = units;
    }

    private string BuildURL(string baseUrl, Dictionary<string, string> parameters)
    {
        // Build the URL
        UriBuilder uriBuilder = new UriBuilder(baseUrl);

        // Add query parameters
        var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var param in parameters)
        {
            query[param.Key] = param.Value;
        }
        uriBuilder.Query = query.ToString();

        // Get the final URL as a string
        string finalUrl = uriBuilder.Uri.ToString();

        return finalUrl;
    }

    public async Task<string> MakeRequestAsync(string apiURI)
    {
        string responseBody = string.Empty;

        using (HttpClient client = new())
        {
            Uri url = new Uri(apiURI);
            responseBody = await client.GetStringAsync(url);
        }

        return responseBody;
    }

    public async Task CallDistanceMatrixAPI()
    {
        string jsonString = await MakeRequestAsync(DistanceMatrixURI);

        System.Console.WriteLine(jsonString);

        // FIXME: Seems like I haven't correctly mapped the DistanceMatrixResponse class to the JSON
        DistanceMatrixResponse apiResponse = JsonSerializer.Deserialize<DistanceMatrixResponse>(jsonString);

        // Console.WriteLine($"Origin: {apiResponse.OriginAddresses[0]}, Destination: {apiResponse.DestinationAddresses[0]}");

        foreach (var row in apiResponse.Rows)
        {
            foreach (var element in row.Elements)
            {
                Console.WriteLine($"Distance: {element.Distance.Text}, Duration: {element.Duration.Text}, Status: {element.Status}");
            }
        }

        // string origin = apiResponse.OriginAddresses[0];
        // string destination = apiResponse.DestinationAddresses[0];

        // foreach (string originAddress in apiResponse.OriginAddresses)
        // { System.Console.WriteLine(originAddress); }

        // string distanceText = apiResponse.Rows[0].Elements[0].Distance.Text;
        // int distanceMetres = apiResponse.Rows[0].Elements[0].Distance.Value;

        // string durationText = apiResponse.Rows[0].Elements[0].Duration.Text;
        // int durationSeconds = apiResponse.Rows[0].Elements[0].Duration.Value;

        // // System.Console.WriteLine($"Origin: {origin}");
        // // System.Console.WriteLine($"Destination: {destination}");
        // System.Console.WriteLine($"Distance: {distanceText} ({distanceMetres} metres)");
        // System.Console.WriteLine($"Commute time: {durationText} ({durationSeconds} seconds)");

        // return responseDictionary;
    }
}