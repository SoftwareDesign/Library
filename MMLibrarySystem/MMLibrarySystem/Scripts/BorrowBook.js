function BorrowSuccessAction(id) {
    var statename = "#state" + id;
    var opterationname = "#col" + id;
    //alert(statename);
    $(statename).html("Borrowed");
    $(opterationname).html("");
}