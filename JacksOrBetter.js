var arrButtons = ['bet-bt', 'draw-bt'];

function GetButtons(callback) {
    var buttons = new Array();
    if (jQuery('#draw-bt').length > 0 && jQuery('#draw-bt').is(":enabled")) {
        buttons[buttons.length] = arrButtons.indexOf('draw-bt');
    }
    if (jQuery('#bet-bt').length > 0 && jQuery('#bet-bt').is(":enabled")) {
        buttons[buttons.length] = arrButtons.indexOf('bet-bt');
    }
    

    if (buttons.length == 0) {
        setTimeout(function () {
            GetButtons(callback);
        }, 1000);
    }
    else {
        callback(buttons);
    }
}

function DoMove(data,callback)
{
    if (data.move != -1) {
        console.log(arrButtons[data.move]);
        if (data.move == 1) {
            console.log(data.hold);
            $.each(data.hold, function (key, value) {
                if (value == 1)
                {
                    jQuery('#player-card-' + (key+1)).click();
                }
            });
        }
        jQuery('#' + arrButtons[data.move]).click();
        callback();
    }
    else {
        console.log('stoped');
        //alert(data);
    }
}

function ProccessMove()
{
    GetButtons(function (buttons) {
        var player = GetPlayersCards("player-cards");
        console.log(player);
        console.log(buttons);

        GetBestMove(function (data) {
            DoMove(data, function () {
                setTimeout(ProccessMove, 2000);
            });
        }, buttons, player);
    });
}




function GetPlayersCards(owner)
{
    var arrCards = new Array();
    $.each($('#'+owner+'  div.card'), function (key, value) {
        var elem = this;
        var rank = $(elem).find('.rank').html();
        var suit = $(elem).find('.suit').html();
        if (suit == '♠') {
            suit = 'S';
        }
        else if (suit == '♣') {
            suit = 'C';
        }
        else if (suit == '♦') {
            suit = 'D';
        }
        else {
            suit = 'H';
        }
        arrCards[arrCards.length] = suit + "_" + rank;
    });

    return arrCards;
}
function GetBestMove(callback, buttons,player) {
    var serviceURL = 'http://localhost:41164/home/JacksOrBetter?jsoncallback=?';
    $.ajax({
        dataType: "json",
        traditional: true,
        url: serviceURL,
        data: { buttons: buttons, player: player },
        success: function (data) {
            callback(data);
        }
    });
}
function testCall() {
    $.getJSON('http://localhost:41164/home/test?jsoncallback=?', function (data) {
        alert(data);
    });
}
ProccessMove();