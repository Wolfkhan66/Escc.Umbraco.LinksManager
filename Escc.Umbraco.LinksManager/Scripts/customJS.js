$("#lookupInboundLinks").click(function() {
    var dest = $("#PermissionsResults");
    dest.html("");
    var btn = $(this);
    var url = $("#searchterm").val();

    if (url.length === 0) {
        dest.html(errorMessage("Please enter a URL"));
        return false;
    }

    url = encodeURIComponent(url);
    btn.prop("disabled", true);
    var applicationRoot = document.location.pathname;
    if (applicationRoot.substr(applicationRoot.length - 1) == '/') {
        applicationRoot = applicationRoot.substr(0, applicationRoot.length - 1);
    }
    dest.html("<img src=\"" + applicationRoot + "/Content/ajax-loader.gif\" class=\"loaderimg\" alt=\"Please wait...\" />");

    $.get($("#apppath").html() + "/Home/FindInboundLinks/", { url: url }, function (data) {
        dest.html(data);
        btn.prop("disabled", false);
    });

    return false;
});

$("#url").keypress(function (e) {
    if (e.which == 13) {
        $("#checkpage").trigger("click");

        if ($("#searchterm").length) $("#searchterm").blur();
    }
});

$("#searchterm").keypress(function (e) {
    if (e.which == 13) {
        if ($("#lookupInboundLinks").length) $("#lookupInboundLinks").trigger("click");

        if ($("#searchterm").length) $("#searchterm").blur();
    }
});

function errorMessage(msg) {
    var rtnMsg = "<div class" + "=\"alert alert-danger\"><p class=\"highlight\">";
    rtnMsg = rtnMsg + msg;
    rtnMsg = rtnMsg + "</p></div>";
    return rtnMsg;
};