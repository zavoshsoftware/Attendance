// self services


function card_keyup_event(e) {
    var evt = window.event || e;
    if (e.ctrlKey && e.key === '1') {
 
        let securityCode = prompt("Enter security code");
        if (securityCode) {
            var xhr = new XMLHttpRequest();
            xhr.withCredentials = true;

            xhr.addEventListener("readystatechange", function () {
                if (this.readyState === 4) {
                    let obj = JSON.parse(this.responseText);
                    if (obj.Extra) {
                        var modal = document.getElementById("atnModal");
                        document.querySelector('#atnModal .atn-modal-header h2').innerText = "کارت ها";


                        xhr.addEventListener("readystatechange", function () {
                            if (this.readyState === 4) {
                                document.querySelector('#atnModal .atn-modal-body').innerHTML = this.responseText;
                                window.onclick = function (event) {
                                    if (event.target == modal) {
                                        modal.style.display = "none";
                                    }
                                }
                                modal.style.display = "block";

                            }
                        });
                        xhr.open("GET", "/cards/hiddencardspartial");
                        xhr.send();


                    }
                    else {
                        alert('رمز اشتباه است');
                    }
                }
            });

            xhr.open("GET", "/api/SecurityCode/" + securityCode + "/auth");
            xhr.send();
        }
    }

    else if (e.ctrlKey && e.key === '2') {
        
        let securityCode = prompt("Enter security code");
        if (securityCode) {
            var xhr = new XMLHttpRequest();
            xhr.withCredentials = true;

            xhr.addEventListener("readystatechange", function () {
                if (this.readyState === 4) {
                    let obj = JSON.parse(this.responseText);
                    if (obj.Extra) {
                        var modal = document.getElementById("atnModal");
                        document.querySelector('#atnModal .atn-modal-header h2').innerText = "تاریخچه کارت ها";


                        xhr.addEventListener("readystatechange", function () {
                            if (this.readyState === 4) {
                                document.querySelector('#atnModal .atn-modal-body').innerHTML = this.responseText;
                                window.onclick = function (event) {
                                    if (event.target == modal) {
                                        modal.style.display = "none";
                                    }
                                }
                                modal.style.display = "block";

                            }
                        });
                        xhr.open("GET", "/cards/RemoveHiddenCardHistoryPartial");
                        xhr.send();


                    }
                    else {
                        alert('رمز اشتباه است');
                    }
                }
            });

            xhr.open("GET", "/api/SecurityCode/" + securityCode + "/auth");
            xhr.send();
        }
    }


}



document.addEventListener('keyup', card_keyup_event, false);
