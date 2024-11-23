using System.Dynamic;

namespace Heritage.Application.DataShaping;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString);
    ExpandoObject ShapeData(T entity, string? fieldsString);
}
