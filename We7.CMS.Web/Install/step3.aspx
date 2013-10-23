﻿<%@ Page Language="C#" AutoEventWireup="false" Inherits="We7.CMS.Install.Step3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<script type="text/javascript" src="js/setup.js"></script>
<script type="text/javascript">
    var ok = true;
    function hideall(force){
        
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
                                                        <asp:DropDownList ID="DbTypeDropDownList" runat="server" onchange="SelectChange(this)">
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
                                                        <asp:TextBox ID="DbFileNameTextBox" runat="server" Width="150px" Enabled="true" TextMode="Password"></asp:TextBox>
                                                        <span id="msg5"></span>( 默认：We7_CMS_DB.DB3 )
                                                    </td>
                                                </tr>
                                                <tr id="tr6">
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="CreateNewDBCheckBox" Checked="true"/>
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
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
