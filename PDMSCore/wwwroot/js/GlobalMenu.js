
function OnGlobalMenuItemClick(target, TypeOfNavItem, URL, data) {
    event.stopPropagation();

    console.log("O nGlobalMenuItemClick: URL = " + URL);

    if (URL !== undefined && URL != "")
        window.location.href = "/" + URL;


    return;
}

