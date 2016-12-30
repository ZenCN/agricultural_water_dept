(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('acceptanceMaterial', acceptanceMaterial);

    //acceptanceMaterial.$inject = [];

    function acceptanceMaterial() {
        return function($scope, $element) {
            $(function() {
                $element.click(function() {
                    easyDialog.open({
                        container: {
                            header: '验收资料',
                            content: $('#acceptance_material').html(),
                            yesFn: function() {
                                $('#overlay').fadeOut('normal', function() {
                                    msg('保存成功！');
                                })
                            },
                            noFn: true
                        }
                    });

                    $('#easyDialogWrapper input[type=file]').fileinput({
                        language: "zh",
                        uploadUrl: 'dt01/upload',
                        showRemove: false,
                        uploadAsync: false,
                        layoutTemplates: {
                            btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                            caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:114px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                        }
                    }).on('filebatchuploadsuccess', function(event, data) {
                        msg(data.files.the_first().name + ' 上传成功');
                    }).on('filebatchuploadcomplete', function() {
                        $element.fileinput('refresh');
                    });
                });
            });
        }
    }
})();