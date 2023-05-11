var yakalananid = -1

$(function () { //sayfa yüklendiğinde
    $('#modal1').on('show.bs.modal', function (e) {

        var yakalananbtn = $(e.relatedTarget);// O anda tıklanan butonnu yakaladık
         yakalananid = yakalananbtn.data("makaleid");

        $('#modal1_body').load("/Yorum/YorumGoster/" + yakalananid);

    });
});
    function islemyap(btn, islem, yorumid, spanid) {

        var button = $(btn) // butonu yakaladık
        var durum = button.data("editmod")//buton düzenleme demi gösterme de mi?

        if (islem == "update")
        {
            if (durum == false) {

                button.data("editmod", true);
                button.removeClass("btn-warning");
                button.addClass("btn-success");
                var span = button.find("span");
                span.removeClass("glyphicon-edit");
                span.addClass("glyphicon-ok");
                $(spanid).attr("contenteditable", true);
                $(spanid).focus();
            }
            else {

                button.data("editmod", false);
                button.removeClass("btn-success");
                button.addClass("btn-warning");
                var span = button.find("span");
                span.removeClass("glyphicon-ok");
                span.addClass("glyphicon-edit");
                $(spanid).attr("contenteditable", false);

                //yazılanı db ye atalım..

                var yorum = $(spanid).text();

                $.ajax({
                    method: "POST", url: "/Yorum/YorumGuncelle/" + yorumid, data: { text: yorum }

                }).done(function (sonuc) {

                    if (sonuc.hata) {

                        alert("Yorum Güncellenemedi");

                    }
                    else {
                        //yorumlar tekrar yüklenir
                        $('#modal1_body').load("/Yorum/YorumGoster/" + yakalananid);
                    }


                }).fail(function (){

                    alert("Bağlantı Hatası");
                });


            }
            

        }
        else if (islem == "delete")
        {
            var onay = confirm("Yorum silinsin mi?");
            if (!onay)
            {
                return false;
            }
            $.ajax({

                method: "GET", url: "/Yorum/YorumSil/" + yorumid

            }).done(function (sonuc)
            {
                if (sonuc.hata)
                {
                    alert("Yorum silinemedi");
                }
                else
                {
                    //Yorumlar tekrar yüklensin
                    $('#modal1_body').load("/Yorum/YorumGoster/" + yakalananid);
                }
               

            }).fail(function (){alert("Sunucu ile bağlantı kurulamadı")});

        }

        else if (islem == "insert")
        {
            var yorum = $("#yapilanyorum").val();
            $.ajax({

                method: "POST", url: "/Yorum/YorumEkle", data: { Text: yorum, id: yakalananid }

            }).done(function (sonuc) {

                if (sonuc.hata) {
                    alert("Yorum silinemedi");
                }
                else {
                    //Yorumlar tekrar yüklensin
                    $('#modal1_body').load("/Yorum/YorumGoster/" + yakalananid);
                }

            }).fail(function () {alert("Baglantı kurulamadı") });

        }
   


    }

    



    $(".begeni").click(function () {
            var currentColor = $(this).css("color");
    if (currentColor === "rgb(255, 0, 0)") {
        $(this).css("color", "black");
            } else {
        $(this).css("color", "red");
            }
        });

//$('#modal1').on('show.bs.modal', function (e) {

//    var btn = $(e.relatedTarget);
//    var mid = btn.data("makaleid");



//    $("#modal1_body").load("/Yorum/YorumGoster/" + mid);

//})
