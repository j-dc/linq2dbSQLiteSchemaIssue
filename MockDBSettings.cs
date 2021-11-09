using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace linq2dbsqliteschemaissue;

public class MockDBSettings : ILinqToDBSettings {
    private readonly string RootPath;
    public MockDBSettings(string rootPath) {
        RootPath = rootPath;
    }

    public IEnumerable<IDataProviderSettings> DataProviders
        => Enumerable.Empty<IDataProviderSettings>();

    public string DefaultConfiguration => "Localstorage";
    public string DefaultDataProvider => "Sqlite.MS";
     

    public IEnumerable<IConnectionStringSettings> ConnectionStrings {
        get {
            yield return new ConnectionStringSettings {
                Name = "LocalStorage.MS",
                ProviderName = "SQLite.MS",
                ConnectionString = $@"Data Source={RootPath}\mydb.db"
            };

            yield return new ConnectionStringSettings {
                Name = "LocalStorage.Classic",
                ProviderName = "SQLite.Classic",
                ConnectionString = $@"Data Source={RootPath}\mydb2.db"
            };
        }
    }
}

public class ConnectionStringSettings : IConnectionStringSettings {
    public string ConnectionString { get; set; }
    public string Name { get; set; }
    public string ProviderName { get; set; }
    public bool IsGlobal => false;
}
