(function() {
    'use strict';

    angular
        .module('app.widget')
        .directive('singUpload', singUpload);

    //singUpload.$inject = [];

    function singUpload() {
        return function($scope, $element) {
            $(function() {
                $element.fileinput({
                    language: "zh",
                    uploadUrl: 'dt01/upload',
                    showRemove: false,
                    showUpload: false,
                    uploadAsync: false,
                    layoutTemplates: {
                        btnBrowse: '<div tabindex="500" class="{css}"{status}>{icon}</div>',
                        caption: '<div class="form-control file-caption {class}">' +
                            '<div style="width:358px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;" class="file-caption-name"></div>' +
                            '</div>'
                    }
                });
            });
        }
    }
})();