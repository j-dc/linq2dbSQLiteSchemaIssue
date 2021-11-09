using LinqToDB;
using LinqToDB.Data;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace linq2dbsqliteschemaissue {
    [TestClass]
    public class CreateTest {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            //always use a new clean database in the TestDir..
            DataConnection.DefaultSettings = new MockDBSettings(context.TestDir);
        }




        [TestMethod]
        public void CreateTableMS() {
            CreateTable("LocalStorage.MS");
        }

        [TestMethod]
        public void CreateTableClassic() {
            CreateTable("LocalStorage.Classic");
        }

        private void CreateTable(string connectionName) {
            using (var db = new LocalDB(connectionName)) {
                //Fetching the schema
                var sp = db.DataProvider.GetSchemaProvider();
                var dbSchema = sp.GetSchema(db);

                //checing if a table with name TestTable exist
                if (!dbSchema.Tables.Any(t => t.TableName == "TestTable")) {
                    //create a table named testtable
                    db.CreateTable<TestTable>();
                }

                //get the newly create table via sqlite_schema
                var dr = db.ExecuteReader("SELECT name FROM sqlite_schema WHERE type = 'table' AND name NOT LIKE 'sqlite_%'; ");
                var list = new List<string>();
                while (dr.Reader is not null && dr.Reader.Read()) {
                    list.Add(dr.Reader.GetString(0));
                }

                //bail out if the table does not exist
                Assert.IsTrue(list.Any(t => t == "TestTable"), "DB should contain a TestTable");

                //Here we insert and read to be really sure the table exists
                //insert data in testtable
                db.Insert(new TestTable() { Name = "Test" });

                //read data from testtable
                Assert.IsTrue(db.Messages.Any(), "Table should contain data");
            }

            //Now we are really really sure the new table exists
            //Just to be sure..  we start a new instance

            using (var db = new LocalDB(connectionName)) {

                //Get the schema
                var sp = db.DataProvider.GetSchemaProvider();
                var dbSchema = sp.GetSchema(db);

                //The schema is not updated!!!
                Assert.IsTrue(dbSchema.Tables.Any(t => t.TableName == "TestTable"), "Schema should contain TestTable");
            }
        }
    }
}