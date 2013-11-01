<%@ Import Namespace="We7.CMS" %>
<%@ Import Namespace="We7.CMS.Install" %>
<%@ Page Language="C#" AutoEventWireup="true" Inherits="We7.CMS.Install.SetupPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<%=SetupPage.header %>
<body class="pubbox_login">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="12" bgcolor="#999" class="login">
            <tr>
                <td bgcolor="#ffffff">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellpadding="8" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            <h1>1、环境监测
                                            </h1>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="180" valign="top"><%=SetupPage.logo %>
                            </td>
                            <td width="520" valign="top" style="padding-bottom:10px;">
                                <p>现在对您的运行环境进行检测，以确认您的环境符合要求.</p>
                                <p><font color="red">注意：</font>如果目录或文件没有写入和删除权限情况，请选择目录或文件->右键属性->安全->添加，
                                在"输入对象名称来选择"中输入"Network Service"，点击"确定".选择"组或用户名称"中的"Network Service"用户组，在下面
                                "Network Service"的权限中勾选"修改"的"允许"复选框，点击"确定"后再次重新刷新本页面确定.</p>
                                <p>
                                    <%
                                        bool err = false;
                                        string result = SetupPage.InitialSystemValidCheck(ref err);
                                        Response.Write("<font color=red>"+result+"</font>");    
                                    %>    
                                </p>
                            </td>
                        </tr>
                        <%if (!err)
                          {%>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <div style="margin:10px 20px 30px 10px; text-align:right">
                                    <input type="button" onclick="javascript:window.location.href='step3.aspx'" class="bprimarypub80" value="下一步"/>
                                </div>
                            </td>
                        </tr>
                        <%} %>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%=SetupPage.footer %>
</body>
</html>
