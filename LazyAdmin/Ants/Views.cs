using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Text;

namespace LazyAdmin.Ants;

internal static class Views
{
    internal static string Index(IEntityType entity)
    {
        var entityName = entity.DisplayName();
        string indexView = $$"""
        @using LazyAdmin.Ants
        @model PageModel<{{entityName}}>
        <div class="d-flex justify-content-between align-items-center mb-2">
            <div style="width: 25%;">
                <h2>{{ViewHelpers.GetName(entityName)}} table</h2>
                <hr />
            </div>
            <a href="/lazyadmin/{{entityName.ToLower()}}/create"  class="btn btn-success">
                Create new {{entityName}}
            </a>
        </div>
        <table class="table">
            <thead>
                <tr>
        {{ViewHelpers.GetProperties(entity)}}
                    <th style="width: 160px;">Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model.Items)
                {
                    <tr>
        {{ViewHelpers.GetPropertyValues(entity)}}
                        <td>
                            <a href="/lazyadmin/{{entityName.ToLower()}}/edit/@item.Id" class="btn btn-primary">Edit</a>
                            <a href="/lazyadmin/{{entityName.ToLower()}}/delete/@item.Id" onclick="return confirm('Are sure delete this item?')" class="btn btn-danger">Delete</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
        @Html.Raw(Paginator.GetPartialView<{{entityName}}>(Model, "{{entityName.ToLower()}}", "index", "lazyadmin"))
        """;
        return indexView;
    }

    internal static string Create(IEntityType entity)
    {
        var entityName = entity.DisplayName();
        string createView = $$"""
        @model {{entityName}}
        <h2>Create new {{entityName}}</h2>
        <hr />
        <form class="d-flex justify-content-center" href="/lazyadmin/{{entityName.ToLower()}}/create" 
              method="post" enctype="multipart/form-data">
            <div class="form-group mt-3 form">
                <div class="form-group">
                    {{ViewHelpers.GetFormFields(entity)}}
                </div>
                <div class="d-flex justify-content-center">
                    <a href="/lazyadmin/{{entityName.ToLower()}}/index" class="btn btn-secondary mt-3 me-2">Cancel</a>
                    <button type="submit" class="btn btn-success mt-3 ms-2">Create</button>
                </div>
            </div>
        </form>
        """;
        return createView;
    }

    
}