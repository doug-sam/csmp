function MoveList(setname, getname) {
    var size = $("#" + setname + " option").size();
    var selsize = $("#" + setname + " option:selected").size();
    if (size > 0 && selsize > 0) {
        $.each($("#" + setname + " option:selected"), function(id, own) {
            var text = $(own).text();
            var tag = $(own).attr("value");
            $("#" + getname).prepend("<option value=\"" + tag + "\">" + text + "</option>");
            $(own).remove();
            $("#" + setname + "").children("option:first").attr("selected", true);
        });
    }
    $.each($("#" + getname + " option"), function(id, own) {
        Orderrole(getname);
    });
}

function Orderrole(listname) {
    var size = $("#" + listname + " option").size();
    var one = $("#" + listname + " option:first-child");
    if (size > 0) {
        var text = $(one).text();
        var tag = parseInt($(one).attr("value"));
        $.each($(one).nextAll(), function(id, own) {
            var nextag = parseInt($(own).attr("value"));
            if (tag > nextag) {
                $(one).remove();
                $(own).after("<option value=\"" + tag + "\">" + text + "</option>");
                one = $(own).next();
            }
        });
    }
}