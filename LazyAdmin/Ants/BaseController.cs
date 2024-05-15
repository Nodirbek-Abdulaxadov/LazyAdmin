using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;

namespace LazyAdmin.Ants;

public class BaseController<TDbContext, TModel>
    : Controller where TModel
        : class where TDbContext
        : DbContext
{
    private readonly TDbContext _context;
    private DbSet<TModel> _modelSet;

    public BaseController(TDbContext context)
    {
        _context = context;
        _modelSet = _context.Set<TModel>();
    }

    public IActionResult Index()
    {
        var list = _modelSet.ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(TModel model)
    {
        _modelSet.Add(model);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}

internal class ControllerCreator
{
    internal static string GetControllerContent(IEntityType entity, DbContext dbContext)
    {
        var controllerClass = new StringBuilder();
        controllerClass.AppendLine("using Microsoft.AspNetCore.Mvc;");
        controllerClass.AppendLine($"using {dbContext!.GetType().Namespace};");
        controllerClass.AppendLine($"using {entity.ClrType.Namespace};");
        controllerClass.AppendLine("using LazyAdmin.Ants;");
        controllerClass.AppendLine();
        controllerClass.AppendLine("namespace AdminLab.Areas.LazyCat.Controllers;");
        controllerClass.AppendLine("[Area(\"LazyAdmin\")]");
        controllerClass.AppendLine($"public class {entity.DisplayName()}Controller : BaseController<{dbContext!.GetType().Name}, {entity.DisplayName()}>");
        controllerClass.AppendLine("{");
        controllerClass.AppendLine($"   public {entity.DisplayName()}Controller({dbContext.GetType().Name} context) : base(context)");
        controllerClass.AppendLine("    { }");
        controllerClass.AppendLine("}");

        return controllerClass.ToString();
    }
}