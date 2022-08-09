function addDiscountCode(productId) {
    var coupon = $("#discount").val();
    var carmodelId = $("#CarModelId").val();
    $('#errorDiv-discount').css('display', 'none');
    if (coupon !== "") {
        $.ajax(
            {
                url: "/discountcodes/DiscountRequestPost",
                data: { coupon: coupon, carModelId: carmodelId, productId: productId},
                type: "GET"
            }).done(function (result) {
                if (result !== "Invald" && result !== "Used" && result !== "Expired" && result !=="InvalidProduct") {
                    $('#price').css('display', 'none');
                    $('#old-price').css('display', 'flex');

                    //$('#price').html( =  )"هزینه کارشناسی : " + prc + " تومان ");
                    document.getElementById('newprice').innerHTML = result;

                }
                else if (result !== true) {
                    $('#errorDiv-discount').css('display', 'block');
                    if (result.toLowerCase() === "used") {
                        $('#errorDiv-discount').html("این کد تخفیف قبلا استفاده شده است.");
                    }
                    else if (result.toLowerCase() === "expired") {
                        $('#errorDiv-discount').html("کد تخفیف شما منقضی شده است..");
                    }
                    else if (result.toLowerCase() === "invald") {
                        $('#errorDiv-discount').html("کد تخفیف نامعتبر است.");
                    }
                    else if (result.toLowerCase() === "invalidproduct") {
                        $('#errorDiv-discount').html("کد تخفیف وارد شده برای این محصول معتبر نمی باشد.");
                    }
                    else if (result.toLowerCase() === "true") {
                        $('#SuccessDiv-discount').css('display', 'block');
                        $('#errorDiv-discount').css('display', 'none');
                    }
                }
            });

    } else {
        $('#SuccessDiv-discount').css('display', 'none');
        $('#errorDiv-discount').html('کد تخفیف را وارد نمایید.');
        $('#errorDiv-discount').css('display', 'block');
    }
}


function addEduDiscountCode() {
    var coupon = $("#discount").val(); 
    var productId = $("#ProductId").val();
    $('#errorDiv-discount').css('display', 'none');
    if (coupon !== "") {
        $.ajax(
            {
                url: "/discountcodes/DiscountRequestPostEducation",
                data: { coupon: coupon,   productId: productId },
                type: "GET"
            }).done(function (result) {
                if (result !== "Invald" && result !== "Used" && result !== "Expired" && result !== "InvalidProduct") {
                    $('#priceinEdu').css('display', 'none');
                    $('#old-price').css('display', 'flex');

                    //$('#price').html( =  )"هزینه کارشناسی : " + prc + " تومان ");
                    document.getElementById('newprice').innerHTML = result;

                }
                else if (result !== true) {
                    $('#errorDiv-discount').css('display', 'block');
                    if (result.toLowerCase() === "used") {
                        $('#errorDiv-discount').html("این کد تخفیف قبلا استفاده شده است.");
                    }
                    else if (result.toLowerCase() === "expired") {
                        $('#errorDiv-discount').html("کد تخفیف شما منقضی شده است..");
                    }
                    else if (result.toLowerCase() === "invald") {
                        $('#errorDiv-discount').html("کد تخفیف نامعتبر است.");
                    }
                    else if (result.toLowerCase() === "invalidproduct") {
                        $('#errorDiv-discount').html("کد تخفیف وارد شده برای این محصول معتبر نمی باشد.");
                    }
                    else if (result.toLowerCase() === "true") {
                        $('#SuccessDiv-discount').css('display', 'block');
                        $('#errorDiv-discount').css('display', 'none');
                    }
                }
            });

    } else {
        $('#SuccessDiv-discount').css('display', 'none');
        $('#errorDiv-discount').html('کد تخفیف را وارد نمایید.');
        $('#errorDiv-discount').css('display', 'block');
    }
}

function submitCarCheck() {
    var name = $("#NemeID").val();
    var phone = $("#PhoneID").val();
    var address = $("#AddressID").val();
    var discount = $("#discount").val();
    var creationYearId = $('#CreationYearId').val();
    var cityId = $('#CityId').val();
    var carmodelId = $("#CarModelId").val();
   
    if (name !== "" && phone !== "") {
        $.ajax(
            {
                url: "/car-check",
                data: {
                    name: name, phone: phone, address: address, discount: discount, creationYearId: creationYearId,
                    cityId: cityId, carModelId: carmodelId
                      },
                type: "POST"
            }).done(function (result) {
                if (result === "false") {
                    $('#errorDiv-discount').html("خطایی رخ داده است. فیلدهای ورودی را کنترل کنید.");

                }
                else {
                    window.location = result;

                }
            });

    } else {
        $('#SuccessDiv-discount').css('display', 'none');
        $('#errorDiv-discount').html('فیلدهای اجباری را تکمیل کنید.');
        $('#errorDiv-discount').css('display', 'block');
    }
}