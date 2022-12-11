using System.Net.Cache;

namespace Services;

public class RollableTable
{
    public RollableTable(string name, TableRow[] rows = null)
    {
        Name = name;
        Rows = new List<TableRow>();

        if (rows != null)
        {
            Rows.AddRange(rows);
        }
    }

    public RollableTable()
    {
    }

    public string Name { get; set; }

    public List<TableRow> Rows { get; set; }

    private LinkedList<TableRow>? RollEnumerable { get; set; }

    public TableRow Roll()
    {
        if (RollEnumerable == null)
        {
            CalcRollEnumerable();
        }
        
        return RollEnumerable.ElementAt(Rnd.Random.Next(RollEnumerable.Count));
    }

    private void CalcRollEnumerable()
    {
        RollEnumerable = new LinkedList<TableRow>();
        foreach (var row in Rows)
        {
            for (int i = 0; i < row.Weight; i++)
            {
                RollEnumerable.AddLast(row);
            }
        }
    }
}