<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/ContentNoMenu.Master"  CodeBehind="TemplateGroupInfo.aspx.cs" Inherits="We7.CMS.Web.Admin.TemplateGroupInfo" Title="模板基本信息"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MyContentPlaceHolder">
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/images/icon_settings.png"/>
        <asp:Label ID="NameLabel" runat="server" Text="修改模板组基本信息"></asp:Label>
    </h2>
    <asp:PlaceHolder ID="ContentHolder" runat="server"></asp:PlaceHolder>
</asp:Content>