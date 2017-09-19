using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Xrm.Pcc.DataLayer.EntityFramework
{
    public static class ModelBuilderExtensions
    {
        public static void SetUndescoreSnakeConventions(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var name = entity.DisplayName();
                entity.Relational().TableName = SnakeCase(Cleanup(name));
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = SnakeCase(Cleanup(property.Name));
                }
                foreach (var mutableKey in entity.GetKeys())
                {
                    var keyName = Cleanup(mutableKey.Relational().Name).Replace("PK_", "pk_").Replace("AK_", "ak_");
                    mutableKey.Relational().Name = SnakeCase(keyName);
                }
                foreach (var mutableForeignKey in entity.GetForeignKeys())
                {
                    var keyName = Cleanup(mutableForeignKey.Relational().Name).Replace("FK_", "fk_");
                    mutableForeignKey.Relational().Name = SnakeCase(keyName);
                }
                foreach (var mutableIndex in entity.GetIndexes())
                {
                    var indexName = mutableIndex.Relational().Name.Replace("IX_", "ix_");
                    mutableIndex.Relational().Name = indexName;
                }
            }
        }
        
        private static string SnakeCase(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length == 1)
            {
                return name?.ToLower();
            }

            return string.Concat(
                name.Select(
                    (x, i) =>
                        i > 0 && i < name.Length-1 && char.IsUpper(x) && (char.IsLower(name[i - 1]) || char.IsLower(name[i + 1]))
                            ? $"_{x.ToString().ToLower()}"
                            : x.ToString().ToLower()));
        }

        private static string Cleanup(string name)
        {
            return name.Replace("Db", string.Empty).Replace("Entity", string.Empty);
        }
    }
}