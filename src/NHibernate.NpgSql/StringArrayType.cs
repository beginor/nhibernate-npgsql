using System;
using System.Data;
using Newtonsoft.Json;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Npgsql;
using NpgsqlTypes;

namespace Beginor.NHibernate.NpgSql {

    public class StringArrayType : IUserType {

        public SqlType[] SqlTypes => new SqlType[] {
            new NpgSqlType(DbType.Object, NpgsqlDbType.Array | NpgsqlDbType.Text)
        };

        public Type ReturnedType => typeof(string[]);

        public bool IsMutable => true;

        public object Assemble(object cached, object owner) {
            return cached;
        }

        public object DeepCopy(object value) {
            if (value == null) {
                return null;
            }
            var list = value as string[];
            var json = JsonConvert.SerializeObject(list);
            var result = JsonConvert.DeserializeObject<string[]>(json);
            return result;
        }

        public object Disassemble(object value) {
            return value;
        }

        public new bool Equals(object x, object y) {
            if (x == null && y == null) {
                return true;
            }
            if (x == null || y == null) {
                return false;
            }
            return ((string[])x).Equals((string[])y);
        }

        public int GetHashCode(object x) {
            if (x == null) {
                return 0;
            }
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner) {
            if (names.Length != 1) {
                throw new InvalidOperationException("Only expecting one column...");
            }
            return rs[names[0]] as string[];
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index) {
            var parameter = ((NpgsqlParameter)cmd.Parameters[index]);
            if (value == null) {
                parameter.Value = DBNull.Value;
            }
            else {
                var arr = value as string[];
                if (arr == null) {
                    throw new InvalidOperationException(
                        $"\"{parameter.ParameterName}\" is not string[]"
                    );
                }
                parameter.Value = arr;
            }
        }

        public object Replace(object original, object target, object owner) {
            return original;
        }

    }

}
