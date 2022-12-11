// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

public class Core
{
    public string SourceId { get; set; }
}

public class ExportSource
{
    public string World { get; set; }

    public string System { get; set; }

    public string CoreVersion { get; set; }

    public string SystemVersion { get; set; }
}

public class Flags
{
    public Core Core { get; set; }

    public ExportSource ExportSource { get; set; }
}

public class Result
{
    public string Id { get; set; }

    public Flags Flags { get; set; }

    public int Type { get; set; }

    public string Text { get; set; }

    public object Img { get; set; }

    public int Weight { get; set; }

    public List<int> Range { get; set; }

    public bool Drawn { get; set; }

    public string DocumentCollection { get; set; }

    public object DocumentId { get; set; }
}

public class Root
{
    public string Name { get; set; }

    public string Img { get; set; }

    public List<Result> Results { get; set; }

    public string Formula { get; set; }

    public bool Replacement { get; set; }

    public bool DisplayRoll { get; set; }

    public Flags Flags { get; set; }

    public string Description { get; set; }

    public Stats Stats { get; set; }
}

public class Stats
{
    public object SystemId { get; set; }

    public object SystemVersion { get; set; }

    public object CoreVersion { get; set; }

    public object CreatedTime { get; set; }

    public object ModifiedTime { get; set; }

    public object LastModifiedBy { get; set; }
}