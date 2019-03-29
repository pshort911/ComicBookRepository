// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//Own Checkbox event handler.
$("body").on("click", "#tblDetails .own-checkbox", function () {

    var comicBookDetails = {
        Own: this.checked === true,
        Id: parseInt(this.id.replace("own", ""))
    };
    var comicBookDetailsJson = JSON.stringify(comicBookDetails);
    //comicBookDetails.Id = this.id.replace("own", "");
    //comicBookDetails.Own = this.checked === true;
    $.ajax({
        type: "POST",
        url: "/ComicBookDetails/UpdateOwnList",
        data: comicBookDetailsJson,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        contentType: "application/json",
        dataType: "json"
    });
});

$("body").on("click", "#tblDetails .want-checkbox", function () {

    var comicBookDetails = {
        Want: this.checked === true,
        Id: parseInt(this.id.replace("want", ""))
    };
    var comicBookDetailsJson = JSON.stringify(comicBookDetails);
    $.ajax({
        type: "POST",
        url: "/ComicBookDetails/UpdateWantList",
        data: comicBookDetailsJson,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        contentType: "application/json",
        dataType: "json"
    });
});

function UpdateComicBookDetails() {

}