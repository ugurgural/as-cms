$(document).ready(function () {
    $(".add-image").click(function () {
        selectFileWithCKFinder();
    });

    $(".add-single-image").click(function () {
        selectMainImageFileWithCKFinder();
    });

    $(".btn-permalink-generate").click(function () {
        $("#Permalink").val(generatePermalinkFromText($("#Title").val()));
    });

    if (!$('#saveContentForm').length) {
        return false;
    }

    var loginValidationSettings = {
        rules: {
            Title: "required",
            Permalink: "required"
        },
        messages: {
            Title: "Bu alan boş geçilemez",
            Permalink: "Bu alan boş geçilemez"
        },
        invalidHandler: function () {
            animate({
                name: 'shake',
                selector: '.auth-container > .card'
            });
        }
    }

    $.extend(loginValidationSettings, config.validations);
    $('#saveContentForm').validate(loginValidationSettings);

    CKEDITOR.replace('Description');
});

$('.images-container.gallery').on({
    drop: function () {
        $("#galleryImages").val("");

        $(".images-container.gallery .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#galleryImages").val($("#galleryImages").val() + $(this).data("fileurl") + ",");
            }
        });
    }
});

$(document).on("click", ".upload-remove", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {

        $(this).parent().parent().remove();
        $("#galleryImages").val("");

        $(".images-container.gallery .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#galleryImages").val($("#galleryImages").val() + $(this).data("fileurl") + ",");
            }     
        });

    }
});

$(document).on("click", ".upload-remove-single", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {
        $(this).parent().parent().remove();
        var newImageItem = "<div class=\"image-container new\"><a href=\"#\" class=\"add-single-image\"><div class=\"image\"> <i class=\"fa fa-plus\"></i> </div></a></div>";
        $(".images-container.single").append(newImageItem);
        $("#pageImage").val("");
    }
});

$(document).on("click", ".add-single-image", function () {
    selectMainImageFileWithCKFinder();
});

function generatePermalinkFromText(text) {
    var maxLength = 100;

    var returnString = text.toLowerCase();
    returnString = returnString.replace(/ö/g, 'o');
    returnString = returnString.replace(/ç/g, 'c');
    returnString = returnString.replace(/ş/g, 's');
    returnString = returnString.replace(/ı/g, 'i');
    returnString = returnString.replace(/ğ/g, 'g');
    returnString = returnString.replace(/ü/g, 'u');
    returnString = returnString.replace(/[^a-z0-9\s-]/g, "");
    returnString = returnString.replace(/[\s-]+/g, " ");
    returnString = returnString.replace(/^\s+|\s+$/g, "");

    if (returnString.length > maxLength)
        returnString = returnString.substring(0, maxLength);

    returnString = returnString.replace(/\s/g, "-");

    return returnString;
}

function selectFileWithCKFinder() {
    CKFinder.modal({
        chooseFiles: true,
        width: 800,
        height: 600,
        onInit: function (finder) {
            finder.on('files:choose', function (evt) {
                var file = evt.data.files.toArray();
                for (var i = 0; i < file.length; i++) {
                    var uploadItem = "<div class=\"image-container\" data-fileurl=\"" + file[i].getUrl() + "\"><div class=\"controls\"><a href=\"#\" class=\"control-btn move\"><i class=\"fa fa-arrows\"></i></a><a href=\"#\" class=\"control-btn remove upload-remove\"><i class=\"fa fa-trash-o\"></i></a></div><div class=\"image\" style=\"background-image:url('" + file[i].getUrl() + "')\"></div></div>";
                    $(".images-container.gallery").append(uploadItem);
                    $("#galleryImages").val($("#galleryImages").val() + file[i].getUrl() + ",");
                }
            });
        }
    });
}

function selectMainImageFileWithCKFinder() {
    CKFinder.modal({
        chooseFiles: true,
        width: 800,
        height: 600,
        onInit: function (finder) {
            finder.on('files:choose', function (evt) {
                var file = evt.data.files.first();
                var uploadItem = "<div class=\"image-container\" data-fileurl=\"" + file.getUrl() + "\"><div class=\"controls\"><a href=\"#\" class=\"control-btn remove upload-remove-single\"><i class=\"fa fa-trash-o\"></i></a></div><div class=\"image\" style=\"background-image:url('" + file.getUrl() + "')\"></div></div>";
                $(".images-container.single").empty();
                $(".images-container.single").append(uploadItem);
                $("#pageImage").val(file.getUrl());
            });
        }
    });
}