﻿<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.Profile.Attachments" Codebehind="Attachments.ascx.cs" %>

<%@ Import Namespace="YAF.Types.Extensions" %>

<YAF:PageLinks runat="server" ID="PageLinks" />

<div class="row">
    <div class="col-sm-auto">
        <YAF:ProfileMenu runat="server"></YAF:ProfileMenu>
    </div>
    <div class="col">
        <div class="row">
            <div class="col">
        <div class="card mb-3">
            <div class="card-header">
                <YAF:IconHeader runat="server"
                                LocalizedTag="TITLE"
                                LocalizedPage="ATTACHMENTS"
                                IconName="paperclip"></YAF:IconHeader>
            </div>
            <div class="card-body">
                <YAF:Pager ID="PagerTop" runat="server" OnPageChange="PagerTop_PageChange" />
                <asp:Repeater runat="server" ID="List" OnItemCommand="List_ItemCommand">
                    <HeaderTemplate>
                        <ul class="list-group list-group-flush mt-3">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="list-group-item d-inline-flex align-items-center">
                            <asp:CheckBox ID="Selected" runat="server"
                                              Text="&nbsp;"
                                              CssClass="form-check" />
                                <%# this.GetPreviewImage(Container.DataItem) %>
                                <%# this.Eval( "FileName") %> <em>(<%# this.Eval("Bytes").ToType<int>() / 1024%> kb)</em>
                            <YAF:ThemeButton ID="ThemeButtonDelete" runat="server"
                                                 CommandName="delete" CommandArgument='<%# this.Eval( "ID") %>' 
                                                 CssClass="ml-2"
                                                 TitleLocalizedTag="DELETE" 
                                                 Size="Small"
                                                 TextLocalizedTag="DELETE"
                                                 Icon="trash"
                                                 Type="Danger"
                                                 ReturnConfirmText='<%#this.GetText("ATTACHMENTS", "CONFIRM_DELETE") %>'>
                                </YAF:ThemeButton>
                            
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
                <YAF:ThemeButton ID="DeleteAttachment2" runat="server"
                                 TextLocalizedTag="BUTTON_DELETEATTACHMENT" TitleLocalizedTag="BUTTON_DELETEATTACHMENT_TT"
                                 ReturnConfirmText='<%#this.GetText("ATTACHMENTS", "CONFIRM_DELETE") %>'
                                 OnClick="DeleteAttachments_Click"
                                 Icon="trash"
                                 Type="Danger"
                                 Visible="<%# this.List.Items.Count > 0 %>"
                                 CssClass="m-3"/>
                <YAF:Pager ID="PagerBottom" runat="server" LinkedPager="PagerTop" />
            </div>
        </div>
    </div>
        </div>
    </div>
</div>