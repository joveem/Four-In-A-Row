var socket_io = require('socket.io');

var { AddConnection, SetUpConnectionMinigame, RemoveConnection, GetFiarPublicQueue, SetFiarPublicQueue, FiarQueuePlayer, FiarAddGame } = require('../model/socket_connections');

var connections = [];
var fiar_public_queue = [];

var games = [];

var io;

exports.setUpWebSockets = function (server) {

    io = socket_io(server);

    io.on('connection', function (socket) {

        AddConnection(socket.id);

        console.log("- CONNECTION ( " + connections.length + " players connected )");

        socket.emit("connecting", {});

        socket.on("send-configs-to-server", function (pack) {

            var connection_already_exists = SetUpConnectionMinigame(socket.id, pack.player_id, pack.minigame_number);

            socket.emit("send-connection-result-to-client", { result: !connection_already_exists ? "succeed" : "failed" });

        })

        socket.on("fiar-enter-random-room", function (pack) {

            FiarQueuePlayer(socket.id, pack.langs);

        })

        socket.on("send-position-to-server", function (pack) {

            socket.broadcast.emit("send-position-to-client", pack);

        })

        socket.on("send-piece-move-to-server", function (pack) {

            socket.broadcast.emit("send-piece-move-to-client", pack);

        })


        socket.on("disconnect", function () {

            RemoveConnection(socket.id);

        });

    });

}

exports.FiarDealWithPublicQueue = function () {

    var matchs = [];
    var socket_ids_to_remove = [];

    var fiar_public_queue = GetFiarPublicQueue();

    fiar_public_queue.map(function (value_1) {

        if (!value_1.combined) {

            fiar_public_queue.map(function (value_2) {

                if (value_2.socket_id != value_1.socket_id) {

                    if (!value_2.combined) {

                        var intersection = value_2.langs.filter(function (x) {

                            console.log("--- " + x + " : " + value_1.langs[value_1.langs.indexOf(x)]);

                            if (value_1.langs.indexOf(x) != -1) {

                                console.log("-/- " + x + " : " + value_1.langs[value_1.langs.indexOf(x)]);

                                return true;

                            } else {

                                console.log("-\\- " + x + " : " + value_1.langs[value_1.langs.indexOf(x)]);

                                return false;

                            }

                        })

                        console.log(intersection);

                        if (intersection.length != 0) {

                            console.log("--- TRUE")

                            value_1.combined = true;
                            value_2.combined = true;

                            matchs.push({
                                socket_id_1: value_1.socket_id,
                                socket_id_2: value_2.socket_id
                            })

                            socket_ids_to_remove.push(value_1.socket_id, value_2.socket_id);

                        }

                    }

                }

            })

        }

    })

    matchs.map(function (value) {

        io.to(value.socket_id_1).emit("open-game", { player_number: 1 });
        io.to(value.socket_id_2).emit("open-game", { player_number: 2 });

        var room_name = value.socket_id_1 + "-" + value.socket_id_2;

        io.sockets.connected[value.socket_id_1].join(room_name);
        io.sockets.connected[value.socket_id_2].join(room_name);

        io.sockets.connected[value.socket_id_1].join("room 2");
        io.sockets.connected[value.socket_id_1].join("room 3");
        io.sockets.connected[value.socket_id_1].join("room 4");

        console.log("------")
        console.log(io.sockets.connected[value.socket_id_1].rooms)
        let rooms = Object.keys(io.sockets.connected[value.socket_id_1].rooms);
        console.log("------")
        console.log(rooms);
        console.log("------")

        FiarAddGame(room_name);

        console.log("- starting game for " + value.socket_id_1 + " and " + value.socket_id_2);

    })

    socket_ids_to_remove.map(function (value) {

        fiar_public_queue.splice(fiar_public_queue.findIndex(x => x.socket_id == value), 1);

        console.log("( " + value + " removed from queue )");

    })

    SetFiarPublicQueue(fiar_public_queue);

}

exports.UpdatePlayersQuantity = function (players_quant_) {

    io.emit("update-players-quantity", { players_quant: players_quant_ });

}
