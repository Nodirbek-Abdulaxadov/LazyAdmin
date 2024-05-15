using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace LazyAdmin.Ants;

internal class Generator
{
    internal Generator()
    {
        EnsureLazyAreaCreated();
    }

    #region Generate Lazy Admin
    internal void GenerateLazyAdmin(List<IEntityType> entities, DbContext dbContext)
    {
        CreateControllers(entities, dbContext);
        CreateLayout(entities);
        CreateImports(entities.Select(x => x.ClrType.Namespace).ToList()!);
        CreateViewStart();
        CreateViews(entities, ViewType.Index);
    }
    #endregion

    #region Default

    private static void EnsureLazyAreaCreated()
    {
        if (!Directory.Exists("Areas"))
        {
            Directory.CreateDirectory("Areas");
        }

        if (!Directory.Exists("Areas/LazyAdmin"))
        {
            Directory.CreateDirectory("Areas/LazyAdmin");
        }

        if (!Directory.Exists("Areas/LazyAdmin/Controllers"))
        {
            Directory.CreateDirectory("Areas/LazyAdmin/Controllers");
        }

        if (!Directory.Exists("Areas/LazyAdmin/Views"))
        {
            Directory.CreateDirectory("Areas/LazyAdmin/Views");
        }

        if (!Directory.Exists("Areas/LazyAdmin/Views/Shared"))
        {
            Directory.CreateDirectory("Areas/LazyAdmin/Views/Shared");
        }
    }

    private static void CreateControllers(List<IEntityType> entities, DbContext dbContext)
    {
        foreach (var entity in entities)
        {
            var content = ControllerCreator.GetControllerContent(entity, dbContext);
            var controllerPath = Path.Combine("Areas", "LazyAdmin", "Controllers", $"{entity.DisplayName()}Controller.cs");
            FileCreate(controllerPath, content);
        }
    }

    private static void CreateViews(List<IEntityType> entities, ViewType viewType)
    {
        foreach (var entity in entities)
        {
            if (!Directory.Exists($"Areas/LazyAdmin/Views/{entity.DisplayName()}"))
            {
                Directory.CreateDirectory($"Areas/LazyAdmin/Views/{entity.DisplayName()}");
            }

            var content = viewType switch
            {
                ViewType.Index => Views.Index(entity),
                /*ViewType.Create => Views.Create(entity),
                ViewType.Edit => Views.Edit(entity),
                ViewType.Details => Views.Details(entity),*/
                _ => Views.Index(entity)
            };

            var viewPath = Path.Combine("Areas", "LazyAdmin", "Views", entity.DisplayName(), $"{viewType}.cshtml");
            FileCreate(viewPath, content);
        }
    }

    private static void CreateLayout(List<IEntityType> entities)
    {
        string filePath = Path.Combine("Areas", "LazyAdmin", "Views", "Shared", "_Layout.cshtml");
        string content = Layouts.Layout(entities.Select(x => x.DisplayName()).ToList());
        FileCreate(filePath, content);
    }

    private static void CreateImports(List<string> namespaces)
    {
        string filePath = Path.Combine("Areas", "LazyAdmin", "Views", "_ViewImports.cshtml");
        StringBuilder viewImports = new StringBuilder();
        foreach (var ns in namespaces)
        {
            viewImports.AppendLine($"@using {ns}");
        }
        FileCreate(filePath, viewImports.ToString());
    }

    private static void CreateViewStart()
    {
        string filePath = Path.Combine("Areas", "LazyAdmin", "Views", "_ViewStart.cshtml");
        string content = Layouts.ViewStart;
        FileCreate(filePath, content);
    }

    private static void FileCreate(string path, string content)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, content);
        }
    }
    #endregion
}