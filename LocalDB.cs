using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System;

namespace linq2dbsqliteschemaissue;
public partial class LocalDB : DataConnection {

    public ITable<TestTable> Messages { get { return this.GetTable<TestTable>(); } }

    public LocalDB(string config) : base(config) {
        InitDataContext();
        InitMappingSchema();
    }

    partial void InitDataContext();
    partial void InitMappingSchema();
}


[Table("TestTable")]
public partial class TestTable {
    [Column("ID"), PrimaryKey, Identity] public int Id { get; set; }
    [Column("Name"), Nullable] public string? Name { get; set; }
    
}


