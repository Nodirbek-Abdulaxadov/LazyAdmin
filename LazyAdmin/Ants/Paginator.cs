namespace LazyAdmin.Ants;

public static class Paginator
{
    public static string GetPartialView<T>(this PageModel<T> model, string controllerName, string actionName, string areaName = "", string routeParameterName = "pageNumber")
    {
        if (model.TotalPages <= 1)
        {
            return string.Empty;
        }

        string GetPageLink(int pageNumber, bool isActive = false, bool isDisabled = false)
        {
            var activeClass = isActive ? " active" : "";
            var disabledClass = isDisabled ? " disabled" : "";
            return $@"
                <li class='page-item{activeClass}{disabledClass}'>
                    <a class='page-link' href='/{areaName}/{controllerName}/{actionName}?{routeParameterName}={pageNumber}'>{pageNumber}</a>
                </li>";
        }

        string GetNavLink(int pageNumber, bool isPrevious = true)
        {
            var direction = isPrevious ? "&laquo;" : "&raquo;";
            var disabledClass = (isPrevious && model.PageNumber == 1) || (!isPrevious && model.PageNumber == model.TotalPages) ? " disabled" : "";
            return $@"
                <li class='page-item{disabledClass}'>
                    <a class='page-link' href='/{areaName}/{controllerName}/{actionName}?{routeParameterName}={pageNumber}'>
                        <span aria-hidden='true'>{direction}</span>
                    </a>
                </li>";
        }

        var pages = "";
        if (model.TotalPages < 6)
        {
            for (int i = 1; i <= model.TotalPages; i++)
            {
                pages += GetPageLink(i, model.PageNumber == i);
            }
        }
        else if (model.PageNumber < 3)
        {
            pages += GetNavLink(model.PageNumber - 1);
            for (int i = 1; i <= 3; i++)
            {
                pages += GetPageLink(i, model.PageNumber == i);
            }
            pages += @"
                <li class='page-item disabled'><a class='page-link'>...</a></li>";
            pages += GetPageLink(model.TotalPages);
            pages += GetNavLink(model.PageNumber + 1, false);
        }
        else if (model.TotalPages - 2 >= model.PageNumber)
        {
            pages += GetNavLink(model.PageNumber - 1);
            pages += GetPageLink(1);
            pages += @"
                <li class='page-item disabled'><a class='page-link'>...</a></li>";

            for (int i = model.PageNumber - 1; i <= model.PageNumber + 1; i++)
            {
                pages += GetPageLink(i, model.PageNumber == i);
            }

            pages += @"
                <li class='page-item disabled'><a class='page-link'>...</a></li>";
            pages += GetPageLink(model.TotalPages);
            pages += GetNavLink(model.PageNumber + 1, false);
        }
        else
        {
            pages += GetNavLink(model.PageNumber - 1);
            pages += GetPageLink(1);
            pages += @"
                <li class='page-item disabled'><a class='page-link'>...</a></li>";

            for (int i = model.TotalPages - 2; i <= model.TotalPages; i++)
            {
                pages += GetPageLink(i, model.PageNumber == i);
            }
        }

        var script = $@"
            var pageNumber = {model.PageNumber};
            var Items = document.getElementsByClassName('page-item');
            for (var i = 0; i < Items.length; i++) {{
                if (Items[i].children[0].innerText == pageNumber) {{
                    Items[i].classList.add('active');
                }}
            }}
            var aTags = document.getElementsByTagName('a');
            var rootUrl = window.location.origin;
            for (var i = 0; i < aTags.length; i++) {{
                var href = aTags[i].getAttribute('href');
                if (href != null && href.startsWith('/')) {{
                    aTags[i].setAttribute('href', rootUrl + href);
                }}
            }}";

        return $@"
            <div class='d-flex justify-content-center'>
                <nav aria-label='Page navigation example'>
                    <ul class='pagination'>
                        {pages}
                    </ul>
                </nav>
            </div>
            <script>{script}</script>";
    }
}

public class PageModel<T>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }

    public int TotalItemsCount { get; set; }

    public List<T> Items { get; set; } = new List<T>();


    public PageModel(List<T> items, int pageNumber, int pageSize = 10)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalItemsCount = items.Count;
        TotalPages = (int)Math.Ceiling((double)TotalItemsCount / (double)pageSize);
        Items = items.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
    }
}