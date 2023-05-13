
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
                var btn = $("button[data-mid"+id+"]") // İlgili butonu bulduk
                btn.data("like",true);
                var span = btn.find("span.like-kalp");
                span.removeClass("glyphicon-heart-empty");
                span.addClass("glyphicon-heart");
                console.log(1);


            }

        }

        
    }).fail(function(){

        alert("Bağlatı Hatası");

    });




});