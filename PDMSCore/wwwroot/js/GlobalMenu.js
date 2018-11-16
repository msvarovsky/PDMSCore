
function OnGlobalMenuItemClick(target, TypeOfNavItem, URL, NavID) {
    event.stopPropagation();

    console.log("OnGlobalMenuItemClick: URL = " + URL);

    if (URL !== undefined && URL != "")
        //window.location.href = "/" + URL;
        window.location.href = "/" + URL + "?NavID=" + NavID;


    return;
}

