(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('multipleUpload', multipleUpload);

    multipleUpload.$inject = ['svr'];

    function multipleUpload(svr) {
        return function ($scope, $element) {
            $(function() {
                $element.fileinput({
                    language: "zh",
                    uploadUrl: $scope.cur_dt + '/upload',
                    uploadAsync: false,
                    uploadExtraData: function() { //uploadExtraData must be an object or function
                        if (md5.files.length) {
                            return { list: angular.toJson(md5.files) };
                        } else {
                            return '';
                        }
                    },
                    layoutTemplates: {
                        icon: '<span class="glyphicon glyphicon-paperclip kv-caption-icon"></span>',
                        caption: '<div style="width:100%;text-align:center;cursor:default;" class="form-control file-caption {class}">' +
                            '<div class="file-caption-name"></div>' +
                            '</div>'
                    }
                }).on('filebatchselected', function(event, files) {
                    md5.files = [];
                    md5.validated = false;

                    var file_names = '';
                    $.each(files, function() {
                        file_names += this.name + '、';

                        var reader = new FileReader();
                        reader.file = this;
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

                                    md5.files.push({ fileName: this.file.name, md5: current.hex() });
                                } else {
                                    msg(method(event.target.result));
                                }
                            } catch (e) {
                                msg(e);
                            }
                        };
                        reader.readAsArrayBuffer(this);
                    });
                    file_names = file_names.length ? file_names.slice(0, file_names.length - 1) : '';

                    function validate() {
                        if (md5.files.length == files.length) { //所有文件均取到md5值之后
                            var md5s = [];
                            $.each(md5.files, function() {
                                md5s.push(this.md5); //抽取md5属性形成字符串数组
                            });

                            svr.http('ext/filevalidate?list=' + md5s, function(response) {
                                md5.validated = true;
                                var arr = [], names = [];
                                angular.forEach(response.data, function(val) {
                                    $.each(md5.files, function () {
                                        if (this.md5 == val) { //find and delete file
                                            if (!names.exist(this.fileName)) {
                                                arr.push(this);
                                                names.push(this.fileName);
                                            }
                                            return false;
                                        }
                                    });
                                });
                                md5.files = arr;

                                if (names.length) {
                                    msg('服务器已存在相同的文件：<strong style="color:#87EC04">' + names.join('、') + '</strong>', 3000);    
                                }
                            });

                            return;
                        }
                        setTimeout(validate, 100);
                    };

                    validate();

                    var $caption = $('.file-input .file-caption-name');
                    if (files.length > 1) { //只选择一个文件时，文件名会在.file-caption-name上显示，故不需要设置file_names
                        $caption.attr('title', file_names)
                            .html('<i class="glyphicon glyphicon-file kv-caption-icon"></i>&nbsp;&nbsp;已选择&nbsp;<strong>' +
                                files.length + '</strong>&nbsp;个文件');
                    }

                    if ($caption.hasClass('tooltipstered')) {
                        $caption.tooltipster('destroy');
                    }

                    $caption.tooltipster({
                        animation: 'fall',
                        delay: 100,
                        theme: 'tooltipster-shadow',
                        side: ['top', 'left', 'right', 'bottom'],
                        distance: 15
                    });
                }).on('filebatchuploadsuccess', function (event, data) {
                    $.each(md5.files, function (i) {  //匹配值
                        $.each(data.files, function (j) {  //待处理值
                            if (this.name == md5.files[i].fileName) {
                                data.files.splice(j, 1);
                                return false;
                            }
                        });
                    });

                    if (data.files.length > 1) {
                        msg(data.files.length + '个文件已全部上传成功');
                    } else {
                        msg(data.files.the_first().name + ' 上传成功');
                    }
                }).on('filebatchuploadcomplete', function() {
                    $element.fileinput('refresh');
                });
            });
        }
    }
})();