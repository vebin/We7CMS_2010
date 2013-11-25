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
                                                        <tr>
                                                            <td valign="top" align="center">
                                                                <img style="border-color:Black; border-width:1px;" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>' border="1" width="200" height="140"/>
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none;">
                                                            <td align="center">
                                                                <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), "编辑")%>'
                                                                    Text="编辑" ID="HyperLinkEdit">
                                                                </asp:HyperLink>
                                                                <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), DataBinder.Eval(((DataListItem)Container).DataItem, "Name"), "删除")%>'
                                                                    Text="删除" ID="HyperLinkDelete">
                                                                </asp:HyperLink>
                                                                <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), "打包")%>'
                                                                    Text="打包下载" ID="HyperLinkDown">
                                                                </asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"/>
                                            </asp:DataList>
                                        </div>
                                        <dt>》可选模板组
                                            <div id="fragment-2">
                                                <asp:DataList ID="TemplateGroupsDataList" runat="server" Width="15%" BorderWidth="0" Cellspacing="20"
                                                    CellPadding="2" ShowFooter="false" ShowHeader="false" RepeatDirection="Horizontal"
                                                    ItemStyle-VerticalAlign="top" RepeatColumns="3">
                                                    <ItemTemplate>
                                                        <table width="100">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Literal ID="LiteralName" runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(), 15)%>'></asp:Literal>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" align="center">
                                                                    <img style="border-color:Black; border-width:1px;" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>' border="1" width="200" height="140"/>
                                                                </td>
                                                            </tr>
                                                            <tr style="display:none;">
                                                                <td align="center">
                                                                    <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), DataBinder.Eval(((DataListItem)Container).DataItem, "Name"), "应用")%>'
                                                                        Text="使用此模板" ID="HyperLinkUsed">
                                                                    </asp:HyperLink>
                                                                    <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), DataBinder.Eval(((DataListItem)Container).DataItem, "Name"), "删除")%>'
                                                                        Text="删除" ID="HyperLinkDelete">
                                                                    </asp:HyperLink>
                                                                    <asp:HyperLink runat="server" NavigateUrl='<%# GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(), "打包")%>'
                                                                        Text="打包下载" ID="HyperLink1">
                                                                    </asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>    
                                    
                                </dl>
                            </div>
                            <div style="display:none">
                                <asp:Button ID="deleteGroupButton" runat="server" OnClick="deleteGroupButton_Click"/>
                                <asp:Button ID="applyGroupButton" runat="server" OnClick="applyGroupButton_Click"/>
                                <asp:TextBox ID="currentGroup" runat="server"></asp:TextBox>
                            </div>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:TextBox ID="ParentTextBox" runat="server" Text="0"></asp:TextBox>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelSuccess" runat="server" Visible="false">
                <h2 class="title">
                    <ul style="font-size:16px;margin-top:20px;padding-left:30px;">
                        ①设置站点参数②选择模板组<span style="color:Red">③完成</span>
                    </ul>
                    <h2></h2>
                    <table style="line-height:32px;width:100%;">
                        <tr>
                            <td align="right" style="width:60px;" valign="top">
                                <img src="/admin/images/success.jpg"/>
                            </td>
                            <td align="left">
                                <% if (CDHelper.Config.IsDemoSite)
                                   { %><strong>演示站点不能修改数据！</strong>
                                <% }
                                   else
                                   {%><strong>恭喜，站点[<asp:Label ID="lblSiteName" runat="server" Text=""></asp:Label>
                                   ]创建成功！</strong><%} %>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" valign="top">
                                <a href="default.aspx" style="font-weight:bolder; font-size:16px;">进入管理后台</a>
                            </td>
                        </tr>
                    </table>
                </h2>
            </asp:Panel>
            <div style="background:url(images/line.gif) repeat-x center 50%">
            </div>
            <div>
                <table cellpadding="0" cellspacing="0" width="60%" border="0">
                    <tr>
                        <td align="center">
                            <span style="padding-right:30px"><span style="padding-right:30px"></span><span style="padding-right:30px"></span>
                                <asp:Button ID="btnPrevious" runat="server" OnClick="PreviousPanel" CausesValidation="false" Text="< 上一步"/>
                                &nbsp;    
                                <asp:Button ID="btnNext" runat="server" OnClick="btnNextPanel" runat="server" Text="下一步 >"/>
                                <span style="padding-right:30px"></span>
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <br />
                <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </center>
    <script type="text/javascript">
        function deleteGroup(name, filename) {
            if (confirm("您确认要删除模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=deleteGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn)
                    btn.click();
            }
        }
        function applyGroup(name, filename) {
            if (confirm("您确认要使用模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=applyGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn)
                    btn.click();
            }
            $("#<%=ImageValue.ClientID %>").blur(function () { $("#logo_preview").attr("src", $(this).val()); });
        }
    </script>
</asp:Content>

