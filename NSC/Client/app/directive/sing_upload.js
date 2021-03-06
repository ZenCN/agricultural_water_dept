﻿(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('singUpload', singUpload);

    //singUpload.$inject = [];

    function singUpload() {
        return function($scope, $element, $attr) {
            $(function() {
                $element.fileinput({
                    language: "zh",
                    uploadUrl: 'dt04/upload',
                    showRemove: false,
                    showUpload: false,
                    uploadAsync: false,
                    allowedFileExtensions: ['doc', 'docx', 'xls', 'xlsx', 'ppt', 'pptx', 'pdf'],
                    layoutTemplates: {
                        btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                        caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:358px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                    }
                }).on('filebatchselected', function (event, files) {
                    if (files.length > 0) {
                        var reader = new FileReader();
                        reader.id = $attr.id;
                        reader.onload = function(event) {
                            try {
                                if (md5.update) {
                                    var batch = 1024 * 1024;
                                    var start = 0;
                                    var current = md5;

                                    while (start < event.total) {
                                        var end = Math.min(start + batch, event.total);
                                        current = current.update(event.target.result.slice(start, end));
                                        start = end;

                                    }

                                    md5[reader.id] = {
                                        value: current.hex()
                                    };
                                } else {
                                    msg(method(event.target.result));
                                }
                            } catch (e) {
                                msg(e);
                            }
                        };
                        reader.readAsArrayBuffer(files.the_first());
                    }
                }).on('filebatchuploadsuccess', function (event, data) {
                    if (isString(data.response)) {
                        md5[$attr.id] = JSON.parse(data.response);

                        if (angular.isObject(md5[$attr.id])) {
                            md5[$attr.id].uploaded = true;
                        } else {
                            throw msg(data.response);
                        }
                    }
                });
            });
        }
    }
})();