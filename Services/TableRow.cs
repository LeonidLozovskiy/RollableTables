namespace Services;

public class TableRow
{
    public string Value { get; set; }

    public int Weight { get; set; } = 1;

    public override string ToString()
    {
        return Value;
    }
}