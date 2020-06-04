var connections = [];
var fiar_public_queue = [];

var games = [];

var { UpdatePlayersQuantity, FiarDealWithPublicQueue } = require('../controller/websocket');

exports.OnSocketConnectionsStart = function(){

    var websocket_controller = require('../controller/websocket');

    UpdatePlayersQuantity = websocket_controller.UpdatePlayersQuantity;
    FiarDealWithPublicQueue = websocket_controller.FiarDealWithPublicQueue;

}

exports.AddConnection = function (socket_id_) {

    connections.push({

        socket_id: socket_id_,

        player_infos: {

            id: undefined,
            minigame_id: undefined,
            state: "connecting",
            minigame_configs: {}

        }

    })

}

/** Give player data to the connection if don't have other connection with this user id.
 * @param {string} socket_id_ The socket id.
 * @param {string} player_id_ The player id.
 * @param {string} minigame_id_ The minigame id.
 * @returns {boolean} true if already exists a connection with this user id, and false if not.
 */

exports.SetUpConnectionMinigame = function (socket_id_, player_id_, minigame_id_) {

    var connection_already_exists = false;

    connections.map(function (value) {

        if (value.player_infos.id == player_id_) {

            connection_already_exists = true;

        }

    })

    if (!connection_already_exists) {


        connections.map(function (value) {

            if (value.socket_id == socket_id_) {

                value.player_infos.minigame_id = minigame_id_;
                value.player_infos.id = player_id_;
                value.player_infos.state = "connected";

                UpdatePlayersQuantity(connections.length);
                console.log("- " + (minigame_id_ == "6" ? "FIAR" : "ERROR") + ` ( ${value.player_infos.id} : ${value.socket_id} )`);

                if (minigame_id_ == "6") {

                    value.player_infos.minigame_configs = {

                        state: "in_lobby",
                        privacy: undefined,
                        room_id: undefined,
                        room_lang: undefined

                    };

                }

            }
        })

    } else {

        RemoveConnection(socket_id_);

    }

    return connection_already_exists;

}

exports.GetFiarPublicQueue = function () {

    return fiar_public_queue;

}

exports.SetFiarPublicQueue = function (fiar_public_queue_) {

    fiar_public_queue = fiar_public_queue_;

}

exports.RemoveConnection = function (socket_id_) {

    connections = connections.filter(function (value) {

        return value.socket_id != socket_id_;

    })

    fiar_public_queue = fiar_public_queue.filter(function (value) {

        return value.socket_id != socket_id_;

    })

    UpdatePlayersQuantity(connections.length);
    console.log("- DISCONNECTION ( " + connections.length + " players connected )");

}

exports.FiarQueuePlayer = function (socket_id_, lang_) {

    fiar_public_queue.push({

        socket_id: socket_id_,
        combined: false,
        langs: lang_.split(',')

    })

    console.log("- player queued in FIAR ( " + lang_ + " )");

    FiarDealWithPublicQueue();

}

exports.FiarAddGame = function (room_name_) {

    games.push({

        id: room_name_,
        minigame_id: 6,
        table_rows: ["0000000", "0000000", "0000000", "0000000", "0000000", "0000000"]

    })

}