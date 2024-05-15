using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace LazyAdmin.Ants;

internal static class Views
{
    internal static string Index(IEntityType entity)
    {
        var entityName = entity.DisplayName();
        var entityNamespace = entity.ClrType.Namespace;
        string indexView = $$"""
        @model List<{{entityName}}>
        <div class="d-flex justify-content-between align-items-center">
            <h2>{{entityName}} table</h2>
            <a asp-action="Create" class="btn btn-success">
                Create new {{entityName}}
            </a>
        </div>
        <table class="table table-striped">
            <thead>
                <tr>
                    {{GetProperties(entity)}}
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        {{GetPropertyValues(entity)}}
                        <td>
                            <a asp-action="Edit" asp-route-id="@item.Id" class="btn btn-primary">Edit</a>
                            <a asp-action="Details" asp-route-id="@item.Id" class="btn btn-info">Details</a>
                            <a asp-action="Delete" asp-route-id="@item.Id" class="btn btn-danger">Delete</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
        """;
        return indexView;
    }

    private static string GetProperties(IEntityType entity)
    {
        var properties = new StringBuilder();
        foreach (var property in entity.GetProperties())
        {
            properties.AppendLine($"                <th>{property.Name}</th>");
        }
        return properties.ToString();
    }

    private static string GetPropertyValues(IEntityType entity)
    {
        var propertyValues = new StringBuilder();
        foreach (var property in entity.GetProperties())
        {
            propertyValues.AppendLine($"                <td>@item.{property.Name}</td>");
        }
        return propertyValues.ToString();
    }
}