using System.Reflection;

namespace ConcordiaAppTestLayer.PersistentLayerTests;

public static class ObjectExtensions
{
    public static bool VerifyAllPropertiesNotNull(this object obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            if (value == null || value == default) return false;
        }
        return true;
    }
}
