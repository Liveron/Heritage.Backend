using System.Dynamic;
using System.Reflection;

namespace Heritage.Application.DataShaping;

public class DataShaper<T> : IDataShaper<T> where T : class
{
    public PropertyInfo[] Properties { get; set; }

    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString)
    {
        List<PropertyInfo> requiredProperties = GetRequiredProperties(fieldsString);
        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject ShapeData(T entity, string? fieldsString)
    {
        List<PropertyInfo> requiredProperties = GetRequiredProperties(fieldsString);
        return FetchDataForEntity(entity, requiredProperties);
    }

    private List<PropertyInfo> GetRequiredProperties(string? fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();

        if (!string.IsNullOrWhiteSpace(fieldsString))
        {
            var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (string field in fields)
            {
                PropertyInfo? property = Properties.FirstOrDefault(property =>
                    property.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));
                
                if (property != null)
                    requiredProperties.Add(property);
            }
        }
        else requiredProperties = [.. Properties];

        return requiredProperties;
    }

    private static List<ExpandoObject> FetchData(
        IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ExpandoObject>();

        foreach (T entity in entities)
        {
            ExpandoObject shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }

        return shapedData;
    }

    private static ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedObject = new ExpandoObject();

        foreach (PropertyInfo property in requiredProperties)
        {
            object? propertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, propertyValue);
        }

        return shapedObject;
    }
}
