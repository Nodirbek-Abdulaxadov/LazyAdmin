using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace LazyAdmin.Ants;

internal static class ViewHelpers
{
    internal static string GetProperties(IEntityType entity)
    {
        var properties = new StringBuilder();
        foreach (var property in entity.GetProperties())
        {
            properties.AppendLine($"            <th>{property.Name}</th>");
        }
        return properties.ToString();
    }

    internal static string GetPropertyValues(IEntityType entity)
    {
        var propertyValues = new StringBuilder();
        foreach (var property in entity.GetProperties())
        {
            propertyValues.AppendLine($"                <td>@item.{property.Name}</td>");
        }
        return propertyValues.ToString();
    }

    internal static string GetFormFields(IEntityType entity)
    {
        var formFields = new StringBuilder();
        foreach (var property in entity.GetProperties())
        {
            if (property.Name == "Id" || property.Name == "CreatedAt" || property.Name == "UpdatedAt")
            {
                continue;
            }

            if (IsEnumType(property))
            {
                formFields.AppendLine($"<div class=\"form-group mb-3\">");
                formFields.AppendLine($"    <label asp-for=\"{property.Name}\" class=\"control-label\"></label>");
                formFields.AppendLine($"    {ViewElements.EnumSelect(property)}");
                formFields.AppendLine($"    <span asp-validation-for=\"{property.Name}\" class=\"text-danger\"></span>");
                formFields.AppendLine($"</div>");
            }

            if (IsCollectionType(property))
            {
                formFields.AppendLine($"<div class=\"form-group mb-3\">");
                formFields.AppendLine($"    <label asp-for=\"{property.Name}\" class=\"control-label\"></label>");
                formFields.AppendLine($"    {ViewElements.ListSelect(property)}");
                formFields.AppendLine($"    <span asp-validation-for=\"{property.Name}\" class=\"text-danger\"></span>");
                formFields.AppendLine($"</div>");
            }

            formFields.AppendLine($"<div class=\"form-group mb-3\">");
            formFields.AppendLine($"    <label asp-for=\"{property.Name}\" class=\"control-label\"></label>");
            formFields.AppendLine($"    {ViewElements.Input(property)}");
            formFields.AppendLine($"    <span asp-validation-for=\"{property.Name}\" class=\"text-danger\"></span>");
            formFields.AppendLine($"</div>");
        }
        return formFields.ToString();
    }

    internal static bool IsNumberType(IProperty property)
    {
        return property.ClrType == typeof(int) || property.ClrType == typeof(decimal) || 
            property.ClrType == typeof(double) || property.ClrType == typeof(float) ||
            property.ClrType == typeof(long) || property.ClrType == typeof(short) ||
            property.ClrType == typeof(byte) || property.ClrType == typeof(sbyte) ||
            property.ClrType == typeof(uint) || property.ClrType == typeof(ulong) ||
            property.ClrType == typeof(ushort);
    }

    internal static bool IsEnumType(IProperty property)
    {
        return property.ClrType.IsEnum;
    }

    internal static bool IsCollectionType(IProperty property)
    {
        return property.ClrType.IsGenericType && property.ClrType.GetGenericTypeDefinition() == typeof(ICollection<>);
    }

    internal static bool IsStringType(IProperty property)
    {
        return property.ClrType == typeof(string);
    }

    internal static string GetName(string name)
    {
        if (name.EndsWith("y"))
        {
            return name.Remove(name.Length - 1) + "ies";
        }
        else if (name.EndsWith("s"))
        {
            return name + "es";
        }
        else
        {
            return name + "s";
        }
    }
}

internal static class ViewElements
{
    internal static string Input(IProperty property)
    {
        if (ViewHelpers.IsNumberType(property))
        {
            return $"<input asp-for=\"{property.Name}\" class=\"form-control\" type=\"number\" />";
        }
        else if (ViewHelpers.IsStringType(property))
        {
            return $"<input asp-for=\"{property.Name}\" class=\"form-control\" />";
        }

        return string.Empty;
    }

    internal static string EnumSelect(IProperty property)
    {
        var select = new StringBuilder();
        select.AppendLine($"<select asp-for=\"{property.Name}\" class=\"form-control\">");
        select.AppendLine($"    @foreach (var item in Enum.GetValues(typeof({property.ClrType.Name})))");
        select.AppendLine("    {");
        select.AppendLine("        <option value=\"@item\">@item</option>");
        select.AppendLine("    }");
        select.AppendLine("</select>");
        return select.ToString();
    }

    internal static string ListSelect(IProperty property)
    {
        return "List select"; // TODO: Implement this
    }
}