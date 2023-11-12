using System;
using System.Collections.Generic;
using System.Text.Json;

public class Distance
{
    public string Text { get; set; }
    public int Value { get; set; }
}

public class Duration
{
    public string Text { get; set; }
    public int Value { get; set; }
}

public class Element
{
    public Distance Distance { get; set; }
    public Duration Duration { get; set; }
    public string Status { get; set; }
}

public class Row
{
    public List<Element> Elements { get; set; }
}

public class DistanceMatrixResponse
{
    public List<string> DestinationAddresses { get; set; }
    public List<string> OriginAddresses { get; set; }
    public List<Row> Rows { get; set; }
    public string Status { get; set; }
}