var users = [];

exports.OnUsersStart = function () {

    users = [
        {
            id: "1001",
            username: "joveem",
            email: "joveem@gmail.com",
            password: "123321"
        }, {
            is: "1002",
            nick: "lysgd",
            email: "lysgd@gmail.com",
            password: "123123"
        }
    ];

}

exports.GetUserById = function (id_) {

    var user_ = undefined;

    users.map(function (value) {

        if (id_ == value.id) {

            user_ = value;

        }
    })

    return user_;

};

exports.GetUserByUsername = function (username_) {

    var user_ = undefined;

    users.map(function (value) {

        if (username_ == value.nick || username_ == value.email) {

            user_ = value;

        }
    })

    return user_;

};