$(document).ready(function () {
    validateLogin();
});

function SubmitLogin() {
    var obj = {
        Email: $("#email").val().trim(),
        Password: $("#password").val().trim(),
    };
    $.ajax({
        url: "/Account/Login",
        data: JSON.stringify(obj),
        type: "POST",
        contentType: "application/json;charset=utf-8",

        success: function (result) {
            console.log(result);
            toastr.success(result.responseText, "Success");
            $.LoadingOverlay("hide");
            window.location = "/JobApplication/TFNDeclare";
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
            toastr.error(errormessage.responseText, "Error occurred");
            $.LoadingOverlay("hide");
        },
    });
}

function validateLogin() {
    $("#login-form").validate({
        rules: {
            email: {
                required: true,
                email: true,
                normalizer: function (value) {
                    return $.trim(value);
                },
            },
            password: {
                required: true,
                normalizer: function (value) {
                    return $.trim(value);
                },
            },
        },
        messages: {
            email: {
                required: "Email cannot be empty",
            },
            password: {
                required: "Password cannot be empty",
            },
        },
    });
}
