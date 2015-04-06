var arrButtons = [ 'bet-bt','stand-bt' , 'hit-bt', 'double-bt', 'split-bt' ];

function GetButtons(callback) {
    var buttons = new Array();
    if (jQuery('#double-bt').length > 0 && jQuery('#double-bt').is(":visible")) {
        buttons[buttons.length] = arrButtons.indexOf('double-bt');
    }
    if (jQuery('#stand-bt').length > 0 && jQuery('#stand-bt').is(":visible")) {
        buttons[buttons.length] = arrButtons.indexOf('stand-bt');
    }
    if (jQuery('#hit-bt').length > 0 && jQuery('#hit-bt').is(":visible")) {
        buttons[buttons.length] = arrButtons.indexOf('hit-bt');
    }
    if (jQuery('#split-bt').length > 0 && jQuery('#split-bt').is(":visible")) {
        buttons[buttons.length] = arrButtons.indexOf('split-bt');
    }
    if (jQuery('#bet-bt').length > 0 && jQuery('#bet-bt').is(":visible")) {
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
    console.log(arrButtons[data]);
    if (data != -1) {
        jQuery('#' + arrButtons[data]).click();
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
        var player = GetPlayersCards("player-cards-1");
        if (jQuery('#player-arrow-2').length > 0 && jQuery('#player-arrow-2').is(":visible")) {
            player = GetPlayersCards("player-cards-2");
        }
        var dealer = GetPlayersCards("dealer-cards");
        console.log(player);
        console.log(dealer);
        console.log(buttons);

        GetBestMove(function (data) {
            DoMove(data, function () {
                setTimeout(ProccessMove, 2000);
            });
        }, buttons, player, dealer);
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
function GetBestMove(callback, buttons,player, dealer) {
    var serviceURL = 'http://localhost:41164/home/GetBestMoveBJ?jsoncallback=?';
    $.ajax({
        dataType: "json",
        traditional: true,
        url: serviceURL,
        data: { buttons: buttons, player: player, dealer: dealer },
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