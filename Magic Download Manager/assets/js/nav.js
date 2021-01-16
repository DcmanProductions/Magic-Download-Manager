const nav = $('#nav');
loadNavigationMachanics();
LoadView();
setNavigationToggle();

function ToggleNavigation() {
    const collapsed = nav.hasClass('navigation-collapsed');
    setNavigationToggle(collapsed);
}

function setNavigationToggle(collapsed = false) {
    nav.addClass(collapsed ? "navigation-expanded" : "navigation-collapsed")
    nav.removeClass(collapsed ? "navigation-collapsed" : "navigation-expanded")
}

function LoadView(url = 'settings', called = null) {
    const view = $('#main');
    url = url.toLowerCase();
    view.empty();
    view.load(`pages/${url}.html`);

    if (called != null) {
        Array.from(document.getElementsByClassName('menuItem')).forEach(s => { s.classList.remove('active') })
        called.classList.add('active')
    }

    setNavigationToggle();
}


function loadNavigationMachanics() {
    var timer;
    nav.on({
        mouseenter: function () {
            timer = setTimeout(() => {
                setNavigationToggle(true);
            }, 1500);
        },
        mouseleave: function () {
            setNavigationToggle(false);
            clearTimeout(timer);
        }
    });
}