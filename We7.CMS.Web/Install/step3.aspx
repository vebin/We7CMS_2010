<%@ Page Language="C#" AutoEventWireup="false" Inherits="We7.CMS.Install.Step3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="js/setup.js"></script>
<script type="text/javascript">
    var ok = true;
    function hideall(force){
        var sel = document.getElementById("DBTypeDropDownList");
        if (sel && '<%=SelectDB %>' != '' && !force) {
            sel.value = '<%=SelectDB %>';
            DBTypeChange('<%=SelectDB %>');
        }
        else {
            hide("tr1");
            hide("tr2");
            hide("tr3");
            hide("tr4");
            hide("tr5");
            hide("tr6");
            document.getElementById("DBTypeDropDownList").selectValue = 0;
        }
    }

    function DBTypeChange(type) {
        show("tr6");
        document.getElementById('msg0').innerHTML = "";

        switch (type) {
            case "SqlServer":
                show("tr1");
                show("tr2");
                show("tr3");
                show("tr4");
                hide("tr5");
                document.getElementById('msg0').innerHTML = '<br/>*您是否在使用SQL Server 2005 Express？ 请在“服务器名”项使用“主机名称\\SQLEXPRESS”。';
                break;
            case "MySQL":
                show("tr1");
                show("tr2");
                show("tr3");
                show("tr4");
                hide("tr5");                    //为何MySQL、Oracle需要隐藏数据文件、创建新数据库选项？为何SQLite、Access要隐藏数据库服务器、数据库名称、数据库用户、密码四个选项,而且需要数据文件？  
                hide("tr6");                    //为何oracle时，不用检查服务器名称、数据库名称、用户名称三项？
                break;
            case "Oracle":
                show("tr1");
                hide("tr2");
                show("tr3");
                show("tr4");
                hide("tr5");
                hide("tr6");
                break;
            case "SQLite":
            case "Access":
                hide("tr1");
                hide("tr2");
                hide("tr3");
                hide("tr4");
                show("tr5");
                break;
            default:
                hideall(true);
                break;
        }
        document.getElementById('dbtype').value = type;
    }

    function show(id) {
        document.getElementById(id).style.display = "";
    }
    function hide(id) {
        document.getElementById(id).style.display = "none";
    }

    function SelectChange() {
        DBTypeChange(document.getElementById('DBTypeDropDownList').value);
        ok = true;
    }

    function checkid(obj, id) {
        var v = obj.value;
        if (v.length == 0) {
            document.getElementById('msg' + id).innerHTML = '<span style=\'#ff0000\'>此处不能为空！</span>';
            ok = false;
        }
        else {
            document.getElementById('msg' + id).innerHTML = '';
            ok = true;
        }
    }

    function checknull() {
        if (document.getElementById('AdminPasswordTextBox').value == "" || document.getElementById('AdminPasswordTextBox').value.length < 6) {
            alert('系统管理员密码不能少于6位!');
            document.getElementById('AdminPasswordTextBox').focus();
            return false;
        }
        if (document.getElementById('repwd').value == "") {
            alert('确认密码不能为空!');
            document.getElementById('repwd').focus();
            return false;
        }
        if (document.getElementById('repwd').value != document.getElementById('AdminPasswordTextBox').value) {
            alert('系统管理员密码两次输入不同，请重新输入!');
            document.getElementById('AdminPasswordTextBox').focus();
            return false;
        }
        if (document.getElementById('DBTypeDropDownList').value == 'Access' || document.getElementById('DBTypeDropDownList').value == 'SQLite') {
            if (!isEmpty('DBFileNameTextBox')) {
                document.Form1.submit();
            }
            else {
                alert('DBFileNameTextBox 不能为空!');
                return false;
            }
        }
        else if (document.getElementById('DBTypeDropDownList').value != 'oracle') {                     
        if (!isEmpty('ServerNameTextBox') && !isEmpty('DatabaseTextBox') && !isEmpty('UserTextBox')) {          
            document.Form1.submit();
        }
        else {
            alert('datasource 不能为空!');
            return false;
        }
        }
        document.Form1.submit();
    }
    function isEmpty(id) {
        if (document.getElementById(id).value.length == 0) {
            return true;
        }
        return false;
    }
    
</script>
<%=header %>
<body onload="hideall()" class="pubbox_login">
    <form id="Form1" method="post" runat="server" onsubmit="return checknull();">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="12" bgcolor="#999" class="login">
            <tr>
                <td bgcolor="#fff">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" align="center">
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellpadding="8" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <h1>2、初始化数据环境
                                            </h1>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="180" valign="top"><%=logo %>
                            </td>
                            <td width="520" valign="top">
                                <asp:Literal ID="msg" runat="server" Text="您当前网站的Web.Config文件设置不正确，请您确保其文件内容正确<BR><FONT color='#996600'>详见《安装说明》</FONT>" Visible="false"></asp:Literal>
                                <table width="100%" cellpadding="8" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <p></p>
                                            <table width="100%" border="0" cellpadding="8" cellspacing="0">
                                                <tr>
                                                    <td width="30%">网站的名称：</td>
                                                    <td style="width:352px;">
                                                        <asp:TextBox ID="WebsiteNameTextBox" runat="server" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>系统管理员名称：</td>
                                                    <td style="width:352px;">
                                                        <asp:TextBox ID="AdminNameTextBox" runat="server" Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>系统管理员密码：<br/>
                                                    （不得少于6位）</td>
                                                    <td style="width:352px;">
                                                        <asp:TextBox ID="AdminPasswordTextBox" runat="server" MaxLength="32" Size="20" Width="150px" TextMode="Password"></asp:TextBox>
                                                        *<br/>
                                                        密码强度：<span id="showmsg"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>确认密码：</td>
                                                    <td style="width:352px;">
                                                        <input name="repwd" id="repwd" type="password" maxlength="32" size="20" class="FormBase" onfocus="this.className='FormFocus'" onblur="this.className='FormBase'" style="width:150px;"/>*
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color:#f5f5f5;">
                                                        数据库类型：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:DropDownList ID="DBTypeDropDownList" runat="server" onchange="SelectChange(this)">
                                                            <asp:ListItem Value="0">请选择数据库类型</asp:ListItem>
                                                            <asp:ListItem Value="SqlServer">SqlServer</asp:ListItem>
                                                            <asp:ListItem Value="MySql">MySql</asp:ListItem>
                                                            <asp:ListItem Value="Oracle">Oracle</asp:ListItem>
                                                            <asp:ListItem Value="SQLite">SQLite</asp:ListItem>
                                                            <asp:ListItem Value="Access">Access</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span id="msg0"></span>
                                                    </td>
                                                </tr>
                                                <tr id="tr1">
                                                    <td style="background-color:#f5f5f5;">
                                                        服务器名称或IP地址：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:TextBox ID="ServerTextBox" runat="server" Width="150px" Enabled="true" onblur="checkid(this, '1')">(local)</asp:TextBox>
                                                        *<span id="msg1"></span>
                                                    </td>
                                                </tr>
                                                <tr id="tr2">
                                                    <td style="background-color:#f5f5f5;">
                                                        数据库名称：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:TextBox ID="DatabaseTextBox" runat="server" Width="150px" Enabled="true" onblur="checkid(this, '2')">We7_CMS</asp:TextBox>
                                                        *<span id="msg2"></span>
                                                    </td>
                                                </tr>
                                                <tr id="tr3">
                                                    <td style="background-color:#f5f5f5;">
                                                        数据库用户名称：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:TextBox ID="UserTextBox" runat="server" Width="150px" Enabled="true" onblur="checkid(this, '3')">We7_CMS</asp:TextBox>
                                                        *<span id="msg3"></span>
                                                    </td>
                                                </tr>
                                                <tr id="tr4">
                                                    <td style="background-color:#f5f5f5;">
                                                        数据库用户密码：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:TextBox ID="PasswordTextBox" runat="server" Width="150px" Enabled="true" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="tr5">
                                                    <td style="background-color:#f5f5f5;">
                                                        数据库文件：
                                                    </td>
                                                    <td style="background-color:#f5f5f5;width:352px;">
                                                        <asp:TextBox ID="DBFileNameTextBox" runat="server" Width="150px" Enabled="true" onblur="checkid(this, '5')">We7_CMS_DB</asp:TextBox>
                                                        <span id="msg5"></span>( 默认：We7_CMS_DB.DB3 )
                                                    </td>
                                                </tr>
                                                <tr id="tr6">
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="CreateNewDBCheckBox" Checked="true" Text="创建新数据库"/>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Panel ID="ConfigMsgPanel" runat="server" Visible="false">
                                    <font color="#ff6633">错误：无法把设置写入db.config文件，您可以将下面文本框内容保存为"db.config"文件，然后通过FTP软件上传到<strong>网站Config目录</strong><br/></font>
                                    <br />
                                    db.config内容：
                                    <input type="button" value="复制到剪贴板" accesskey="c" onclick="HighlightAll(this.form.txtMsg)"/>
                                    <asp:TextBox ID="txtMsg" runat="server" Height="180" TextMode="MultiLine" Width="98%"></asp:TextBox>
                                </asp:Panel>
                                <div style="margin:10px 20px 30px 10px;text-align:right">
                                    <asp:Button ID="ResetDBInfo" runat="server" OnClick="ResetDBInfo_Click" class="bprimarypub80" Text="下一步"></asp:Button >
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%=footer %>
    <input id="dbtype" type="hidden"/>
    </form>
</body>
</html>
