//User Defined Actions Stored Here - Ugur Gural

$(document).ready(function () {

    var updateOutput = function (e) {
        var list = e.length ? e : $(e.target),
            output = list.data('output');
        if (window.JSON) {
            output.val(window.JSON.stringify(list.nestable('serialize')));//, null, 2));
        } else {
            output.val('JSON browser support required for this demo.');
        }
    };

    // activate Nestable for list 1
    $('#nestable').nestable({
        group: 1
    })
    .on('change', updateOutput);


    // output initial serialised data
    updateOutput($('#nestable').data('output', $('#hdnMenuOrder')));


    $('.menu-order-remove-button').click(function () {
        var confirmDialog = confirm("Menüyü Silmek İstediğinizden Emin Misiniz ?");
        var removeCurrentItem = false;
        if (confirmDialog) {
            $.ajax({
                type: "POST",
                url: '/Menu/RemoveMenuItem?menuItemID=' + $(this).data("id"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    removeCurrentItem = true;
                },
                error: function () {
                    alert("hata oluştu.");
                }
            });

            if (removeCurrentItem) {
                $(this).parent().remove();
                updateOutput($('#nestable').data('output', $('#hdnMenuOrder')));
            }
        }
    });

    $('.menu-order-edit-button').click(function () {
        window.location.replace("/menu/yeni-menu-ekle?menuID=" + $(this).data("id"));
    });


    if (!$('#saveMenuForm').length) {
        return false;
    }

    var loginValidationSettings = {
        rules: {
            Name: "required",
            Caption: "required",
            ItemType: "required",

        },
        messages: {
            Name: "Bu alan boş geçilemez",
            Caption: "Bu alan boş geçilemez",
            ItemType: "Bu alan boş geçilemez",
        },
        invalidHandler: function () {
            animate({
                name: 'shake',
                selector: '.auth-container > .card'
            });
        }
    }

    $.extend(loginValidationSettings, config.validations);
    $('#saveMenuForm').validate(loginValidationSettings);

});