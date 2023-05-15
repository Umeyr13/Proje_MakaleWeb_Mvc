
$(function () {
    //data-makaleid atribute ü olan div i bana getir
    var makaledizi=[];

    $("div[data-makaleid]").each(function(i,e){

        makaledizi.push($(e).data("makaleid"));

    });

    $.ajax({
        method: "POST",
        url: "/Makale/MakaleGetir",
        data: { makaleidleri: makaledizi }


     }).done(function(sonuc){

        if(sonuc.liste !=null && sonuc.liste.length>0)
        {
            for(var i = 0 ; i<sonuc.liste.length;i++)
            {
                var id = sonuc.liste[i];
                var btn = $("button[data-mid="+id+"]") // İlgili butonu bulduk
                btn.data("like",true);
                var span = btn.find("span.like-kalp-"+id);
                span.removeClass("glyphicon-heart-empty");
                span.addClass("glyphicon-heart");
                


            }

        }

        
    }).fail(function(){

        alert("Bağlatı Hatası");

    });

    $("button[data-like]").click(function () {

        var btn = $(this);
        var like = btn.data("like");
        var mid = btn.data("mid");
        var spanLike=  $("span.like-kalp-"+mid)
        var spanCount = $("span.like-"+mid)

        $.ajax({
            method: "POST"
            , url: "/Makale/MakaleBegen"
            , data: {makaleid: mid, begeni: !like} //burada yazan isimler Action dakiler ile aynı olmalı
        }).done(function (sonuc) {
            if (sonuc.hata)
            {
                alert("Begeni işlemi gerçekleşmedi");
            }
            else {
                var begeni = !like;
                btn.data("like", !like)
                spanCount.text(sonuc.begenisayisi);//begeni sayısını verdik
                spanLike.removeClass("glyphicon-heart-empty")
                spanLike.removeClass("glyphicon-heart")
                if (begeni)//begendiyse
                {
                    spanLike.addClass("glyphicon-heart")                  
                    btn.css("color", "red");
                }
                else
                {
                    spanLike.addClass("glyphicon-heart-empty")
                    btn.css("color", "red");
                }
             
            }

        }).fail(function () {
            alert("Sunucu ile baglantı kesildi");
        } );
    } );


});