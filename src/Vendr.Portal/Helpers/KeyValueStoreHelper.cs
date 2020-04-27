using System;
using System.Linq;
using Umbraco.Core.Composing;

namespace Vendr.Portal.Helpers
{
    internal static class KeyValueStoreHelper
    {
        public static void AddOrUpdate(string key, string value)
        {
            using (var scope = Current.ScopeProvider.CreateScope())
            {
                scope.Database.Execute("DELETE FROM umbracoKeyValue WHERE [key] = @key", new { key });
                scope.Database.Execute("INSERT INTO umbracoKeyValue VALUES (@key, @value, @date)", new { key, value, date = DateTime.UtcNow });
                scope.Complete();
            }
        }

        public static string Get(string key)
        {
            string value = null;
            using (var scope = Current.ScopeProvider.CreateScope())
            {
                value = scope.Database.Fetch<string>("SELECT [value] FROM umbracoKeyValue WHERE [key] = @key", new { key }).FirstOrDefault();
                scope.Complete();
            }
            return value;
        }

        public static string GetAndDelete(string key)
        {
            string value = null;
            using (var scope = Current.ScopeProvider.CreateScope())
            {
                value = scope.Database.Fetch<string>("SELECT [value] FROM umbracoKeyValue WHERE [key] = @key", new { key }).FirstOrDefault();
                Delete(key);
                scope.Complete();
            }
            return value;
        }

        public static int Delete(string key)
        {
            var rowsAffected = 0;
            using (var scope = Current.ScopeProvider.CreateScope())
            {
                rowsAffected = scope.Database.Execute("DELETE FROM umbracoKeyValue WHERE [key] = @key", new { key });
                scope.Complete();
            }
            return rowsAffected;
        }

        public static bool ValidateAndDelete(string key, string value)
        {
            var storedValue = Get(key);
            if (storedValue == value)
            {
                Delete(key);
                return true;
            }
            
            return false;
        }
    }
}