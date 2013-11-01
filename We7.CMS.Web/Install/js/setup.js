function HighlightAll(obj) {
    obj.focus();
    obj.select();
    if (obj) {
        obj.createTextRange().executeCommand("Copy");
        window.status = "将模板内容复制到剪贴板";
        setTimeout("window.status=''", 1800);
    }
}
