exports.setUpModel = function () {

    var { OnSocketConnectionsStart } = require('./socket_connections');
    var { OnUsersStart } = require('./users');

    OnSocketConnectionsStart();
    OnUsersStart();

}