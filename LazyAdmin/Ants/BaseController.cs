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

    public IActionResult Index(int pageNumber = 1)
    {
        var list = _modelSet.ToList();
        var pagedList = new PageModel<TModel>(list, pageNumber);
        return View(pagedList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(TModel model)
    {
        if (ModelState.IsValid)
        {
            _modelSet.Add(model);
            _context.SaveChanges();
            return Redirect($"/lazyadmin/{typeof(TModel).Name.ToLower()}/index");
        }

        return View(model);
    }

    public IActionResult Delete(long id)
    {
        var model = _modelSet.Find(id);
        if (model == null)
        {
            return NotFound();
        }

        _modelSet.Remove(model);
        _context.SaveChanges();
        return Redirect($"/lazyadmin/{typeof(TModel).Name.ToLower()}/index");
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