
function OnGlobalMenuItemClick(target, TypeOfNavItem, URL, data) {
    event.stopPropagation();

    console.log("OnGlobalMenuItemClick: URL = " + URL);

    if (URL !== undefined && URL != "")
        window.location.href = URL;


    return;
}

