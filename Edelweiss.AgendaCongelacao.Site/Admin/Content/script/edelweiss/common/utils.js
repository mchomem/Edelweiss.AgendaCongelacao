Utils = {

    self: this

    , onlyNumbers: function (text) {

        text = text.match(/\d+/g).join([]);

        return text;
    }

}