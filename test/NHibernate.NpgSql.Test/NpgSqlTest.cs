using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using NUnit.Framework;
using Beginor.NHibernate.NpgSql.Test.Data;

namespace Beginor.NHibernate.NpgSql.Test {

    [TestFixture]
    public class NpgSqlTest {

        private ISessionFactory factory;

        [OneTimeSetUp]
        public void InitializeDatabase() {
            var configuration = new Configuration();
            var configFile = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "hibernate.config"
            );
            configuration.Configure(configFile);
            factory = configuration.BuildSessionFactory();
        }

        [Test]
        public void CanDoCrud() {
            using (var session = factory.OpenSession()) {
                var entity = new TestEntity {
                    Name = "Test 1",
                    Tags = new string[] { "hello", "world" },
                    JsonField = "{ \"val\": 1 }",
                    JsonbField = "{ \"val\": 1 }",
                    UpdateTime = DateTime.Now
                };

                session.Save(entity);
                session.Flush();
                session.Clear();

                Assert.Greater(entity.Id, 0);

                Console.WriteLine($"entity id: {entity.Id}");

                var query = session.Query<TestEntity>();
                var entities = query.ToList();
                Assert.NotNull(entities);

                session.Delete(entity);
            }
        }

    }

}
