using Microsoft.AspNetCore.Mvc;
using LazyAdmin.Data;
using LazyAdmin.Models;
using LazyAdmin.Ants;

namespace AdminLab.Areas.LazyCat.Controllers;
[Area("LazyAdmin")]
public class BookController : BaseController<AppDbContext, Book>
{
   public BookController(AppDbContext context) : base(context)
    { }
}
