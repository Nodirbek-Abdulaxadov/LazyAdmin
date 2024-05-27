using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace LazyAdmin.Ants;

internal class ViewModelCreator
{
    internal static void CreateViewModels(IEntityType entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var entityName = entity.ClrType.Name;
        var properties = GetNonRelationalProperties(entity);

        CreateCreateViewModelFile($"Create{entityName}ViewModel.cs", entityName, properties);
        CreateViewModelFile($"{entityName}ViewModel.cs", entityName, properties);
    }

    private static List<IProperty> GetNonRelationalProperties(IEntityType entityType)
        => entityType.GetProperties()
                     .Where(p => IsJustType(p.ClrType)).ToList();

    private static void CreateCreateViewModelFile(string fileName, string entityName, List<IProperty> properties)
    {
        var filePath = Path.Combine("Areas/LazyAdmin/ViewModels", $"{entityName}ViewModels");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath = Path.Combine(filePath, fileName);

        var builder = new StringBuilder();
        builder.AppendLine($"namespace Areas.LazyAdmin.ViewModels.{entityName}ViewModels;");
        builder.AppendLine();
        builder.AppendLine($"public class Create{entityName}ViewModel");
        builder.AppendLine("{");

        foreach (var property in properties)
        {
            if (property.Name == "Id") continue;

            builder.AppendLine($"    public {GetCSharpType(property.ClrType)} {property.Name} {{ get; set; }}");
        }

        builder.AppendLine("}");    

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, builder.ToString());
    }

    private static void CreateViewModelFile(string fileName, string entityName, List<IProperty> properties)
    {
        var filePath = Path.Combine("Areas/LazyAdmin/ViewModels", $"{entityName}ViewModels");
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath = Path.Combine(filePath, fileName);

        var builder = new StringBuilder();
        builder.AppendLine($"namespace Areas.LazyAdmin.ViewModels.{entityName}ViewModels;");
        builder.AppendLine();
        builder.AppendLine($"public class {entityName}ViewModel : Create{entityName}ViewModel");
        builder.AppendLine("{");

        foreach (var property in properties)
        {
            if (property.Name != "Id") continue;

            builder.AppendLine($"    public {GetCSharpType(property.ClrType)} {property.Name} {{ get; set; }}");
        }

        builder.AppendLine("}");

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        File.WriteAllText(filePath, builder.ToString());
    }

    private static string GetCSharpType(Type type)
    {
        if (type == typeof(int))
            return "int";
        if (type == typeof(long))
            return "long";
        if (type == typeof(short))
            return "short";
        if (type == typeof(byte))
            return "byte";
        if (type == typeof(bool))
            return "bool";
        if (type == typeof(char))
            return "char";
        if (type == typeof(decimal))
            return "decimal";
        if (type == typeof(double))
            return "double";
        if (type == typeof(float))
            return "float";
        if (type == typeof(string))
            return "string";
        if (type == typeof(DateTime))
            return "DateTime";
        if (type == typeof(Guid))
            return "Guid";

        // Handle nullable types
        if (Nullable.GetUnderlyingType(type) != null)
        {
            return $"{GetCSharpType(Nullable.GetUnderlyingType(type)!)}?";
        }

        return type.Name; // Fallback to the CLR type name if not matched
    }

    private static bool IsJustType(Type type)
        =>  (type == typeof(byte)
        || type == typeof(sbyte)
        || type == typeof(short)
        || type == typeof(ushort)
        || type == typeof(int)
        || type == typeof(uint)
        || type == typeof(long)
        || type == typeof(ulong)
        || type == typeof(float)
        || type == typeof(double)
        || type == typeof(bool)
        || type == typeof(char)
        || type == typeof(decimal)
        || type == typeof(Enum)
        || type == typeof(Guid)
        || type == typeof(string)
        || type == typeof(DateTime)
        || type == typeof(DateOnly)
        || type == typeof(TimeOnly));
}