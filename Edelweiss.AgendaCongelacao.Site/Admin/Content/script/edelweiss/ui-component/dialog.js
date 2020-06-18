/**
 * Author: Misael C. Homem
 * Date: 23/10/2019
 * Version: 1.0.0
 * Description: a custom modal (dialogs)
 * Dependencies:
 *      w3.css
 *      jquery 3.4.1
 */

Dialog = {

    self: this

    , containerId: undefined

    , MessageType: {
        ERROR: 'Error'
        , INFORMATION: 'Information'
        , SUCCESS: 'Success'
        , WARNING: 'Warning'
    }

    , attachEvent: function () {
        $(document).keydown(function (e) {
            if (e.keyCode === 27) {
                $('#' + Dialog.containerId).html('');
            }
        });
    }

    , show: function (title, message, messageType, containerId = 'dialogContainer') {

        Dialog.containerId = containerId;
        Dialog.attachEvent();

        var dialogContainer = $('#' + containerId);
        dialogContainer.html('');

        var dialog = $('<div></div>');
        dialog.attr
            ({
                'id': 'edelweiss-admin-custom-dialog'
                , 'class': 'w3-modal'
            });

        var divModalDialog = $('<div></div>');
        divModalDialog.attr
            ({
                'id': 'modalDialog'
                , 'class': 'w3-modal-content w3-animate-top w3-card-4'
            });

        var modalHeader = $('<header></header>');
        modalHeader.attr
            ({
                'id': 'modalHeader'
                , 'class': 'w3-container'
            });

        var modalTitle = $('<h2></h2>');
        modalTitle.attr
            ({
                'id': 'modalTitle'
            });

        modalTitle.text(title);

        var btnCloseX = $('<span></span>');
        btnCloseX.attr
            ({
                'id': 'btnCloseX'
                , 'class':'w3-button w3-large w3-display-topright'
            });
        btnCloseX.html('x');

        btnCloseX.on('click', function () {
            dialogContainer.html('');
        });

        modalHeader.append(btnCloseX);
        modalHeader.append(modalTitle);

        var modalBody = $('<div></div>');
        modalBody.attr
            ({
                'id': 'modalBody'
                , 'class': 'w3-container'
            });

        var pContent = $('<p></p>');
        pContent.attr('id', 'pContent');
        pContent.html(message);

        modalBody.append(pContent);

        var modalFooter = $('<footer></footer>');
        modalFooter.attr
            ({
                'id': 'modalFooter'
                , 'class': 'w3-container w3-border-top'
            });

        var containerButton = $('<div></div>');
        containerButton.attr({
            'id': 'containerButton'
            , 'class': 'w3-margin-top w3-margin-bottom w3-right'
        });

        var btnClose = $('<button></button>');
        btnClose.attr
            ({
                'id': 'btnClose'
                , 'class': 'w3-btn'
            });
        btnClose.css('width', '75px');
        btnClose.text('Ok');
        btnClose.on('click', function () {
            dialogContainer.html('');
        });

        containerButton.append(btnClose);
        modalFooter.append(containerButton);
        divModalDialog.append(modalHeader);
        divModalDialog.append(modalBody);
        divModalDialog.append(modalFooter);

        var defaultHeaderClass = 'w3-container';
        var defaultButtonClass = 'w3-btn';

        switch (messageType) {
            case Dialog.MessageType.ERROR:
                modalHeader.attr('class', defaultHeaderClass + ' w3-red');
                btnClose.attr('class', defaultButtonClass + ' w3-red');
                break;

            case Dialog.MessageType.INFORMATION:
                modalHeader.attr('class', defaultHeaderClass + ' w3-blue');
                btnClose.attr('class', defaultButtonClass + ' w3-blue');
                break;

            case Dialog.MessageType.SUCCESS:
                modalHeader.attr('class', defaultHeaderClass + ' w3-green');
                btnClose.attr('class', defaultButtonClass + ' w3-green');
                break;

            case Dialog.MessageType.WARNING:
                modalHeader.attr('class', defaultHeaderClass + ' w3-yellow');
                btnClose.attr('class', defaultButtonClass + ' w3-yellow');
                break;

            default:
                modalHeader.attr('class', defaultHeaderClass + ' w3-blue');
                btnClose.attr('class', defaultButtonClass + ' w3-blue');
                break;
        }
        
        dialog.append(divModalDialog);
        dialogContainer.append(dialog);
        dialog.css('display', 'block');
        btnClose.focus();
    }
};
