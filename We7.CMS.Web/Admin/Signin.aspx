<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signin.aspx.cs" Inherits="We7.CMS.Web.Admin.Signin" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>登陆<%=ProductBrand %>网站管理中心</title>
    <script type="text/javascript" src="/admin/cgi-bin/DialogHelper.js"></script>
    <script type="text/javascript" src="/admin/ajax/jquery/jquery.latest.min.js"></script>
    <script type="text/javascript" src="/admin/cgi-bin/Menu/common.js"></script>
    <style type="text/css">
        *
        {
            padding: 0;
            margin: 0;    
        }
        body
        {
            color: #333;
            text-align: center;
            background-color: #fbfbfb;
            font: 12px/1.5em Arial;    
        }
        td
        {
            font: 12px/1.5em Arial;    
        }
        th
        {
            font: 12px/1.5em Arial;    
        }
        .loginbox
        {
            margin: 180px auto 60px;
            text-align: left;    
        }
        .logo
        {
            padding: 90px 70px 30px 0px;
            background: url(/admin/images/login_logo.gif) no-repeat 100% 50%;
            width: 226px;
            text-align: right;
        }
        .logo p
        {
            margin: -50px 0px 0px;    
        }
        table.loginform
        {
            margin-top: 10px;    
        }
        .loginform th
        {
            padding: 3px;
            font-size: 12px;
            color: #666;    
        }
        .loginform td
        {
            padding: 3px;
            font-size: 12px;
            color: #666;    
        }
        .submit
        {
            background: Transparent url(/admin/images/login_button.jpg) no-repeat;
            border-bottom-style:none;
            width: 73px;
            height: 26px;
            cursor: pointer;
            border-width: 0px;        
        }
        .footer
        {
            left:50%;
            margin-left: -250px;
            width: 500px;
            color: #999;
            bottom: 10px;
            position:absolute;          
        }
    </style>

</head>
<body id="classic">
    <form id="mainForm" runat="server">
    <WEC:MessagePanel ID="Messages" runat="server"></WEC:MessagePanel>
    <table class="loginbox" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td class="logo">
                    <p>请输入你的用户名、密码，登陆到本站，维护本站信息</p>
                </td>
                <td>
                    <table class="loginform" cellpadding="0" cellspacing="0" style="height:180px;">
                        <tbody>
                            <tr>
                                <th>
                                    登录名：
                                </th>
                                <td>
                                    <asp:TextBox ID="LoginNameTextBox" runat="server" Columns="30"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    密码：
                                </th>
                                <td>
                                    <asp:TextBox ID="PasswordTextBox" runat="server" Columns="30" TextMode="Password"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="tbAuthCode2" runat="server" visible="false">
                                <th>
                                    验证码：
                                </th>
                                <td>
                                    <asp:TextBox ID="CodeNumberTextBox" runat="server" Columns="30"></asp:TextBox>
                                </td>
                                <td>
                                    <img alt="x" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx" runat="server" id="Img2"/>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="SubmitButton" runat="server" Text="   " OnClick="SubmitButton_Click" CssClass="submit"/>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="MessageLabel" runat="server" Text="" FontColor="red"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <p></p>
    <p class="footer">
        <asp:Label ID="CopyrightLiteral" runat="server"></asp:Label>
    </p>
    </form>
</body>
</html>
