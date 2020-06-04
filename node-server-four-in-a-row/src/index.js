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


http_server.listen(process.env.PORT || 2929, function () {

    console.log('Server listening on localhost:' + (process.env.PORT || 2929) + ' port');
    console.log("---server started---");

})