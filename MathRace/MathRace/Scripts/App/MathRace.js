//util, print msgs --
function print_msg(msg, style, time) {
    $('#msg').html('<div class="msg msg-' + style + '">' + msg + '</div>');
    $('#msg').fadeIn();
    setTimeout(function () { $('#msg').fadeOut(); }, time || 500);
}

function show_error(msg) { print_msg(msg, 'error'); }

function show_ok(msg) { print_msg(msg, 'ok'); }
//end print msgs --

$(document).ready(function() {

    var raceHub = $.connection.race;

    raceHub.time = function(remaining) {
        race.time(remaining);
    };

    raceHub.newOperation = function(operation) {
        $('#operations').html(operation);
        $('input.input_player').val('').select(); //reset and select.
    };

    raceHub.history = function(history) {
        race.history(history); //bind score history (and sort by date)
    };

    raceHub.scores = function(scores) {
        race.scores(scores);
        if (scores.length) { //effect
            $('.scores').addClass('selected');
            setTimeout(function() {
                $('.scores').removeClass('selected');
            }, 200);
        }
    };

    raceHub.hallOfFame = function(hallOfFame) {
        race.hall_of_fame(hallOfFame);
    };

    raceHub.newGame = function() {
        print_msg('new game, new score, hurry up!!', 'new', 2000);
    };

    raceHub.resultOperation = function(result) {
        if (result == 1)
            print_msg('good!!', 'success');
        else if (result == 2)
            print_msg('other player solved this quest faster than you!', 'warning', 1000);
        else
            print_msg('nooooooooo!', 'error');
    };

    //knockout model and binding

    function Race() {
        var self = this;
        self.name = ko.observable();

        self.input_player1 = ko.observable();
        self.scores = ko.observableArray();
        self.history = ko.observableArray();
        self.hall_of_fame = ko.observableArray();
        self.time = ko.observableArray();

        self.valid_name = ko.computed(function() {
            return (self.name() && self.name().length > 2);
        }, self);

        self.sendOperationResult = function() {
            raceHub.solveOperation(self.input_player1(), self.name());
        };
    }

    var race = new Race();
    ko.applyBindings(race);
    //end knockout model and binding

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

    $.connection.hub.start();
});