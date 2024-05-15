var isCollapsed = false;

function toggleButton() {
    var sidebar = document.getElementById("sidebar");
    var main = document.getElementById("main");
    if (isCollapsed) {
        var links = document.getElementsByClassName("nav-link-text")
        for (var i = 0; i < links.length; i++) {
            links[i].style.display = "inline";
        }
        sidebar.style.width = "275px";
        main.style.marginLeft = "275px";
        isCollapsed = false;
    } else {
        sidebar.style.width = "70px";
        main.style.marginLeft = "70px";
        var links = document.getElementsByClassName("nav-link-text")
        for (var i = 0; i < links.length; i++) {
            links[i].style.display = "none";
        }
        isCollapsed = true;
    }
}