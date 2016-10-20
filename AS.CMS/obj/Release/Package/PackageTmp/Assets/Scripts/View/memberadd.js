$(document).ready(function () {
    $(".add-single-image").click(function () {
        selectMainImageFileWithCKFinder();
    });

    if (!$('#saveMemberForm').length) {
        return false;
    }

    var loginValidationSettings = {
        rules: {
            FirstName: "required",
            LastName: "required",
            Email: "required",
            Password: "required"
        },
        messages: {
            FirstName: "Bu alan boş geçilemez",
            LastName: "Bu alan boş geçilemez",
            Email: "Bu alan boş geçilemez",
            Password: "Bu alan boş geçilemez"
        },
        invalidHandler: function () {
            animate({
                name: 'shake',
                selector: '.auth-container > .card'
            });
        }
    }

    $.extend(loginValidationSettings, config.validations);
    $('#saveMemberForm').validate(loginValidationSettings);
});

$(document).on("click", ".upload-remove-single", function () {
    var confirmDialog = confirm("Resmi Silmek İstediğinizden Emin Misiniz ?");
    if (confirmDialog) {
        $(this).parent().parent().remove();
        var newImageItem = "<div class=\"image-container new\"><a href=\"#\" class=\"add-single-image\"><div class=\"image\"> <i class=\"fa fa-plus\"></i> </div></a></div>";
        $(".images-container.single").append(newImageItem);
        $("#memberPicture").val("");
    }
});

$(document).on("click", ".add-single-image", function () {
    selectMainImageFileWithCKFinder();
});

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
                $("#memberPicture").val(file.getUrl());
            });
        }
    });
}