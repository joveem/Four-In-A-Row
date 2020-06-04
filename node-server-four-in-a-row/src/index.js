var express = require('express');
var bodyParser = require('body-parser');
var express_app = express();
var http_server = require('http').Server(express_app);
express_app.use(bodyParser.json());
express_app.use(bodyParser.urlencoded({ extended: false }));
express_app.use(express.static("__dirname"));


var { setUpWebSockets } = require('./controller/websocket');
var { setUpModel } = require('./model/setup_model')


setUpModel();
setUpWebSockets(http_server);

express_app.get('/', function (req, res) {

    res.send('Hello World!');

});

require('./controller/auth')(express_app);


http_server.listen(2929, function () {

    console.log('Server listening on localhost:2929 port');

})

express_app.listen(2928, function () {

    console.log('API listening on localhost:2928 port');

})

console.log("---server started---");