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
                    UpdateTime = DateTime.Now,
                    Int16Arr = new short[] { 1, 2, 3 },
                    Int32Arr = new int[] { 1, 2, 3 },
                    Int64Arr = new long[] { 1L, 2L, 3L },
                    FloatArr = new float[] { 1.1F, 2.2F, 3.3F },
                    DoubleArr = new double[] { 1.1, 2.2, 3.3 },
                    BooleanArr = new bool[] { true, false }
                };

                session.Save(entity);
                session.Flush();
                session.Clear();

                Assert.Greater(entity.Id, 0);

                Console.WriteLine($"entity id: {entity.Id}");

                var query = session.Query<TestEntity>();
                var entities = query.ToList();
                Assert.NotNull(entities);

                foreach (var e in entities) {
                    session.Delete(e);
                }

                session.Flush();

            }
        }

    }

}
