/**
 * Author: Misael C. Homem
 * Date: 24/09/2019
 * Version: 1.0.0
 * Description: a custom modal (dialogs)
 * Dependencies:
 *      Bootstrap 4.3.1
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

    , show: function (title, message, messageType, containerId = 'dialogContainer') {

        Dialog.containerId = containerId;

        var dialogContainer = $('#' + containerId);
        dialogContainer.html('');

        var dialog = $('<div></div>');
        dialog.attr
            ({
                'id': 'edelweiss-custom-dialog'
                , 'class': 'modal fade show'
                , 'tabindex': '-1'
                , 'role': 'dialog'
                , 'aria-hidden': 'true'
            });

        var divModalDialog = $('<div></div>');
        divModalDialog.attr
            ({
                'id': 'modalDialog'
                , 'class': 'modal-dialog'
                , 'role': 'document'
            });

        var modalContent = $('<div></div>');
        modalContent.attr
            ({
                'id': 'modalContent'
                , 'class': 'modal-content'
            });

        var modalHeader = $('<div></div>');
        modalHeader.attr
            ({
                'id': 'modalHeader'
                , 'class': 'modal-header text-white'
            });

        var modalTitle = $('<h5></h5>');
        modalTitle.attr
            ({
                'id': 'modalTitle'
                , 'class': 'modal-title'
            });

        modalTitle.text(title);

        var btnCloseX = $('<button></button>');
        btnCloseX.attr
            ({
                'id': 'btnCloseX'
                , 'type': 'button'
                , 'class': 'close'
                , 'data-dismiss': 'modal'
                , 'aria-label': 'Close'
            });
        btnCloseX.on('click', function () {
            dialogContainer.html('');
        });

        var iconX = $('<span></span>');
        iconX.attr('aria-hidden', 'true');
        iconX.html('&times;');

        btnCloseX.append(iconX);

        modalHeader.append(modalTitle);
        modalHeader.append(btnCloseX);

        var modalBody = $('<div></div>');
        modalBody.attr
            ({
                'id': 'modalBody'
                , 'class': 'modal-body'
            });

        var pContent = $('<p></p>');
        pContent.attr('id', 'pContent');
        pContent.html(message);

        modalBody.append(pContent);

        var modalFooter = $('<div></div>');
        modalFooter.attr
            ({
                'id': 'modalFooter'
                , 'class': 'modal-footer py-2'
            });

        var btnClose = $('<button></button>');
        btnClose.attr
            ({
                'id': 'btnClose'
                , 'class': 'btn'
            });
        btnClose.css('width','75px');
        btnClose.text('Ok');
        btnClose.on('click', function () {
            dialogContainer.html('');
        });

        modalFooter.append(btnClose);
        modalContent.append(modalHeader);
        modalContent.append(modalBody);
        modalContent.append(modalFooter);

        var defaultHeaderClass = 'modal-header text-white py-2';
        var defaultButtonClass = 'btn btn-sm text-white';

        switch (messageType) {
            case Dialog.MessageType.ERROR:
                modalHeader.attr('class', defaultHeaderClass + ' bg-danger');
                btnClose.attr('class', defaultButtonClass +  ' bg-danger');
                break;

            case Dialog.MessageType.INFORMATION:
                modalHeader.attr('class', defaultHeaderClass + ' bg-primary');
                btnClose.attr('class', defaultButtonClass + ' bg-primary');
                break;

            case Dialog.MessageType.SUCCESS:
                modalHeader.attr('class', defaultHeaderClass + ' bg-success');
                btnClose.attr('class', defaultButtonClass + ' bg-success');
                break;

            case Dialog.MessageType.WARNING:
                modalHeader.attr('class', defaultHeaderClass + ' bg-warning');
                btnClose.attr('class', defaultButtonClass + ' bg-warning');
                break;

            default:
                modalHeader.attr('class', defaultHeaderClass + ' bg-primary');
                btnClose.attr('class', defaultButtonClass + ' bg-primary');
                break;
        }

        divModalDialog.append(modalContent);
        dialog.append(divModalDialog);
        dialogContainer.append(dialog);

        dialog.css('display', 'block');
    }

};
