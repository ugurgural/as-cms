$(document).ready(function () {
    $(".add-image").click(function () {
        selectFileWithCKFinder();
    });

    $(".add-single-image").click(function () {
        selectMainImageFileWithCKFinder();
    });

    if (!$('#saveEventForm').length) {
        return false;
    }

    var loginValidationSettings = {
        rules: {
            Name: "required",
            Description: "required",
            Restriction: "required"
        },
        messages: {
            Name: "Bu alan boş geçilemez",
            Description: "Bu alan boş geçilemez",
            Restriction: "Bu alan boş geçilemez"
        },
        invalidHandler: function () {
            animate({
                name: 'shake',
                selector: '.auth-container > .card'
            });
        }
    }

    $.extend(loginValidationSettings, config.validations);
    $('#saveEventForm').validate(loginValidationSettings);

    CKEDITOR.replace('Description');
    CKEDITOR.replace('Restriction');
});

$('.images-container.gallery').on({
    drop: function () {
        $("#EventDocument").val("");

        $(".images-container.gallery .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#EventDocument").val($("#EventDocument").val() + $(this).data("fileurl") + ",");
            }
        });
    }
});

$(document).on("click", ".upload-remove", function () {
    var confirmDialog = confirm("Dosyayı Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {

        $(this).parent().parent().remove();
        $("#EventDocument").val("");

        $(".images-container.gallery .image-container").each(function (item) {
            if ($(this).data("fileurl") != undefined) {
                $("#EventDocument").val($("#EventDocument").val() + $(this).data("fileurl") + ",");
            }
        });

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
                    $("#EventDocument").val($("#EventDocument").val() + file[i].getUrl() + ",");
                }
            });
        }
    });
}