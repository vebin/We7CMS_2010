<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewSiteWizard.aspx.cs" 
        Inherits="We7.CMS.Web.Admin.NewSiteWizard" MasterPageFile="~/admin/theme/ContentNoMenu.Master"%>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <center>
        <div id="dvContent" style="width:620px;height:100%;margin-top:80px;border:solid 10px #f0f0f0;
            text-align:left;padding-left:15px;display:table;">
            <h2 class="title">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/admin/Images/icons_look.gif"/>
                <asp:Label ID="TitleLabel" runat="server" Text="新建站点向导"></asp:Label>
                <span class="summary">
                    <asp:Label ID="SummaryLabel" runat="server" Text="分三步创建新站点"></asp:Label>
                </span>
            </h2>
            <asp:Panel ID="ContentPanel" runat="server">
                <div id="breadcrumb">
                    <div class="Content">
                        <asp:Panel ID="pnlSiteConfig" runat="server" Visible="true">
                            <h2 class="title">
                                <ul style="font-size:16px;margin-top:20px;padding-left:30px;">
                                    <span style="color:Red">①设置站点参数</span>②选择模板组③完成
                                </ul>
                                <h2>
                                </h2>
                                <div style="padding-left:10px;padding-top:3px;">
                                    <table align="center" cellpadding="3" cellspacing="3" width="95%">
                                        <tr>
                                            <td align="left" width="100">站点名称：</td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSiteName" runat="server" class="colorblur" Columns="35"
                                                    MaxLength="50" onblur="this.className='colorblur';"
                                                    onfocus="this.className='colorfocus';">
                                                </asp:TextBox>
                                                <font color="red">*</font>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txtSiteName" Display="Dynamic" ErrorMessage="请填写站点名称">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                    ControlToValidate="txtSiteName" Display="Dynamic" ErrorMessage="*"
                                                    ValidationExpression="[^']+">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">公司名称：</td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSiteFullName" runat="server" class="colorblur" Columns="35"
                                                    MaxLength="100" onblur="this.className='colorblur';"
                                                    onfocus="this.className='colorfocus';">
                                                </asp:TextBox>
                                                <font color="red">*</font>
                                                <asp:RequiredFieldValidator ID="RequiredValidator3" runat="server"
                                                    ControlToValidate="txtSiteFullName" ErrorMessage="请填写公司名称">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">站点logo：</td>
                                            <td align="left">
                                                <img id="logo_preview" src="<%=ImageValue.Text %>" alt="logo预览图" style="max-width:175px;"/>
                                                <br />
                                                <asp:TextBox ID="ImageValue" runat="server" MaxLength="400" Width="238">
                                                </asp:TextBox>
                                                <font color="red">*</font>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                    ControlToValidate="ImageValue" ErrorMessage="请您上传站点logo">
                                                </asp:RequiredFieldValidator>
                                                <br />
                                                <asp:FileUpload ID="fuImage" runat="server" class="file_style" Style="width:200px;"/>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="btnUpload" runat="server" CausesValidation="false" CssClass="button_style"
                                                    OnClick="btnUpload_Click" Text="上传"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">版权：</td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCopyright" runat="server" class="onblur" Columns="35"
                                                    Height="80" MaxLength="100" onblur="this.className='onblur';"
                                                    onfocus="this.className='onfocus';" Style="float:left;"
                                                    TextMode="Multiline" Width="380">
                                                </asp:TextBox>
                                                <font color="red">*</font>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="txtCopyright" Display="Dynamic" ErrorMessage="请填写版权">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">网站备案信息</td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtIcpInfo" runat="server" class="onblur" Columns="35"
                                                    Height="80" MaxLength="100" onblur="this.className='onblur';"
                                                    onfocus="this.className='onfocus';" Style="float:left;"
                                                    TextMode="Multiline" Width="380">
                                                </asp:TextBox>
                                                <font color="red">*</font>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                    ControlToValidate="txtIcpInfo" Display="Dynamic" ErrorMessage="请填写网站备案信息">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                &nbsp;<asp:Literal ID="ltlMsg" runat="server"></asp:Literal>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </div>
                            </h2>
                        </asp:Panel>
                        <asp:Panel ID="pnlSiteTemplate" runat="server" Visible="false">
                            <h2 class="title">
                                <ul style="font-size:16px;margin-top:20px;padding-left:30px;">
                                    ①设置站点参数<span style="color:Red">②选择模板组</span>③完成
                                </ul>
                            </h2>
                            <div class="toolbar">
                                <li style="display:none" class="smallButton4"><a class="editAction" href="Template/TemplateGroupInfo.aspx">创建模板组</a></li>
                                <li class="smallButton4">
                                    <asp:HyperLink ID="UploadHyperLink" Enabled="false" NavigateUrl="~/admin/Plugin/PluginAdd.aspx" runat="server" Visible="false">
                                    上传模板组
                                    </asp:HyperLink>
                                </li>
                            </div>
                            <br />
                            <div id="conbox">
                                <dl>
                                    <dt>》当前模板组
                                        <div id="fragment-1">
                                            <h3>
                                                <span>
                                                    <asp:Label ID="UseTemplateGroupsLabel" runat="server" Text=""></asp:Label>
                                                </span>
                                            </h3>
                                            <asp:DataList ID="UseTemplateGroupsDataList" Width="15%" BorderWidth="0" CellSpacing="10"
                                                CellPadding="2" runat="server" ShowFooter="false" ShowHeader="false" RepeatDirection="Horizontal"
                                                ItemStyle-VerticalAlign="Top" RepeatColumns="3">
                                                <ItemTemplate>
                                                    <table width="100">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Literal ID="LiteralName" runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(), 15)%>'></asp:Literal>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"/>
                                            </asp:DataList>
                                        </div>
                                        <dt>》可选模板组
                                            <div id="fragment-2">
                                            </div>    
                                    
                                </dl>
                            </div>
                            <div>
                            </div>
                        </asp:Panel>

                    </div>
                    <div>
                    
                    </div>
                </div>
            </asp:Panel>
        </div>
    </center>
</asp:Content>

