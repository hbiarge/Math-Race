// util, print msgs --
function print_msg(msg, style, time) {
    $('#msg').html('<div class="msg msg-' + style + '">' + msg + '</div>');
    $('#msg').fadeIn();
    setTimeout(function () { $('#msg').fadeOut(); }, time || 500);
}

function show_error(msg) { print_msg(msg, 'error'); }

function show_ok(msg) { print_msg(msg, 'ok'); }
// end print msgs --

// Hub configuration
function configureRaceHub(raceHub, race) {
    raceHub.client.time = function (remaining) {
        race.time(remaining);
    };

    raceHub.client.newOperation = function (operation) {
        $('#operations').html(operation);
        $('input.input_player').val('').select(); //reset and select.
    };

    raceHub.client.history = function (history) {
        race.history(history); //bind score history (and sort by date)
    };

    raceHub.client.scores = function (scores) {
        race.scores(scores);
        if (scores.length) { //effect
            $('.scores').addClass('selected');
            setTimeout(function () {
                $('.scores').removeClass('selected');
            }, 200);
        }
    };

    raceHub.client.hallOfFame = function (hallOfFame) {
        race.hall_of_fame(hallOfFame);
    };

    raceHub.client.newGame = function () {
        print_msg('new game, new score, hurry up!!', 'new', 2000);
    };

    raceHub.client.resultOperation = function (result) {
        if (result == 1)
            print_msg('good!!', 'success');
        else if (result == 2)
            print_msg('other player solved this quest faster than you!', 'warning', 1000);
        else
            print_msg('nooooooooo!', 'error');
    };
}
// end hub configuration

//knockout model and binding
function Race(hub) {
    var self = this;
    self.hub = hub;
    self.name = ko.observable();

    self.input_player1 = ko.observable();
    self.scores = ko.observableArray();
    self.history = ko.observableArray();
    self.hall_of_fame = ko.observableArray();
    self.time = ko.observableArray();

    self.valid_name = ko.computed(function () {
        return (self.name() && self.name().length > 2);
    }, self);

    self.sendOperationResult = function () {
        hub.server.solveOperation(self.input_player1(), self.name());
    };
}
//end knockout model and binding

$(document).ready(function () {

    //jQuery bindings
    $('input.input_player').click(function (e) {
        this.select();
    });

    $('#name').focus();

    $('input.input_player').keydown(function (e) {
        if (e.keyCode == '13') { //press return
            race.sendOperationResult();
            $('input.input_player').select();
        }
    });

    var raceHub = $.connection.race;

    // Enable logging
    $.connection.hub.logging = true;

    // Configure error callback
    $.connection.hub.error(function (error) {
        log('SignalR error: ' + error);
    });

    var race = new Race(raceHub);
    
    configureRaceHub(raceHub, race);
    
    ko.applyBindings(race);

    $.connection.hub.start();
});