using LiteDB;

namespace Services;

public class TablesService
{
    public void SaveTable(RollableTable table)
    {
        using (var db = new LiteDatabase(@"Tables.db"))
        {
            var collection = db.GetCollection<RollableTable>("Tables");

            if (collection.FindOne(x => x.Name == table.Name) != null)
            {
                return;
            }
            
            collection.Insert(table);
        }
    }

    public RollableTable[] GetTables()
    {
        using (var db = new LiteDatabase(@"Tables.db"))
        {
            var collection = db.GetCollection<RollableTable>("Tables");

            return collection.FindAll().ToArray();
        }
    }
    
    public TableRow RollTable(string tableName)
    {
        using (var db = new LiteDatabase(@"Tables.db"))
        {
            var collection = db.GetCollection<RollableTable>("Tables");

            return collection.FindOne(x => x.Name == tableName).Roll() ?? throw new Exception("Таблица не найдена");
        }
    }

    public void DeleteTable(string tableName)
    {
        using (var db = new LiteDatabase(@"Tables.db"))
        {
            var collection = db.GetCollection<RollableTable>("Tables");
            collection.DeleteMany(x => x.Name == tableName);
        }
    }

    public void UpdateTable(RollableTable table)
    {
        using (var db = new LiteDatabase(@"Tables.db"))
        {
            var collection = db.GetCollection<RollableTable>("Tables");
            var dbTable = collection.FindOne(x => x.Name == table.Name);
            dbTable.Rows = table.Rows;
            collection.Update(dbTable);
        }
    }
}