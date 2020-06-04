var express = require('express');

var router = express.Router();

var { GetUserById, GetUserByUsername } = require('../model/users');


router.post("/login", /* async */ function(req, res){

    var { login, password } = req.body;

    var user = GetUserByUsername(login);

    if( user != undefined){

        if( user.password == password ){

            // send code for user authenticated (1) + user id 
            res.send("1," + user.id);

        }else{

            // send code for user not authenticated (2) + 0
            res.send("2,0");

        }

    }else{

        // send code for user not found (0) + 0
        res.send("0,0");

    }

})

module.exports = (app) => app.use('/auth', router)