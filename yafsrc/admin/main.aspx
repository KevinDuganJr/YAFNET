<%@ Page language="c#" Codebehind="main.aspx.cs" AutoEventWireup="false" Inherits="yaf.admin.main" %>

<asp:repeater runat=server id=ActiveList>
<HeaderTemplate>
	<table width=100% cellspacing=1 cellpadding=0 class=content>
	<tr>
		<td class=header1 colspan=5>Who is Online</td>
	</tr>
	<tr>
		<td class=header2>Name</td>
		<td class=header2>IP Address</td>
		<td class=header2>Location</td>
		<td class=header2>Forum Location</td>
		<td class=header2>Topic Location</td>
	</tr>
</HeaderTemplate>
<ItemTemplate>
	<tr>
		<td class=post><%# DataBinder.Eval(Container.DataItem,"Name") %></td>
		<td class=post><%# DataBinder.Eval(Container.DataItem,"IP") %></td>		
		<td class=post><%# DataBinder.Eval(Container.DataItem,"Location") %></td>
		<td class=post><%# FormatForumLink(DataBinder.Eval(Container.DataItem,"ForumID"),DataBinder.Eval(Container.DataItem,"ForumName")) %></td>		
		<td class=post><%# FormatTopicLink(DataBinder.Eval(Container.DataItem,"TopicID"),DataBinder.Eval(Container.DataItem,"TopicName")) %></td>
	</tr>
</ItemTemplate>
<FooterTemplate>
	</table>
</FooterTemplate>
</asp:repeater>
