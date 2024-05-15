using Microsoft.AspNetCore.Mvc;
using LazyAdmin.Data;
using LazyAdmin.Models;
using LazyAdmin.Ants;

namespace AdminLab.Areas.LazyCat.Controllers;
[Area("LazyAdmin")]
public class TestController : BaseController<AppDbContext, Test>
{
   public TestController(AppDbContext context) : base(context)
    { }
}
