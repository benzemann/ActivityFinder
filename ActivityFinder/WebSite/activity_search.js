
$(document).ready(function () {
    // Search button press
    $('#search_btn').on('click', function (event) {
        $('#activity_rows').append(
            '<div class="col-xs-10 col-xs-offset-1 col-sm-4 col-md-3 col-lg-3 col-sm-offset-0 activity_rows">'+
            '<a class="nounderline" href= "http://www.ticketmaster.dk/event/428197?language=da-dk&track=DiscoveryAPI&subchannel_id=1" >'+
            '<div class="panel panel-default">' +
                '<img src="http://media.ticketmaster.com/img/tat/dam/a/71d/d7271cd3-9989-4728-8f4f-a5c57302271d_377681_CUSTOM.jpg"' +
                'class="img-responsive" style="padding-left: 0px;  padding-right: 0px;" />' +
                '<div class="activity_title">INFERNAL<br>&nbsp;</div>'+
                '<div class="activity_category">Musik</div>'+
                '<div class="activity_icon glyphicon glyphicon-calendar"><i class="activity_icon_tekst">27-10-2017 19:00</i></div>'+
                '<div class="activity_icon glyphicon glyphicon-map-marker"><i class="activity_icon_tekst">Toldbodgade 6, 8000 Aarhus C</i></div>'+
                '<div class="activity_icon glyphicon glyphicon-piggy-bank"><i class="activity_icon_tekst">200 kr.</i></div>'+
            '</div></a></div>'
        );
    });
    // IF user presses enter while typing in the search area
    $('#search_area').keypress(function (e) {
        if (e.which == 13) {//Enter key pressed
            $('#search_btn').click();
        }
    });

});
