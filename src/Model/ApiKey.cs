using System.Text.Json;

/// <summary>
/// Represents a data structure for storing an API key.
/// </summary>
public class ApiKeyData
{
    public string apiKey { get; set; }
}

/// <summary>
/// Represents a utility class for retrieving an API key from a JSON file.
/// </summary>
public class ApiKey
{
    private string RelativeDirectoryPath { get; set; }

    private string JsonDirectory { get; set; }

    private string JsonFileName { get; set; }

    public string JsonFilePath
    {
        get
        {
            string relativeFilePath = Path.Combine(JsonDirectory, JsonFileName);
            return Path.GetFullPath(relativeFilePath);
        }
    }

    public string Key
    {
        get
        {
            string jsonContent = File.ReadAllText(JsonFilePath);
            ApiKeyData jsonData = JsonSerializer.Deserialize<ApiKeyData>(jsonContent);
            string apiKeyValue = jsonData.apiKey;

            return apiKeyValue;
        }
    }

    public ApiKey(string relativeDirectoryPath = ".", string jsonDirectory = "JSON", string jsonFileName = "ApiKey.json")
    {
        RelativeDirectoryPath = relativeDirectoryPath;
        JsonDirectory = jsonDirectory;
        JsonFileName = jsonFileName;
    }
}