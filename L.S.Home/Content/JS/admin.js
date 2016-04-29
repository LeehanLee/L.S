/// <reference path="outjs/formValidate.js" />
/// <reference path="F:\MySolution\L.Study\L.S.Home\Scripts/jquery-1.12.0.js" />
/// <reference path="F:\MySolution\L.Study\L.S.Home\datetimepicker/js/locales/bootstrap-datetimepicker.zh-CN.js" />
/// <reference path="F:\MySolution\L.Study\L.S.Home\Scripts/jquery.validate.js" />
/// <reference path="outjs/bootbox.js" />
$(function () {
    //让底部始终在底部
    LS.admin.myfn.keepfootbuttom();
    $(window).resize(function () { LS.admin.myfn.keepfootbuttom(); });
    $(window).scroll(function () { LS.admin.myfn.keepfootbuttom(); });

    $('.form_datetime').datetimepicker({
        language: 'zh-CN',
        weekStart: 1,
        todayBtn: 1,
        autoclose: 1,
        todayHighlight: 1,
        format: 'yyyy-mm-dd hh:ii:ss',
        pickerPosition: "bottom-left"
    });

    //表单提交的时候进行验证
    $("form").submit(function () {
        var result = formValidate.validateForm("#"+this.id);
        return result;
    });

    //table中单击行可选中前面的复选框
    $(".ls-table tr").click(function () {
        var $this = $(this);
        var selectthis = $this.find("input[type=checkbox].ls-table-selecttr");
        //本来想这里用trigger("click")，无奈IE8貌似不支持这个函数
        selectthis.click();
    });

    //点击复选框自己的时候阻止事件再冒泡到父级的tr上，如果不阻止，则点击复选框自己的时候将没有效果，因为相当于触发了两次点击事件，清除了点击效果
    $("input[type='radio'].ls-table-selecttr, input[type='checkbox'].ls-table-selecttr").click(function (event) {
        event.stopPropagation();
    });
    $(".ls-table tr .btn").click(function (event) {
        event.stopPropagation();
    });

    //全选
    $(".ls-table .ls-table-selectall").click(function () {
        var $this = $(this);
        if ($this.is(':checked')) {
            $(".ls-table input[type='radio'].ls-table-selecttr,.ls-table input[type='checkbox'].ls-table-selecttr").prop("checked",true);
        } else {
            $(".ls-table input[type='radio'].ls-table-selecttr,.ls-table input[type='checkbox'].ls-table-selecttr").prop("checked", false);
        }
        
    });

    $(".ls-table-delete").click(function () {
        var deleteurl = $(this).attr("url");
        if (deleteurl == null) {
            bootbox.alert("敢问将要往哪里删除？");
            return false;
        } else {
            var selectedlist = $(".ls-table input[type='radio']:checked.ls-table-selecttr,.ls-table input[type='checkbox']:checked.ls-table-selecttr");
            if (selectedlist.length <= 0) {
                bootbox.alert("请选择要删除的数据");
                return false;
            } else {
                var selectedidarray = [];
                selectedlist.each(function () {
                    var $this = $(this);
                    selectedidarray.push($this.val());
                });
                var ids = selectedidarray.join(',');
                $.post(deleteurl, { ids: ids }, LS.admin.myfn.AjaxSuccess);
            }
        }
    });
    $(".ls-table-available").click(function () {
        var deleteurl = $(this).attr("url");
        if (deleteurl == null) {
            bootbox.alert("敢问将要往哪里启用？");
            return false;
        } else {
            var selectedlist = $(".ls-table input[type='radio']:checked.ls-table-selecttr,.ls-table input[type='checkbox']:checked.ls-table-selecttr");
            if (selectedlist.length <= 0) {
                bootbox.alert("请选择要启用的数据");
                return false;
            } else {
                var selectedidarray = [];
                selectedlist.each(function () {
                    var $this = $(this);
                    selectedidarray.push($this.val());
                });
                var ids = selectedidarray.join(',');
                $.post(deleteurl, { ids: ids }, LS.admin.myfn.AjaxSuccess);
            }
        }
    });
    $(".ls-table-unavailable").click(function () {
        var unavailableurl = $(this).attr("url");
        if (unavailableurl == null) {
            bootbox.alert("敢问将要往哪里禁用？");
            return false;
        } else {
            var selectedlist = $(".ls-table input[type='radio']:checked.ls-table-selecttr,.ls-table input[type='checkbox']:checked.ls-table-selecttr");
            if (selectedlist.length <= 0) {
                bootbox.alert("请选择要禁用的数据");
                return false;
            } else {
                var selectedidarray = [];
                selectedlist.each(function () {
                    var $this = $(this);
                    selectedidarray.push($this.val());
                });
                var ids = selectedidarray.join(',');
                $.post(unavailableurl, { ids: ids }, LS.admin.myfn.AjaxSuccess);
            }
        }
    });
});

var LS = {
    admin: {
        myfn: {
            LoginBegin: function () {
                $(".btn[type=submit]").button('loading');
            },
            LoginFailure: function (result) {
                bootbox.alert(result.msg || "服务器内部发生错误，请与开发人员联系");
                $(".btn[type=submit]").button('reset');
            },
            LoginSuccess: function (result) {
                if (result.success) {
                    $(".btn[type=submit]").button('complete');
                    if (result.url) {
                        window.location.href = result.url;
                    }
                } else {
                    $(".btn[type=submit]").button('reset');
                    bootbox.alert(result.msg || "操作失败");
                }
            },
            AjaxSuccess: function (result) {
                $(".form-actions .btn[type=submit]").button('complete');
                if (result.success) {
                    if (result.msg) {
                        bootbox.alert({
                            str: result.msg, cb: function (e) {
                                if (result.url) {
                                    window.location.href = result.url;
                                }
                            }
                        });
                    }
                } else {
                    $(".form-actions .btn[type=submit]").button('reset');
                    bootbox.alert(result.msg || "操作失败");
                }
            },
            AjaxBegin: function (selector) {
                $(".form-actions .btn[type=submit]").button('loading');
            },
            AjaxFailure: function (result) {
                bootbox.alert(result.msg || "服务器内部发生错误，请与开发人员联系");
                $(".form-actions .btn[type=submit]").button('reset');
            },
            keepfootbuttom: function () {
                if ($(window).height() > $(document.body).height()) {
                    $(".footer").addClass("navbar-fixed-bottom");
                } else {
                    $(".footer").removeClass("navbar-fixed-bottom");
                }
            },

            level2menuposition: function () {
                var scrollTop = $(document).scrollTop()
                console.log(scrollTop);
                if (scrollTop > 160) {
                    $(".level2-menu-container").addClass("affix").removeClass("affix-top");
                } else {
                    $(".level2-menu-container").removeClass("affix").addClass("affix-top");
                }
            },

            //部门树
            dialogeopendeptree: function (obj, url) {
                if (typeof obj == "string") {
                    var $this = $("#" + obj);
                    var $thisname = $("#" + obj + "name");
                }
                if (typeof obj == "object") {
                    $this = $("#" + obj.id);
                    $thisname = $("#" + obj.name);
                }
                var bodyid = "deptreebody";
                var searchboxid = "depsearchbox";
                var deptreedialoge = bootbox.alert({ header: "请选择", bodyid: bodyid, searchboxid: searchboxid, toggledisplay: true, width: 560, height: 500, classes: "treecontainer" });

                url = $.setQueryStr(url, "selectNodeID", $this.val());
                $("#" + bodyid).jstree({
                    "core": {
                        "multiple": false,//不允许多选
                        'data': {
                            'url': url,
                            'data': function (node) {
                                return { 'id': node.id, text: node.text };
                            }
                        }
                    },
                    //"plugins": ["search", "state"]
                    "plugins": ["search"]
                });
                $("#" + bodyid).jstree().toggle_icons();
                var to = false;
                $('#' + searchboxid).keyup(function () {
                    if (to) { clearTimeout(to); }
                    to = setTimeout(function () {
                        var v = $('#' + searchboxid).val();
                        $('#' + bodyid).jstree(true).search(v);
                    }, 250);
                });
                //展开与折叠功能
                $("#toggle_all" + bodyid).click(function () {
                    var $this = $(this);
                    var treestate = $this.attr("treestate");
                    if (treestate == "close") {
                        $("#" + bodyid).jstree().open_all();
                        $this.attr("treestate", "open");
                        $this.text("折叠");
                    }
                    if (treestate == "open") {
                        $("#" + bodyid).jstree().close_all();
                        $this.attr("treestate", "close");
                        $this.text("展开");
                    }
                });
                $(deptreedialoge).bind("hide", function () {
                    var allselected = $("#" + bodyid).jstree().get_selected(true);
                    //if (allselected.length <= 0 && $this.val().trim().length <= 0) {
                    //    bootbox.alert("请选择部门");
                    //    return false;
                    //}
                    var depids = [];
                    var names = [];
                    $(allselected).each(function (i) {
                        depids.push(allselected[i].id);
                        names.push(allselected[i].text);
                    });
                    //if (depids.length > 0) {
                    $this.val(depids.join(','));
                    $thisname.val(names.join(','));
                    $thisname.text(names.join(','));
                    //}
                });

            },

            //权限树
            dialogeopenrighttree: function (obj, url) {
                if (typeof obj == "string") {
                    var $this = $("#" + obj);
                    var $thisname = $("#" + obj + "name");
                }
                if (typeof obj == "object") {
                    $this = $("#" + obj.id);
                    $thisname = $("#" + obj.name);
                }
                var bodyid = "righttreebody";
                var searchboxid = "rightsearchbox";
                var righttreedialoge = bootbox.alert({ header: "请选择", bodyid: bodyid, searchboxid: searchboxid, toggledisplay: obj.toggledisplay, toggleselect: obj.toggleselect, width: 560, height: 500, classes: "treecontainer" });

                url = $.setQueryStr(url, "selectNodeID", $this.val());
                $("#" + bodyid).jstree({
                    "checkbox": {
                        //"keep_selected_style": true,
                        "three_state": false,
                    },
                    "core": {
                        "multiple": obj.multiple == undefined ? true : obj.multiple,
                        'data': {
                            'url': url,
                            'data': function (node) {
                                return { 'id': node.id, text: node.text };
                            }
                        }
                    },
                    //"plugins": ["search", "state"]                    
                    "plugins": ["checkbox", "search"]
                });
                $("#" + bodyid).jstree().toggle_icons();
                var to = false;
                $('#' + searchboxid).keyup(function () {
                    if (to) { clearTimeout(to); }
                    to = setTimeout(function () {
                        var v = $('#' + searchboxid).val();
                        $('#' + bodyid).jstree(true).search(v);
                    }, 250);
                });
                //展开与折叠功能
                $("#toggle_all" + bodyid).click(function () {
                    var $this = $(this);
                    var treestate = $this.attr("treestate");
                    if (treestate == "close") {
                        $("#" + bodyid).jstree().open_all();
                        $this.attr("treestate", "open");
                        $this.text("折叠");
                    }
                    if (treestate == "open") {
                        $("#" + bodyid).jstree().close_all();
                        $this.attr("treestate", "close");
                        $this.text("展开");
                    }
                });
                //全选与取消全选功能
                $("#toggle_select_all" + bodyid).click(function () {
                    var $this = $(this);
                    var treestate = $this.attr("treestate");
                    if (treestate == "unselected") {
                        $("#" + bodyid).jstree().select_all();
                        $this.attr("treestate", "selected");
                        $this.text("取消");
                    }
                    if (treestate == "selected") {
                        $("#" + bodyid).jstree().deselect_all();
                        $this.attr("treestate", "unselected");
                        $this.text("全选");
                    }
                });
                $(righttreedialoge).bind("hide", function () {
                    var allselected = $("#" + bodyid).jstree().get_selected(true);
                    //if (allselected.length <= 0 && $this.val().trim().length <= 0) {
                    //    bootbox.alert("请选择权限");
                    //    return false;
                    //}
                    var rightids = [];
                    var names = [];
                    $(allselected).each(function (i) {
                        rightids.push(allselected[i].id);
                        names.push(allselected[i].text);
                    });
                    //if (rightids.length > 0) {
                    $this.val(rightids.join(','));
                    $thisname.val(names.join(','));
                    $thisname.text(names.join(','));
                    //}
                });

            },

            //角色树
            dialogeopenroletree: function (obj, url) {
                if (typeof obj == "string") {
                    var $this = $("#" + obj);
                    var $thisname = $("#" + obj + "name");
                }
                if (typeof obj == "object") {
                    $this = $("#" + obj.id);
                    $thisname = $("#" + obj.name);
                }
                var bodyid = "roletreebody";
                var searchboxid = "rolesearchbox";
                var roletreedialoge = bootbox.alert({ header: "请选择", bodyid: bodyid, searchboxid: searchboxid, toggledisplay: obj.toggledisplay, toggleselect: obj.toggleselect, width: 560, height: 500, classes: "treecontainer" });

                url = $.setQueryStr(url, "selectNodeID", $this.val());
                $("#" + bodyid).jstree({
                    "checkbox": {
                        //"keep_selected_style": true,
                        "three_state": false,
                    },
                    "core": {
                        "multiple": obj.multiple == undefined ? true : obj.multiple,
                        'data': {
                            'url': url,
                            'data': function (node) {
                                return { 'id': node.id, text: node.text };
                            }
                        }
                    },
                    //"plugins": ["search", "state"]                    
                    "plugins": ["checkbox", "search"]
                });
                $("#" + bodyid).jstree().toggle_icons();
                var to = false;
                $('#' + searchboxid).keyup(function () {
                    if (to) { clearTimeout(to); }
                    to = setTimeout(function () {
                        var v = $('#' + searchboxid).val();
                        $('#' + bodyid).jstree(true).search(v);
                    }, 250);
                });
                //展开与折叠功能
                $("#toggle_all" + bodyid).click(function () {
                    var $this = $(this);
                    var treestate = $this.attr("treestate");
                    if (treestate == "close") {
                        $("#" + bodyid).jstree().open_all();
                        $this.attr("treestate", "open");
                        $this.text("折叠");
                    }
                    if (treestate == "open") {
                        $("#" + bodyid).jstree().close_all();
                        $this.attr("treestate", "close");
                        $this.text("展开");
                    }
                });
                //全选与取消全选功能
                $("#toggle_select_all" + bodyid).click(function () {
                    var $this = $(this);
                    var treestate = $this.attr("treestate");
                    if (treestate == "unselected") {
                        $("#" + bodyid).jstree().select_all();
                        $this.attr("treestate", "selected");
                        $this.text("取消");
                    }
                    if (treestate == "selected") {
                        $("#" + bodyid).jstree().deselect_all();
                        $this.attr("treestate", "unselected");
                        $this.text("全选");
                    }
                });
                $(roletreedialoge).bind("hide", function () {
                    var allselected = $("#" + bodyid).jstree().get_selected(true);
                    //if (allselected.length <= 0 && $this.val().trim().length <= 0) {
                    //    bootbox.alert("请选择角色");
                    //    return false;
                    //}
                    var roleids = [];
                    var names = [];
                    $(allselected).each(function (i) {
                        roleids.push(allselected[i].id);
                        names.push(allselected[i].text);
                    });
                    //if (roleids.length > 0) {
                    $this.val(roleids.join(','));
                    $thisname.val(names.join(','));
                    $thisname.text(names.join(','));
                    //}
                });

            },
        }
    }
};

$.extend({
    //获取url中的参数
    request: function (name, url) {
        var vars = [], hash;
        var url = url || window.location.href;
        var hashes = url.slice(url.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = decodeURIComponent(hash[1]);
        }
        if (name && name != "") {
            return vars[name];
        } else {
            return vars;
        }
    },
    setQueryStr: function (url, name, value) {
        if (value) {
            var qv = $.request(name, url);
            var qparam = name + "=" + value;
            if (qv) {
                return url.replace(name + "=" + qv, qparam);
            } else {
                return url + (url.indexOf("?") == -1 ? "?" : "&") + qparam;
            }
        }
        return url;
    },
});

/*
 *	通用JS验证类
 *	使用方法：
 *
 *  <input name="" type="text" validate="zip_code" empty="yes" min=10 max=10 /><span></span>
 *	validate="zip_code"		验证是否是邮政编码
 * 	empty="yes"				验证是否允许为空
 *	min=10					最小长度
 * 	max=10					最大长度
 *	<span></span>			显示提示内容
 */
var formValidate = window.formValidate || (function (document, $) {

    var that = {};

    that.options = {
        number: { reg: /^[0-9]+$/, str: '必须为数字' },
        decimal: { reg: /^[-]{0,1}(\d+)[\.]+(\d+)$/, str: '必须为DECIMAL格式' },
        english: { reg: /^[A-Za-z]+$/, str: '必须为英文字母' },
        upper_english: { reg: /^[A-Z]+$/, str: '必须为大写英文字母' },
        lower_english: { reg: /^[a-z]+$/, str: '必须为小写英文字母' },
        email: { reg: /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/, str: 'Email格式不正确' },
        chinese: { reg: /[\u4E00-\u9FA5\uf900-\ufa2d]/ig, str: '必须含有中文' },
        url: { reg: /^[a-zA-z]+:\/\/[^s]*/, str: 'URL格式不正确' },
        mobile: { reg: /^1[345678]\d{9}$/, str: '手机号码格式不正确' },
        ip: { reg: /^(\d+)\.(\d+)\.(\d+)\.(\d+)$/, str: 'IP地址格式不正确' },
        money: { reg: /^[0-9]+[\.][0-9]{0,3}$/, str: '金额格式不正确' },
        number_letter: { reg: /^[0-9a-zA-Z\_]+$/, str: '只允许输入英文字母、数字、_' },
        zip_code: { reg: /^[a-zA-Z0-9 ]{3,12}$/, str: '邮政编码格式不正确' },
        account: { reg: /^[a-zA-Z][a-zA-Z0-9_]{4,15}$/, str: '账号名不合法，允许5-16字符，字母下划线和数字' },
        qq: { reg: /[1-9][0-9]{4,}/, str: 'QQ账号不正确' },
        card: { reg: /^(\d{6})(18|19|20)?(\d{2})([01]\d)([0123]\d)(\d{3})(\d|X)?$/, str: '身份证号码不正确' },
        datetime: { reg: /^([12]\d{3})[-/](\d|[0][1-9]|[1][0-2])[-/](\d|[0-2][0-9]|[3][0-1])\s{1}(\d|[0-1][0-9]|[2][0-3]):(([0-5][0-9])|([0-5][0-9]:\d{1,2}))$/, str: '请输入正确的日期格式，如1997-02-15 12:25' },
        date: { reg: /^([12]\d{3})-([0][1-9]|[1][0-2])-([0-2][0-9]|[3][0-1])$/, str: '请输入正确的日期格式如：1997-02-15' },
        time: { reg: /^(\d|[0-1][0-9]|[2][0-3]):([0-5][0-9])$/, str: '请输入正确的时间格式，如12:25' },
    };

    //设置参数
    that.setOptions = function (options) {
        for (var key in options) {
            if (key in this.options) {
                this.options[key] = options[key];
            }
        }
    };

    that.validateForm = function (selector) {
        var formChind = $(selector).find("input[validate],textarea[validate],select[validate]");
        var testResult = true;
        formChind.each(function (i) {
            var child = formChind.eq(i);
            var value = child.val();
            var len = value.length;
            var childSpan = child.siblings("span.ls-msg").first();
            childSpan.html('');
            child.closest(".control-group").addClass("success").removeClass("error");
            var msg = child.attr("msg");
            //属性中是否为空的情况
            if (child.attr('empty')) {

                if (child.attr('empty') == 'yes' && value == '') {
                    return;
                }
                if (child.attr('empty') == 'no' && (value == null || value.trim() == '')) {
                    if (childSpan) {
                        childSpan.text(msg == undefined ? '此输入项不能为空' : msg);
                        child.closest(".control-group").removeClass("success").addClass("error");
                        testResult = false;
                    }
                    return;
                }
            }

            //属性中min 和 max 最大和最小长度
            var min = null;
            var max = null;
            if (child.attr('min')) min = child.attr('min');
            if (child.attr('max')) max = child.attr('max');
            if (min && max) {
                if (len < min || len > max) {
                    if (childSpan) {
                        childSpan.text(msg == undefined ? ('此输入项字符长度应该在' + min + '与' + max + '之间') : msg);
                        child.closest(".control-group").removeClass("success").addClass("error");
                        testResult = false;
                        return;
                    }
                }
            } else if (min) {
                if (len < min) {
                    if (childSpan) {
                        childSpan.text(msg == undefined ? ('此输入项字符长度应大于' + min) : msg);
                        child.closest(".control-group").removeClass("success").addClass("error");
                        testResult = false;
                        return;
                    }
                }
            } else if (max) {
                if (len > max) {
                    if (childSpan) {
                        childSpan.text(msg == undefined ? ('此输入项字符长度应小于' + max) : msg);
                        child.closest(".control-group").removeClass("success").addClass("error");
                        testResult = false;
                        return;
                    }
                }
            }

            //正则校验
            if (child.attr('validate')) {
                var type = child.attr('validate');
                var result = that.check(value, type);
                if (childSpan) {
                    if (result != true) {
                        childSpan.text(msg == undefined ? result : msg);
                        child.closest(".control-group").removeClass("success").addClass("error");
                        testResult = false;
                    }
                }
            }

        });
        return testResult;
    }

    //检测单个正则选项
    that.check = function (value, type) {
        if (this.options[type]) {
            var val = this.options[type]['reg'];
            if (!val.test(value)) {
                return this.options[type]['str'];
            }
            return true;
        } else {
            return '找不到该表单验证正则项';
        }
    };
    return that;

}(document, window.jQuery));

window.formValidate = formValidate;