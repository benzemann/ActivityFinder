var id_iterator = 0;
$(document).ready(function () {
    // Search button press
    $('#search_btn').on('click', function (event) {
        var address = $('#search_area').val();

        $.ajax({
            url: "http://webservice20170717071554.azurewebsites.net/api/activity/" + address
        }).then(function (data) {
            $('#activity_rows').empty();
            $.each(data, function (indx) {
                var image = data[indx].Image;
                if (image === null || !image) {
                    image = "https://www.municipay.com/wp-content/themes/Artificial-Reason-WP/img/no_image.png";
                }
                var date = "-</i>";
                if (data[indx].StartDate !== null) {
                    date = data[indx].StartDate + "</i>";
                } else if (data[indx].OpenHours !== null) {
                    date = '</i><button type="button" class="btn btn-xs" data-toggle="modal" data-target="#openHoursModal" id="' + id_iterator + '">Åbningstider</button>';
                    
                }
                var address = "";
                if (data[indx].Address !== null) {
                    address = data[indx].Address + ", ";
                }
                if (data[indx].PostalCode !== null) {
                    address += data[indx].PostalCode + " ";
                }
                if (data[indx].City !== null) {
                    address += data[indx].City;
                }
                var website = data[indx].Website;
                if (website === null) {
                    website = data[indx].Url;
                }

                var price = data[indx].Price;
                if (price === null || !price) {
                    price = '-';
                }
                var website = data[indx].Website;
                if (website === null || !website) {
                    website = data[indx].Url;
                }
                //'<a class="nounderline" href= "' + data[indx].Website + '" >' +
                $('#activity_rows').append(
                    '<div class="col-xs-10 col-xs-offset-1 col-sm-4 col-md-3 col-lg-3 col-sm-offset-0 activity_rows">' +
                    '<div class="panel panel-default activity_panel">' +
                    '<a class="nounderline" href= "' + website + '">' +
                    '<img class="activity_img img-responsive" src="' + image + '" align="middle"' +
                    'class="img-responsive" style="padding-left: 0px;  padding-right: 0px;" />' +
                    '</a>' +
                    '<div class="activity_title">' + data[indx].Title + '</div>' +
                    '<div class="activity_category">' + data[indx].Category + '</div>' +
                    '<div class="activity_icon glyphicon glyphicon-calendar"><i class="activity_icon_tekst">' + date + '</div>' +
                    '<div class="activity_icon glyphicon glyphicon-map-marker"><i class="activity_icon_tekst">' + address + '</i></div>' +
                    '<div class="activity_icon glyphicon glyphicon-piggy-bank"><i class="activity_icon_tekst">' + price + '</i></div>' +
                    '</div></div>'
                );
                var id = '#' + id_iterator;
                $(id).on('click', function (event) {
                    $('#aabningstider').text(data[indx].OpenHours);
                });
                id_iterator += 1;
            });
           
        });
    });
    // IF user presses enter while typing in the search area
    $('#search_area').keypress(function (e) {
        if (e.which === 13) {//Enter key pressed
            $('#search_btn').click();
        }
    });

});
